using BepInEx;
using BepInEx.IL2CPP;
using BepInEx.Logging;
using HarmonyLib;

namespace AnonymousImpostorsMod
{
    [BepInPlugin("org.bepinex.plugins.AnonymousImpostorsMod", "Anonymous Impostors Mod", version)]
    public class AnonymousImpostors : BasePlugin
    {
        public const string version = "1.3";

        public static ManualLogSource log;
        private readonly Harmony harmony;

        public AnonymousImpostors()
        {
            AnonymousImpostors.log = base.Log;
            this.harmony = new Harmony("Anonymous Impostors Mod");
        }

        public override void Load()
        {
            AnonymousImpostors.log.LogMessage($"Anonymous Impostors Mod loaded - v{version} by Wunax");
            this.harmony.PatchAll();
        }
    }
}
