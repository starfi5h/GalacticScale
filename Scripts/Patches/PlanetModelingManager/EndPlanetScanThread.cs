using System.Collections.Generic;
using HarmonyLib;
using static PlanetModelingManager;

namespace GalacticScale
{
    public partial class PatchOnPlanetModelingManager
    {
        [HarmonyPrefix, HarmonyPatch(typeof(PlanetModelingManager), nameof(PlanetModelingManager.EndPlanetScanThread))]
        public static bool EndPlanetScanThread()
        {
            Queue<PlanetData> queue = scnPlanetReqList;
            lock (queue)
            {
                scnPlanetReqList.Clear();
            }
            ThreadFlagLock threadFlagLock = planetScanThreadFlagLock;
            lock (threadFlagLock)
            {
                // Change: If the ThreadFlag is already ended, don't change it to ending
                if (planetScanThreadFlag == ThreadFlag.Running)
                {
                    planetScanThreadFlag = ThreadFlag.Ending;
                }
            }
            return false;
        }
    }
}
