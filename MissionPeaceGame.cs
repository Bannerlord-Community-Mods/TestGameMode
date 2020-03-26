using System;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.MissionRepresentatives;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace BaseMPMode
{
    public class MissionPeaceGame : MissionMultiplayerGameModeBaseClient
    {
        private PeaceGameMissionRepresentative _myRepresentative;

		
		public override bool IsGameModeUsingGold
		{
			get
			{
				return false;
			}
		}

		public override bool IsGameModeTactical
		{
			get
			{
				return true;
			}
		}

		public override bool IsGameModeUsingRoundCountdown
		{
			get
			{
				return true;
			}
		}

		public override MissionLobbyComponent.MultiplayerGameType GameType
		{
			get
			{
				return MissionLobbyComponent.MultiplayerGameType.Battle;
			}
		}

		public override int GetGoldAmount()
		{
			return 0;
		}

		public override void OnBehaviourInitialize()
		{
			base.OnBehaviourInitialize();
			NetworkCommunicator.OnPeerComponentAdded += this.OnPeerComponentAdded;
		}

		public override void OnGoldAmountChangedForRepresentative(MissionRepresentativeBase representative, int goldAmount)
		{
		}


		public override void AfterStart()
		{
			base.Mission.SetMissionMode(MissionMode.Battle, true);
		}

		protected override void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegistererContainer registerer)
		{
			if (GameNetwork.IsClient)
			{
				registerer.Register<BotsControlledChange>(new GameNetworkMessage.ServerMessageHandlerDelegate<BotsControlledChange>(this.HandleServerEventBotsControlledChangeEvent));

			}
		}

		private void HandleServerEventBotsControlledChangeEvent(BotsControlledChange message)
		{
				MissionPeer component = message.Peer.GetComponent<MissionPeer>();
            			this.OnBotsControlledChanged(component, message.AliveCount, message.TotalCount);
		}
public void OnBotsControlledChanged(MissionPeer missionPeer, int botAliveCount, int botTotalCount)
		{
			//missionPeer.BotsUnderControlAlive = botAliveCount;
			//missionPeer.BotsUnderControlTotal = botTotalCount;
			
		}



private void OnPeerComponentAdded(PeerComponent component)
		{
			if (component.IsMine && component is MissionRepresentativeBase)
				this._myRepresentative = (component as PeaceGameMissionRepresentative);
		}

	

		public override void OnRemoveBehaviour()
		{
			NetworkCommunicator.OnPeerComponentAdded -= this.OnPeerComponentAdded;
		}
	}
    }
