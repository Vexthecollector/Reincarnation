using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Verse;

namespace Reincarnation
{
    [StaticConstructorOnStartup]
    public class MyPatch
    {
        static MyPatch() {
            var harmony = new Harmony("vex.Reincarnation.patch");
            harmony.PatchAll();
        }
    }
}
