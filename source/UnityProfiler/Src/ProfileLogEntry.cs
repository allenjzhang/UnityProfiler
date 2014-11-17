using System;
using System.Diagnostics;

namespace UnityProfileLib
{
	public class ProfileLogEntry
	{
		public const int StartType = 0;
		public const int CompletedType = 1;

		private Stopwatch _stopwatch;

		public ProfileLogEntry()
		{
			_stopwatch = new Stopwatch();
			ActivityId = Trace.CorrelationManager.ActivityId;
			Type = StartType;
			Start();
		}

		public void Start()
		{
			if (Type != CompletedType)
			{
				StartTime = DateTime.UtcNow;
				ElapsedTimeMs = 0;
				_stopwatch.Start();
			}
			else
			{
				throw new InvalidOperationException("PerfLogEntry already completed.");
			}
		}

		public void Complete()
		{
			if (Type != CompletedType)
			{
				_stopwatch.Stop();
				ElapsedTimeMs = _stopwatch.ElapsedMilliseconds;
				Type = CompletedType;
			}
			else
			{
				throw new InvalidOperationException("PerfLogEntry already completed.");
			}
		}

		public new string ToString()
		{
			return string.Format("$${0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}", Type, ActivityId, NestDepth, CallingMethod, CurrentMethod, StartTime, ElapsedTimeMs);
		}

		public static ProfileLogEntry Parse(string data)
		{
			ProfileLogEntry p = new ProfileLogEntry();
			string[] fields = data.Split('\t');
			p.Type = int.Parse(fields[0]);
			p.ActivityId = Guid.Parse(fields[1]);
			p.NestDepth = int.Parse(fields[2]);
			p.CallingMethod = fields[3];
			p.CurrentMethod = fields[4];
			p.StartTime = DateTime.Parse(fields[5]);
			p.ElapsedTimeMs = long.Parse(fields[6]);

			return p;
		}

		public int Type { get; private set; }

		public Guid ActivityId { get; private set; }

		public int NestDepth { get; set; }

		public string Module { get; set; }
		public string CurrentMethod { get; set; }
		public string CallingMethod { get; set; }
		public DateTime StartTime { get; private set; }
		public long ElapsedTimeMs { get; private set; }
	}
}
