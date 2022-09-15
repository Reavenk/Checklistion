using System;
using System.Collections.Generic;
using System.Text;

namespace Checklistion
{
    // TODO: Docstring
    public enum ProcessResult
    {
        Success,
        Empty,
        ErrorMissing,
        AlreadyProcessed
    }

    // TODO: Docstring
    class ProcessedDirectory
    {
        System.IO.DirectoryInfo directory;

        public Queue<string> toProcess = new Queue<string>();
        public Dictionary<System.IO.FileInfo, ProcessedFile> files = new Dictionary<System.IO.FileInfo, ProcessedFile>();

        // Note that in explaining what the regex will capture in the comment below, it will
        // be picked up as a false posititive when checklistion is run on the project.
        // ...
        // Such is life...
        //
        // // > ☐ GRP_SUB_ID-GUID : Requirement
        //          ^ cap               ^ cap
        static System.Text.RegularExpressions.Regex regexPattern =
            new System.Text.RegularExpressions.Regex("^[^>]*>\\s*☐\\s*([\\w_-]+)\\s*:\\s*(.*)$");

        // Group pattern A
        static System.Text.RegularExpressions.Regex regexGroupWSub =
            new System.Text.RegularExpressions.Regex("(\\w+)_(\\w+)_([\\w-]+)");

        // Group pattern B
        static System.Text.RegularExpressions.Regex regexGroupNoSub =
            new System.Text.RegularExpressions.Regex("(\\w+)_([\\w-]+)");

        public ProcessedDirectory(System.IO.DirectoryInfo dir)
        { 
            this.directory = dir;
        }

        public void PopulateTodos()
        { 
            foreach(System.IO.FileInfo fi in this.directory.EnumerateFiles())
                this.toProcess.Enqueue(fi.FullName);
        }

        public void ProcessAllTodos()
        { 
            while(toProcess.Count > 0)
            { 
                string fileToDo = toProcess.Dequeue();
                this.ProcessFile(fileToDo);
            }
        }

        ProcessResult ProcessFile(string filePath)
        { 
            System.IO.FileInfo fi = new System.IO.FileInfo(filePath);
            if(!fi.Exists)
                return ProcessResult.ErrorMissing;

            if(files.ContainsKey(fi))
                return ProcessResult.AlreadyProcessed;

            ProcessedFile procfile = new ProcessedFile(fi);
            this.files.Add(fi, procfile);

            string fileContents = System.IO.File.ReadAllText(fi.FullName, Encoding.UTF8);
            string [] fileLines = fileContents.Split( new char[]{ '\n' });
            for(uint i = 0; i < fileLines.Length; ++i)
            {
                string line = fileLines[i];

                System.Text.RegularExpressions.Match match = regexPattern.Match(line);
                if(!match.Success)
                    continue;

                string allID = match.Groups[1].Value.Trim();
                string descr = match.Groups[2].Value.Trim();

                string group    = "MISC";
                string subgroup = "DEFAULT";
                string id       = "";

                do
                {
                    System.Text.RegularExpressions.Match matchGroupAll = regexGroupWSub.Match(allID);
                    if(matchGroupAll.Success)
                    { 
                        group       = matchGroupAll.Groups[1].Value;
                        subgroup    = matchGroupAll.Groups[2].Value;
                        id          = matchGroupAll.Groups[3].Value;
                        break;
                    }
                    System.Text.RegularExpressions.Match matchGroupNoSub = regexGroupNoSub.Match(allID);
                    if(matchGroupNoSub.Success)
                    {
                        group = matchGroupNoSub.Groups[1].Value;
                        id = matchGroupNoSub.Groups[2].Value;
                        break;
                    }

                    id = allID;

                } while(false);

                // TODO: Check to make sure there are no empty variables. 

                procfile.AddEntry(i, group, subgroup, id, descr);
            }


            return ProcessResult.Success;
        }

    }
}
