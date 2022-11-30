using HarmonyLib;
using PulsarModLoader.Patches;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using static PulsarModLoader.Patches.HarmonyHelpers;

namespace Max_Players
{
    [HarmonyPatch(typeof(PLSaveGameIO), "SaveToFile")]
    class SaveFix
    {
        static void WriteItemCountPatch(BinaryWriter writer, int num, int classID, PLPlayer cachedPlayerOfClass)
        {
            List<PLPawnItem> itemList = new List<PLPawnItem>();
            foreach (PLPlayer player in PLServer.Instance.AllPlayers)
            {
                if (player != null && player.TeamID == 0 && player.GetClassID() == classID && cachedPlayerOfClass != player)
                {
                    foreach (PLPawnItem item in player.MyInventory.AllItems)
                    {
                        if (item != null)
                        {
                            itemList.Add(item);
                        }
                    }
                }
            }
            num += itemList.Count;
            writer.Write(num);
            foreach (PLPawnItem item in itemList)
            {
                writer.Write((int)item.PawnItemType);
                writer.Write(item.SubType);
                writer.Write(item.Level);
            }
        }
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> findSequence = new List<CodeInstruction>()
            {
                new CodeInstruction(OpCodes.Ldloc_S),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(BinaryWriter), "Write", new Type[] { typeof(int) })),
                new CodeInstruction(OpCodes.Ldloc_S),
                new CodeInstruction(OpCodes.Ldnull)
            };
            int hashcode = instructions.ToArray()[(FindSequence(instructions, findSequence, CheckMode.NONNULL) - 4)].operand.GetHashCode();

            List<CodeInstruction> patchSequence = new List<CodeInstruction>()
            {
                new CodeInstruction(OpCodes.Ldloc_S, (byte)23),
                new CodeInstruction(OpCodes.Ldloc_S, (byte)18),
                new CodeInstruction(OpCodes.Ldloc_S, (byte)19),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SaveFix), "WriteItemCountPatch"))
            };
            var Instructions = instructions.ToList();
            for (int i = 0; i < Instructions.Count; i++)
            {
                if (Instructions[i].opcode == OpCodes.Ldloc_S)
                {
                    if (Instructions[i].operand.GetHashCode() == hashcode)
                    {
                        if (Instructions[i + 1].opcode == OpCodes.Callvirt && (MethodInfo)Instructions[i + 1].operand == AccessTools.Method(typeof(BinaryWriter), "Write", new Type[] { typeof(int) }))
                        {
                            Instructions.RemoveRange(i, 2);
                            Instructions.InsertRange(i, patchSequence.Select(c => c.FullClone()));
                            break;
                        }
                    }
                }
            }
            return Instructions.AsEnumerable();

            /*List<CodeInstruction> targetSequence = new List<CodeInstruction>()
            {
                new CodeInstruction(OpCodes.Ldloc_S, operand),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(BinaryWriter), "Write", new Type[] { typeof(int) }))
            };

            List<CodeInstruction> injectionSequence = new List<CodeInstruction>()
            {
                new CodeInstruction(OpCodes.Ldloc_S, (byte)23),
                new CodeInstruction(OpCodes.Ldloc_S, (byte)18),
                new CodeInstruction(OpCodes.Ldloc_S, (byte)19),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SaveFix), "WriteItemCountPatch"))
            };

            return PatchBySequence(instructions, targetSequence, injectionSequence, PatchMode.REPLACE, CheckMode.ALWAYS, true); //Failed to find first ldloc_s*/
        }
    }
}
