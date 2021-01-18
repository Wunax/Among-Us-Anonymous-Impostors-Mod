using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnhollowerBaseLib;
using UnityEngine;

using GameOptionsMenu = PHCKLDDNJNP;
using OptionBehaviour = LLKOLCLGCBD;
using ToggleOption = BCLDBBKFJPK;

namespace AnonymousImpostorsMod
{
    public static class GameOptionsMenuPatch
    {
        [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Start))]
        public static class GameOptionsMenuStartPatch
        {
            public static void Postfix(GameOptionsMenu __instance)
            {
                if (!CustomGameOptions.anonymousImpostorsEnabled)
                    return;
                if (UnityEngine.Object.FindObjectsOfType<ToggleOption>().Count == 4)
                {
                    ToggleOption original = (from x in UnityEngine.Object.FindObjectsOfType<ToggleOption>().ToList<ToggleOption>()
                                            where x.TitleText.Text == "Anonymous Votes"
                                            select x).First<ToggleOption>();
                    GameOptionsMenuUpdatePatch.impostorSoloWin = UnityEngine.Object.Instantiate<ToggleOption>(original);
                    GameOptionsMenuUpdatePatch.impostorSoloWin.TitleText.Text = "Impostor Solo Win";
                    GameOptionsMenuUpdatePatch.impostorSoloWin.NHLMDAOEOAE = CustomGameOptions.impostorSoloWin;
                    GameOptionsMenuUpdatePatch.impostorSoloWin.CheckMark.enabled = CustomGameOptions.impostorSoloWin;
                    OptionBehaviour[] array = new OptionBehaviour[__instance.KJFHAPEDEBH.Count + 1];
                    __instance.KJFHAPEDEBH.ToArray<OptionBehaviour>().CopyTo(array, 0);
                    array[array.Length - 1] = GameOptionsMenuUpdatePatch.impostorSoloWin;
                    __instance.KJFHAPEDEBH = new Il2CppReferenceArray<OptionBehaviour>(array);
                }
            }     
        }

        [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Update))]
        public static class GameOptionsMenuUpdatePatch
        {
            public static ToggleOption impostorSoloWin;

            public static void Postfix(GameOptionsMenu __instance)
            {
                if (!CustomGameOptions.anonymousImpostorsEnabled)
                    return;
                ToggleOption option = (from x in UnityEngine.Object.FindObjectsOfType<ToggleOption>().ToList<ToggleOption>()
                                           where x.TitleText.Text == "Anonymous Votes"
                                           select x).First<ToggleOption>();
                impostorSoloWin.transform.position = option.transform.position - new Vector3(0f, 5.5f, 0f);
            }
        }
    }
}
