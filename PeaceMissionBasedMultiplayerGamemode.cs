using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TaleWorlds.Core;
using TaleWorlds.Diamond;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.LegacyGUI.Missions;
using TaleWorlds.MountAndBlade.Source.Missions;
using TaleWorlds.MountAndBlade.Test.MpHeroTest;
using TaleWorlds.MountAndBlade.View.Missions;

namespace BaseMPMode
{
    
    [MissionManager]
    public class PeaceMissionBasedMultiplayerGamemode : MissionBasedMultiplayerGameMode
    {
        public PeaceMissionBasedMultiplayerGamemode(string name) : base(name)
        {
            
        }
        
        public override void StartMultiplayerGame(string scene)
        {
            if (base.Name == "PeaceGameMode")
            {
	            
	            ScoreboardFactory.Register("PeaceGameMode",new PeaceGameModeScoreData());
                OpenPeaceMission(scene);
                return;
            }
         
        }

        [MissionMethod]
        public static void OpenPeaceMission(string scene)
        {
            MissionState.OpenNew("PeaceGameMode", new MissionInitializerRecord(scene), missionController => GameNetwork.IsServer ? (IEnumerable<MissionBehaviour>) new MissionBehaviour[]
            {
                (MissionBehaviour) MissionLobbyComponent.CreateBehaviour(),
                (MissionBehaviour) new MissionPeaceGameServer(),
                (MissionBehaviour) new MissionPeaceGame(),
                (MissionBehaviour) new MultiplayerTimerComponent(),
                (MissionBehaviour) new MultiplayerMissionAgentVisualSpawnComponent(),
                (MissionBehaviour) new SpawnComponent((SpawnFrameBehaviourBase) new TeamDeathmatchSpawnFrameBehavior(), new PeaceGameSpawningBehavior()),
                (MissionBehaviour) new MissionLobbyEquipmentNetworkComponent(),
                (MissionBehaviour) new MultiplayerTeamSelectComponent(),
                (MissionBehaviour) new MissionHardBorderPlacer(),
                (MissionBehaviour) new MissionBoundaryPlacer(),
                (MissionBehaviour) new MissionBoundaryCrossingHandler(),
                (MissionBehaviour) new MultiplayerPollComponent(),
                (MissionBehaviour) new MultiplayerAdminComponent(),
                (MissionBehaviour) new MultiplayerGameNotificationsComponent(),
                (MissionBehaviour) new MissionOptionsComponent(),
                (MissionBehaviour) new MissionScoreboardComponent("PeaceGameMode"),
                (MissionBehaviour) new AgentBattleAILogic(),
                (MissionBehaviour) new AgentFadeOutLogic()
            } : (IEnumerable<MissionBehaviour>) new MissionBehaviour[]
            {
                (MissionBehaviour) MissionLobbyComponent.CreateBehaviour(),
                (MissionBehaviour) new MissionPeaceGame(),
                (MissionBehaviour) new MultiplayerTimerComponent(),
                (MissionBehaviour) new MultiplayerMissionAgentVisualSpawnComponent(),
                (MissionBehaviour) new MissionLobbyEquipmentNetworkComponent(),
                (MissionBehaviour) new MultiplayerTeamSelectComponent(),
                (MissionBehaviour) new MissionHardBorderPlacer(),
                (MissionBehaviour) new MissionBoundaryPlacer(),
                (MissionBehaviour) new MissionBoundaryCrossingHandler(),
                (MissionBehaviour) new MultiplayerPollComponent(),
                (MissionBehaviour) new MultiplayerGameNotificationsComponent(),
                (MissionBehaviour) new MissionOptionsComponent(),
                (MissionBehaviour) new MissionScoreboardComponent("PeaceGameMode")
            }, true, true, false);
        }
    }
}

class PeaceMissionOptionsComponent : MissionOptionsComponent
{
    public new void OnAddOptionsUIHandler()
    {
        Debug.DebugManager.Print("PeaceMissionOptionsComponent::OnAddOptionsUIHandler");
        base.OnAddOptionsUIHandler();
    }
}

[ViewCreatorModule]
public class MultiplayerMissionViews
{

	[ViewMethod("PeaceGameMode")]
	public static MissionView[] OpenTeamDeathmatchMission(Mission mission)
	{
		List<MissionView> list = new List<MissionView>();
		list.Add(ViewCreator.CreateLobbyUIHandler());
		list.Add(ViewCreator.CreateMissionMultiplayerPreloadView(mission));
		list.Add(ViewCreator.CreateMultiplayerTeamSelectUIHandler());
		list.Add(ViewCreator.CreateMissionKillNotificationUIHandler());
		list.Add(ViewCreator.CreateMissionAgentStatusUIHandler(mission));
		list.Add(ViewCreator.CreateMissionMainAgentEquipmentController(mission));
		list.Add(ViewCreator.CreateMissionMultiplayerEscapeMenu("PeaceGameMode"));
		list.Add(ViewCreator.CreateMissionScoreBoardUIHandler(mission, false));
		list.Add(ViewCreator.CreateMultiplayerEndOfRoundUIHandler());
		list.Add(ViewCreator.CreateLobbyEquipmentUIHandler());
		list.Add(ViewCreator.CreateMissionAgentLabelUIHandler(mission));
		list.Add(ViewCreator.CreatePollInitiationUIHandler());
		list.Add(ViewCreator.CreatePollProgressUIHandler());
		list.Add(ViewCreator.CreateMissionFlagMarkerUIHandler());
		list.Add(ViewCreator.CreateMultiplayerMissionHUDExtensionUIHandler());
		list.Add(ViewCreator.CreateMultiplayerMissionDeathCardUIHandler(null));
		list.Add(ViewCreator.CreateOptionsUIHandler());
		if (!GameNetwork.IsClient)
		{
			list.Add(ViewCreator.CreateMultiplayerAdminPanelUIHandler());
		}

		list.Add(ViewCreator.CreateMissionBoundaryCrossingView());
		list.Add(new MissionBoundaryWallView());
		list.Add(new MissionItemContourControllerView());
		list.Add(new MissionAgentContourControllerView());
		return list.ToArray();
	}
}