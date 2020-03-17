using HugsLib;
using HugsLib.Settings;
using RimWorld;
using Verse;

namespace LongDistance
{
    public class LongDistanceMod : ModBase
    {
        internal static JobDef InviteJob, BreakupJob;
        internal static ThoughtDef RejectedThoughtInviter, RejectedThoughtTarget, RejectedThoughtInviterMood, AcceptedThoughtInviter, AcceptedThoughtTarget, AcceptedThoughtInviterMood;

        internal static SettingHandle<float> MinDaysToJoin, MaxDaysToJoin, JoinBaseChance, JoinOpinionFactor, JoinSkillFactor, JoinFactionFactor, BreakupBaseChance, BreakupOpinionFactor;
        internal static SettingHandle<int> PositiveRelationTreshold, NegativeRelationTreshold;

        public override string ModIdentifier => "LongDistance";
        public override void DefsLoaded()
        {
            InviteJob = DefDatabase<JobDef>.GetNamed("bdew_longdistance_invite");
            BreakupJob = DefDatabase<JobDef>.GetNamed("bdew_longdistance_breakup");
            RejectedThoughtInviter = DefDatabase<ThoughtDef>.GetNamed("bdew_longdistance_inv_rejected");
            RejectedThoughtTarget = DefDatabase<ThoughtDef>.GetNamed("bdew_longdistance_inv_rejected_target");
            RejectedThoughtInviterMood = DefDatabase<ThoughtDef>.GetNamed("bdew_longdistance_inv_rejected_mood");
            AcceptedThoughtInviter = DefDatabase<ThoughtDef>.GetNamed("bdew_longdistance_inv_accepted");
            AcceptedThoughtTarget = DefDatabase<ThoughtDef>.GetNamed("bdew_longdistance_inv_accepted_target");
            AcceptedThoughtInviterMood = DefDatabase<ThoughtDef>.GetNamed("bdew_longdistance_inv_accepted_mood");

            MinDaysToJoin = Settings.GetHandle("MinDaysToJoin", "LongDistance.Settings.MinDaysToJoin.Name".Translate(), "LongDistance.Settings.MinDaysToJoin.Desc".Translate(), 0.5f);
            MaxDaysToJoin = Settings.GetHandle("MaxDaysToJoin", "LongDistance.Settings.MaxDaysToJoin.Name".Translate(), "LongDistance.Settings.MaxDaysToJoin.Desc".Translate(), 3f);

            JoinBaseChance = Settings.GetHandle("JoinBaseChance", "LongDistance.Settings.JoinBaseChance.Name".Translate(), "LongDistance.Settings.JoinBaseChance.Desc".Translate(), 0f);
            JoinOpinionFactor = Settings.GetHandle("JoinOpinionFactor", "LongDistance.Settings.JoinOpinionFactor.Name".Translate(), "LongDistance.Settings.JoinOpinionFactor.Desc".Translate(), 0.01f);
            JoinSkillFactor = Settings.GetHandle("JoinSkillFactor", "LongDistance.Settings.JoinSkillFactor.Name".Translate(), "LongDistance.Settings.JoinSkillFactor.Desc".Translate(), 0.1f);
            JoinFactionFactor = Settings.GetHandle("JoinFactionFactor", "LongDistance.Settings.JoinFactionFactor.Name".Translate(), "LongDistance.Settings.JoinFactionFactor.Desc".Translate(), 0.002f);

            BreakupBaseChance = Settings.GetHandle("BreakupBaseChance", "LongDistance.Settings.BreakupBaseChance.Name".Translate(), "LongDistance.Settings.BreakupBaseChance.Desc".Translate(), 0.2f);
            BreakupOpinionFactor = Settings.GetHandle("BreakupOpinionFactor", "LongDistance.Settings.BreakupOpinionFactor.Name".Translate(), "LongDistance.Settings.BreakupOpinionFactor.Desc".Translate(), 0.02f);

            PositiveRelationTreshold = Settings.GetHandle("PositiveRelationTreshold", "LongDistance.Settings.PositiveRelationTreshold.Name".Translate(), "LongDistance.Settings.PositiveRelationTreshold.Desc".Translate(), 50);
            NegativeRelationTreshold = Settings.GetHandle("NegativeRelationTreshold", "LongDistance.Settings.NegativeRelationTreshold.Name".Translate(), "LongDistance.Settings.NegativeRelationTreshold.Desc".Translate(), -50);
        }
    }
}