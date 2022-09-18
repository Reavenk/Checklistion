using System;
using System.Collections.Generic;
using System.Text;

namespace Checklistion.Docufy
{
    class DocufyMarkdown : IDocufy
    {
        public const string ID = "markdown";

        string IDocufy.ID => DocufyMarkdown.ID;

        bool IDocufy.WriteChecklist(string outFile, ProcessEngine processed, DocGenOptions opts)
        { 
            Checklist.Grouping grouping = processed.GenerateGrouping();

            System.IO.StreamWriter textOut = new System.IO.StreamWriter(outFile);
            textOut.WriteLine( "# Checklist");
            textOut.WriteLine( "TODO: Maybe a space here for extra details?");

            foreach(var gIt in grouping.groups)
            { 
                textOut.WriteLine( "## Group: " + gIt.Key);

                foreach(var sIt in gIt.Value.subGroups)
                { 
                    textOut.WriteLine( "### " + sIt.Key);

                    foreach(Checklist.Entry e in sIt.Value.entries)
                    { 
                        textOut.Write("* [ ] " + EscapeMarkdownString(e.requirement));
                        
                        if(opts.verbose != DocGenOptions.Verbose.Compact)
                            textOut.Write( " &nbsp;[loc](" + e.file + ":" + e.fileline.ToString() + ")");

                        textOut.Write("\n");
                    }
                }
            }

            textOut.Close();
            return true;
        }

        HashSet<string> IDocufy.SupportedExts => new HashSet<string> { "md" };

        static string EscapeMarkdownString(string str)
        {
            // TODO: Figure out markdown escaping
            return str;
        }
    }
}
