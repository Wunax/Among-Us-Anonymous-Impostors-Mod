using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

using PlayerControl = FFGALNAPKCD;
using ToggleButton = BCLDBBKFJPK;

namespace AnonymousImpostorsMod
{
    [HarmonyPatch]
    public static class ToggleButtonPatch
    {
        [HarmonyPatch(typeof(ToggleButton), nameof(ToggleButton.Toggle))]
        public static bool Prefix(ToggleButton __instance)
        {
            if (__instance.TitleText.Text == "Anonymous Impostors")
            {
                CustomGameOptions.anonymousImpostorsEnabled = !CustomGameOptions.anonymousImpostorsEnabled;
                PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);
                __instance.NHLMDAOEOAE = CustomGameOptions.anonymousImpostorsEnabled;
                __instance.CheckMark.enabled = CustomGameOptions.anonymousImpostorsEnabled;
                if (GameOptionsMenuPatch.GameOptionsMenuUpdatePatch.impostorSoloWin != null)
                    GameOptionsMenuPatch.GameOptionsMenuUpdatePatch.impostorSoloWin.gameObject.SetActive(CustomGameOptions.anonymousImpostorsEnabled);
                return false;
            }
            if (__instance.TitleText.Text == "Impostor Solo Win")
            {
                CustomGameOptions.impostorSoloWin = !CustomGameOptions.impostorSoloWin;
                PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);
                __instance.NHLMDAOEOAE = CustomGameOptions.impostorSoloWin;
                __instance.CheckMark.enabled = CustomGameOptions.impostorSoloWin;
                return false;
            }
            return true;
        }
    }
}
