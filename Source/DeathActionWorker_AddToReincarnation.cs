using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI.Group;

namespace Reincarnation
{
    public class DeathActionWorker_AddToReincarnation : DeathActionWorker
    {
        public override void PawnDied(Corpse corpse, Lord prevLord)
        {
            Pawn pawn = corpse.InnerPawn;
            if (pawn.Faction == Faction.OfPlayer)
            {
                PawnManager.Instance.addDeadPawn(pawn);
                Log.Message("Added a pawn!");
            }
        }
    }
}
