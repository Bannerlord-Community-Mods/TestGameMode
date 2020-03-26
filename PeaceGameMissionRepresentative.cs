using System;
using System.Collections.Generic;
using NetworkMessages.FromServer;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace BaseMPMode
{
    public class PeaceGameMissionRepresentative: MissionRepresentativeBase

    {


		private int _killCountOnSpawn;

		private int _assistCountOnSpawn;

		public override void OnAgentInteraction(Agent targetAgent)
		{
		}

		public override bool IsThereAgentAction(Agent targetAgent)
		{
			return false;
		}

		public override void OnAgentSpawned()
		{
			this._killCountOnSpawn = base.MissionPeer.KillCount;
			this._assistCountOnSpawn = base.MissionPeer.AssistCount;
		}

		
	}
    }
