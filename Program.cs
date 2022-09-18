using System;
using System.Collections.Generic;

namespace Checklistion
{
    class Program
    {
        static void Main(string[] args)
        {
            HashSet<string> dirsToProcess = new HashSet<string>();  // The directories to look for files in.
            int numToGen = 0;                                       // If non-zero, show generated entries in console (And exit)
            string fileOutSave;                                     // The location to save the output

            bool showHelp = false;                                  // If true, show help (and exit)
            bool spitSymbol = false;                                // Show the checkbox symbol (and exit)
            bool processDir = true;                                 // If false, don't do any actual scanning work and extraction work.
            bool onlyFilesLeft = false;
            HashSet<string> formats = new HashSet<string>();        // Formats to support
            string implID = "";

            Docufy.DocGenOptions opts = new Docufy.DocGenOptions();

            for (int ia = 0; ia < args.Length; ++ia)
            { 
                if(args[ia].Length == 0)
                    continue; // Sanity check

                if (!args[ia].StartsWith("-") || onlyFilesLeft)
                {
                    // If not a flag or a parameter to a flag, it's a directory to target.
                    // TODO: Handle specific files also?
                    dirsToProcess.Add(args[ia]);
                }
                else if(args[ia] == "--")
                { 
                    onlyFilesLeft = true;
                }
                else if(args[ia] == "-h" || args[ia] == "--help")
                {
                    // > ☐ CMD_LIST_357f8e43fa84 : Command line parameter --help shows help.
                    // > ☐ CMD_LIST_c91c68e61af8 : Command line parameter -h shows help.
                    showHelp = true;
                }
                else if( args[ia][0] != '-')
                {
                    dirsToProcess.Add(args[ia]);
                }
                else if( args[ia] == "--gen-entries")
                { 
                    ++ia;
                    numToGen = System.Convert.ToInt32(args[ia]);
                }
                else if( args[ia] == "--symbol")
                {
                    // > ☐ CMD_LIST_90054ab8139d : Command line parameter --symbol outputs empty checkbox symbol.
                    spitSymbol = true;
                }
                else if( args[ia] == "-C") // TODO: UNIMPLMENTED/UNTESTED:
                { 
                    ++ia;
                    dirsToProcess.Add(args[ia]);
                }
                else if( args[ia] == "-O") // TODO: UNIMPLMENTED/UNTESTED:
                { 
                    ++ia;
                    fileOutSave = args[ia];
                }
                else if( args[ia] == "--exts") // TODO: UNIMPLMENTED/UNTESTED:
                {
                    // > ☐ CMD_LIST_58e37c892cbd : Command line parameter --ext specifies supported extentions.
                    for (; ia < args.Length; ++ia)
                    { 
                        if(args[ia].StartsWith("-"))
                        {
                            --ia;
                            break;
                        }
                        formats.Add(args[ia].ToLower());
                    }
                }
                else if( args[ia] == "--impl")
                {
                    ++ia;
                    implID = args[ia];
                }
                else if(args[ia] == "--compact")
                {
                    opts.verbose = Docufy.DocGenOptions.Verbose.Compact;
                }
            }

            //Console.WriteLine("Hello World!");
            if(showHelp)
            {
                // TODO: Add all help entries
                // > ☐ CMD_HELP_1ad914248035 : Help text is accurate.
                // > ☐ CMD_HELP_47e5fa2df884 : Help text shows all supported command line features.
                Console.WriteLine("Checklistion");
                Console.WriteLine("A program to extract checklists from a codebase.");
                Console.WriteLine("");
                Console.WriteLine("Usage: checklistion [--version][--help][-C <path>][--gen-entries][--symbol]");
                Console.WriteLine("\t--help\tShow help");
                Console.WriteLine("\t--gen-entries <num>\tGenerate entries");
                Console.WriteLine("\t-C <dir>\tProcess directory");

                // > ☐ CMD_HELP_46286456a093 : Showing help does not process a codebase.
                processDir = false;
            }

            if (spitSymbol)
            {
                Console.WriteLine("Checkbox symbol, ☐");
                processDir = false;
            }

            if(numToGen > 0)
            {
                SpitGeneratedExamples(numToGen);
                processDir = false;
            }

            //////////////////////////////////////////////////
            //
            //  DECIDE IMPLEMENTATION
            //
            //////////////////////////////////////////////////
            Dictionary<string, Docufy.IDocufy> supportedImpls = GenerateSupportedImplementationRegistry();
            Docufy.IDocufy impl = new Docufy.DocufyText();

            if (!string.IsNullOrEmpty(implID))
            { 
                Console.WriteLine("Specified implementation " + implID + ".");
                if(supportedImpls.ContainsKey(implID))
                { 
                    impl = supportedImpls[implID];
                    Console.WriteLine("Switched to implementation " + impl.ID + ".");
                }
                else
                { 
                    Console.WriteLine("ERROR: Implementation " + implID + " is not a known id, using default " + impl.ID + ".");
                }
            }
            else
                Console.WriteLine("Using default implementation " + impl.ID + ".");


            //////////////////////////////////////////////////

                if (processDir == false)
                return;

            if (dirsToProcess.Count == 0)
                dirsToProcess.Add(System.IO.Directory.GetCurrentDirectory());

            // TODO: Note the defaults are added
            if(formats.Count == 0)
            {
                // Default to extentions that I use
                // (wleu 09/15/2022)
                formats.Add("cpp");
                formats.Add("c");
                formats.Add("h");
                formats.Add("js");
                formats.Add("py");
            }

            // TODO: More console output

            Console.WriteLine($"Processing {dirsToProcess.Count} directories.");

            ProcessEngine processEngine = new ProcessEngine();
            processEngine.AddFormats(formats);
            processEngine.EnqueueDirectories(dirsToProcess);
            processEngine.Validate();
            processEngine.ProcessAllDirectories();

            impl.WriteChecklist("out.txt", processEngine, opts);
        }

        static bool RegisterImplementation(
            Dictionary<string, Docufy.IDocufy> register,
            Docufy.IDocufy newDocufy)
        { 
            if(register.ContainsKey(newDocufy.ID))
                return false;

            register.Add(newDocufy.ID, newDocufy);
            return true;
        }

        static void SpitGeneratedExamples(int num)
        {
            for (int i = 0; i < num; ++i)
            {
                string guid = System.Guid.NewGuid().ToString();
                string id = guid.Substring(guid.Length - 12);
                Console.WriteLine("> ☐ CAT_SUB_" + id + ": --");
            }
        }

        static Dictionary<string, Docufy.IDocufy> GenerateSupportedImplementationRegistry()
        {
            Dictionary<string, Docufy.IDocufy> retRegistry = 
                new Dictionary<string, Docufy.IDocufy>();
            //
            RegisterImplementation(retRegistry, new Docufy.DocufyText());
            RegisterImplementation(retRegistry, new Docufy.DocufyMarkdown());

            return retRegistry;
        }
    }
}
