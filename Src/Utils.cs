using Verse;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace LongDistance
{
    class Utils
    {
        public static void HandleInvite(Pawn inviter, Pawn target)
        {
            float successChance = LongDistanceSettings.JoinBaseChance +
                (target.relations.OpinionOf(inviter) * LongDistanceSettings.JoinOpinionFactor) +
                (inviter.skills.GetSkill(SkillDefOf.Social).Level * LongDistanceSettings.JoinSkillFactor);

            if (target.Faction != null)
                successChance += target.Faction.GoodwillWith(inviter.Faction) * LongDistanceSettings.JoinFactionFactor;

            successChance = Mathf.Clamp01(successChance);

            if (Rand.Value < successChance)
            {
                if (target.needs.mood != null)
                {
                    target.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(LongDistanceDefs.RejectedThoughtTarget, inviter);
                    target.needs.mood.thoughts.memories.TryGainMemory(LongDistanceDefs.AcceptedThoughtTarget, inviter);
                }

                if (inviter.needs.mood != null)
                {
                    inviter.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(LongDistanceDefs.RejectedThoughtInviter, target);
                    inviter.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(LongDistanceDefs.RejectedThoughtInviterMood, target);
                    inviter.needs.mood.thoughts.memories.TryGainMemory(LongDistanceDefs.AcceptedThoughtInviter, target);
                }

                Faction oldFaction = target.Faction;
                int relChange = 0;

                target.SetFaction(inviter.Faction, inviter);

                if (oldFaction != null)
                {
                    int rel = oldFaction.GoodwillWith(inviter.Faction);
                    if (rel > LongDistanceSettings.PositiveRelationTreshold)
                        relChange = Rand.RangeInclusive(2, 4) * 5;
                    else if (rel < LongDistanceSettings.NegativeRelationTreshold)
                        relChange = -Rand.RangeInclusive(2, 4) * 5;
                    else
                        relChange = Rand.RangeInclusive(-3, 3) * 5;
                }

                if (relChange != 0)
                    oldFaction.TryAffectGoodwillWith(inviter.Faction, relChange, true, true, LongDistanceDefs.RecruitedHistoryEvent, inviter);

                Letters.SendAcceptedLetter(target, inviter, inviter.Map, oldFaction, relChange);

                Find.World.GetComponent<ArrivalsManager>().Schedule(target, inviter, inviter.Map,
                    Mathf.FloorToInt(Rand.Range(LongDistanceSettings.MinDaysToJoin, LongDistanceSettings.MaxDaysToJoin) * GenDate.TicksPerDay));
            }
            else
            {
                if (target.needs.mood != null)
                    target.needs.mood.thoughts.memories.TryGainMemory(LongDistanceDefs.RejectedThoughtTarget, inviter);

                if (inviter.needs.mood != null)
                    inviter.needs.mood.thoughts.memories.TryGainMemory(LongDistanceDefs.RejectedThoughtInviter, target);

                bool brokeUp = false;

                foreach (var rel in inviter.GetRelations(target))
                {
                    if (!IsBreakableRelationship(rel)) continue;

                    float breakupChance = Mathf.Clamp01(LongDistanceSettings.BreakupBaseChance - inviter.relations.OpinionOf(target) * LongDistanceSettings.BreakupOpinionFactor);

                    brokeUp = Rand.Value < breakupChance;

                    if (brokeUp) Breakup(inviter, target);
                }

                Letters.SendRejectLetter(target, inviter, inviter.Map, brokeUp);
            }
        }

        public static bool MovePawnToColony(Pawn pawn, Pawn inviter, Map destination)
        {
            Map map = destination;

            if (map.ParentFaction != Faction.OfPlayer)
            {
                if (inviter.IsCaravanMember() || PawnUtility.IsTravelingInTransportPodWorldObject(pawn) || inviter.Map.ParentFaction != Faction.OfPlayer)
                    map = Find.AnyPlayerHomeMap;
                else
                    map = inviter.Map;
            }

            if (!TryFindEntryCell(map, out IntVec3 cell))
                return false;

            if (pawn.apparel.AnyApparelLocked)
            {
                var req = new PawnGenerationRequest
                {
                    Tile = inviter.Map.Tile
                };
                PawnApparelGenerator.GenerateStartingApparelFor(pawn, req);
            }

            GenSpawn.Spawn(pawn, cell, map, WipeMode.Vanish);

            Letters.SendJoinLetter(pawn, inviter, map);

            return true;
        }

        private static bool TryFindEntryCell(Map map, out IntVec3 cell)
        {
            return CellFinder.TryFindRandomEdgeCellWith((c => map.reachability.CanReachColony(c) && !c.Fogged(map)), map, CellFinder.EdgeRoadChance_Friendly, out cell);
        }

        public static void Breakup(Pawn initiator, Pawn recipient)
        {
            if (initiator.relations.DirectRelationExists(PawnRelationDefOf.Spouse, recipient))
            {
                initiator.relations.RemoveDirectRelation(PawnRelationDefOf.Spouse, recipient);
                initiator.relations.AddDirectRelation(PawnRelationDefOf.ExSpouse, recipient);
                if (recipient.needs.mood != null)
                {
                    recipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.DivorcedMe, initiator);
                    recipient.needs.mood.thoughts.memories.RemoveMemoriesOfDef(ThoughtDefOf.GotMarried);
                    recipient.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.HoneymoonPhase, initiator);
                }
                if (initiator.needs.mood != null)
                {
                    initiator.needs.mood.thoughts.memories.RemoveMemoriesOfDef(ThoughtDefOf.GotMarried);
                    initiator.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.HoneymoonPhase, recipient);
                }
                SpouseRelationUtility.ChangeNameAfterDivorce(initiator);
                SpouseRelationUtility.ChangeNameAfterDivorce(recipient);
            }
            else
            {
                initiator.relations.TryRemoveDirectRelation(PawnRelationDefOf.Lover, recipient);
                initiator.relations.TryRemoveDirectRelation(PawnRelationDefOf.Fiance, recipient);
                initiator.relations.AddDirectRelation(PawnRelationDefOf.ExLover, recipient);
                if (recipient.needs.mood != null)
                    recipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.BrokeUpWithMe, initiator);
            }
            TaleRecorder.RecordTale(TaleDefOf.Breakup, initiator, recipient);
        }

        public static bool IsInvitableRelationship(PawnRelationDef rel)
        {
            return rel.opinionOffset > 0 && rel != PawnRelationDefOf.Bond;
        }

        public static bool IsBreakableRelationship(PawnRelationDef rel)
        {
            return rel == PawnRelationDefOf.Spouse || rel == PawnRelationDefOf.Fiance || rel == PawnRelationDefOf.Lover;
        }
    }
}
