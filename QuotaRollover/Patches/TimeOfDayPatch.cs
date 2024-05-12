using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotaRollover.Patches
{
    [HarmonyPatch(typeof(TimeOfDay))]
    internal class TimeOfDayPatch
    {
        [HarmonyPatch("SetNewProfitQuota")]
        [HarmonyPrefix, HarmonyAfter]
        static bool GetQuotaFulfilledHost(ref int ___quotaFulfilled, ref int ___profitQuota, out int __state)
        {
            QuotaRolloverBase.logger.LogInfo($"days: {TimeOfDay.Instance.daysUntilDeadline} time: {TimeOfDay.Instance.timeUntilDeadline} ID: {StartOfRound.Instance.currentLevelID}");

            if (TimeOfDay.Instance.timeUntilDeadline <= 0)
            {
                __state = ___quotaFulfilled - ___profitQuota;
                QuotaRolloverBase.logger.LogInfo($"Host Got New Quota at: {__state} ful: {___quotaFulfilled}");
                return true;
            }
            QuotaRolloverBase.logger.LogInfo("returned FALSE");
            __state = ___quotaFulfilled;
            return false;
        }

        [HarmonyPatch("SetNewProfitQuota")]
        [HarmonyPostfix, HarmonyBefore]
        static void SetQuotaFulfilledHost(ref int ___quotaFulfilled, int __state)
        {
            ___quotaFulfilled = __state;
            QuotaRolloverBase.logger.LogInfo($"Host Set New Quota at: {__state}");
        }


        [HarmonyPatch("SyncNewProfitQuotaClientRpc")]
        [HarmonyPrefix]
        static void GetNewQuotaFulfilledClient(ref int ___quotaFulfilled, ref int ___profitQuota, out int __state)
        {
            __state = ___quotaFulfilled - ___profitQuota;
            QuotaRolloverBase.logger.LogInfo($"Client Got New Quota at: {__state}");
        }

        [HarmonyPatch("SyncNewProfitQuotaClientRpc")]
        [HarmonyPostfix]
        static void SetNewQuotaFulfiledClient(ref int ___quotaFulfilled, int __state)
        {
            if (___quotaFulfilled == 0)
            {
                ___quotaFulfilled = __state;
                QuotaRolloverBase.logger.LogInfo($"Client Set New Quota at: {__state}");
            }
        }
    }
}
