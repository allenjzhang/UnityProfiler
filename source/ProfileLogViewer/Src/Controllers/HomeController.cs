using System.Web.Mvc;
using Newtonsoft.Json;
using ProfileLogViewer.Models;

namespace ProfileLogViewer.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public string GetProfileData()
		{
			ProfileStat p = new ProfileStat()
			{
				AverageDuration = 100,
				MethodName = "Test.Bar",
				TimeStamp = 123456,
				TotalCount = 10
			};
			return JsonConvert.SerializeObject(p);
		}
	}
}