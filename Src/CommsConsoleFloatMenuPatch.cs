using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace LongDistance
{
    [HarmonyPatch(typeof(Building_CommsConsole), nameof(Building_CommsConsole.GetFloatMenuOptions))]
    public class CommsConsoleFloatMenuPatch
    {
        public static void Postfix(Building_CommsConsole __instance, ref IEnumerable<FloatMenuOption> __result, Pawn myPawn)
        {
            foreach (var r in __result)
            {
                if (r.action == null)
                    return;
            }

            var res = new List<FloatMenuOption>(__result);

            HashSet<Pawn> toCheck = new HashSet<Pawn>();
            toCheck.AddRange(myPawn.relations.FamilyByBlood);
            foreach (var rel in myPawn.relations.DirectRelations)
                toCheck.Add(rel.otherPawn);

            foreach (var otherPawn in toCheck)
            {
                var rel = myPawn.GetMostImportantRelation(otherPawn);

                if (
                    otherPawn.IsColonistPlayerControlled ||
                    otherPawn.Faction == myPawn.Faction ||
                    otherPawn.Map == myPawn.Map ||
                    otherPawn.Dead ||
                    otherPawn.IsPrisoner ||
                    !Utils.IsInvitableRelationship(rel)
                ) continue;

                res.Add(new FloatMenuOption(
                    "LongDistance.Invite".Translate(
                        otherPawn.Named("PAWN"),
                        rel.GetGenderSpecificLabel(otherPawn).CapitalizeFirst().Named("REL"),
                        otherPawn.Faction.Named("FACTION")
                    ),
                    () => myPawn.jobs.TryTakeOrderedJob(new Job(LongDistanceMod.InviteJob, otherPawn, __instance)),
                    otherPawn.Faction.def.FactionIcon,
                    otherPawn.Faction.Color
                ));

                if (Utils.IsBreakableRelationship(rel))
                {
                    res.Add(new FloatMenuOption(
                        "LongDistance.Breakup".Translate(
                            otherPawn.Named("PAWN"),
                            rel.GetGenderSpecificLabel(otherPawn).CapitalizeFirst().Named("REL"),
                            otherPawn.Faction.Named("FACTION")
                        ),
                        () => myPawn.jobs.TryTakeOrderedJob(new Job(LongDistanceMod.BreakupJob, otherPawn, __instance)),
                        otherPawn.Faction.def.FactionIcon,
                        otherPawn.Faction.Color
                    ));
                }
            }

            __result = res;
        }
    }
}