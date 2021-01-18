using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using AnonymousImpostorsMod.API;

using ShipStatus = HLBNNHFCNAJ;
using LifeSuppSystem = PPIIPAAMDAD;
using ReactorSystem = KJKDNMBDHKJ;
using GameOverReason = AIMMJPEOPEC;

namespace AnonymousImpostorsMod
{
    public static class ShipStatusPatch
    {
        [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.ECNNLBKGIKE))]
        public static class CheckVictoryPatch
        {
            public static bool Prefix()
            {
                var shipStatusInstance = ShipStatus.Instance;
                if (!shipStatusInstance)
                    return true;
                if (!CustomGameOptions.anonymousImpostorsEnabled || !CustomGameOptions.impostorSoloWin)
                    return true;
                if (PlayerController.GetAllPlayersAlive().Count <= 1)
                    return true;
                if (PlayerController.GetImpostorsAlive().Count <= 1)
                    return true;
                if (shipStatusInstance.CheckTaskCompletion())
                {
                    ShipStatus.PLBGOMIEONF(GameOverReason.HumansByTask, false);
                    return false;
                }
                var LifeSupp = shipStatusInstance.Systems[LJFDDJHBOGF.LifeSupp].Cast<LifeSuppSystem>();
                if (LifeSupp.BCKOBJLJEFE && LifeSupp.HMJFAFANEEL <= 0)
                {
                    shipStatusInstance.ICKBKNMHKCM();
                    return false;
                }
                var Reactor = shipStatusInstance.Systems[LJFDDJHBOGF.Reactor].Cast<ReactorSystem>();
                if (Reactor.BCKOBJLJEFE && Reactor.HMJFAFANEEL <= 0)
                {
                    shipStatusInstance.ICKBKNMHKCM();
                    return false;
                }
                return false;
            }
        }
    }
}
