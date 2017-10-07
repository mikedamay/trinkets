using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using com.TheDisappointedProgrammer.IOCC;

namespace com.TheDisappointedProgrammer.Drive
{
    [Bean(Name="bylist")]
    public class FileSearcherByList : IFileSearcher
    {
        [BeanReference] private IFileLister fileLister;
        /// <summary>
        /// obtains a complete list of files from drive and searches for the named argument
        /// </summary>
        /// <param name="fileName">file to search for</param>
        /// <returns>a google api file id - good for manipulating data,
        /// null if no file is found</returns>
        public string GetId(string fileName)
        {
            IList<(string fileName, string fileId)> fileSpecs
                = fileLister.ListFiles();
            string matchingFileId = fileSpecs.Where(
                    f => f.fileName == fileName).Select(f => f.fileId)
                .FirstOrDefault();
            return matchingFileId;
        }
    }
}