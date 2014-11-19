using System;
using System.Web.Http;
using System.Web.Script.Services;

namespace ProfileLogViewer.Controllers
{
	[ScriptService]
    public class ProfileDataController : ApiController
    {
		// GET: api/ProfileData/Stats/test.bar
		[HttpGet]
		public int getcallstacks(string id)
		{
			return 0;			
		}

        // GET: api/ProfileData/Stats/test.bar
		[HttpGet]		
		public double[][] getstats(string id)
		{
			Random r = new Random();
			int numDays = (int) (DateTime.Now - DateTime.Parse("2012/01/01")).TotalDays;
			double[][] data = new double[numDays][];

			for (int i = 0; i < numDays; i++)
			{
				data[i] = new double[3];
				data[i][0] = DateTime.Parse("2012/01/01").AddDays(i).Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
				data[i][1] = r.Next(50)+300;
				data[i][2] = r.NextDouble() * 1000;
			}

			return data;
		}

    }
}
