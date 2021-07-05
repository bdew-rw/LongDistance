using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;
using System;
using System.Reflection;
using UnityEngine;

namespace LongDistance
{
    [HarmonyPatch(typeof(Building_CommsConsole), nameof(Building_CommsConsole.GetFloatMenuOptions))]
    public class CommsConsoleFloatMenuPatch
    {
        private static MethodInfo mGetFailureReason;

        private static FloatMenuOption MakeOpt(string key, Pawn pawn, PawnRelationDef rel, Building_CommsConsole console, Action action)
        {
            return new FloatMenuOption(
                key.Translate(
                    pawn.Named("PAWN"),
                    rel.GetGenderSpecificLabel(pawn).CapitalizeFirst().Named("REL"),
                    pawn.Faction == null ? "LongDistance.Neutral".Translate().Named("FACTION") : pawn.Faction.Name.Named("FACTION")
                ),
                action,
                pawn.Faction?.def.FactionIcon,
                pawn.Faction == null ? Color.white : pawn.Faction.Color
            );
        }

        public static void Postfix(Building_CommsConsole __instance, ref IEnumerable<FloatMenuOption> __result, Pawn myPawn)
        {
            if (mGetFailureReason == null)
            {
                mGetFailureReason = typeof(Building_CommsConsole).GetMethod("GetFailureReason", BindingFlags.NonPublic | BindingFlags.Instance);
                Log.Message("Grabbed GetFailureReason: " + mGetFailureReason.ToString());
            }

            if (mGetFailureReason.Invoke(__instance, new object[] { myPawn }) != null) return;

            var res = new List<FloatMenuOption>(__result);

            HashSet<Pawn> toCheck = new HashSet<Pawn>();
            toCheck.AddRange(myPawn.relations.FamilyByBlood);
            foreach (var rel in myPawn.relations.DirectRelations)
                toCheck.Add(rel.otherPawn);

            foreach (var otherPawn in toCheck)
            {
                if (otherPawn == null || otherPawn.relations == null) continue;

                var rel = myPawn.GetMostImportantRelation(otherPawn);
                if (rel == null) continue;

                if (
                    otherPawn.IsColonistPlayerControlled ||
                    otherPawn.Faction == myPawn.Faction ||
                    otherPawn.Map == myPawn.Map ||
                    otherPawn.Dead ||
                    otherPawn.IsPrisoner ||
                    !Utils.IsInvitableRelationship(rel)
                ) continue;

                res.Add(MakeOpt("LongDistance.Invite", otherPawn, rel, __instance,
                    () => myPawn.jobs.TryTakeOrderedJob(new Job(LongDistanceMod.InviteJob, otherPawn, __instance))
                ));

                if (Utils.IsBreakableRelationship(rel))
                {
                    res.Add(MakeOpt("LongDistance.Breakup", otherPawn, rel, __instance,
                        () => myPawn.jobs.TryTakeOrderedJob(new Job(LongDistanceMod.BreakupJob, otherPawn, __instance))
                    ));
                }
            }

            __result = res;
        }
    }
}