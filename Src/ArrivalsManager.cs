using System.Collections.Generic;
using Verse;
using RimWorld.Planet;


namespace LongDistance
{
    class ArrivalsManager : WorldComponent
    {
        internal List<ArrivalEntry> entries = new List<ArrivalEntry>();

        public ArrivalsManager(World world) : base(world) { }

        internal class ArrivalEntry : IExposable
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

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref entries, "entries", LookMode.Deep);
        }

        public override void WorldComponentTick()
        {
            base.WorldComponentTick();
            if (entries.Count > 0)
            {
                foreach (var ent in entries)
                {
                    ent.ticks--;
                    if (ent.ticks <= 0) Utils.MovePawnToColony(ent.pawn, ent.inviter, ent.destination);
                }
                entries.RemoveAll((e) => e.ticks <= 0);
            }
        }

        public void Schedule(Pawn pawn, Pawn inviter, Map destination, int ticks)
        {
            entries.Add(new ArrivalEntry(pawn, inviter, destination, ticks));
        }
    }
}
