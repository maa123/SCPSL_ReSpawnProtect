using Smod.Sprotect;

using Smod2;
using Smod2.API;
using Smod2.Attributes;
using Smod2.EventHandlers;
using Smod2.Events;

using System.Collections.Generic;


namespace ReSpawnProtect{
    [PluginDetails(
        author = "maa123",
        name = "ReSpawnProtect",
        description = "リスキル防止",
        id = "maa123.ReSpawnProtect",
        version = "0.6",
        SmodMajor = 3,
        SmodMinor = 1,
        SmodRevision = 4
    )]
    class ReSpawnProtect : Plugin{
        public override void OnDisable(){
            this.Info("ReSpawnProtectが無効化されました");
        }
        public override void OnEnable(){
            this.Info("ReSpawnProtectが読み込まれました");
        }

        public override void Register(){
            this.AddEventHandlers(new PlEventHandler(this));
        }
    }
}


namespace Smod.Sprotect{

    class PlEventHandler : IEventHandlerPlayerHurt, IEventHandlerSetRole {

        private Plugin plugin;

        private List<string> playerlist = new List<string>();

        public PlEventHandler(Plugin plugin){
            this.plugin = plugin;
        }

        public void OnSetRole(PlayerSetRoleEvent ev){
            this.plugin.Info((string)ev.Player.Name);
            if (!playerlist.Contains(ev.Player.Name)){
                playerlist.Add(ev.Player.Name);
                System.Timers.Timer t = new System.Timers.Timer{
                    Interval = 10000,
                    AutoReset = false,
                };
                t.Elapsed += (sender, e)=> {
                    playerlist.Remove(ev.Player.Name);
                    t.Stop();
                    using(t){ }
                };
                t.Start();
            }
        }


        public void OnPlayerHurt(PlayerHurtEvent ev){
            if (playerlist != null && (playerlist.Contains(ev.Player.Name) || playerlist.Contains(ev.Attacker.Name))){
                ev.Damage = 0.0F;
            }
        }
    }
}
