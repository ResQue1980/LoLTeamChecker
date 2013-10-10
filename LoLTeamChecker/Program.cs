using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Windows.Forms;
using LoLTeamChecker.Gui;
using LoLTeamChecker.Properties;
using LoLTeamChecker.Proxy;

namespace LoLTeamChecker
{
	internal static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main()
		{
			bool created;
			using (var mutex = new Mutex(true, "LoLTeamCheckerApp", out created))
			{
				if (created)
				{
					Application.EnableVisualStyles();
					Application.SetCompatibleTextRenderingDefault(false);
					Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
					Application.Run(new MainForm());
				}
				else
				{
					MessageBox.Show("LoLTeamChecker is already running");
				}
			}
		}
	}
}

