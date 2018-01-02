﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace DotNetCoreMnist
{
    public static class MnistReader
    {
        public static List<MnistEntry> Load(string labelFile, string imageFile, int maxItem = -1)
        {
            var label = LoadLabels(labelFile, maxItem);
            var images = LoadImages(imageFile, maxItem);

            if (label.Count == 0 || images.Count == 0)
            {
                return new List<MnistEntry>();
            }

            return label.Select((t, i) => new MnistEntry { Label = t, Image = images[i] }).ToList();
        }

        private static List<byte[]> LoadImages(string filename, int maxItem = -1)
        {
            var result = new List<byte[]>();

            if (File.Exists(filename))
            {
                using (var instream = new GZipStream(File.Open(filename, FileMode.Open), CompressionMode.Decompress))
                {
                    using (var reader = new BinaryReader(instream))
                    {
                        var magicNumber = ReverseBytes(reader.ReadInt32());
                        var numberOfImage = ReverseBytes(reader.ReadInt32());
                        var rowCount = ReverseBytes(reader.ReadInt32());
                        var columnCount = ReverseBytes(reader.ReadInt32());
                        if (maxItem != -1)
                        {
                            numberOfImage = Math.Min(numberOfImage, maxItem);
                        }

                        for (var i = 0; i < numberOfImage; i++)
                        {
                            var image = reader.ReadBytes(rowCount * columnCount);
                            result.Add(image);
                        }
                    }
                }
            }

            return result;
        }

        private static List<int> LoadLabels(string filename, int maxItem = -1)
        {
            var result = new List<int>();

            if (File.Exists(filename))
            {
                using (var instream = new GZipStream(File.Open(filename, FileMode.Open), CompressionMode.Decompress))
                {
                    using (var reader = new BinaryReader(instream))
                    {
                        var magicNumber = ReverseBytes(reader.ReadInt32());
                        var numberOfItem = ReverseBytes(reader.ReadInt32());
                        if (maxItem != -1)
                        {
                            numberOfItem = Math.Min(numberOfItem, maxItem);
                        }

                        for (var i = 0; i < numberOfItem; i++)
                        {
                            result.Add(reader.ReadByte());
                        }
                    }
                }
            }

            return result;
        }

        private static int ReverseBytes(int v)
        {
            var intAsBytes = BitConverter.GetBytes(v);
            Array.Reverse(intAsBytes);
            return BitConverter.ToInt32(intAsBytes, 0);
        }
    }
}
