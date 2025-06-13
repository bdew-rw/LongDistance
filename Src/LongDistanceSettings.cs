using Verse;

namespace LongDistance
{
    public class LongDistanceSettings : ModSettings
    {
        public static float MinDaysToJoin = 0.5f;
        public static float MaxDaysToJoin = 3f;
        public static float JoinBaseChance = 0f;
        public static float JoinOpinionFactor = 0.01f;
        public static float JoinSkillFactor = 0.1f;
        public static float JoinFactionFactor = 0.002f;
        public static float BreakupBaseChance = 0.2f;
        public static float BreakupOpinionFactor = 0.02f;
        public static float PositiveRelationTreshold = 50f;
        public static float NegativeRelationTreshold = -50f;
        public static bool AllowLeftBehind = false;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref MinDaysToJoin, "MinDaysToJoin", 0.5f);
            Scribe_Values.Look(ref MaxDaysToJoin, "MaxDaysToJoin", 3f);
            Scribe_Values.Look(ref JoinBaseChance, "JoinBaseChance", 0f);
            Scribe_Values.Look(ref JoinOpinionFactor, "JoinOpinionFactor", 0.01f);
            Scribe_Values.Look(ref JoinSkillFactor, "JoinSkillFactor", 0.1f);
            Scribe_Values.Look(ref JoinFactionFactor, "JoinFactionFactor", 0.002f);
            Scribe_Values.Look(ref BreakupBaseChance, "BreakupBaseChance", 0.2f);
            Scribe_Values.Look(ref BreakupOpinionFactor, "BreakupOpinionFactor", 0.02f);
            Scribe_Values.Look(ref PositiveRelationTreshold, "PositiveRelationTreshold", 50f);
            Scribe_Values.Look(ref NegativeRelationTreshold, "NegativeRelationTreshold", -50f);
            Scribe_Values.Look(ref AllowLeftBehind, "AllowLeftBehind", false);
            base.ExposeData();
        }
    }
}