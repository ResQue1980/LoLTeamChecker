﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using NotMissing.Logging;

namespace LoLTeamChecker.Util
{
	public class ProcessMonitor
	{
		public class ProcessEventArgs : EventArgs
		{
			public ProcessEventArgs(Process process)
			{
				Process = process;
			}
			public Process Process { get; set; }
		}
		public string[] ProcessNames { get; protected set; }
		public Thread CheckThread { get; protected set; }
		public Process CurrentProcess { get; protected set; }

		/// <summary>
		/// Called when a new process is detected
		/// </summary>
		public event EventHandler<ProcessEventArgs> ProcessFound;

		public ProcessMonitor(string[] processnames)
		{
			ProcessNames = processnames;
			CheckThread = new Thread(CheckLoop) { IsBackground = true };
		}
		public void Start()
		{
			if (!CheckThread.IsAlive)
				CheckThread.Start();
		}
		public void Stop()
		{
			CheckThread = null;
		}

		protected void CheckLoop()
		{
			while (CheckThread != null)
			{
				if (CurrentProcess == null || CurrentProcess.HasExited)
				{
					for (int i = 0; i < ProcessNames.Length; i++)
					{
						CurrentProcess = Process.GetProcessesByName(ProcessNames[i]).FirstOrDefault();
						if (CurrentProcess != null)
						{
							if (ProcessFound != null)
								ProcessFound(this, new ProcessEventArgs(CurrentProcess));
							break;
						}
					}
				}
				Thread.Sleep(500);
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		~ProcessMonitor()
		{
			Dispose(false);
		}

		protected virtual void Dispose(bool dispose)
		{
			if (dispose)
			{
				CheckThread = null;
			}
		}
	}
}
