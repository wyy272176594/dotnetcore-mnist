using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Text;

namespace UnitTest
{
    [TestClass]
    public class HttpTest
    {
        [TestMethod]
        public async System.Threading.Tasks.Task TestMethodAsync()
        {
            //28 * 28 image
            Bitmap bitmap = new Bitmap(@"");
            List<byte> bytes = new List<byte>();
            int j = 0;
            for (var y = 0; y < bitmap.Height; y++)
            {
                for (var x = 0; x < bitmap.Width; x++)
                {
                    var color = bitmap.GetPixel(x, y);
                    bytes.Add(color.G);
                }
            }
            var json = JsonConvert.SerializeObject(new { Bytes = bytes });
            var client = new HttpClient();
            var response = client.PostAsync(@"https://www.wang-yueyang.com/api/nets/prediction",
                        new StringContent(JsonConvert.SerializeObject(new { Bytes = bytes }), Encoding.UTF8, "application/json"))
                        .Result;
            Assert.AreEqual( System.Net.HttpStatusCode.OK, response.StatusCode);
            string result = await response.Content.ReadAsStringAsync();
            //image label
            Assert.AreEqual("", result);
        }
    }
}
