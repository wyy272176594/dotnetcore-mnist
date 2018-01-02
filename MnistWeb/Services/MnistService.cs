using ConvNetSharp.Core;
using ConvNetSharp.Core.Serialization;
using ConvNetSharp.Volume;
using ConvNetSharp.Volume.Double;
using MnistWeb.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MnistWeb.Services
{
    public class MnistService : IMnistService
    {
        private readonly INetRepository _netRepository;
        private Net<double> _net;
        public MnistService(INetRepository netRepository)
        {
            _netRepository = netRepository;
            var json = _netRepository.GetSingle(7).NetText;
            _net = SerializationExtensions.FromJson<double>(json);
        }

        public int Calculate(byte[] image)
        {
            if (image.Length != 28 * 28)
            {
                return -1;
            }
            var dataShape = new Shape(28, 28, 1, 1);
            var data = new double[dataShape.TotalLength];
            var dataVolume = BuilderInstance.Volume.From(data, dataShape);

            var j = 0;
            for (var y = 0; y < 28; y++)
            {
                for (var x = 0; x < 28; x++)
                {
                    dataVolume.Set(x, y, 0, 0, image[j++]);
                }
            }
            _net.Forward(dataVolume);
            var prediction = _net.GetPrediction();
            return prediction[0];
        }
    }
}
