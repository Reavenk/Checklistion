using System;
using System.Collections.Generic;
using System.Text;

namespace Checklistion.Docufy
{
    // TODO: Docstring
    // > ☐ DOCU_TEXT_0613af08f14a : Checklistion has a solution to output a raw text checklist (via DocufyText).
    class DocufyText : IDocufy
    {
        public const string ID = "rawtext";

        string IDocufy.ID => DocufyText.ID;

        public DocufyText()
        {}

        bool IDocufy.WriteChecklist(string outFile, ProcessEngine processed, DocGenOptions opts)
        { 
            Checklist.Grouping grouping = processed.GenerateGrouping();

            System.IO.StreamWriter textOut = new System.IO.StreamWriter(outFile);

            foreach(var gIt in grouping.groups)
            {
                textOut.Write("\n");
                textOut.WriteLine("GROUP : " + gIt.Key);

                foreach(var sIt in gIt.Value.subGroups)
                {
                    textOut.WriteLine("\t" + sIt.Key);

                    foreach(Checklist.Entry e in sIt.Value.entries)
                    { 
                        textOut.WriteLine("\t\t☐ " + e.requirement);

                        // > ☐ DOCU_TEXT_7edaace503bf : DocufyText has the ability to display a compact checklist.
                        if (opts.verbose != Docufy.DocGenOptions.Verbose.Compact)
                        {
                            // > ☐ DOCU_TEXT_7edaace503bf : DocufyText has the ability to show checklist entry ids.
                            // > ☐ DOCU_TEXT_7c03a2baa2e9 : DocufyText has the ability to show checklist entry filepaths.
                            // > ☐ DOCU_TEXT_f42b56c1df38 : DocufyText has the ability to show checklist entry line numbers.
                            textOut.WriteLine("\t\t\tID: " + e.id + "    FILE: " + e.file.FullName + "    LINE: " + e.fileline.ToString());
                            textOut.Write("\n");
                        }
                    }
                }
            }

            textOut.Close();
            return true;
        }

        HashSet<string> IDocufy.SupportedExts => new HashSet<string>{"txt"};
    }
}
