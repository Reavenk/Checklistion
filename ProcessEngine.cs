using System;
using System.Collections.Generic;
using System.Text;

namespace Checklistion
{
    public class ProcessEngine
    {
        HashSet<string> supportedExts = new HashSet<string>();

        Queue<string> toProcess = new Queue<string>();

        Dictionary<System.IO.DirectoryInfo, ProcessedDirectory> processedDirs
            = new Dictionary<System.IO.DirectoryInfo, ProcessedDirectory>();

        public void EnqueueDirectories(IEnumerable<string> paths)
        { 
            foreach( string p in paths)
                toProcess.Enqueue(p);
        }

        public bool DoNextDirectory()
        { 
            if(this.toProcess.Count == 0)
                return false;

            string dirToProc = this.toProcess.Dequeue();
            return this.ProcessDirectory(dirToProc, true) != ProcessResult.ErrorMissing;
        }

        public ProcessResult ProcessDirectory(string dirPath, bool queueRecursive)
        { 
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(dirPath);
            if(!di.Exists)
                return ProcessResult.ErrorMissing;

            if(processedDirs.ContainsKey(di))
                return ProcessResult.AlreadyProcessed;

            if(queueRecursive)
            { 
                foreach(System.IO.DirectoryInfo dChild in di.EnumerateDirectories())
                    toProcess.Enqueue(dChild.FullName);
            }

            ProcessedDirectory procdir = new ProcessedDirectory(di);
            processedDirs.Add(di, procdir);

            procdir.PopulateTodos();
            procdir.ProcessAllTodos();

            return ProcessResult.Success;
        }

        public void AddFormats(IEnumerable<string> formats)
        {
            foreach(string f in formats)
                this.AddFormat(f);
        }

        public bool AddFormat(string format)
        { 
            return this.supportedExts.Add(format.ToLower());
        }

        public void ProcessAllDirectories()
        { 
            while(this.DoNextDirectory()){ }
        }

        public bool Validate()
        { 
            // TODO:
            return true;
        }

        public Checklist.Grouping GenerateGrouping()
        { 
            Checklist.Grouping groupingRet = new Checklist.Grouping();

            foreach(ProcessedDirectory pd in this.processedDirs.Values)
            { 
                foreach(ProcessedFile pf in pd.files.Values)
                {
                    foreach(Checklist.Entry e in pf.entries)
                    { 
                        groupingRet.
                            GetGroup(e.group).
                            GetSub(e.subgroup).
                            AddEntry(
                                e.file,
                                e.fileline,
                                e.id,
                                e.requirement);
                    }
                }
            }

            return groupingRet;
        }

    }
}
