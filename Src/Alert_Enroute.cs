using System.Text;
using RimWorld;
using Verse;

namespace LongDistance.Src
{
    class Alert_Enroute : Alert
    {
        private ArrivalsManager Manager => Find.World.GetComponent<ArrivalsManager>();

        public override string GetLabel()
        {
            return "LongDistance.Enroute.Title".Translate();
        }

        public override TaggedString GetExplanation()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var row in Manager.entries)
            {
                sb.AppendLine("LongDistance.Enroute.Row".Translate(
                    row.pawn.Named("PAWN"),
                    row.inviter.Named("INVITER"),
                    row.inviter.GetMostImportantRelation(row.pawn).GetGenderSpecificLabel(row.pawn).Named("REL"),
                    ((row.ticks / 60000f).ToString("0.#") + " " + "Days".Translate()).Named("TIME")
                ));
            }

            return "LongDistance.Enroute.Text".Translate(sb.ToString());
        }

        public override AlertReport GetReport()
        {
            return Manager.entries.Count > 0 ? AlertReport.Active : AlertReport.Inactive;
        }
    }
}
