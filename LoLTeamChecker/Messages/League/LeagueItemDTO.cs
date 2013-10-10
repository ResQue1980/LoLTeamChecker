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
using System.Linq;
using System.Text;
using FluorineFx;
using LoLTeamChecker.Assets;
using LoLTeamChecker.Flash;

namespace LoLTeamChecker.Messages.League
{
	public class LeagueItemDTO	: BaseObject
	{
        public LeagueItemDTO(ASObject obj)
			: base(obj)
		{
			BaseObject.SetFields(this, obj);
		}
        [InternalName("previousDayLeaguePosition")]
        public int PreviousDayLeaguePosition { get; set; }
        [InternalName("hotStreak")]
        public bool HotStreak { get; set; }
        // Only non-null for people who are playing their advancement series
        [InternalName("miniSeries")]
        //public MiniSeriesDTO miniSeries { get; set; }
        public string MiniSeries { get; set; }
        [InternalName("freshBlood")]
        public bool FreshBlood { get; set; }
        [InternalName("tier")]
        public string Tier { get; set; }
        // Not sure about this one, possibly a date sometimes
        [InternalName("lastPlayed")]
        public long LastPlayed { get; set; }
        [InternalName("playerOrTeamId")]
        public string PlayerOrTeamId { get; set; }
        [InternalName("leaguePoints")]
        public int LeaguePoints { get; set; }
        [InternalName("inactive")]
        public bool Inactive { get; set; }
        [InternalName("rank")]
        public string Rank { get; set; }
        [InternalName("veteran")]
        public bool Veteran { get; set; }
        [InternalName("queueType")]
        public string QueueType { get; set; }
        [InternalName("losses")]
        public int Losses { get; set; }
        [InternalName("playerOrTeamName")]
        public string PlayerOrTeamName { get; set; }
        [InternalName("wins")]
        public int Wins { get; set; }
	}
}
