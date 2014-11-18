
namespace ProfileLogViewer.Models
{
	public class ProfileStat
	{
		public string MethodName { get; set; }

		public int TimeStamp { get; set; }

		public float AverageDuration { get; set; }

		public int TotalCount { get; set; }
	}
}