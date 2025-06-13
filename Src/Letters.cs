using RimWorld;
using System.Text;
using Verse;

namespace LongDistance
{
    class Letters
    {
        public static void SendAcceptedLetter(Pawn joining, Pawn inviter, Map map, Faction faction, int relChange)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("LongDistance.InviteAccepted.Text".Translate(joining.Named("PAWN"), inviter.Named("INVITER"), map.Parent.Label.Named("COLONY"))
                    .AdjustedFor(joining, "PAWN").AdjustedFor(inviter, "INVITER"));

            if (faction != null && relChange > 0)
            {
                sb.AppendLine();
                sb.AppendLine("LongDistance.InviteAccepted.FactionPos".Translate(faction.NameColored.Named("FACTION"), relChange.Named("CHG")));
            }

            if (faction != null && relChange < 0)
            {
                sb.AppendLine();
                sb.AppendLine("LongDistance.InviteAccepted.FactionNeg".Translate(faction.NameColored.Named("FACTION"), relChange.Named("CHG")));
            }

            Find.LetterStack.ReceiveLetter(
                "LongDistance.InviteAccepted.Title".Translate(),
                sb.ToString().TrimEndNewlines(),
                LetterDefOf.PositiveEvent,
                inviter,
                faction
            );
        }

        public static void SendJoinLetter(Pawn joining, Pawn inviter, Map map)
        {
            Find.LetterStack.ReceiveLetter(
                "LongDistance.InviteJoined.Title".Translate(joining.Named("PAWN")),
                "LongDistance.InviteJoined.Text".Translate(joining.Named("PAWN"), inviter.Named("INVITER"), map.Parent.Label.Named("COLONY"))
                    .AdjustedFor(joining, "PAWN").AdjustedFor(inviter, "INVITER"),
                LetterDefOf.PositiveEvent,
                new LookTargets(joining, inviter)
            );
        }

        public static void SendRejectLetter(Pawn joining, Pawn inviter, Map map, bool breakup)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("LongDistance.InviteRejected.Text".Translate(joining.Named("PAWN"), inviter.Named("INVITER"), map.Parent.Label.Named("COLONY"))
                    .AdjustedFor(joining, "PAWN").AdjustedFor(inviter, "INVITER"));

            if (breakup)
            {
                sb.AppendLine();
                sb.AppendLine("LongDistance.InviteRejected.Breakup".Translate(inviter.Named("INVITER")).AdjustedFor(inviter, "INVITER"));
            }

            Find.LetterStack.ReceiveLetter(
                "LongDistance.InviteRejected.Title".Translate(),
                sb.ToString().TrimEndNewlines(),
                LetterDefOf.NegativeEvent,
                inviter
            );
        }

        public static void SendBreakupLetter(Pawn initiator, Pawn lover)
        {
            Find.LetterStack.ReceiveLetter(
                "LetterLabelBreakup".Translate(),
                "LongDistance.Breakup.Text".Translate(initiator.Named("PAWN"), lover.Named("LOVER"))
                    .AdjustedFor(initiator, "PAWN").AdjustedFor(lover, "LOVER"),
                LetterDefOf.NeutralEvent, initiator
            );
        }
    }
}
