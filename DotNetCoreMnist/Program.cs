﻿using ConvNetSharp.Core;
using ConvNetSharp.Core.Layers;
using ConvNetSharp.Core.Layers.Double;
using ConvNetSharp.Core.Serialization;
using ConvNetSharp.Core.Training;
using ConvNetSharp.Volume;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCoreMnist
{
    class Program
    {
        private readonly CircularBuffer<double> _testAccWindow = new CircularBuffer<double>(100);
        private readonly CircularBuffer<double> _trainAccWindow = new CircularBuffer<double>(100);
        private Net<double> _net;
        private int _stepCount;
        private SgdTrainer<double> _trainer;

        static void Main(string[] args)
        {
            Program program = new Program();
            program.MnistDemo(false,5);
            //Console.WriteLine("Hello World!");
        }

        private void MnistDemo(bool creatNew = true,int trainId=1)
        {
            var datasets = new DataSets();
            if (!datasets.Load(100))
            {
                return;
            }

            // Create network
            if (creatNew)
            {
                this._net = new Net<double>();
                this._net.AddLayer(new InputLayer(28, 28, 1));
                this._net.AddLayer(new ConvLayer(5, 5, 8) { Stride = 1, Pad = 2 });
                this._net.AddLayer(new ReluLayer());
                this._net.AddLayer(new PoolLayer(2, 2) { Stride = 2 });
                this._net.AddLayer(new ConvLayer(5, 5, 16) { Stride = 1, Pad = 2 });
                this._net.AddLayer(new ReluLayer());
                this._net.AddLayer(new PoolLayer(3, 3) { Stride = 3 });
                this._net.AddLayer(new FullyConnLayer(10));
                this._net.AddLayer(new SoftmaxLayer(10));
            }
            else
            {
                HttpClient httpClient = new HttpClient();
                var res = httpClient.GetStringAsync($"https://www.wang-yueyang.com/api/nets/net/{trainId}").Result;
                var net = JsonConvert.DeserializeObject<Net>(res);
                this._net = SerializationExtensions.FromJson<double>(net.NetText);
            }
            this._trainer = new SgdTrainer<double>(this._net)
            {
                LearningRate = 0.01,
                BatchSize = 20,
                L2Decay = 0.001,
                Momentum = 0.9
            };

            Console.WriteLine("Convolutional neural network learning...[Press any key to stop]");
            do
            {
                var trainSample = datasets.Train.NextBatch(this._trainer.BatchSize);
                Train(trainSample.Item1, trainSample.Item2, trainSample.Item3);

                var testSample = datasets.Test.NextBatch(this._trainer.BatchSize);
                Test(testSample.Item1, testSample.Item3, this._testAccWindow);

                Console.WriteLine("Loss: {0} Train accuracy: {1}% Test accuracy: {2}%", this._trainer.Loss,
                    Math.Round(this._trainAccWindow.Items.Average() * 100.0, 2),
                    Math.Round(this._testAccWindow.Items.Average() * 100.0, 2));

                Console.WriteLine("Example seen: {0} Fwd: {1}ms Bckw: {2}ms", this._stepCount,
                    Math.Round(this._trainer.ForwardTimeMs, 2),
                    Math.Round(this._trainer.BackwardTimeMs, 2));
            } while (!Console.KeyAvailable);

            //训练结果上传到Service中
            Task.Run(() =>
            {
                var step = 3;
                var json = _net.ToJson();
                while (step > 0)
                {                  
                    var client = new HttpClient();
                    var x = client.PostAsync(@"https://www.wang-yueyang.com/api/nets/AddNet",
                        new StringContent(JsonConvert.SerializeObject(new { NetText = json }), Encoding.UTF8, "application/json"));
                    x.Wait();
                    if (x.Result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        break;
                    }
                    step--;
                }
            }).Wait();
        }

        private void Test(Volume<double> x, int[] labels, CircularBuffer<double> accuracy, bool forward = true)
        {
            if (forward)
            {
                this._net.Forward(x);
            }

            var prediction = this._net.GetPrediction();

            for (var i = 0; i < labels.Length; i++)
            {
                accuracy.Add(labels[i] == prediction[i] ? 1.0 : 0.0);
            }
        }

        private void Train(Volume<double> x, Volume<double> y, int[] labels)
        {
            this._trainer.Train(x, y);

            Test(x, labels, this._trainAccWindow, false);

            this._stepCount += labels.Length;
        }

    }
}
