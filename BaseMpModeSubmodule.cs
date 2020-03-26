using System;
using System.Linq;
using TaleWorlds.MountAndBlade;

namespace BaseMPMode
{
    public class BaseMpModeSubmodule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            Module.CurrentModule.AddMultiplayerGameMode(new PeaceMissionBasedMultiplayerGamemode("PeaceGameMode") );
            
            Module.CurrentModule.GetMultiplayerGameTypes().First(x=>x.GameType =="PeaceGameMode").Scenes.Add("mp_tdm_map_001_spring");
            base.OnSubModuleLoad();
        }
    }
}