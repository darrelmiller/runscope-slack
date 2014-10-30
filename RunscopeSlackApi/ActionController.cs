using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace HttpCheckApi
{
    public class ActionController : ApiController
    {
        public IHttpActionResult Post()
        {

            return new ResponseMessageResult(new HttpResponseMessage()
            {
                Content = new StringContent("{ \"text\" : \"action completed\" }", Encoding.UTF8,"application/json")
            });
        }
    }
}
