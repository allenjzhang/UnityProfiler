using System;
using System.Web.Http;
using System.Web.Script.Services;

namespace ProfileLogViewer.Controllers
{
	[ScriptService]
    public class ProfileDataController : ApiController
    {
		//// GET: api/ProfileData
		//public IEnumerable<string> Get()
		//{
		//	return new string[] { "value1", "value2" };
		//}

        // GET: api/ProfileData/Stats/test.bar
		[HttpGet]		
		public double[][] ProfileStats(string id)
		{
			Random r = new Random();
			int numDays = (int) (DateTime.Now - DateTime.Parse("2012/01/01")).TotalDays;
			double[][] data = new double[numDays][];

			for (int i = 0; i < numDays; i++)
			{
				data[i] = new double[6];
				data[i][0] = DateTime.Parse("2012/01/01").AddDays(i).Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
				data[i][1] = 100;
				data[i][2] = 200;
				data[i][3] = 300;
				data[i][4] = 150;
				data[i][5] = r.NextDouble() * 1000;
			}

			return data;
			//string x = JsonConvert.SerializeObject(data);


			//StringBuilder sb = new StringBuilder();

			//double numDays = (DateTime.Now - DateTime.Parse("2012/01/01")).TotalDays;

			//sb.Append("[");
			//for (double i = 0; i <= numDays; i++)
			//{
			//	sb.Append("[");
			//	sb.Append(DateTime.Parse("2012/01/01").AddDays(i).Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds);
			//	sb.Append(",");
			//	sb.Append("100,100,100,");
			//	sb.Append(r.NextDouble() * 1000);
			//	sb.Append("]");
			//	if (i != numDays)
			//		sb.Append(",");
			//}
			//sb.Append("]");
			//return x;
		}

    }
}
