using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using QuotaRollover.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotaRollover
{
    [BepInPlugin("Boxofbiscuits97.QuotraRollover", "Quota Rollover", "2.4.0")]
    public class QuotaRolloverBase : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("Boxofbiscuits97.QuotraRollover");

        private static QuotaRolloverBase Instance;

        public static ManualLogSource logger;

        void Awake()
        {
            if (Instance == null) Instance = this;
            logger = Logger;

            logger.LogInfo("Mod Boxofbiscuits97.QuotraRollover is loaded!");

            harmony.PatchAll(typeof(QuotaRolloverBase));
            harmony.PatchAll(typeof(TimeOfDayPatch));
        }
    }
}
