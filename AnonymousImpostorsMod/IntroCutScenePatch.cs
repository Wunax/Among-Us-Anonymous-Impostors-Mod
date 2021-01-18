using HarmonyLib;
using AnonymousImpostorsMod.API;

using PlayerControl = FFGALNAPKCD;
using IntroClass = PENEIDJGGAF.CKACLKCOJFO;

namespace AnonymousImpostorsMod
{
    [HarmonyPatch]
    public static class IntroCutScenePatch
    {
        [HarmonyPatch(typeof(IntroClass), "MoveNext")]
        public static void Prefix(IntroClass __instance)
        {
            if (!CustomGameOptions.anonymousImpostorsEnabled)
                return;
            PlayerController localPlayer = PlayerController.GetLocalPlayer();

            if (localPlayer.PlayerData.IsImpostor)
            {
                var team = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
                team.Add(localPlayer.PlayerControl);

                __instance.yourTeam = team;
            }
        }
    }
}
