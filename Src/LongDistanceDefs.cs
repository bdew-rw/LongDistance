using RimWorld;
using Verse;

namespace LongDistance
{
    [DefOf]
    public static class LongDistanceDefs
    {
        [DefAlias("bdew_longdistance_invite")] public static JobDef InviteJob;
        [DefAlias("bdew_longdistance_breakup")] public static JobDef BreakupJob;

        [DefAlias("bdew_longdistance_inv_rejected")] public static ThoughtDef RejectedThoughtInviter;
        [DefAlias("bdew_longdistance_inv_rejected_target")] public static ThoughtDef RejectedThoughtTarget;
        [DefAlias("bdew_longdistance_inv_rejected_mood")] public static ThoughtDef RejectedThoughtInviterMood;
        [DefAlias("bdew_longdistance_inv_accepted")] public static ThoughtDef AcceptedThoughtInviter;
        [DefAlias("bdew_longdistance_inv_accepted_target")] public static ThoughtDef AcceptedThoughtTarget;
        [DefAlias("bdew_longdistance_inv_accepted_mood")] public static ThoughtDef AcceptedThoughtInviterMood;

        [DefAlias("bdew_longdistance_recruited")] public static HistoryEventDef RecruitedHistoryEvent;

        static LongDistanceDefs()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(LongDistanceDefs));
        }
    }

}