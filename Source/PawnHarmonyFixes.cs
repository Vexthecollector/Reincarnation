using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using HarmonyLib;
using Verse;

namespace Reincarnation
{
    [HarmonyPatch(typeof(PregnancyUtility), nameof(PregnancyUtility.ApplyBirthOutcome_NewTemp))]
    public static class PawnHarmonyFixes
    {

        static void Postfix(ref Thing __result)
        {
            if (__result.GetType() == typeof(Pawn))
            {
                Pawn pawn = PawnManager.Instance.returnRandomDeadPawn();
                if (pawn != null)
                {
                    ((Pawn)__result).Name = GeneratePawnName(pawn, (Pawn)__result);
                    ((Pawn)__result).skills = GeneratePawnSkills(pawn, ((Pawn)__result));
                    ((Pawn)__result).gender = pawn.gender;
                    ((Pawn)__result).story.traits = pawn.story.traits;
                }
            }
        }

        static Name GeneratePawnName(Pawn pawn, Pawn result)
        {
            try
            {

                Name name;
                string nickname = "";
                string lastname;
                string firstname;
                if (pawn.Name.GetType() == typeof(NameTriple))
                {
                    firstname = ((NameTriple)pawn.Name).First;
                    nickname = ((NameTriple)pawn.Name).Nick;
                }
                else
                {
                    firstname = ((NameSingle)pawn.Name).Name;
                }

                lastname = PawnNamingUtility.GetLastName(result);

                name = new NameTriple(firstname, nickname, lastname);
                return name;
            }
            catch
            {
                return result.Name;
            }
        }

        static Pawn_SkillTracker GeneratePawnSkills(Pawn pawn, Pawn result)
        {
            try
            {

                Pawn_SkillTracker pawn_SkillTracker = result.skills;
                List<SkillRecord> newSkills = new List<SkillRecord>();
                foreach (SkillRecord skill in pawn.skills.skills)
                {
                    SkillRecord skillRecord = new SkillRecord(result, skill.def);
                    float multiplier = 0.1f + (byte)skill.passion * 0.05f;
                    float xpSinceLastLevel = skill.XpTotalEarned * multiplier;
                    while (xpSinceLastLevel >= SkillRecord.XpRequiredToLevelUpFrom(skillRecord.levelInt))
                    {
                        xpSinceLastLevel -= SkillRecord.XpRequiredToLevelUpFrom(skillRecord.levelInt);
                        skillRecord.levelInt++;
                    }
                    skillRecord.passion = skill.passion;

                    newSkills.Add(skillRecord);

                }
                pawn_SkillTracker.skills = newSkills;

                return pawn_SkillTracker;
            }
            catch
            {
                return pawn.skills;
            }
        }
    }
}
