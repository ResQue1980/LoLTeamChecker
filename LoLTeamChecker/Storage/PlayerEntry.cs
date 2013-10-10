using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using LoLTeamChecker.Messages.GameLobby;
using LoLTeamChecker.Messages.GameLobby.Participants;
using LoLTeamChecker.Messages.GameStats;
using LoLTeamChecker.Messages.GameStats.PlayerStats;
using System.Linq;
using NotMissing;

namespace LoLTeamChecker.Storage
{
    public class PlayerEntry : ICloneable
    {
        public PlayerEntry()
        {
        }
        public PlayerEntry(PlayerParticipant plr)
            : this()
        {
            Name = plr.Name;
            Id = plr.SummonerId;
        }
        public PlayerEntry(PlayerStatsSummary stats)
            : this()
        {
            Name = stats.SummonerName;
            Id = stats.UserId;
        }

        public string Note { get; set; }
        public Color NoteColor { get; set; }
        public string Name { get; set; }
        public string InternalName { get; set; }
        public Int64 Id { get; set; }

        public object Clone()
        {
            return new PlayerEntry
            {
                Note = Note,
                NoteColor = NoteColor,
                Name = Name,
                InternalName = InternalName,
                Id = Id,
            };
        }
    }
}
