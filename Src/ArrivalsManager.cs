using RimWorld.Planet;
using System.Collections.Generic;
using Verse;


namespace LongDistance
{
    partial class ArrivalsManager : WorldComponent
    {
        private List<ArrivalEntry> entries = new List<ArrivalEntry>();

        public ArrivalsManager(World world) : base(world) { }

        public static ArrivalsManager Current => Find.World.GetComponent<ArrivalsManager>();

        public bool HasScheduled => entries.Any();
        public IEnumerable<ArrivalEntry> Scheduled => entries;
        public bool IsScheduled(Pawn pawn) => entries.Any(x => x.pawn == pawn);

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
                    if (ent.pawn == null)
                        Log.Error("Arrivals manager - entry has null pawn - removing");
                    if (ent.inviter == null)
                        Log.Error("Arrivals manager - entry has null inviter - removing");
                    if (ent.destination == null)
                        Log.Error("Arrivals manager - entry has null destination - removing");

                    ent.ticks--;

                    if (ent.ticks <= 0 && ent.pawn != null && ent.inviter != null && ent.destination != null)
                        Utils.MovePawnToColony(ent.pawn, ent.inviter, ent.destination);
                }
                entries.RemoveAll((e) => e.ticks <= 0 || e.pawn == null || e.inviter == null || e.destination == null);
            }
        }

        public void Schedule(Pawn pawn, Pawn inviter, Map destination, int ticks)
        {
            entries.Add(new ArrivalEntry(pawn, inviter, destination, ticks));
        }
    }
}
