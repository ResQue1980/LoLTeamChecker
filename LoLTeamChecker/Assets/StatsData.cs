﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LoLTeamChecker.Properties;
using Newtonsoft.Json;

namespace LoLTeamChecker.Assets
{
	public class StatsData : Dictionary<string, string>
	{
        static Dictionary<string, string> _instance = new Dictionary<string, string>(){
            {"WIN", "Victories"},
            {"CHAMPIONS_KILLED", "Champion Kills"},
            {"TOTAL_ASSISTS", "Assists"},
            {"FRIENDLY_HQ_LOST", "Friendly Nexus Destroyed"},
            {"TOTAL_TIMES_SPELL4_CAST", "Spell 4 Casts"},
            {"UNREAL_KILLS", "Unreal Kills"},
            {"MAX_CHAMPIONS_KILLED", "Max Kills"},
            {"TOTAL_PLAYER_SCORE", "Total Score"},
            {"SPELL2_CAST", "Spell 2 Casts"},
            {"TOTAL_CHAMPION_KILLS", "Total Champion Kills"},
            {"EXP", "Champion Experience"},
            {"MINIONS_KILLED", "Minions Slain"},
            {"MAGIC_DAMAGE_TAKEN", "Magic Damage Taken"},
            {"SUPER_MONSTER_KILLED", "Epic Monster "},
            {"TEAM_OBJECTIVE", "Quests Completed"},
            {"LEVEL", "Champion Level"},
            {"TOTAL_GOLD_EARNED", "Gold Earned"},
            {"TRIPLE_KILLS", "Triple Kills"},
            {"TOTAL_TURRETS_KILLED", "Total Turrets Destroyed"},
            {"SPELL3_CAST", "Spell 3 Casts"},
            {"QUADRA_KILLS", "Quadra Kills"},
            {"TOTAL_DAMAGE_TAKEN", "Damage Taken"},
            {"LARGEST_MULTI_KILL", "Largest Multi Kill"},
            {"MAGIC_DAMAGE_DEALT_PLAYER", "Magic Damage Dealt"},
            {"OBJECTIVE_CATEGORY", "Objective"},
            {"TOTAL_PHYSICAL_DAMAGE_DEALT", "Physical Damage Dealt"},
            {"NODE_KILL_DEFENSE", "Assists on Point"},
            {"TOTAL_FIRST_BLOOD", "Total First Blood"},
            {"ITEMS_PURCHASED", "Purchased Items"},
            {"SPELL4_CAST", "Spell 4 Casts"},
            {"LARGEST_CRITICAL_STRIKE", "Largest Critical Strike"},
            {"MISC_CATEGORY", "Misc."},
            {"PHYSICAL_DAMAGE_DEALT_PLAYER", "Physical Damage Dealt"},
            {"CONSUMABLES_PURCHASED", "Consumables Purchased"},
            {"MOST_SPELLS_CAST", "Maximum Spells Cast"},
            {"OBJECTIVE_PLAYER_SCORE", "Objective Score"},
            {"TOTAL_UNITS_HEALED", "Allies Healed"},
            {"TOTAL_SESSIONS_WON", "Won"},
            {"TOTAL_MAGIC_DAMAGE_DEALT", "Magic Damage Dealt"},
            {"MAX_NUM_DEATHS", "Max Deaths"},
            {"NODE_NEUTRALIZE", "Points Neutralized\n\nPoint Neutralize: 40 Score"},
            {"NODE_KILL_OFFENSE", "Kills on Point"},
            {"MAX_TIME_PLAYED", "Longest Game"},
            {"NODE_CAPTURE", "Points Captured\n\nPoint Capture: 40 Score"},
            {"TOTAL_DOUBLE_KILLS", "Double Kills"},
            {"TOTAL_NEUTRAL_MINIONS_KILLED", "Monsters Killed"},
            {"TOTAL_DAMAGE_DEALT", "Damage Dealt"},
            {"TOTAL_TIME_SPENT_DEAD", "Time Spent Dead"},
            {"NEUTRAL_MINIONS_KILLED", "Neutral Monsters Killed"},
            {"STAT_TYPE_TOTAL_CHAMPION_DEATHS", "Deaths"},
            {"NUM_DEATHS", "Deaths"},
            {"HQ_KILLED", "Destroyed Nexus"},
            {"FRIENDLY_DAMPEN_LOST", "Friendly Dampeners Lost"},
            {"TOTAL_TIMES_SPELL1_CAST", "Spell 1 Casts"},
            {"NODE_NEUTRALIZE_ASSIST", "Point Neutralizations Assisted"},
            {"DEATHS", "Deaths"},
            {"FRIENDLY_TURRET_LOST", "Friendly Turrets Lost"},
            {"KILLING_SPREES", "Killing Sprees"},
            {"TOTAL_MINION_KILLS", "Minions Killed"},
            {"TIME_PLAYED", "Time Played"},
            {"MAX_LARGEST_CRITICAL_STRIKE", "Largest Critical Strike"},
            {"LOSE", "Losses"},
            {"PENTA_KILLS", "Penta Kills"},
            {"PHYSICAL_DAMAGE_TAKEN", "Physical Damage Taken"},
            {"GOLD", "Gold Earned"},
            {"SCORE_CATEGORY", "Score"},
            {"DAMAGE_TAKEN_CATEGORY", "Damage Taken & Healed"},
            {"AVE_LEVEL_AT_GAME_END", "Average Level at End of Game"},
            {"NODE_CAPTURE_ASSIST", "Point Captures Assisted"},
            {"TOTAL_TRIPLE_KILLS", "Triple Kills"},
            {"TOTAL_SESSIONS_LOST", "Lost"},
            {"TOTAL_TIMES_SPELL2_CAST", "Spell 2 Casts"},
            {"SUMMON_SPELL1_CAST", "Summoner Spell 1"},
            {"BARRACKS_KILLED", "Inhibitors Destroyed"},
            {"CHAMPION_KILLS", "Champions Killed"},
            {"MOST_CHAMPION_KILLS_PER_SESSION", "Max Champions Kills"},
            {"COMBAT_CATEGORY", "Combat"},
            {"TOTAL_HEAL", "Healing Done"},
            {"LONGEST_KILLING_SPREE", "Longest Killing Spree"},
            {"TOTAL_CONS_PURCHASED", "Consumables Purchased"},
            {"CHAMPION_KDA_DOMINION", "Champions Killed/Deaths/Assists\n\nKill on/off Point: 30/15 Score\nAssist on/off Point: 10/5 Score"},
            {"GOLD_SPENT", "Gold Spent"},
            {"ASSISTS", "Assists"},
            {"TOTAL_QUADRA_KILLS", "Quadra Kills"},
            {"DOUBLE_KILLS", "Double Kills"},
            {"TOTAL_TIMES_SPELL3_CAST", "Spell 3 Casts"},
            {"GOLD_EARNED", "Gold Earned"},
            {"TOTAL_LEAVES", "Left"},
            {"COMBAT_PLAYER_SCORE", "Combat Score"},
            {"TURRETS_KILLED", "Turrets Destroyed"},
            {"SUMMON_SPELL2_CAST", "Summoner Spell 2"},
            {"TOTAL_DEATHS_PER_SESSION", "Average Deaths Per Session"},
            {"TOTAL_GOLD_SPENT", "Gold Spent"},
            {"SPELL1_CAST", "Spell 1 Casts"},
            {"LARGEST_KILLING_SPREE", "Largest Killing Spree"},
            {"TOTAL_SESSIONS_PLAYED", "Games Played"},
            {"TIME_SPENT_LIVING", "Time Spent Alive"},
            {"DAMAGE_DONE_CATEGORY", "Damage Done"},
            {"TOTAL_PENTA_KILLS", "Penta Kills"},
            {"CHAMPION_KDA_CLASSIC", "Champions Killed/Deaths/Assists"}
        };
		public static StatsData Instance { get { return (StatsData)_instance; } }

		static StatsData()
		{
        }

		public static string Get(string key)
		{
			string ret;
			return _instance.TryGetValue(key, out ret) ? ret : key;
		}
	}
}
