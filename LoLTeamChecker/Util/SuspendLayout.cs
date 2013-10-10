using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LoLTeamChecker.Util
{

	/// <summary>
	/// Suspends layout on construction, resumes on dispose.
	/// </summary>
	public class SuspendLayout : IDisposable
	{
		protected readonly Control control;
		public SuspendLayout(Control cont)
		{
			control = cont;
			control.SuspendLayout();
		}
		public void Dispose()
		{
			control.ResumeLayout();
		}
	}
}
