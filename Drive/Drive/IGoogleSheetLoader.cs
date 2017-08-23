using System.IO;

namespace com.TheDisappointedProgrammer.Drive
{
    internal interface IGoogleSheetLoader
    {
        byte[] LoadSheet(string sheetNme);
        void SaveSheet(string folderName, string sheetName, Stream stream);
    }
}