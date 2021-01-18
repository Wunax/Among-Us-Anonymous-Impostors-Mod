using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

using EndGameManager = ABNGEPFHMHP;
using TempData = GKBKEMKIPIE;
using GameOverReason = AIMMJPEOPEC;
using WinningPlayer = HKBFDMLCCPE;

namespace AnonymousImpostorsMod
{
    [HarmonyPatch]
    public static class EndGameManagerPatch
    {
        [HarmonyPatch(typeof(EndGameManager), nameof(EndGameManager.Start))]
        public static void Prefix()
        {
            if (!CustomGameOptions.impostorSoloWin)
                return;
            int reason = (int) TempData.NGHMMPMNHDL;
            if (reason != 4) // not sabotage
            {
                for (int i = TempData.IPJNKEJJOHI.Count - 1; i >= 0; i--)
                {
                    if (TempData.IPJNKEJJOHI[i].DAPKNDBLKIA && TempData.IPJNKEJJOHI[i].LLKILMJOFEN) // is impostor and dead
                        TempData.IPJNKEJJOHI.RemoveAt(i);
                }
            }
        }
    }
}
