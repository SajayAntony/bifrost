using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web.Models;

namespace Web.Controllers
{
    public class ConfigController : ApiController
    {
        public HttpResponseMessage POST(config config)
        {
            Trace.WriteLine("POST config: " + JsonConvert.SerializeObject(config, Formatting.Indented));

            if (!config.isValidProbingPath())
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                    "config.resourcesDirectory does not exist.");
            }

            config.UpdateApp();
            Trace.WriteLine("POST config: " + JsonConvert.SerializeObject(WebApiApplication.Config, Formatting.Indented));
            return Request.CreateResponse(HttpStatusCode.OK, WebApiApplication.Config);
        }
    }
}
