/*
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
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using LoLTeamChecker.Assets;
using LoLTeamChecker.Messages.League;
using LoLTeamChecker.Messages.GameLobby;
using LoLTeamChecker.Messages.GameLobby.Participants;
using LoLTeamChecker.Messages.Statistics;
using LoLTeamChecker.Messages.Summoner;
using LoLTeamChecker.Storage;
using System.Linq;
using NotMissing.Logging;

namespace LoLTeamChecker.Gui.Controls
{
	public partial class PlayerControl : UserControl
	{
        public TeamControl Parent { get; set; }
        public PlayerEntry Player { get; set; }
        public AggregatedStats PlayerStats { get; set; }
        public PictureBox[] favChampIcons;
        public ToolTip[] favChampKdas;
        public string DefaultGameTab { get; set; }
        static protected Dictionary<string, string> LeagueRegions = new Dictionary<string, string>
		{
			{"NA", "na"},
			{"EUW", "euw"},
			{"EUN", "eune"}
		};

		public PlayerControl()
		{
			SetStyle(ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);

			InitializeComponent();

			LoadingPicture.Visible = false;
            LevelLabel.Text = "";
            favChampIcons = new PictureBox[] { champIcon1, champIcon2, champIcon3, champIcon4, champIcon5 };
            favChampKdas = new ToolTip[] { new ToolTip(), new ToolTip(), new ToolTip(), new ToolTip(), new ToolTip() };
            foreach (ToolTip toolTip in favChampKdas)
            {
                toolTip.InitialDelay = 1;
                toolTip.ReshowDelay = 1;
            }
		}
		public PlayerControl(TeamControl parent)
			: this()
		{
			Parent = parent;
		}

		protected override void OnLoad(EventArgs e)
		{
			InfoTabs.ContextMenuStrip = ContextMenuStrip; //Set here because its virtual
			base.OnLoad(e);
		}

		const int BorderSize = 5;
		protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
			base.OnPaint(e);
			//var pen = new Pen(Player != null && Player.NoteColor.A != 0 ? Player.NoteColor : Color.Green, BorderSize);
			//e.Graphics.DrawRectangle(pen, BorderSize, BorderSize, Width - BorderSize * 2, Height - BorderSize * 2);
		}

		void SetName(string str)
		{
			NameLabel.Links.Clear();
			NameLabel.Text = str;
		}

		public void SetLoading(bool loading)
		{
			if (InvokeRequired)
			{
				BeginInvoke(new Action<bool>(SetLoading), loading);
				return;
			}
            if (loading)
            {
                InfoTabs.TabPages.Clear();
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(DefaultGameTab) && InfoTabs.TabPages[DefaultGameTab] != null)
                    InfoTabs.SelectTab(DefaultGameTab);
            }
			LoadingPicture.Visible = loading;
		}

		void SetTitle(PlayerEntry ply)
		{
			SetName(ply.Name);
			NameLabel.Links.Add(0, ply.Name.Length, Tuple.Create(ply.Id, ply.Name));
		}
		void SetTitle(Participant part)
		{
			var opart = part as ObfuscatedParticipant;
			var gpart = part as GameParticipant;
			var ppart = part as PlayerParticipant;
			if (gpart != null)
			{
				SetName(gpart.Name);
			}
			else if (opart != null)
			{
				SetName("Summoner " + opart.GameUniqueId);
			}
			else
			{
				SetName("Unknown");
			}

			if (ppart != null)
			{
				NameLabel.Links.Add(0, ppart.Name.Length, Tuple.Create(ppart.SummonerId, ppart.Name));
			}
		}

		void SetLevel(int level)
		{
			if (InvokeRequired)
			{
				Invoke(new Action<int>(SetLevel), level);
				return;
			}
            if (level != 30)
			    LevelLabel.Text = "" + (level != 0 ? Convert.ToString(level) : "?");
		}

		void RemoveAll(Predicate<TabPage> find)
		{
			for (int i = 0; i < InfoTabs.TabPages.Count; i++)
			{
				if (find(InfoTabs.TabPages[i]))
					InfoTabs.TabPages.RemoveAt(i--);
			}
		}

		public void SetEmpty()
		{
			if (InvokeRequired)
			{
				Invoke(new Action(SetEmpty));
				return;
			}
			Player = null;
            ClearStats();
			InfoTabs.TabPages.Clear();
			SetLevel(0);
			SetTeam(0);
			Invalidate(); //Force the border to redraw.
		}

        public void AddTab(TabPage page)
        {
            InfoTabs.TabPages.Add(page);
        }

		public void SetPlayer(PlayerEntry plr)
		{
			if (InvokeRequired)
			{
				Invoke(new Action<PlayerEntry>(SetPlayer), plr);
				return;
			}
			Player = plr;
			SetTitle(plr);

			RemoveAll(p => (p.Tag as string) == "Note");

			if (!string.IsNullOrWhiteSpace(plr.Note))
			{
				SuspendLayout();

				var tab = new TabPage("Note")
				{
					Tag = "Note",
					BackColor = this.BackColor
				};
				var lbl = new Label
				{
					Font = new Font(Font.FontFamily, Font.SizeInPoints, FontStyle.Bold),
					Text = plr.Note
				};
				tab.Controls.Add(lbl);
                AddTab(tab);

				ResumeLayout();
			}

			Invalidate(); //Forces the color change
		}
		public void SetParticipant(Participant part)
		{
			if (InvokeRequired)
			{
				Invoke(new Action<Participant>(SetParticipant), part);
				return;
			}
			Player = null;
			SetTitle(part);
		}

        public void SetStats(PublicSummoner summoner, PlayerLifetimeStats stats)
        {
            if (summoner == null || stats == null)
                return;

            if (InvokeRequired)
            {
                Invoke(new Action<PublicSummoner, PlayerLifetimeStats>(SetStats), summoner, stats);
                return;
            }

            SetLevel(summoner.SummonerLevel);
            RemoveAll(p => (p.Tag as string) == "Stats");

            foreach (var stat in stats.PlayerStatSummaries.PlayerStatSummarySet)
            {
                if (!stat.PlayerStatSummaryType.Contains("RankedSolo5x5"))
                    continue;
                var sc = new StatsControl { Dock = DockStyle.Fill, Tag = "Stats" };
                sc.SetStatSummary(stat);

                var tab = new TabPage(MinifyStatType(stat.PlayerStatSummaryType))
                {
                    BackColor = this.BackColor,
                    Tag = "Stats"
                };
                tab.Controls.Add(sc);
                AddTab(tab);
            }
        }

        public void ClearStats()
        {
            /*
            champKda.Text = "?.??";
            champGames.Text = "????";
            champPercentWon.Text = "??.?%";
            currChampIcon.Image = null;
            totalPercentWon.Text = "??.?%";
            division.Text = "????";
            totalGames.Text = "????";
            totalKda.Text = "?.??";
            */

            champKda.Text = "";
            champGames.Text = "";
            champPercentWon.Text = "";
            currChampIcon.Image = null;
            totalPercentWon.Text = "";
            division.Text = "";
            totalGames.Text = "";
            totalKda.Text = "";
            for (int i = 0; i < 5; i++)
            {
                favChampIcons[i].Image = null;
                favChampKdas[i].SetToolTip(favChampIcons[i], "");
            }
        }
        public void SetRankedStats(PublicSummoner summoner, AggregatedStats stats)
        {
            if (summoner == null || stats == null)
                return;

            if (InvokeRequired)
            {
                Invoke(new Action<PublicSummoner, AggregatedStats>(SetRankedStats), summoner, stats);
                return;
            }
            PlayerStats = stats;
            int kills = 0;
            int assists = 0;
            int deaths = 0;
            Dictionary<int, int> favChamps = new Dictionary<int, int>();
            foreach (var stat in stats.LifetimeStatistics)
            {
                if (stat.ChampionID == 0)
                {
                    if (stat.StatType.Contains("TOTAL_CHAMPION_KILLS"))
                    {
                        kills = Convert.ToInt32(stat.Value);
                    }
                    if (stat.StatType.Contains("TOTAL_ASSISTS"))
                    {
                        assists = Convert.ToInt32(stat.Value);
                    }
                    if (stat.StatType.Contains("TOTAL_DEATHS_PER_SESSION"))
                    {
                        deaths = Convert.ToInt32(stat.Value);
                    }
                    if (stat.StatType.Contains("TOTAL_SESSIONS_PLAYED"))
                    {
                        totalGames.Text = Convert.ToInt32(stat.Value).ToString();
                    }
                    if (stat.StatType.Contains("TOTAL_SESSIONS_WON"))
                    {
                        totalPercentWon.Text = Convert.ToInt32(stat.Value).ToString();
                    }
                }
                else
                {
                    if (stat.StatType.Contains("TOTAL_SESSIONS_PLAYED"))
                    {
                        if (favChamps.Count() <= 5)
                        {
                            favChamps.Add(stat.ChampionID, Convert.ToInt32(stat.Value));
                        }
                        else
                        {
                            var min = favChamps.OrderBy(kvp => kvp.Value).First();
                            var minKey = min.Key;
                            var minValue = min.Value;
                            if (Convert.ToInt32(stat.Value) > minValue)
                            {
                                favChamps.Remove(minKey);
                                favChamps.Add(stat.ChampionID, Convert.ToInt32(stat.Value));
                            }
                        }
                    }
                }
            }
            totalPercentWon.Text = ((100.0f * (Convert.ToInt32(totalPercentWon.Text)) / Convert.ToDouble(totalGames.Text)) + 0.0001).ToString().Substring(0, 4) + "%";
            if (Convert.ToDouble(totalPercentWon.Text.Substring(0, 4)) > 50.0f)
                totalPercentWon.ForeColor = Color.DarkGreen;
            else
                totalPercentWon.ForeColor = Color.DarkRed;
            float totalKdaCalc = (kills + assists) / (float)deaths;
            totalKda.Text = totalKdaCalc.ToString().Substring(0, 4);
            if (totalKdaCalc < 2.25)
            {
                totalKda.ForeColor = Color.DarkRed;
            }
            else if (totalKdaCalc < 3)
            {
                totalKda.ForeColor = Color.DarkOrange;
            }
            else
            {
                totalKda.ForeColor = Color.DarkGreen;
            }
            for (int i = 0; i < 5; i++)
            {
                var favChamp = favChamps.OrderBy(kvp => kvp.Value).Last();
                var name = ChampNames.GetOrDefault(favChamp.Key);
                favChampIcons[i].Image = ChampIcons.Get(favChamp.Key);
                favChamps.Remove(favChamp.Key);
                foreach (var stat in stats.LifetimeStatistics)
                {
                    if (favChamp.Key == stat.ChampionID)
                    {
                        if (stat.StatType.Contains("TOTAL_CHAMPION_KILLS"))
                        {
                            kills = Convert.ToInt32(stat.Value);
                        }
                        if (stat.StatType.Contains("TOTAL_ASSISTS"))
                        {
                            assists = Convert.ToInt32(stat.Value);
                        }
                        if (stat.StatType.Contains("TOTAL_DEATHS_PER_SESSION"))
                        {
                            deaths = Convert.ToInt32(stat.Value);
                        }
                    }
                }
                float tempChampKda = (kills + assists) / (float)deaths;
                favChampKdas[i].SetToolTip(favChampIcons[i], tempChampKda.ToString());
            }
        }

        public void SetCurrentChamp(Int64 champId)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<Int64>(SetCurrentChamp), champId);
                return;
            }
            Console.WriteLine("champ" + champId);
            if (champId == 0)
                return;
            currChampIcon.Image = ChampIcons.Get(Convert.ToInt32(champId));
            int champKills = 0;
            int champAssists = 0;
            int champDeaths = 0;
            bool playedChamp = false;
            if (PlayerStats != null)
            {
                foreach (var stat in PlayerStats.LifetimeStatistics)
                {
                    if (stat.ChampionID == champId)
                    {
                        playedChamp = true;
                        if (stat.StatType.Contains("TOTAL_CHAMPION_KILLS"))
                        {
                            champKills = Convert.ToInt32(stat.Value);
                        }
                        if (stat.StatType.Contains("TOTAL_ASSISTS"))
                        {
                            champAssists = Convert.ToInt32(stat.Value);
                        }
                        if (stat.StatType.Contains("TOTAL_DEATHS_PER_SESSION"))
                        {
                            champDeaths = Convert.ToInt32(stat.Value);
                        }
                        if (stat.StatType.Contains("TOTAL_SESSIONS_PLAYED"))
                        {
                            champGames.Text = Convert.ToInt32(stat.Value).ToString();
                        }
                        if (stat.StatType.Contains("TOTAL_SESSIONS_WON"))
                        {
                            champPercentWon.Text = Convert.ToInt32(stat.Value).ToString();
                        }
                    }
                }
            }
            if (playedChamp)
            {
                champPercentWon.Text = ((100.0f * (Convert.ToInt32(champPercentWon.Text)) / Convert.ToDouble(champGames.Text)) + 0.0001).ToString().Substring(0, 4) + "%";
                if (Convert.ToDouble(champPercentWon.Text.Substring(0, 4)) > 50.0f)
                    champPercentWon.ForeColor = Color.DarkGreen;
                else
                    champPercentWon.ForeColor = Color.DarkRed;
                float champKdaCalc = (champKills + champAssists) / (float)champDeaths;
                champKda.Text = (champKdaCalc+0.0001).ToString().Substring(0, 4);
                if (champKdaCalc < 2.25)
                {
                    champKda.ForeColor = Color.DarkRed;
                }
                else if (champKdaCalc < 3)
                {
                    champKda.ForeColor = Color.DarkOrange;
                }
                else
                {
                    champKda.ForeColor = Color.DarkGreen;
                }
            } else {
                champKda.Text = "?.??";
                champPercentWon.Text = "??.?%";
                champGames.Text = "0";
            }
        }

        public int ToInt(String romanNumeral)
        {
            switch (romanNumeral)
            {
                case "I":
                    return 1;
                case "II":
                    return 2;
                case "III":
                    return 3;
                case "IV":
                    return 4;
                case "V":
                    return 5;
                default:
                    return 0;
            }
        }
        public void SetLeagueInfo(PublicSummoner summoner, SummonerLeaguesDTO leagueInfo)
        {
            if (summoner == null || leagueInfo == null)
                return;

            if (InvokeRequired)
            {
                Invoke(new Action<PublicSummoner, SummonerLeaguesDTO>(SetLeagueInfo), summoner, leagueInfo);
                return;
            }
            foreach (var entry in leagueInfo.summonerLeagues)
            {
                foreach (var entry2 in entry.entries)
                {
                    if (entry2.PlayerOrTeamName.Equals(summoner.Name))
                    {
                        division.Text = entry2.Tier + "." + ToInt(entry2.Rank) + "." + entry2.LeaguePoints;
                        return;
                    }
                }
            }
        }
        public void onImageMouseOver(object sender, EventArgs e, PictureBox pic, float champKda)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(pic, champKda.ToString());            
        }
		public static string MinifyStatType(string name)
		{
			if (name == null)
				return null;

			var replacements = new Dictionary<string, string>
			{
				{"Ranked", "R"},
				{"Premade", "P"},
				{"Solo", "S"},
				{"Unranked", "UR"},
			};
			foreach (var kv in replacements)
				name = name.Replace(kv.Key, kv.Value);
			return name;
		}

		public void SetChamps(ChampionStatInfoList champs)
		{
			if (champs == null || champs.Count < 1)
				return;

			if (InvokeRequired)
			{
				Invoke(new Action<ChampionStatInfoList>(SetChamps), champs);
				return;
			}

			RemoveAll(p => (p.Tag as string) == "Champs");

			var layout = new TableLayoutPanel();
			layout.Dock = DockStyle.Fill;
			foreach (var champ in champs)
			{
				if (champ.ChampionId == 0)
					continue;

				var lbl = new Label
				{
					Font = new Font("Bitstream Vera Sans Mono", 8.25F, FontStyle.Bold),
					AutoSize = true,
					Text = string.Format("{0} ({1})", ChampNames.Get(champ.ChampionId), champ.TotalGamesPlayed)
				};
				layout.Controls.Add(lbl);
			}

			var tab = new TabPage("Champs")
			{
				BackColor = this.BackColor,
				Tag = "Champs"
			};
			tab.Controls.Add(layout);
            AddTab(tab);
		}
		static Label CreateLabel(string text)
		{
			var label = new Label
			{
				Font = new Font("Bitstream Vera Sans Mono", 8.25F, FontStyle.Bold),
				Text = text,
				TextAlign = ContentAlignment.MiddleLeft,
				Margin = Padding.Empty,
				AutoSize = false
			};
			label.Width = label.PreferredWidth;
			label.Height = 20;
			return label;
		}

		static PictureBox CreateSpellPicture(int id)
		{
			return new PictureBox
			{
				Image = SpellIcons.Get(id),
				Margin = Padding.Empty,
				SizeMode = PictureBoxSizeMode.StretchImage,
				Size = new Size(20, 20)
			};
		}

		static Color GetKdrColor(int kills, int deaths)
		{
			if (deaths == 0)
				deaths = 1;

			double ratio = (double)kills / deaths;
			ratio = Math.Min(ratio, 1);

			var orange = Color.Orange;
			var red = Color.Red;
			var green = Color.Green;

			Color top;
			Color bot;
			if (ratio < 0.5d)
			{
				top = red;
				bot = orange;
				ratio *= 2;
			}
			else
			{
				top = orange;
				bot = green;
				ratio -= 0.5d;
				ratio *= 2;
			}
			return Interpolate(top, bot, ratio);
		}

		static byte Interpolate(byte from, byte to, double step)
		{
			return (byte)(from + (to - from) * step);

		}
		static Color Interpolate(Color from, Color to, double step)
		{
			return Color.FromArgb(
				Interpolate(from.A, to.A, step),
				Interpolate(from.R, to.R, step),
				Interpolate(from.G, to.G, step),
				Interpolate(from.B, to.B, step)
			);
		}

		static void kdrtest()
		{
			for (var i = 0; i < 10; i++)
			{
				var color = GetKdrColor(i, 10);
				Debug.WriteLine(string.Format("Color.FromArgb({0}, {1}, {2}, {3})", color.A, color.R, color.G, color.B));
			}
		}

		public void SetTeam(int num)
		{
			if (InvokeRequired)
			{
				Invoke(new Action<int>(SetTeam), num);
				return;
			}

			if (num == 0)
			{
				TeamLabel.Text = "";
			}
			else
			{

				TeamLabel.Text = string.Format("{0}", num);
			}
		}

		private void NameLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if (e.Link.LinkData == null)
				return;
			var plr = (Tuple<Int64, string>)e.Link.LinkData;
			string region;
			if (!LeagueRegions.TryGetValue(MainSettings.Instance.Region, out region))
			{
				StaticLogger.Info("Region " + MainSettings.Instance.Region + " is not supported");
				return;
			}

			string url = null;
			if (e.Button == MouseButtons.Left)
			{
				url = string.Format("http://www.lolking.net/summoner/{0}/{1}", region, plr.Item1);
			}
			else if (e.Button == MouseButtons.Middle)
			{
                url = null;
			}

			if (url != null)
			{
				Process.Start(url);
				e.Link.Visited = true;
			}
		}
        public void UpdateAcceptedStatus(int i)
        {
            switch (i)
            {
                case 0:
                    acceptedStatus.Image = Properties.Resources.circle_yellow;
                    break;
                case 1:
                    acceptedStatus.Image = Properties.Resources.circle_green;
                    break;
                case 2:
                    acceptedStatus.Image = Properties.Resources.circle_red;
                    break;
                default:
                    break;
            }
        }
	}
}
