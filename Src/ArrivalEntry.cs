using Verse;

namespace LongDistance
{
    public class ArrivalEntry : IExposable
    {
        public Pawn pawn, inviter;
        public Map destination;
        public int ticks;

        public ArrivalEntry() { }

        public ArrivalEntry(Pawn pawn, Pawn inviter, Map destination, int ticks)
        {
            this.pawn = pawn;
            this.inviter = inviter;
            this.destination = destination;
            this.ticks = ticks;
        }

        public void ExposeData()
        {
            Scribe_References.Look(ref pawn, "pawn");
            Scribe_References.Look(ref inviter, "inviter");
            Scribe_References.Look(ref destination, "destination");
            Scribe_Values.Look(ref ticks, "ticks");
        }
    }
}
