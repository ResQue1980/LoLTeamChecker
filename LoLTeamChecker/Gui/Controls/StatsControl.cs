﻿/*
copyright (C) 2011-2012 by high828@gmail.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LoLTeamChecker.Messages.Statistics;

namespace LoLTeamChecker.Gui.Controls
{
	public partial class StatsControl : UserControl
	{
		protected readonly Dictionary<Control, string> Prepends = new Dictionary<Control, string>();
		public StatsControl()
		{
			InitializeComponent();

			foreach (Control control in flowLayoutPanel1.Controls)
				Prepends.Add(control, control.Text);
		}

		public void SetStatSummary(PlayerStatSummary stat)
		{
			var values = new Dictionary<Control, string>()
			{
				//{GameType, stat.PlayerStatSummaryType},
				//{MaxElo, stat.MaxRating.ToString()},	
				//{Elo, stat.Rating.ToString()},
				{Wins, stat.Wins.ToString()},
				{Losses, stat.Losses.ToString()},
				{Leaves, stat.Leaves.ToString()}
			};
			foreach (var label in values)
			{
				label.Key.Text = Prepends[label.Key] + label.Value;
			}
		}
	}
}
