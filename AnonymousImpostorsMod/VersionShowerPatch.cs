using HarmonyLib;

using TextRenderer = AELDHKGBIFD;

namespace AnonymousImpostorsMod
{
    [HarmonyPatch(typeof(BOCOFLHKCOJ), "Start")]
    public static class VersionShowerPatch
    {
        public static void Postfix(BOCOFLHKCOJ __instance)
        {
            TextRenderer text = __instance.text;
            text.Text += $"\n[3EE605ff]Anonymous Impostors Mod[ffffffff] loaded - v[1C26E6ff]{AnonymousImpostors.version}[ffffffff] by [E66100ff]Wunax";
        }
    }
}
