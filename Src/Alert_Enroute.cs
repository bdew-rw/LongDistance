using RimWorld;
using System.Text;
using Verse;

namespace LongDistance.Src
{
    class Alert_Enroute : Alert
    {
        public override string GetLabel()
        {
            return "LongDistance.Enroute.Title".Translate();
        }

        public override TaggedString GetExplanation()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var row in ArrivalsManager.Current.Scheduled)
            {
                string rel;
                if (row.pawn.relations == null)
                {
                    Log.Warning($"Pawn {row.pawn.Name.ToStringFull} has null relations?");
                    rel = "???";
                }
                else if (row.inviter.relations == null)
                {
                    Log.Warning($"Inviter {row.inviter.Name.ToStringFull} has null relations?");
                    rel = "???";
                }
                else
                {
                    rel = row.inviter.GetMostImportantRelation(row.pawn).GetGenderSpecificLabel(row.pawn);
                }

                sb.AppendLine("LongDistance.Enroute.Row".Translate(
                    row.pawn.Named("PAWN"),
                    row.inviter.Named("INVITER"),
                    rel.Named("REL"),
                    ((row.ticks / 60000f).ToString("0.#") + " " + "Days".Translate()).Named("TIME")
                ));
            }

            return "LongDistance.Enroute.Text".Translate(sb.ToString());
        }

        public override AlertReport GetReport()
        {
            return ArrivalsManager.Current.HasScheduled ? AlertReport.Active : AlertReport.Inactive;
        }
    }
}
