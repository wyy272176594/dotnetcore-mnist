﻿using ConvNetSharp.Volume;
using ConvNetSharp.Volume.Double;
using System;
using System.Collections.Generic;
using System.Text;
using Volume = ConvNetSharp.Volume.Volume<double>;

namespace DotNetCoreMnist
{
    internal class DataSet
    {
        private readonly List<MnistEntry> _trainImages;
        private readonly Random _random = new Random(RandomUtilities.Seed);
        private int _start;
        private int _epochCompleted;

        public DataSet(List<MnistEntry> trainImages)
        {
            this._trainImages = trainImages;
        }

        public Tuple<Volume, Volume, int[]> NextBatch(int batchSize)
        {
            const int w = 28;
            const int h = 28;
            const int numClasses = 10;

            var dataShape = new Shape(w, h, 1, batchSize);
            var labelShape = new Shape(1, 1, numClasses, batchSize);
            var data = new double[dataShape.TotalLength];
            var label = new double[labelShape.TotalLength];
            var labels = new int[batchSize];

            // Shuffle for the first epoch
            if (this._start == 0 && this._epochCompleted == 0)
            {
                for (var i = this._trainImages.Count - 1; i >= 0; i--)
                {
                    var j = this._random.Next(i);
                    var temp = this._trainImages[j];
                    this._trainImages[j] = this._trainImages[i];
                    this._trainImages[i] = temp;
                }
            }

            var dataVolume = BuilderInstance.Volume.From(data, dataShape);

            for (var i = 0; i < batchSize; i++)
            {
                var entry = this._trainImages[this._start];

                labels[i] = entry.Label;

                var j = 0;
                for (var y = 0; y < h; y++)
                {
                    for (var x = 0; x < w; x++)
                    {
                        dataVolume.Set(x, y, 0, i, entry.Image[j++] / 255.0);
                    }
                }

                label[i * numClasses + entry.Label] = 1.0;

                this._start++;
                if (this._start == this._trainImages.Count)
                {
                    this._start = 0;
                    this._epochCompleted++;
                    Console.WriteLine($"Epoch #{this._epochCompleted}");
                }
            }


            var labelVolume = BuilderInstance.Volume.From(label, labelShape);

            return new Tuple<Volume, Volume, int[]>(dataVolume, labelVolume, labels);
        }
    }
}
