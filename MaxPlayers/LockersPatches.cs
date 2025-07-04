using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;
using static PulsarModLoader.Patches.HarmonyHelpers;

namespace MaxPlayers
{
    [HarmonyPatch(typeof(PLTabMenu), "OnClickPID")]
    internal class PLTabMenuPatch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> targetSequence = new List<CodeInstruction>()
            {
                new CodeInstruction(OpCodes.Ldfld),
                new CodeInstruction(OpCodes.Ldfld),
                new CodeInstruction(OpCodes.Ldsfld),
                new CodeInstruction(OpCodes.Ldfld),
                new CodeInstruction(OpCodes.Call)
            };
            List<CodeInstruction> injectedSequence = new List<CodeInstruction>()
            {
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(PLTabMenuPatch), "PatchMethod")),
            };
            return PatchBySequence(instructions, targetSequence, injectedSequence, patchMode: PatchMode.REPLACE, CheckMode.NEVER);
        }
        static bool PatchMethod(PLTabMenu Instance)
        {
            return Instance.TargetContainer.PlayerOwner != null && Instance.TargetContainer.PlayerOwner.GetClassID() == PLNetworkManager.Instance.LocalPlayer.GetClassID();
        }
    }
}
