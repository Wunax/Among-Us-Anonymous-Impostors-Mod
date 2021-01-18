using System.Collections.Generic;
using HarmonyLib;
using Hazel;
using AnonymousImpostorsMod.API;

using PlayerControl = FFGALNAPKCD;
using NetClient = FMLLKEACGIO;

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
                    case (byte) CustomGameOptions.customGameOptionsRpc.impostorSoloWin:
                        {
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
                    MessageWriter messageWriter = NetClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte) CustomGameOptions.customGameOptionsRpc.impostorSoloWin, SendOption.None, -1);
                    messageWriter.Write(CustomGameOptions.impostorSoloWin);
                    NetClient.Instance.FinishRpcImmediately(messageWriter);
                }
            }
        }
    }
}
