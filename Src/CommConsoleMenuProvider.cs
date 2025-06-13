using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace LongDistance.Src
{
    public class CommConsoleMenuProvider : FloatMenuOptionProvider
    {
        protected override bool Drafted => false;

        protected override bool Undrafted => true;

        protected override bool Multiselect => false;

        protected override bool RequiresManipulation => true;

        public override bool SelectedPawnValid(Pawn pawn, FloatMenuContext context)
        {
            if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking)) return false;
            return base.SelectedPawnValid(pawn, context);
        }

        public override bool TargetThingValid(Thing thing, FloatMenuContext context)
        {
            if (thing is Building_CommsConsole console)
            {
                if (!context.FirstSelectedPawn.CanReach(thing, PathEndMode.InteractionCell, Danger.Some)) return false;
                if (!console.CanUseCommsNow) return false;
                return true;
            }
            return false;
        }

        public override IEnumerable<FloatMenuOption> GetOptionsFor(Thing clickedThing, FloatMenuContext context)
        {
            var myPawn = context.FirstSelectedPawn;

            HashSet<Pawn> toCheck = new HashSet<Pawn>();
            toCheck.AddRange(myPawn.relations.FamilyByBlood);
            foreach (var rel in myPawn.relations.DirectRelations)
                toCheck.Add(rel.otherPawn);

            foreach (var otherPawn in toCheck)
            {
                if (otherPawn == null || otherPawn.relations == null) continue;

                var rel = myPawn.GetMostImportantRelation(otherPawn);
                if (rel == null) continue;

                if (Utils.CanPawnBeInvited(myPawn, otherPawn) && Utils.IsInvitableRelationship(rel))
                {
                    yield return MakeOpt("LongDistance.Invite", otherPawn, rel, () =>
                        myPawn.jobs.TryTakeOrderedJob(new Job(LongDistanceDefs.InviteJob, otherPawn, clickedThing))
                    );

                    if (Utils.IsBreakableRelationship(rel))
                    {
                        yield return MakeOpt("LongDistance.Breakup", otherPawn, rel, () =>
                            myPawn.jobs.TryTakeOrderedJob(new Job(LongDistanceDefs.BreakupJob, otherPawn, clickedThing))
                        );
                    }
                }
            }
        }

        private static FloatMenuOption MakeOpt(string key, Pawn pawn, PawnRelationDef rel, Action action)
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
    }
}
