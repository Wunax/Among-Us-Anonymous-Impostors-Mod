using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using AnonymousImpostorsMod.API;

using EndGameManager = ABNGEPFHMHP;
using TempData = GKBKEMKIPIE;
using GameOverReason = AIMMJPEOPEC;
using WinningPlayer = HKBFDMLCCPE;

namespace AnonymousImpostorsMod
{
    [HarmonyPatch]
    public static class EndGameManagerPatch
    {
        [HarmonyPatch(typeof(ABNGEPFHMHP.EHKHLOLEFFD), nameof(ABNGEPFHMHP.EHKHLOLEFFD.MoveNext))]
        public static void Postfix(ABNGEPFHMHP.EHKHLOLEFFD __instance)
        {
            if (!CustomGameOptions.anonymousImpostorsEnabled || !CustomGameOptions.impostorSoloWin)
                return;
            int reason = (int)TempData.NGHMMPMNHDL;
            if (reason != 4) // not sabotage
            {
                var localPlayer = PlayerController.GetLocalPlayer();
                if (localPlayer.PlayerData.IsImpostor && localPlayer.PlayerData.IsDead)
                {
                    __instance.field_Public_ABNGEPFHMHP_0.WinText.Text = "Defeat";
                    __instance.field_Public_ABNGEPFHMHP_0.WinText.Color = new UnityEngine.Color(1, 0, 0, 1);
                    __instance.field_Public_ABNGEPFHMHP_0.BackgroundBar.material.color = new UnityEngine.Color(1, 0, 0, 1);
                }
            }
        }
    }
}
