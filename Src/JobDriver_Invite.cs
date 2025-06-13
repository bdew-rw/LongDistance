using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace LongDistance
{
    public class JobDriver_Invite : JobDriver
    {
        private const TargetIndex PawnInd = TargetIndex.A;
        private const TargetIndex ConsoleInd = TargetIndex.B;

        private Pawn Target => (Pawn)job.GetTarget(PawnInd).Thing;
        private Building_CommsConsole Console => (Building_CommsConsole)job.GetTarget(ConsoleInd).Thing;


        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(Console, job, 1, -1, null, errorOnFailed);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {

            yield return Toils_Goto.GotoThing(ConsoleInd, PathEndMode.InteractionCell)
                .FailOnDespawnedNullOrForbidden(ConsoleInd)
                .FailOnSomeonePhysicallyInteracting(ConsoleInd)
                .FailOn(toil => !pawn.CanReach(Console, PathEndMode.InteractionCell, Danger.Deadly));

            yield return Toils_General.Wait(50)
                .FailOnCannotTouch(ConsoleInd, PathEndMode.InteractionCell)
                .WithProgressBarToilDelay(ConsoleInd)
                .PlaySustainerOrSound(SoundDefOf.RadioComms_Ambience);

            yield return new Toil
            {
                initAction = () =>
                {
                    Utils.HandleInvite(pawn, Target);
                }
            };
        }
    }
}