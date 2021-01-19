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
                if (UnityEngine.Object.FindObjectsOfType<ToggleOption>().Count == 4)
                {
                    ToggleOption anonymousVote = GameObject.FindObjectsOfType<ToggleOption>().ToList().Where(x => x.TitleText.Text == "Anonymous Votes").First();

                    GameOptionsMenuUpdatePatch.anonymousImpostors = GameObject.Instantiate(anonymousVote);
                    GameOptionsMenuUpdatePatch.anonymousImpostors.TitleText.Text = "Anonymous Impostors";
                    GameOptionsMenuUpdatePatch.anonymousImpostors.NHLMDAOEOAE = CustomGameOptions.anonymousImpostorsEnabled;
                    GameOptionsMenuUpdatePatch.anonymousImpostors.CheckMark.enabled = CustomGameOptions.anonymousImpostorsEnabled;

                    GameOptionsMenuUpdatePatch.impostorSoloWin = GameObject.Instantiate(anonymousVote);
                    GameOptionsMenuUpdatePatch.impostorSoloWin.TitleText.Text = "Impostor Solo Win";
                    GameOptionsMenuUpdatePatch.impostorSoloWin.NHLMDAOEOAE = CustomGameOptions.impostorSoloWin;
                    GameOptionsMenuUpdatePatch.impostorSoloWin.CheckMark.enabled = CustomGameOptions.impostorSoloWin;

                    OptionBehaviour[] options = new OptionBehaviour[__instance.KJFHAPEDEBH.Count + 2];
                    __instance.KJFHAPEDEBH.ToArray().CopyTo(options, 0);
                    options[options.Length - 2] = GameOptionsMenuUpdatePatch.anonymousImpostors;
                    options[options.Length - 1] = GameOptionsMenuUpdatePatch.impostorSoloWin;
                    __instance.KJFHAPEDEBH = new Il2CppReferenceArray<OptionBehaviour>(options);
                }
            }     
        }

        [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Update))]
        public static class GameOptionsMenuUpdatePatch
        {
            public static ToggleOption anonymousImpostors;
            public static ToggleOption impostorSoloWin;

            public static void Postfix()
            {
                ToggleOption option = GameObject.FindObjectsOfType<BCLDBBKFJPK>().ToList().Where(x => x.TitleText.Text == "Anonymous Votes").First();
                if (anonymousImpostors != null)
                    anonymousImpostors.transform.position = option.transform.position - new Vector3(0f, 5.5f, 0f);
                if (CustomGameOptions.anonymousImpostorsEnabled && impostorSoloWin != null)
                    impostorSoloWin.transform.position = option.transform.position - new Vector3(0f, 6f, 0f);
            }
        }
    }
}
