using HarmonyLib;
using AnonymousImpostorsMod.API;

using ChatController = GEHKHGLKFHE;
using PlayerControl = FFGALNAPKCD;

namespace AnonymousImpostorsMod
{
    [HarmonyPatch]
    public static class ChatControllerPatch
    {
        public static bool isImpostor;

        [HarmonyPatch(typeof(ChatController), "AddChat")]
        public static void Prefix(PlayerControl KMCAKLLFNIM)
        {
            PlayerController player = new PlayerController(KMCAKLLFNIM);
            if (player.PlayerData.IsImpostor && !PlayerController.GetLocalPlayer().Equals(player))
            {
                player.PlayerData.IsImpostor = false;
                isImpostor = true;
            }
        }

        [HarmonyPatch(typeof(ChatController), "AddChat")]
        public static void Postfix(PlayerControl KMCAKLLFNIM)
        {
            PlayerController player = new PlayerController(KMCAKLLFNIM);
            if (isImpostor)
            {
                player.PlayerData.IsImpostor = true;
                isImpostor = false;
            }
        }
    }
}
