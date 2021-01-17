using System.Collections.Generic;
using HarmonyLib;
using AnonymousImpostorsMod.API;

using PlayerControl = FFGALNAPKCD;

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
    }
}
