using System;
using System.Threading;
using System.Threading.Tasks;
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
using GameOptionsMenu = PHCKLDDNJNP;
using GameStates = KHNHJFFECBP.KGEKNMMAKKN;

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

        public static void updateGameSettingsText(Hud __instance)
        {
            if (__instance.GameSettings.Text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Count() == 19)
                GameSettingsText = __instance.GameSettings.Text;
            if (GameSettingsText != null)
            {
                string append = String.Empty;
                if (CustomGameOptions.anonymousImpostorsEnabled)
                    append += "Anonymous Impostors: On" + "\n";
                else
                    append += "Anonymous Impostors: Off" + "\n";
                if (CustomGameOptions.anonymousImpostorsEnabled)
                {
                    if (CustomGameOptions.impostorSoloWin)
                        append += "Impostor Solo Win: On" + "\n";
                    else
                        append += "Impostor Solo Win: Off" + "\n";
                }
                __instance.GameSettings.Text = GameSettingsText + append;
            }
        }

        public static void hideGameMenuSettings()
        {
            if (GameOptionsMenuPatch.GameOptionsMenuUpdatePatch.anonymousImpostors != null && GameOptionsMenuPatch.GameOptionsMenuUpdatePatch.impostorSoloWin != null)
            {
                bool isActive = GameObject.FindObjectsOfType<GameOptionsMenu>().Count != 0;
                GameOptionsMenuPatch.GameOptionsMenuUpdatePatch.anonymousImpostors.gameObject.SetActive(isActive);
                GameOptionsMenuPatch.GameOptionsMenuUpdatePatch.impostorSoloWin.gameObject.SetActive(isActive);
            }
        }

        [HarmonyPatch(typeof(Hud), "Update")]
        public static void Postfix(Hud __instance)
        {
            if (PlayerControl.LocalPlayer == null)
                return;
            if (GameData.currentGame.GameState != GameStates.Started)
                updateGameSettingsText(__instance);
            if (CustomGameOptions.anonymousImpostorsEnabled)
            {
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
            if (GameData.currentGame.GameState != GameStates.Started)
            {
                Task.Run(async () =>
                {
                    await Task.Delay(100);
                    hideGameMenuSettings();
                });
            }
        }
    }
}
