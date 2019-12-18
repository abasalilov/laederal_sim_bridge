using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NodeManikinBridge.Models;
using System.Diagnostics;
using System.ComponentModel;


namespace NodeManikinBridge.Controllers
{
    public class vocalController : ApiController
    {
        // GET: api/vocal
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/vocal/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/vocal
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/vocal/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/vocal/5
        public void Delete(int id)
        {
        }
    }
}
