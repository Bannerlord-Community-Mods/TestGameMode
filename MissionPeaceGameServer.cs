using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.MissionRepresentatives;

namespace BaseMPMode
{
    public class MissionPeaceGameServer :  MissionMultiplayerGameModeBase
    {
      public const int MaxScoreToEndMatch = 100000;
      

      
      		private MissionScoreboardComponent _missionScoreboardComponent;
      
      		public override bool IsGameModeHidingAllAgentVisuals
      		{
      			get
      			{
      				return true;
      			}
      		}
      
      		public override MissionLobbyComponent.MultiplayerGameType GetMissionType()
      		{
      			return MissionLobbyComponent.MultiplayerGameType.Battle;
      		}
      
      		public override void OnBehaviourInitialize()
      		{
      			base.OnBehaviourInitialize();
      			this._missionScoreboardComponent = base.Mission.GetMissionBehaviour<MissionScoreboardComponent>();
      		}
      
      		public override void AfterStart()
      		{
	            var availableCultures = MBObjectManager.Instance.GetObjectTypeList<BasicCultureObject>()
		            .Where(x => x.IsMainCulture).ToList();
	            MBMultiplayerOptionsAccessor.SetCultureTeam2(availableCultures.GetRandomElement());
	            MBMultiplayerOptionsAccessor.SetNumberOfBotsTeam1(200);
	            MBMultiplayerOptionsAccessor.SetNumberOfBotsTeam2(200);
	            MBMultiplayerOptionsAccessor.SetCultureTeam1(availableCultures.GetRandomElement());
      			BasicCultureObject cultureTeam = MBMultiplayerOptionsAccessor.GetCultureTeam1(MBMultiplayerOptionsAccessor.MultiplayerOptionsAccessMode.CurrentMapOptions);
      			BasicCultureObject cultureTeam2 = MBMultiplayerOptionsAccessor.GetCultureTeam2(MBMultiplayerOptionsAccessor.MultiplayerOptionsAccessMode.CurrentMapOptions);
      			Banner banner = new Banner(cultureTeam.BannerKey, cultureTeam.BackgroundColor1, cultureTeam.ForegroundColor1);
      			Banner banner2 = new Banner(cultureTeam2.BannerKey, cultureTeam2.BackgroundColor2, cultureTeam2.ForegroundColor2);
      			base.Mission.Teams.Add(BattleSideEnum.Attacker, cultureTeam.BackgroundColor1, cultureTeam.ForegroundColor1, banner, true, false, true);
      			base.Mission.Teams.Add(BattleSideEnum.Defender, cultureTeam2.BackgroundColor2, cultureTeam2.ForegroundColor2, banner2, true, false, true);
      		}
      
      		protected override void HandleNewClientAfterSynchronized(NetworkCommunicator networkPeer)
      		{
      			networkPeer.AddComponent<PeaceGameMissionRepresentative>();
      		}
      
      		public override void OnPeerChangedTeam(NetworkCommunicator peer, Team oldTeam, Team newTeam)
      		{
      			
      		}
      
      		public override int GetScoreForKill(Agent killedAgent)
      		{
      			return MultiplayerClassDivisions.GetMPHeroClassForCharacter(killedAgent.Character).TroopCost;
      		}
      
      		public override int GetScoreForAssist(Agent killedAgent)
      		{
      			return (int)((float)MultiplayerClassDivisions.GetMPHeroClassForCharacter(killedAgent.Character).TroopCost * 0.5f);
      		}
      
      		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
      		{
      			if (blow.DamageType != DamageTypes.Invalid && (agentState == AgentState.Unconscious || agentState == AgentState.Killed) && affectedAgent.IsHuman)
      			{
      				if (affectorAgent != null && affectorAgent.IsEnemyOf(affectedAgent))
      				{
      					this._missionScoreboardComponent.ChangeTeamScore(affectorAgent.Team, this.GetScoreForKill(affectedAgent));
      				}
      				else
      				{
      					this._missionScoreboardComponent.ChangeTeamScore(affectedAgent.Team, -this.GetScoreForKill(affectedAgent));
      				}
      				
      			}
      		}
      
      		public override bool CheckForMatchEnd()
      		{
      			return this._missionScoreboardComponent.Sides.Any((MissionScoreboardComponent.MissionScoreboardSide side) => side.SideScore >= MBMultiplayerOptionsAccessor.GetMinScoreToWinMatch(MBMultiplayerOptionsAccessor.MultiplayerOptionsAccessMode.CurrentMapOptions));
      		}
      
      		public override Team GetWinnerTeam()
      		{
      			Team result = null;
      			MissionScoreboardComponent.MissionScoreboardSide[] sides = this._missionScoreboardComponent.Sides;
      			if (sides[1].SideScore < MBMultiplayerOptionsAccessor.GetMinScoreToWinMatch(MBMultiplayerOptionsAccessor.MultiplayerOptionsAccessMode.CurrentMapOptions) && sides[0].SideScore >= MBMultiplayerOptionsAccessor.GetMinScoreToWinMatch(MBMultiplayerOptionsAccessor.MultiplayerOptionsAccessMode.CurrentMapOptions))
      			{
      				result = base.Mission.Teams.Defender;
      			}
      			if (sides[0].SideScore < MBMultiplayerOptionsAccessor.GetMinScoreToWinMatch(MBMultiplayerOptionsAccessor.MultiplayerOptionsAccessMode.CurrentMapOptions) && sides[1].SideScore >= MBMultiplayerOptionsAccessor.GetMinScoreToWinMatch(MBMultiplayerOptionsAccessor.MultiplayerOptionsAccessMode.CurrentMapOptions))
      			{
      				result = base.Mission.Teams.Attacker;
      			}
      			return result;
      		}
      	}
    
}