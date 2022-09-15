using System;
using System.Collections.Generic;
using System.Text;

namespace Checklistion.Docufy
{
    interface IDocufy
    {
        // TODO: Docstring
        bool WriteChecklist(string outFile, ProcessEngine processed);
    }
}
