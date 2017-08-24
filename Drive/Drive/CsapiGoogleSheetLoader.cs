using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using com.TheDisappointedProgrammer.IOCC;
using google = Google.Apis.Drive.v3;

namespace com.TheDisappointedProgrammer.Drive
{
    [Bean]
    public class CsapiGoogleSheetLoaderByList : IGoogleSheetLoader
    {
        [BeanReference(Name = "bylist")] private readonly IFileSearcher fileSearcher = null;
        [BeanReference(Factory = typeof(DriveServiceFactory))]
          private readonly google.DriveService driveService = null;
        [BeanReference] private ILogger logger;
        public byte[] LoadSheet(string sheetName)
        {
            MemoryStream stream = new MemoryStream();
            string matchingFileId;
            if ((matchingFileId = fileSearcher.GetId(sheetName)) == null)
            {
                throw new Exception($"Failed to find a google sheet for {sheetName}");
            }
            var request = driveService.Files.Export(matchingFileId, "text/csv");
            if (request == null)
            {
                throw new Exception($"Failed to create a request for {sheetName} {matchingFileId}");
            }
            request.Download(stream);
            return stream.GetBuffer();
        }

        public void SaveSheet(string folderName, string sheetName, Stream stream)
        {
            string folderId;
            if ((folderId = fileSearcher.GetId(folderName)) == null)
            {
                throw new Exception($"Failed to find a google folder: {folderName}");
            }
            google.Data.File fileMetaData = new google.Data.File()
            {
                Name = sheetName
                ,MimeType = "application/vnd.google-apps.spreadsheet"
                ,Parents = new List<string>
                {
                    folderId
                }
            };
            google.FilesResource.CreateMediaUpload request;
            request = driveService.Files.Create(fileMetaData, stream, "text/csv");
            request.Fields = "id, parents";
            request.Upload();
            logger.LogLine($"id={request.ResponseBody.Id} parent={request.ResponseBody.Parents?.FirstOrDefault()}");
        }
    }
}