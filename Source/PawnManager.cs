using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using RimWorld;
using Verse;

namespace Reincarnation
{
    public class PawnManager
    {
        List<Pawn> favouritedPawns;
        List<Pawn> deadPawns;


        private PawnManager()
        {

            initialize();

        }
        private static readonly PawnManager instance = new PawnManager();
        public static PawnManager Instance
        {
            get { return instance; }
        }

        public void addDeadPawn(Pawn pawn)
        {
            deadPawns.AddDistinct(pawn);
            savePawns();
        }

        public void addFavouritedPawn(Pawn pawn)
        {
            favouritedPawns.AddDistinct(pawn);
            savePawns();
        }

        public void removeDeadPawn(Pawn pawn)
        {
            deadPawns.Remove(pawn);
            savePawns();
        }
        public void removeFavouritedPawn(Pawn pawn)
        {
            favouritedPawns.Remove(pawn);
            savePawns();
        }

        public Pawn returnRandomDeadPawn()
        {
            try
            {

                Pawn pawn = deadPawns.First();
                removeDeadPawn(pawn);
                return pawn;
            }
            catch { return null; }

        }

        public void initialize()
        {
            deadPawns = new List<Pawn>();
            favouritedPawns = new List<Pawn>();

            String fpath = GenFilePaths.ConfigFolderPath + "/ReincarnationFavouritedPawns.xml";
            String dpath = GenFilePaths.ConfigFolderPath + "/ReincarnationDeadPawns.xml";
            if (!File.Exists(fpath))
            {

                File.Create(fpath).Close();
                SafeSaver.Save(fpath, "ReincarnationFavouritedPawns", delegate
                {
                    ScribeMetaHeaderUtility.WriteMetaHeader();
                });
            }
            if (!File.Exists(dpath))
            {

                File.Create(dpath).Close();
                SafeSaver.Save(dpath, "ReincarnationDeadPawns", delegate
                {
                    ScribeMetaHeaderUtility.WriteMetaHeader();
                });
            }
            readPawns();
        }

        private void savePawns()
        {

            String fpath = GenFilePaths.ConfigFolderPath + "/ReincarnationFavouritedPawns.xml";
            String dpath = GenFilePaths.ConfigFolderPath + "/ReincarnationDeadPawns.xml";

            SafeSaver.Save(fpath, "ReincarnationFavouritedPawns", delegate
            {
                ScribeMetaHeaderUtility.WriteMetaHeader();
                if (!favouritedPawns.NullOrEmpty())
                {
                    Scribe_Collections.Look(ref favouritedPawns, "favouritedPawns", LookMode.Deep);
                }

            });
            SafeSaver.Save(dpath, "ReincarnationDeadPawns", delegate
            {
                ScribeMetaHeaderUtility.WriteMetaHeader();
                if (!deadPawns.NullOrEmpty())
                {
                    Scribe_Collections.Look(ref deadPawns, "deadPawns", LookMode.Deep);
                }

            });

        }



        private void readPawns()
        {

            String fpath = GenFilePaths.ConfigFolderPath + "/ReincarnationFavouritedPawns.xml";
            String dpath = GenFilePaths.ConfigFolderPath + "/ReincarnationDeadPawns.xml";

            Scribe.loader.InitLoading(fpath);
            try
            {
                ScribeMetaHeaderUtility.LoadGameDataHeader(ScribeMetaHeaderUtility.ScribeHeaderMode.None, logVersionConflictWarning: true);
                Scribe_Collections.Look(ref favouritedPawns, "favouritedPawns", LookMode.Deep);
                Scribe.loader.FinalizeLoading();
            }
            catch (Exception e) 
            {
                Log.Warning(e.ToString());
                Scribe.ForceStop();
            }
            Scribe.loader.InitLoading(dpath);
            try
            {
                ScribeMetaHeaderUtility.LoadGameDataHeader(ScribeMetaHeaderUtility.ScribeHeaderMode.None, logVersionConflictWarning: true);
                Scribe_Collections.Look(ref deadPawns, "deadPawns", LookMode.Deep);
                Scribe.loader.FinalizeLoading();
            }
            catch (Exception e)
            {
                Log.Warning(e.ToString());
                Scribe.ForceStop();
            }
            if (favouritedPawns == null) favouritedPawns = new List<Pawn>();
            if (deadPawns == null) deadPawns = new List<Pawn>();
        }

    }
}

