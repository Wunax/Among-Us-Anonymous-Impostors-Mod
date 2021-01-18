using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

using PingTracker = ELDIDNABIPI;

namespace AnonymousImpostorsMod
{
    [HarmonyPatch]
    public static class PingTrackerPatch
    {
        [HarmonyPatch(typeof(PingTracker), nameof(PingTracker.Update))]
        public static void Postfix(PingTracker __instance)
        {
            __instance.text.Centered = true;
            if (CustomGameOptions.anonymousImpostorsEnabled)
                __instance.text.Text += "\nAnonymous Impostors [37ff00ff]enabled";
            else
                __instance.text.Text += "\nAnonymous Impostors [ff0000ff]disabled";
        }
    }
}
