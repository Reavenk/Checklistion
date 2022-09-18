using System;
using System.Collections.Generic;
using System.Text;

namespace Checklistion.Docufy
{
    /// <summary>
    /// A class that converts extracted Checklistion data into a 
    /// saved file.
    /// 
    /// What file format to save as will depend on the implementation.
    /// </summary>
    public interface IDocufy
    {
        /// <summary>
        /// Save out a formatted checklist to file.
        /// </summary>
        /// <param name="outFile">The file.</param>
        /// <param name="processed">
        /// The extracted checklist data to format.
        /// </param>
        /// <param name="opts">Various shared options on document generation.</param>
        /// <returns>True if success. Else, false.</returns>
        public bool WriteChecklist(string outFile, ProcessEngine processed, DocGenOptions opts);

        /// <summary>
        /// The supported saved file types supported by the implementation.
        /// 
        /// This will be used to decide if the IDocufy implementation 
        /// should be used if we need to decide an implementation based 
        /// on file extention name.
        /// 
        /// Entries should be all lowercase, valid extention names. Do NOT
        /// prefix with a "*." or "." - just the actual extention will do.
        /// </summary>
        public HashSet<string> SupportedExts {get; }

        /// <summary>
        /// The string identifier of the IDocufy implementation.
        /// 
        /// This should be one word, all lowercase. 
        /// 
        /// This will be the string used via command line to specify
        /// the implementation.
        /// </summary>
        public string ID {get; }
    }
}
