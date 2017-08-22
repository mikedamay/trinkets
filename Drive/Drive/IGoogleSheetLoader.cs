namespace com.TheDisappointedProgrammer.Drive
{
    internal interface IGoogleSheetLoader
    {
        byte[] LoadSheet(string sheetNme);
    }
}