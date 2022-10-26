using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

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
            base.ExposeData();
        }
    }


    public class LongDistanceMod : Mod
    {
        public readonly LongDistanceSettings settings;

        public LongDistanceMod(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<LongDistanceSettings>();
            var harmony = new Harmony("BDew.LongDistance");
            harmony.PatchAll();
            Log.Message("Long Distance loaded");
        }

        public override string SettingsCategory()
        {
            return "LongDistance.Name".Translate();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);

            AddSettingsNumberLine(listing, "MinDaysToJoin", ref LongDistanceSettings.MinDaysToJoin, 0.1f, 5f, "F1");
            AddSettingsNumberLine(listing, "MaxDaysToJoin", ref LongDistanceSettings.MaxDaysToJoin, 0.1f, 5f, "F1");
            AddSettingsNumberLine(listing, "JoinBaseChance", ref LongDistanceSettings.JoinBaseChance, 0f, 1f, "F3");
            AddSettingsNumberLine(listing, "JoinOpinionFactor", ref LongDistanceSettings.JoinOpinionFactor, 0f, 1f, "F3");
            AddSettingsNumberLine(listing, "JoinSkillFactor", ref LongDistanceSettings.JoinSkillFactor, 0f, 1f, "F3");
            AddSettingsNumberLine(listing, "JoinFactionFactor", ref LongDistanceSettings.JoinFactionFactor, 0f, 1f, "F3");
            AddSettingsNumberLine(listing, "BreakupBaseChance", ref LongDistanceSettings.BreakupBaseChance, 0f, 1f, "F3");
            AddSettingsNumberLine(listing, "BreakupOpinionFactor", ref LongDistanceSettings.BreakupOpinionFactor, 0f, 1f, "F3");
            AddSettingsNumberLine(listing, "PositiveRelationTreshold", ref LongDistanceSettings.PositiveRelationTreshold, -100f, 100f, "N0");
            AddSettingsNumberLine(listing, "NegativeRelationTreshold", ref LongDistanceSettings.NegativeRelationTreshold, -100f, 100f, "N0");

            listing.End();

            base.DoSettingsWindowContents(inRect);
        }

        private static void AddSettingsNumberLine(Listing_Standard listing, string name, ref float val, float min, float max, string fmt)
        {
            var rect = listing.GetRect(30f);

            var rectLabel = rect.LeftPart(0.5f).Rounded();
            var rectRight = rect.RightPart(0.5f).Rounded();
            var rectField = rectRight.LeftPart(0.25f).Rounded();
            var rectSlider = rectRight.RightPart(0.7f).Rounded();

            Text.Anchor = TextAnchor.MiddleLeft;

            TooltipHandler.TipRegion(rect, $"LongDistance.Settings.{name}.Desc".Translate());

            Widgets.Label(rectLabel, $"LongDistance.Settings.{name}.Name".Translate());

            string buffer = val.ToString(fmt);
            Widgets.TextFieldNumeric(rectField, ref val, ref buffer, min, max);

            Text.Anchor = TextAnchor.UpperLeft;

            float num = Widgets.HorizontalSlider(rectSlider, val, min, max, middleAlignment: true);

            if (num != val)
            {
                SoundDefOf.DragSlider.PlayOneShotOnCamera();
                val = num;
            }

            listing.Gap(2f);
        }
    }
}