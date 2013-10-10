using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NotMissing.Logging;

namespace LoLTeamChecker.Util
{
	public class SimpleLogTimer : IDisposable
	{
		readonly Stopwatch m_Watch;
		string m_Message;
		Levels m_Level;

		protected SimpleLogTimer(Levels level, string message)
		{
			m_Level = level;
			m_Watch = Stopwatch.StartNew();
			m_Message = message;
		}

		public static SimpleLogTimer Start(Levels level, string message)
		{
			return new SimpleLogTimer(level, message);
		}
		public static SimpleLogTimer Start(string message)
		{
			return Start(Levels.Trace, message);
		}

		#region Dispose

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				m_Watch.Stop();
				StaticLogger.Log(m_Level, string.Format("[Timing] {0} in {1}ms", m_Message, m_Watch.ElapsedMilliseconds));
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~SimpleLogTimer()
		{
			Dispose(false);
		}

		#endregion
	}
}
