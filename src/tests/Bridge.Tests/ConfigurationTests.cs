using Bridge.Tests.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Bridge.Tests
{
    public class ConfigurationTests : IClassFixture<BridgeLaunchFixture>
    {
        [Fact]
        public void ConfigResourcesPath() {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.BaseAddress);
                var result = client.PostAsJsonAsync("/config/", 
                                        new { resourcesDirectory = "."}).Result;
                Assert.Equal(result.StatusCode, HttpStatusCode.OK);
            }
        }

        [Fact]
        public void ConfigTestCommand()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.BaseAddress);
                var result = client.PostAsJsonAsync("/config/",
                                        new { resourcesDirectory = Path.GetFullPath(".") }).Result;
                Assert.Equal(result.StatusCode, HttpStatusCode.OK);

                var commandResult = client.PutAsJsonAsync("/resource/", 
                                    new { name = typeof(TestCommand).FullName }).Result;
                dynamic obj = commandResult.Content.ReadAsAsync(typeof(object)).Result;
                var id = obj.id;
                Assert.Equal<string>((string)obj.details, TestCommand.Value);
                var guid = Guid.Parse(id.ToString());
                Assert.Equal(commandResult.StatusCode, HttpStatusCode.OK);
            }
        }
    }
}
