using System.Collections.Generic;
using System.Web.Http;

namespace ProfileLogViewer.Controllers
{
    public class ProfileDataController : ApiController
    {
        // GET: api/ProfileData
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ProfileData/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ProfileData
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/ProfileData/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ProfileData/5
        public void Delete(int id)
        {
        }
    }
}
