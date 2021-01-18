using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;
using AnonymousImpostorsMod.API;

using Hud = PIEFJFEOGOL;
using HudManager = PPAEIPHJPDH<PIEFJFEOGOL>;
using MeetingHud = OOCJALPKPEP;
using PlayerMeeting = HDJGDMFCHDN;
using PlayerControl = FFGALNAPKCD;

namespace AnonymousImpostorsMod
{
    [HarmonyPatch]
    public static class HudPatch
    {
        public static string GameSettingsText = null;

        public static void updateMeetingHud(MeetingHud __instance, PlayerController localPlayer)
        {
            foreach (PlayerMeeting player in __instance.HBDFFAHBIGI)
            {
                if (player.NameText.Text != localPlayer.PlayerControl.nameText.Text)
                {
                    player.NameText.Color = new Color(1f, 1f, 1f, 1f);
                }
            }
        }
        public static void hideOtherImpostors(PlayerController localPlayer)
        {
            var listPlayers = PlayerController.GetAllPlayers();
            foreach (var player in listPlayers)
            {
                if (!player.Equals(localPlayer) && player.PlayerData != null && player.PlayerData.IsImpostor)
                {
                    player.PlayerControl.nameText.Color = new Color(1f, 1f, 1f, 1f);
                }
            }
        }

        public static void setClosestTargetKillButton(PlayerController localPlayer)
        {
            PlayerControl closestPlayerControl = localPlayer.PlayerControl.CLKILNOCHEP();
            if (closestPlayerControl == null)
                return;
            PlayerController closest = new PlayerController(closestPlayerControl);
            if (PlayerController.distBeetweenPlayers(localPlayer, closest) <= GameOptionsData.getKillDistance())
                HudManager.IAINKLDJAGC.KillButton.SetTarget(closest.PlayerControl);
        }

        public static void UpdateGameSettingsText(Hud __instance)
        {
            if (__instance.GameSettings.Text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Count() == 19)
                GameSettingsText = __instance.GameSettings.Text;
            if (GameSettingsText != null)
            {
                if (CustomGameOptions.impostorSoloWin)
                    __instance.GameSettings.Text = GameSettingsText + "Impostor Solo Win: On" + "\n";
                else
                    __instance.GameSettings.Text = GameSettingsText + "Impostor Solo Win: Off" + "\n";
            }
        }

        [HarmonyPatch(typeof(Hud), "Update")]
        public static void Postfix(Hud __instance)
        {
            if (PlayerControl.LocalPlayer == null)
                return;
            if (!CustomGameOptions.anonymousImpostorsEnabled)
                return;
            UpdateGameSettingsText(__instance);
            PlayerController localPlayer = PlayerController.GetLocalPlayer();
            if (localPlayer.PlayerData != null && localPlayer.PlayerData.IsImpostor)
            {
                if (MeetingHud.Instance != null)
                    updateMeetingHud(MeetingHud.Instance, localPlayer);
                hideOtherImpostors(localPlayer);
                if (!localPlayer.PlayerData.IsDead)
                    setClosestTargetKillButton(localPlayer);
            }
        }
    }
}
