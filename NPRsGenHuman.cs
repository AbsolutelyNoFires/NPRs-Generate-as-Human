using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Aurora;
using Aurora.Properties;
using Lib;
using HarmonyLib;


namespace NPRsGenHuman
{
    public class NPRsGenHuman : AuroraPatch.Patch
    {
        public override string Description => "Instead of only 30%, all NPRs generate as 'human' when the setting is active.";
        //1654
        protected override void Loaded(Harmony harmony)
        {
            Type game = AuroraAssembly.GetType("aw");
            var myNPRsGenHumantranspiler = typeof(NPRsGenHuman).GetMethod("NPRsGenHumantranspiler");
            
            IEnumerable<MethodInfo> alltheg2s = game.GetMethods().Where(thing => thing.Name == "g2");
            MethodInfo foundmethod = null;

            foreach (MethodInfo thismethod in alltheg2s)
            {
                if (thismethod.GetParameters().Length == 10)
                {
                    String retype = thismethod.ReturnType.ToString();
                    if (thismethod.ReturnType.ToString() == "ad")
                    {
                        foundmethod = thismethod;
                        harmony.Patch(foundmethod, transpiler: new HarmonyMethod(myNPRsGenHumantranspiler));
                    }
                }
            }

            
        }
        public static IEnumerable<CodeInstruction> NPRsGenHumantranspiler(IEnumerable<CodeInstruction> instructions)
        {
            var theseILcodes = new List<CodeInstruction>(instructions);
            // if (A_8 == 1 && global::ju.j(3) == 2)
            // added a breakpoint to comapre theseILcodes to the ilspy view
            theseILcodes[14].opcode = OpCodes.Ldc_I4_1;
            theseILcodes[16].opcode = OpCodes.Ldc_I4_1;
            return (theseILcodes);
        }
    }   
}
