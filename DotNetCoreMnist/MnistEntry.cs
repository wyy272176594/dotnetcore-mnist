using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCoreMnist
{
    public class MnistEntry
    {
        public byte[] Image { get; set; }

        public int Label { get; set; }

        public override string ToString()
        {
            return "Label: " + this.Label;
        }
    }
}
