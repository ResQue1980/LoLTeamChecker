using System;
using FluorineFx;
using LoLTeamChecker.Flash;
using NotMissing;

namespace LoLTeamChecker.Messages.GameLobby
{
    [Message(".PlatformGameLifecycleDTO")]
    public class PlatformGameLifecycleDTO : MessageObject, ICloneable
    {
        public PlatformGameLifecycleDTO()
            : base(null)
        {
        }

        public PlatformGameLifecycleDTO(ASObject obj)
            : base(obj)
        {
            BaseObject.SetFields(this, obj);
        }

        [InternalName("gameSpecificLoyaltyRewards")]
        public int GameSpecificLoyaltyRewards
        {
            get;
            set;
        }

        [InternalName("reconnectDelay")]
        public int ReconnectDelay
        {
            get;
            set;
        }

        [InternalName("dataVersion")]
        public int DataVersion
        {
            get;
            set;
        }

        [InternalName("lastModifiedDate")]
        public string LastModifiedDate
        {
            get;
            set;
        }
        [InternalName("game")]
        public GameDTO Game
        {
            get;
            set;
        }
        /*[InternalName("playerCredentials")]
        public PlayerCredentialsDTO PlayerCredentials
        {
            get;
            set;
        }*/
        [InternalName("gameName")]
        public string GameName
        {
            get;
            set;
        }
        [InternalName("connectivityStateEnum")]
        public int ConnectivityStateEnum
        {
            get;
            set;
        }
        [InternalName("futureData")]
        public int FutureData
        {
            get;
            set;
        }

        public object Clone()
        {
            return new PlatformGameLifecycleDTO
            {
                GameSpecificLoyaltyRewards = GameSpecificLoyaltyRewards,
                ReconnectDelay = ReconnectDelay,
                DataVersion = DataVersion,
                LastModifiedDate = LastModifiedDate,
                Game = Game,
                GameName = GameName,
                ConnectivityStateEnum = ConnectivityStateEnum,
                FutureData = FutureData,
            };
        }
    }
}
