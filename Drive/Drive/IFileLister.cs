using System.Collections.Generic;

namespace com.TheDisappointedProgrammer.Drive
{
    internal interface IFileLister
    {
        IList<(string fileName, string fileId)> ListFiles();
    }
}