﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;


namespace Bridge.Tests
{
    public class ResourceControllerTests : IClassFixture<BridgeLaunchFixture>
    {
        const string baseAddress = "http://localhost:8080";

        [Fact]
        public void ResourcePut() {
            using (HttpClient client = new HttpClient()) {
                client.BaseAddress = new Uri(baseAddress);
                var result = client.PutAsJsonAsync("/resource/", new { name = "Bridge.Commands.WhoAmI"}).Result;
                Assert.Equal(result.StatusCode , HttpStatusCode.OK);
            }
        }

        [Fact]
        public void ValidateResourceId()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAddress);
                var result = client.PutAsJsonAsync("/resource/", new { name = "Bridge.Commands.WhoAmI" }).Result;
                Assert.Equal(result.StatusCode, HttpStatusCode.OK);
                dynamic obj = result.Content.ReadAsAsync(typeof(object)).Result;
                var id = obj.id;
                var guid = Guid.Parse(id.ToString());
                // This means we were able to parse a guid."
            }
        }
    }
}
