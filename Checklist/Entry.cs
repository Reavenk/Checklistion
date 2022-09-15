// > ☐ TEST_PARSE_fbd213c882b3 : This entry is captured.

using System;
using System.Collections.Generic;
using System.Text;

namespace Checklistion.Checklist
{
    // TODO: Docstring
    struct Entry
    {
        public System.IO.FileInfo file;
        public uint fileline;
        public string group;
        public string subgroup;
        public string id;
        public string requirement;

        public Entry(
            System.IO.FileInfo file,
            uint line,
            string group,
            string subgroup,
            string id,
            string requirement)
        {
            this.file           = file;
            this.fileline       = line;
            this.group          = group;
            this.subgroup       = subgroup;
            this.id             = id;
            this.requirement    = requirement;
        }
    }
}
