using System;
using System.Collections.Generic;
using System.Text;

namespace Checklistion.Docufy
{
    /// <summary>
    /// Various options to pass into Docufy
    /// </summary>
    public class DocGenOptions
    {
        public enum Verbose
        { 
            Compact,
            Normal,
            Extensive
        }

        public Verbose verbose = Verbose.Normal;
    }
}
