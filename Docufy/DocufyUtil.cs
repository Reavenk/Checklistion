using System;
using System.Collections.Generic;
using System.Text;

namespace Checklistion.Docufy
{
    static public class DocufyUtil
    {
        public static string GetFirstExtention(IDocufy docu)
        { 
            HashSet<string> exts = docu.SupportedExts;
            
            foreach(string e in exts)
                return e;

            return "__UNKNOWN";
        }
    }
}
