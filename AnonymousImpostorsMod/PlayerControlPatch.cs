using System;
using System.Collections.Generic;
using HarmonyLib;
using Hazel;
using AnonymousImpostorsMod.API;

using PlayerControl = FFGALNAPKCD;
using NetClient = FMLLKEACGIO;
using HudManager = PPAEIPHJPDH<PIEFJFEOGOL>;
using GameStates = KHNHJFFECBP.KGEKNMMAKKN;

namespace AnonymousImpostorsMod
{
    public class PlayerControlPatch
    {
        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.CLKILNOCHEP))]
        public static class PlayerControlFindClosestPlayerPatch
        {
            public static List<PlayerController> otherImpostors = new List<PlayerController>();

            public static void Prefix(PlayerControl __instance)
            {
                if (!CustomGameOptions.anonymousImpostorsEnabled)
                    return;
                var localPlayer = new PlayerController(__instance);
                var allPlayers = PlayerController.GetAllPlayers();
                foreach (var player in allPlayers)
                {
                    if (!localPlayer.Equals(player) && player.PlayerData.IsImpostor)
                    {
                        otherImpostors.Add(player);
                        player.PlayerData.IsImpostor = false;
                    }
                }
            }

            public static void Postfix()
            {
                if (!CustomGameOptions.anonymousImpostorsEnabled)
                    return;
                foreach (var player in otherImpostors)
                {
                    player.PlayerData.IsImpostor = true;
                }
                otherImpostors.Clear();
            }
        }

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.HandleRpc))]
        public static class PlayerControlHandleRpcPatch
        {
            public static void Postfix(byte HKHMBLJFLMC, MessageReader ALMCIJKELCP)
            {
                switch (HKHMBLJFLMC)
                {
                    case (byte) CustomGameOptions.customGameOptionsRpc.syncCustomSettings:
                        {
                            CustomGameOptions.anonymousImpostorsEnabled = ALMCIJKELCP.ReadBoolean();
                            CustomGameOptions.impostorSoloWin = ALMCIJKELCP.ReadBoolean();
                            break;
                        }
                }
            }
        }

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcSyncSettings))]
        public static class PlayerControlRpcSyncSettingsPatch
        {
            public static void Postfix()
            {
                if (PlayerControl.AllPlayerControls.Count > 1)
                {
                    MessageWriter messageWriter = NetClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte) CustomGameOptions.customGameOptionsRpc.syncCustomSettings, SendOption.None, -1);
                    messageWriter.Write(CustomGameOptions.anonymousImpostorsEnabled);
                    messageWriter.Write(CustomGameOptions.impostorSoloWin);
                    NetClient.Instance.FinishRpcImmediately(messageWriter);
                }
            }
        }

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcSendChat))]
        public static class PlayerControlRpcSendChatPatch
        {
            public static bool Prefix(PlayerControl __instance, string PGIBDIEPGIC)
            {
                string msg = PGIBDIEPGIC;
                PlayerController localPlayer = PlayerController.GetLocalPlayer();
                PlayerController host = PlayerController.getHost();

                if (!msg.StartsWith("/anonymous", StringComparison.InvariantCultureIgnoreCase) && !msg.StartsWith("/soloimpostor", StringComparison.InvariantCultureIgnoreCase))
                    return true;
                if (!localPlayer.Equals(host)) {
                    HudManager.IAINKLDJAGC.Chat.AddChat(localPlayer.PlayerControl, $"Only the host ({host.PlayerControl.nameText.Text}) can change the settings of the mod.");
                    HudManager.IAINKLDJAGC.Chat.TextArea.SetText(string.Empty);
                    return false;
                }
                if (GameData.currentGame.GameState == GameStates.Started)
                {

                }
                string[] args = msg.Split(' ');
                if (msg.StartsWith("/anonymous", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (args.Length < 2 || (!args[1].Equals("on", StringComparison.InvariantCultureIgnoreCase) && !args[1].Equals("off", StringComparison.InvariantCultureIgnoreCase)))
                    {
                        HudManager.IAINKLDJAGC.Chat.AddChat(localPlayer.PlayerControl, "Incorrect use: /anonymous on|off");
                        HudManager.IAINKLDJAGC.Chat.TextArea.SetText(string.Empty);
                        return false;
                    }
                    if (args[1].Equals("on", StringComparison.InvariantCultureIgnoreCase))
                    {
                        CustomGameOptions.anonymousImpostorsEnabled = true;
                        HudManager.IAINKLDJAGC.Chat.AddChat(localPlayer.PlayerControl, "Anonymous Impostors [37ff00ff]enabled");
                    }
                    else if (args[1].Equals("off", StringComparison.InvariantCultureIgnoreCase))
                    {
                        CustomGameOptions.anonymousImpostorsEnabled = false;
                        HudManager.IAINKLDJAGC.Chat.AddChat(localPlayer.PlayerControl, "Anonymous Impostors [ff0000ff]disabled");
                    }
                } else if (msg.StartsWith("/soloimpostor", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (args.Length < 2 || (!args[1].Equals("on", StringComparison.InvariantCultureIgnoreCase) && !args[1].Equals("off", StringComparison.InvariantCultureIgnoreCase)))
                    {
                        HudManager.IAINKLDJAGC.Chat.AddChat(localPlayer.PlayerControl, "Incorrect use: /soloimpostor on|off");
                        HudManager.IAINKLDJAGC.Chat.TextArea.SetText(string.Empty);
                        return false;
                    }
                    if (!CustomGameOptions.anonymousImpostorsEnabled)
                    {
                        HudManager.IAINKLDJAGC.Chat.AddChat(localPlayer.PlayerControl, "Anonymous Impostors must be enabled to change this setting.");
                        HudManager.IAINKLDJAGC.Chat.TextArea.SetText(string.Empty);
                        return false;
                    }
                    if (args[1].Equals("on", StringComparison.InvariantCultureIgnoreCase))
                    {
                        CustomGameOptions.impostorSoloWin = true;
                        HudManager.IAINKLDJAGC.Chat.AddChat(localPlayer.PlayerControl, "Impostors must win solo [37ff00ff]enabled");
                    }
                    else if (args[1].Equals("off", StringComparison.InvariantCultureIgnoreCase))
                    {
                        CustomGameOptions.impostorSoloWin = false;
                        HudManager.IAINKLDJAGC.Chat.AddChat(localPlayer.PlayerControl, "Impostors must win solo [ff0000ff]disabled");
                    }
                }
                HudManager.IAINKLDJAGC.Chat.TextArea.SetText(string.Empty);
                localPlayer.PlayerControl.RpcSyncSettings(PlayerControl.GameOptions);
                return false;
            }
        }
    }
}
