using System;
using System.Collections.Generic;
using System.Text;

namespace Checklistion
{
    class ProcessedFile
    {
        public System.IO.FileInfo file;
        public List<Checklist.Entry> entries = new List<Checklist.Entry>();

        public ProcessedFile(System.IO.FileInfo file)
        { 
            this.file = file;
        }

        public void AddEntry(uint fileline, string group, string subgroup, string id, string req)
        { 
            this.entries.Add(new Checklist.Entry(this.file, fileline, group, subgroup, id, req));
        }
    }
}
