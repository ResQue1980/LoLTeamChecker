using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FluorineFx;
using FluorineFx.AMF3;
using FluorineFx.IO;
using FluorineFx.Messaging.Messages;
using FluorineFx.Messaging.Rtmp.Event;
using LoLTeamChecker.Gui.Controls;
using LoLTeamChecker.Messages.Account;
using LoLTeamChecker.Messages.Champion;
using LoLTeamChecker.Messages.Commands;
using LoLTeamChecker.Messages.GameLobby;
using LoLTeamChecker.Messages.GameLobby.Participants;
using LoLTeamChecker.Messages.GameStats;
using LoLTeamChecker.Messages.Readers;
using LoLTeamChecker.Messages.Statistics;
using LoLTeamChecker.Messages.Summoner;
using LoLTeamChecker.Properties;
using LoLTeamChecker.Proxy;
using LoLTeamChecker.Storage;
using LoLTeamChecker.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NotMissing.Logging;

namespace LoLTeamChecker.Gui
{
	public partial class MainForm : Form
	{
		public static readonly string Version = AssemblyAttributes.FileVersion + AssemblyAttributes.Configuration;
		const string SettingsFile = "settings.json";

		readonly Dictionary<string, Icon> Icons;
        readonly Dictionary<string, CertificateHolder> Certificates;
		readonly List<PlayerCache> PlayersCache = new List<PlayerCache>();
		readonly ProcessQueue<string> TrackingQueue = new ProcessQueue<string>();
		readonly ProcessMonitor launcher = new ProcessMonitor(new[] { "LoLLauncher" });

		RtmpsProxyHost Connection;
		MessageReader Reader;
		CertificateInstaller Installer;
		ProcessInjector Injector;
		GameDTO CurrentGame;
		List<ChampionDTO> Champions;
		SummonerData SelfSummoner;

		MainSettings Settings { get { return MainSettings.Instance; } }

		public MainForm()
		{
            String regionFromFileName = System.AppDomain.CurrentDomain.FriendlyName;
            Console.WriteLine(regionFromFileName);
            regionFromFileName = regionFromFileName.Replace("LoLTeamChecker", "");
            Console.WriteLine(regionFromFileName);
            regionFromFileName = regionFromFileName.Replace(".exe", "");
            Console.WriteLine(regionFromFileName);
            if (regionFromFileName.Length != 0)
                Settings.Region = regionFromFileName;

            InitializeComponent();
            Logger.Instance.Register(new DefaultListener(Levels.All, OnLog));
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.ThreadException += Application_ThreadException;
            //StaticLogger.Info(string.Format("Version {0}", Version));

            //Settings.Load(SettingsFile);

            Icons = new Dictionary<string, Icon>
			{
                {"Red",  Icon.FromHandle(Resources.circle_red.GetHicon())},
				{"Yellow",  Icon.FromHandle(Resources.circle_yellow.GetHicon())},
				{"Green",  Icon.FromHandle(Resources.circle_green.GetHicon())},
			};

            Certificates = LoadCertificates();
            if (Certificates.Count < 1)
            {
                MessageBox.Show("Unable to load any certificates");
                Application.Exit();
                return;
            }
            var cert = Certificates.FirstOrDefault(kv => kv.Key == Settings.Region).Value;
            if (cert == null)
                cert = Certificates.First().Value;

            Injector = new ProcessInjector("lolclient");
            Connection = new RtmpsProxyHost(2099, cert.Domain, 2099, cert.Certificate);
            Reader = new MessageReader(Connection);

            Connection.Connected += Connection_Connected;
            Injector.Injected += Injector_Injected;
            Reader.ObjectRead += Reader_ObjectRead;

            Connection.CallResult += Connection_Call;
            Connection.Notify += Connection_Notify;

            /*
			foreach (var kv in Certificates)
				RegionList.Items.Add(kv.Key);
			int idx = RegionList.Items.IndexOf(Settings.Region);
			RegionList.SelectedIndex = idx != -1 ? idx : 0;	 //This ends up calling UpdateRegion so no reason to initialize the connection here.
            */
            Installer = new CertificateInstaller(Certificates.Select(c => c.Value.Certificate).ToArray());
            Installer.Uninstall();
            if (!Installer.IsInstalled)
            {
                Installer.Install();
            }

            TrackingQueue.Process += TrackingQueue_Process;
            launcher.ProcessFound += launcher_ProcessFound;

            StaticLogger.Info("Startup Completed");
		}
        Dictionary<string, CertificateHolder> LoadCertificates()
        {
            var ret = new Dictionary<string, CertificateHolder>();
            ret["BR"] = new CertificateHolder("prod.br.lol.riotgames.com", LoLTeamChecker.Certificates.ByteCertificates.brCert);
            ret["EUN"] = new CertificateHolder("prod.eun1.lol.riotgames.com", LoLTeamChecker.Certificates.ByteCertificates.eunCert);
            ret["EUW"] = new CertificateHolder("prod.eu.lol.riotgames.com", LoLTeamChecker.Certificates.ByteCertificates.euwCert);
            ret["KR"] = new CertificateHolder("prod.kr.lol.riotgames.com", LoLTeamChecker.Certificates.ByteCertificates.krCert);
            ret["LAN"] = new CertificateHolder("prod.la1.lol.riotgames.com", LoLTeamChecker.Certificates.ByteCertificates.lanCert);
            ret["LAS"] = new CertificateHolder("prod.la2.lol.riotgames.com", LoLTeamChecker.Certificates.ByteCertificates.lasCert);
            ret["NA"] = new CertificateHolder("prod.na1.lol.riotgames.com", LoLTeamChecker.Certificates.ByteCertificates.naCert);
            ret["OCE"] = new CertificateHolder("prod.oc1.lol.riotgames.com", LoLTeamChecker.Certificates.ByteCertificates.oceCert);
            ret["PBE"] = new CertificateHolder("prod.pbe1.lol.riotgames.com", LoLTeamChecker.Certificates.ByteCertificates.pbeCert);
            ret["PH"] = new CertificateHolder("prodph.lol.garenanow.com", LoLTeamChecker.Certificates.ByteCertificates.phCert);
            ret["RU"] = new CertificateHolder("prod.ru.lol.riotgames.com", LoLTeamChecker.Certificates.ByteCertificates.ruCert);
            ret["SG"] = new CertificateHolder("prod.lol.garenanow.com", LoLTeamChecker.Certificates.ByteCertificates.sgCert);
            ret["TH"] = new CertificateHolder("prodth.lol.garenanow.com", LoLTeamChecker.Certificates.ByteCertificates.thCert);
            ret["TR"] = new CertificateHolder("prod.tr.lol.riotgames.com", LoLTeamChecker.Certificates.ByteCertificates.trCert);
            ret["TW"] = new CertificateHolder("prodtw.lol.garenanow.com", LoLTeamChecker.Certificates.ByteCertificates.twCert);
            ret["VN"] = new CertificateHolder("prodvn.lol.garenanow.com", LoLTeamChecker.Certificates.ByteCertificates.vnCert);
            return ret;
        }

		void launcher_ProcessFound(object sender, ProcessMonitor.ProcessEventArgs e)
		{
            return;
			try
			{
				if (!Settings.DeleteLeaveBuster)
					return;

				var dir = Path.GetDirectoryName(e.Process.MainModule.FileName);
				if (dir == null)
				{
					StaticLogger.Warning("Launcher module not found");
					return;
				}

				var needle = "\\RADS\\";
				var i = dir.LastIndexOf(needle, StringComparison.InvariantCulture);
				if (i == -1)
				{
					StaticLogger.Warning("Launcher Rads not found");
					return;
				}

				dir = dir.Remove(i + needle.Length);
				dir = Path.Combine(dir, "projects\\lol_air_client\\releases");

				if (!Directory.Exists(dir))
				{
					StaticLogger.Warning("lol_air_client directory not found");
					return;
				}

				foreach (var ver in new DirectoryInfo(dir).GetDirectories())
				{
					var filename = Path.Combine(ver.FullName, "deploy\\preferences\\global\\global.properties");
					if (!File.Exists(filename))
					{
						StaticLogger.Warning(filename + " not found");
						continue;
					}

					ASObject obj = null;
					using (var amf = new AMFReader(File.OpenRead(filename)))
					{
						try
						{
							obj = amf.ReadAMF3Data() as ASObject;
							if (obj == null)
							{
								StaticLogger.Warning("Failed to read " + filename);
								continue;
							}
						}
						catch (Exception ex)
						{
                            StaticLogger.Warning("LeaverBuster: Unable to read global.properties '" + filename + "'");
							continue;
						}
					}
					object leaver;
					object locale;
					if ((obj.TryGetValue("leaverData", out leaver) && leaver != null) ||
						(obj.TryGetValue("localeData", out locale) && locale != null))
					{
						obj["leaverData"] = null;
						obj["localeData"] = null;
						using (var amf = new AMFWriter(File.Open(filename, FileMode.Create, FileAccess.Write)))
						{
							try
							{
								amf.WriteAMF3Data(obj);
								StaticLogger.Info("Removed leaverData/localeData from global.properties");
							}
							catch (Exception ex)
							{
                                StaticLogger.Warning("LeaverBuster: Unable to write global.properties '" + filename + "'");
								continue;
							}
						}
					}
					else
					{
						StaticLogger.Info("leaverData/localeData already removed from global.properties");
					}
				}
			}
			catch (Exception ex)
			{
				StaticLogger.Error(ex);
			}
		}

		void TrackingQueue_Process(object sender, ProcessQueueEventArgs<string> e)
		{
			try
			{
				var hr = (HttpWebRequest)WebRequest.Create("http://bit.ly/unCoIY");
				hr.ServicePoint.Expect100Continue = false;
				hr.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:8.0) Gecko/20100101 Firefox/8.0";
				hr.Referer = string.Format("http://LoLTeamChecker-{0}-app.org/{1}", Version, e.Item);
				hr.AllowAutoRedirect = false;
				using (var resp = (HttpWebResponse)hr.GetResponse())
				{
				}
			}
			catch (WebException we)
			{
				StaticLogger.Warning(we);
			}
			catch (Exception ex)
			{
				StaticLogger.Warning(ex);
			}
		}

		void Injector_Injected(object sender, EventArgs e)
		{
			if (Created)
				BeginInvoke(new Action(UpdateIcon));
		}

		//Allows for FInvoke(delegate {});
		void FInvoke(MethodInvoker inv)
		{
			BeginInvoke(inv);
		}

		void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
		{
			LogException(e.Exception, true);
		}

		void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			LogException((Exception)e.ExceptionObject, !e.IsTerminating);
			//Bypass the queue and log it now if we are terminating.
			if (e.IsTerminating)
				TrackingQueue_Process(this, new ProcessQueueEventArgs<string> { Item = string.Format("error/{0}", Parse.ToBase64(e.ExceptionObject.ToString())) });
		}

		void LogException(Exception ex, bool track)
		{
            var log = string.Format(
               "[{0}] {1} ({2:MM/dd/yyyy HH:mm:ss.fff})",
               Levels.Fatal.ToString().ToUpper(),
               string.Format("{0} [{1}]", ex.Message, Parse.ToBase64(ex.ToString())),
               DateTime.UtcNow);

			if (track)
				TrackingQueue.Enqueue(string.Format("error/{0}", Parse.ToBase64(ex.ToString())));
		}

		void Log(Levels level, object obj)
		{
			var log = string.Format(
					"[{0}] {1} ({2:MM/dd/yyyy HH:mm:ss.fff})",
					level.ToString().ToUpper(),
					obj,
					DateTime.UtcNow);
		}

        void OnLog(Levels level, object obj)
        {
            if (level == Levels.Trace && !Settings.TraceLog)
                return;
            if (level == Levels.Debug && !Settings.DebugLog)
                return;

            if (level == Levels.Error && obj is Exception)
            {
                TrackingQueue.Enqueue(string.Format("error/{0}", Parse.ToBase64(obj.ToString())));
            }

            if (obj is Exception)
                Log(level, string.Format("{0} [{1}]", ((Exception)obj).Message, Parse.ToBase64(obj.ToString())));
            else
                Log(level, obj);
        }


		void UpdateIcon()
		{
			if (!Injector.IsInjected)
				Icon = Icons["Red"];
			else if (Connection != null && Connection.IsConnected)
				Icon = Icons["Green"];
			else
				Icon = Icons["Yellow"];
		}

		void Connection_Connected(object sender, EventArgs e)
		{
			if (Created)
				BeginInvoke(new Action(UpdateIcon));
		}

		void Reader_ObjectRead(object obj)
		{
			if (obj is GameDTO)
				UpdateLists((GameDTO)obj);
			else if (obj is EndOfGameStats)
				ClearCache(); //clear the player cache after each match.
			else if (obj is List<ChampionDTO>)
				Champions = (List<ChampionDTO>)obj;
			else if (obj is LoginDataPacket)
				SelfSummoner = ((LoginDataPacket)obj).AllSummonerData.Summoner;
		}

		public void ClearCache()
		{
			lock (PlayersCache)
			{
				PlayersCache.Clear();
			}
		}

		public void UpdateLists(GameDTO lobby)
		{
			if (InvokeRequired)
			{
				BeginInvoke(new Action<GameDTO>(UpdateLists), lobby);
				return;
			}
			var teamids = new List<Int64>();
			var teams = new List<TeamParticipants> { lobby.TeamOne, lobby.TeamTwo };
			var lists = new List<TeamControl> { teamControl1, teamControl2 };
            
            for (int i = 0; i < lists.Count; i++)
			{
				var list = lists[i];
				var team = teams[i];
				for (int o = 0; o < list.Players.Count; o++)
				{
					if (o < team.Count)
					{
						PlayerControl plycontrol = list.Players[o] as PlayerControl;
                        plycontrol.UpdateAcceptedStatus(Convert.ToInt32(lobby.StatusOfParticipants.Substring(i*5+o, 1)));
						//plycontrol.acce
                    }
				}
			}
            /*var cmd = new PlayerCommands(Connection);
            GameDTO tempGame = cmd.GetGame(Convert.ToInt32(lobby.Id));
            Console.WriteLine("test");
            Console.WriteLine(tempGame.Name);
            for (int q = 0; q < tempGame.TeamOne.Count; q++)
            {
                Console.WriteLine((tempGame.TeamTwo[q] as PlayerParticipant).InternalName);
            }
            for (int q = 0; q < tempGame.TeamTwo.Count; q++)
            {
                Console.WriteLine((tempGame.TeamTwo[q] as PlayerParticipant).InternalName);
            }*/

            // correlating names for ARAM
            if (lobby.GameMode.Equals("ARAM"))
            {
                int blankCount = 0;
                int teamIdx = 1;
                if (SelfSummoner != null && lobby.TeamOne.Find(p => p is PlayerParticipant && ((PlayerParticipant)p).SummonerId == SelfSummoner.SummonerId) != null)
                {
                    teamIdx = 0;
                }
                for (int i = 0; i < lobby.PlayerChampionSelections.Count; i++)
                {
                    if ((lobby.PlayerChampionSelections[i].Spell1Id == 0.0))
                    {
                        PlayerParticipant player = new PlayerParticipant();
                        player.AccountId = 1;
                        player.InternalName = lobby.PlayerChampionSelections[i].SummonerInternalName;
                        player.Name = lobby.PlayerChampionSelections[i].SummonerInternalName;
                        player.PickMode = 1;
                        player.PickTurn = 0;
                        player.ProfileIconId = 2;
                        player.QueueRating = 18;
                        player.SummonerId = 1;
                        player.TeamParticipantId = 0;// (teams[1 - teamIdx][blankCount] as PlayerParticipant).TeamParticipantId;
                        teams[1 - teamIdx][blankCount++] = player;
                    }
                }
            }

            if (CurrentGame != null)
            {
                var oldteams = new List<TeamParticipants> { CurrentGame.TeamOne, CurrentGame.TeamTwo };
                var newteams = new List<TeamParticipants> { lobby.TeamOne, lobby.TeamTwo };
                for (int i = 0; i < lobby.PlayerChampionSelections.Count; i++)
                {
                    Console.WriteLine("i:" + i + " : " + lobby.PlayerChampionSelections[i].SummonerInternalName);
                    for (int j = 0; j < lobby.TeamOne.Count; j++)
                    {
                        Console.WriteLine("j:" + j + " : " + (lobby.TeamOne[j] as PlayerParticipant).InternalName);
                        if (lobby.PlayerChampionSelections[i].SummonerInternalName.Equals((lobby.TeamOne[j] as PlayerParticipant).InternalName))
                        {
                            teamControl1[j].SetCurrentChamp(lobby.PlayerChampionSelections[i].ChampionId);
                            break;
                        }
                        if (lobby.PlayerChampionSelections[i].SummonerInternalName.Equals((lobby.TeamTwo[j] as PlayerParticipant).InternalName))
                        {
                            teamControl2[j].SetCurrentChamp(lobby.PlayerChampionSelections[i].ChampionId);
                            break;
                        }
                    }
                }
            }
			if (CurrentGame == null || CurrentGame.Id != lobby.Id)
			{
				CurrentGame = lobby;
			}
			else
			{
				//Check if the teams are the same.
				//If they are the same that means nothing has changed and we can return.
				var oldteams = new List<TeamParticipants> { CurrentGame.TeamOne, CurrentGame.TeamTwo };
				var newteams = new List<TeamParticipants> { lobby.TeamOne, lobby.TeamTwo };
                bool same = true;
                for (int i = 0; i < oldteams.Count && i < newteams.Count; i++)
                {
                    if (!oldteams[i].SequenceEqual(newteams[i]))
                    {
                        same = false;
                        break;
                    }
                }
                if (same)
					return;
                				
				CurrentGame = lobby;
			}
            
            //Load the opposite team first. Not currently useful with the way things are loaded.
            /*if (SelfSummoner != null && lobby.TeamOne.Find(p => p is PlayerParticipant && ((PlayerParticipant)p).SummonerId == SelfSummoner.SummonerId) != null)
            {
                teams.Reverse();
                lists.Reverse();
            }*/

			for (int i = 0; i < lists.Count; i++)
			{
				var list = lists[i];
				var team = teams[i];

				if (team == null)
				{
					list.Visible = false;
					continue;
				}
				list.Visible = true;

				for (int o = 0; o < list.Players.Count; o++)
				{
					list.Players[o].Tag = null;
					if (o < team.Count)
					{
						var plycontrol = list.Players[o];
						plycontrol.Visible = true;
						var ply = team[o] as PlayerParticipant;
						if (ply != null)// && ply.SummonerId != 0)
                        {
							//Used for synchronization.
							//Ensures that when the loaded data comes back to the UI thread that its what we wanted.
							plycontrol.Tag = ply;

							plycontrol.SetLoading(true);
							plycontrol.SetEmpty();
							plycontrol.SetParticipant(ply);
                            Task.Factory.StartNew(() => LoadPlayer(ply, plycontrol), TaskCreationOptions.LongRunning);
						}
						else
						{
							plycontrol.SetEmpty();
							plycontrol.SetParticipant(team[o]);
						}

						if (ply != null)
						{
							if (ply.TeamParticipantId != 0)
							{
								var idx = teamids.FindIndex(t => t == ply.TeamParticipantId);
								if (idx == -1)
								{
									idx = teamids.Count;
									teamids.Add(ply.TeamParticipantId);
								}
								plycontrol.SetTeam(idx + 1);
							}
						}
					}
					else
					{
						list.Players[o].Visible = false;
						list.Players[o].SetEmpty();
					}
				}
			}
		}

		void LoadPlayerUIFinish(PlayerCache ply, PlayerControl control)
		{
			FInvoke(delegate
			{
				//Now that we are back on the UI thread.
                //Lets check to make sure the control still wants this specific data.
                //if (control.Tag == null || ((PlayerParticipant)control.Tag).SummonerId != ply.Summoner.SummonerId)
                if (control.Tag == null || ((PlayerParticipant)control.Tag).InternalName != ply.Summoner.InternalName)
                    return;
                control.SetRankedStats(ply.Summoner, ply.RankedStats);
                control.SetLeagueInfo(ply.Summoner, ply.LeagueInfo);
                control.DefaultGameTab = Settings.DefaultGameTab;
				control.SetPlayer(ply.Player);
                /*if (ply.Stats != null)
				    control.SetStats(ply.Summoner, ply.Stats);
                if (ply.RecentChamps != null)
				    control.SetChamps(ply.RecentChamps);
                if (ply.Games != null)
				    control.SetGames(ply.Games);*/
				control.SetLoading(false);

			});
		}

		/// <summary>
		/// Query and cache player data
		/// </summary>
		/// <param name="player">Player to load</param>
		/// <param name="control">Control to update</param>
		void LoadPlayer(PlayerParticipant player, PlayerControl control)
		{
			PlayerCache existing;
			var ply = new PlayerCache();
			try
            {
				lock (PlayersCache)
				{
					//Clear the cache every 50000 players to prevent crashing afk lobbies.
					if (PlayersCache.Count > 50000)
						PlayersCache.Clear();

					//Does the player already exist in the cache?
					if ((existing = PlayersCache.Find(p => p.Player != null && p.Player.Id == player.SummonerId)) == null)
					{
						PlayersCache.Add(ply);
					}
				}

				//If another thread is loading the player data, lets wait for it to finish and use its data.
				if (existing != null)
				{
					existing.LoadWait.WaitOne();
					LoadPlayerUIFinish(existing, control);
					return;
				}


				using (SimpleLogTimer.Start("Player query"))
				{
					//var entry = Recorder.GetPlayer(player.SummonerId);
					//ply.Player = entry ?? ply.Player;
				}

				using (SimpleLogTimer.Start("Stats query"))
				{
                    var cmd = new PlayerCommands(Connection);
                    var summoner = cmd.GetPlayerByName(player.Name);
					if (summoner != null)
					{
                        ply.Summoner = summoner;
                        ply.CurrentChampion = Convert.ToInt32(player.QueueRating);
                        ply.RankedStats = cmd.GetAggregatedStatList(summoner.AccountId, "CLASSIC", "CURRENT");
                        ply.LeagueInfo = cmd.GetLeagueInfo(summoner.SummonerId);
                        /*
                        if ((Settings.LoadWhatData & LoadDataEnum.Stats) != 0)
						    ply.Stats = cmd.RetrievePlayerStatsByAccountId(summoner.AccountId);
                        if ((Settings.LoadWhatData & LoadDataEnum.TopChamps) != 0)
                            ply.RecentChamps = cmd.RetrieveTopPlayedChampions(summoner.AccountId, "CLASSIC");
                        if ((Settings.LoadWhatData & LoadDataEnum.RecentGames) != 0) 
                            ply.Games = cmd.GetRecentGames(summoner.AccountId);
                         * */
					}
					else
					{
						StaticLogger.Debug(string.Format("Player {0} not found", player.Name));
						ply.LoadWait.Set();
						return;
					}
				}

				using (SimpleLogTimer.Start("Seen query"))
				{
					if (SelfSummoner != null && SelfSummoner.SummonerId != ply.Summoner.SummonerId && ply.Games != null)
						ply.SeenCount = ply.Games.GameStatistics.Count(pgs => pgs.FellowPlayers.Any(fp => fp.SummonerId == SelfSummoner.SummonerId));
				}

				ply.LoadWait.Set();
				LoadPlayerUIFinish(ply, control);
			}
			catch (Exception ex)
			{
				ply.LoadWait.Set(); //
				StaticLogger.Warning(ex);
			}
		}

		private void InstallButton_Click(object sender, EventArgs e)
		{
			if (!Wow.IsAdministrator)
			{
				MessageBox.Show("You must run LoLTeamChecker as admin to install/uninstall it");
				return;
			}
			try
			{

				if (Installer.IsInstalled)
				{
					Installer.Uninstall();
				}
				else
                {
                    Installer.Uninstall();
					Installer.Install();
				}
			}
			catch (UnauthorizedAccessException uaex)
			{
				MessageBox.Show("Unable to fully install/uninstall. Make sure LoL is not running.");
				StaticLogger.Warning(uaex);
			}
			//InstallButton.Text = Installer.IsInstalled ? "Uninstall" : "Install";
			UpdateIcon();
		}

		static T GetParent<T>(Control c) where T : Control
		{
			if (c == null)
				return null;
			if (c.GetType() == typeof(T))
			{
				return (T)c;
			}
			return GetParent<T>(c.Parent);
		}

		private void MainForm_Shown(object sender, EventArgs e)
		{
			//SetTitle("(Checking)");
            //Task.Factory.StartNew(GetGeneral, TaskCreationOptions.LongRunning);
			TrackingQueue.Enqueue("startup");

            //Settings_Loaded(this, new EventArgs());
            UpdateIcon();

			//Add this after otherwise it will save immediately due to RegionList.SelectedIndex
			//Settings.PropertyChanged += Settings_PropertyChanged;

			//Start after the form is shown otherwise Invokes will fail
			Connection.Start();
			Injector.Start();
			launcher.Start();
		}


		static string CallArgToString(object arg)
		{
			if (arg is RemotingMessage)
			{
				return ((RemotingMessage)arg).operation;
			}
			if (arg is DSK)
			{
				var dsk = (DSK)arg;
				var ao = dsk.Body as ASObject;
				if (ao != null)
					return ao.TypeName;
			}
			if (arg is CommandMessage)
			{
				return CommandMessage.OperationToString(((CommandMessage)arg).operation);
			}
			return arg.ToString();
		}

		void Connection_Call(object sender, Notify call, Notify result)
		{
			if (InvokeRequired)
			{
				BeginInvoke(new Action<object, Notify, Notify>(Connection_Call), sender, call, result);
				return;
			}

            var text = string.Format(
                "({1}), ({2})",
                call.ServiceCall.Arguments.ToString(),
                string.Join(", ", call.ServiceCall.Arguments.Select(CallArgToString)),
                string.Join(", ", result.ServiceCall.Arguments.Select(CallArgToString))
            );

            /*
            var children = new List<TreeNode>();
            var bodies = RtmpUtil.GetBodies(call);
            foreach (var body in bodies)
            {
                children.Add(GetNode(body.Item1) ?? new TreeNode(body.Item1 != null ? body.Item1.ToString() : ""));
            }
            treeView1.Nodes.Add(new TreeNode(!RtmpUtil.IsResult(call) ? "Call" : "Return", children.ToArray()));*/

			var item = new ListViewItem(text)
			{
				Tag = new List<Notify> { call, result }
			};
            listView1.Items.Add(item);

		}
		void Connection_Notify(object sender, Notify notify)
		{
			if (InvokeRequired)
			{
				BeginInvoke(new Action<object, Notify>(Connection_Notify), sender, notify);
				return;
			}

            var text = string.Format(
                "Recv {0}({1})",
                !string.IsNullOrEmpty(notify.ServiceCall.ServiceMethodName) ? notify.ServiceCall.ServiceMethodName + " " : "",
                string.Join(", ", notify.ServiceCall.Arguments.Select(CallArgToString))
            );

			var item = new ListViewItem(text)
			{
				Tag = new List<Notify> { notify }
			};

            listView1.Items.Add(item);
		}

		static TreeNode GetNode(object arg, string name = "")
		{
			if (arg is ASObject)
			{
				var ao = (ASObject)arg;
				var children = new List<TreeNode>();
				foreach (var kv in ao)
				{
					var node = GetNode(kv.Value, kv.Key);
					if (node == null)
						node = new TreeNode(kv.Key + " = " + (kv.Value ?? "null"));
					children.Add(node);
				}

				string typename = ao.TypeName;
				if (typename == null && children.Count > 1)
					typename = "ASObject";

				string text = name;
				if (!string.IsNullOrEmpty(text))
					text += " ";
				text += (typename != null ? "(" + typename + ")" : "null");

				return new TreeNode(text, children.ToArray());
			}
			if (arg is Dictionary<string, object>)
			{
				if (string.IsNullOrEmpty(name))
					name = "Dictionary";

				var dict = (Dictionary<string, object>)arg;
				var children = new List<TreeNode>();
				foreach (var kv in dict)
				{
					var node = GetNode(kv.Value, kv.Key);
					if (node == null)
						node = new TreeNode(kv.Key + " = " + (kv.Value ?? "null"));
					children.Add(node);
				}
				return new TreeNode(name, children.ToArray());
			}
			if (arg is ArrayCollection)
			{
				var list = (ArrayCollection)arg;
				var children = new List<TreeNode>();
				for (int i = 0; i < list.Count; i++)
				{
					var node = GetNode(list[i], "[" + i + "]");
					if (node == null)
						node = new TreeNode(list[i].ToString());
					children.Add(node);
				}
				if (!string.IsNullOrEmpty(name))
					name += " ";
				name += "(Array)";
				if (children.Count < 1)
				{
					name += " = { }";
				}
				return new TreeNode(name, children.ToArray());
			}
			if (arg is object[])
			{
				var list = (object[])arg;
				var children = new List<TreeNode>();
				for (int i = 0; i < list.Length; i++)
				{
					var node = GetNode(list[i], "[" + i + "]");
					if (node == null)
						node = new TreeNode(list[i].ToString());
					children.Add(node);
				}
				if (!string.IsNullOrEmpty(name))
					name = " ";
				name += "(Array)";
				if (children.Count < 1)
				{
					name += " = { }";
				}
				return new TreeNode(name, children.ToArray());
			}
			return null;
		}

		/// <summary>
		/// Recursively adds a "TypeName" key to the ASObjects as newtonsoft doesn't serialize it.
		/// </summary>
		/// <param name="obj"></param>
		void AddMissingTypeNames(object obj)
		{
			if (obj == null)
				return;

			if (obj is ASObject)
			{
				var ao = (ASObject)obj;
				ao["TypeName"] = ao.TypeName;
				foreach (var kv in ao)
					AddMissingTypeNames(kv.Value);
			}
			else if (obj is IList)
			{
				var list = (IList)obj;
				foreach (var item in list)
					AddMissingTypeNames(item);
			}
		}

		private void MainForm_ResizeBegin(object sender, EventArgs e)
		{
			SuspendLayout();
		}

		private void MainForm_ResizeEnd(object sender, EventArgs e)
		{
			ResumeLayout();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			var cmd = new PlayerCommands(Connection);
			var summoner = cmd.GetPlayerByName(SelfSummoner.Username);
			if (summoner != null)
			{
				cmd.RetrievePlayerStatsByAccountId(summoner.AccountId);
				cmd.RetrieveTopPlayedChampions(summoner.AccountId, "CLASSIC");
				cmd.GetRecentGames(summoner.AccountId);
			}


			//var cmd = new PlayerCommands(Connection);
			//var obj = cmd.InvokeServiceUnknown(
			//    "gameService",
			//    "quitGame"
			//);

			//if (Champions == null)
			//    return;

			//var sorted = Champions.OrderBy(c => ChampNames.Get(c.ChampionId)).ToList();

			//var cmd = new PlayerCommands(Connection);
			//for (int i = 0; i < sorted.Count; i++)
			//{
			//    if (sorted[i].FreeToPlay || sorted[i].Owned)
			//    {
			//        var id = sorted[i].ChampionId;
			//        //ThreadPool.QueueUserWorkItem(delegate
			//        //{
			//        var obj = cmd.InvokeServiceUnknown(
			//            "gameService",
			//            "selectChampion",
			//            id
			//        );
			//        //});
			//    }
			//}
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var cmd = new PlayerCommands(Connection);
            /*ChampionDTOList champs = cmd.GetAvailableChampions();
            foreach (ChampionDTO champ in champs)
            {
                foreach (ChampionSkinDTO champSkin in champ.ChampionSkins){
                    if (champSkin.Owned == true)
                    {
                        Console.WriteLine("own skin " + champSkin.SkinID + " for champ " + Content.Data.ChampIds.ChampData[champ.ChampionId]);
                    }
                }
            }*/
            //cmd.SelectChampion(121);
            //cmd.SelectSkin(Convert.ToInt64(textBox1.Text), Convert.ToInt64(textBox2.Text));
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();

            if (listView1.SelectedItems.Count < 1)
                return;

            var notifies = listView1.SelectedItems[0].Tag as List<Notify>;
            if (notifies == null)
                return;

            foreach (var notify in notifies)
            {
                var children = new List<TreeNode>();
                var bodies = RtmpUtil.GetBodies(notify);
                foreach (var body in bodies)
                {
                    children.Add(GetNode(body.Item1) ?? new TreeNode(body.Item1 != null ? body.Item1.ToString() : ""));
                }
                treeView1.Nodes.Add(new TreeNode(!RtmpUtil.IsResult(notify) ? "Call" : "Return", children.ToArray()));
            }

            foreach (TreeNode node in treeView1.Nodes)
            {
                node.Expand();
                foreach (TreeNode node2 in node.Nodes)
                {
                    node2.Expand();
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            var cmd = new PlayerCommands(Connection);
            cmd.SelectChampion(Convert.ToInt64(textBox1.Text));
        }

	}
}
