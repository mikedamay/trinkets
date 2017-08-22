using System;
using System.Collections.Generic;
using System.Linq;
using com.TheDisappointedProgrammer.IOCC;
using Google.Apis.Drive.v3;

namespace com.TheDisappointedProgrammer.Drive
{
    [Bean]
    public class CsapiFileLister : IFileLister
    {
        [BeanReference(Factory=typeof(DriveServiceFactory))] private DriveService driveService;
        public IList<(string fileName, string fileId)> ListFiles()
        {

            // Define parameters of request.
            FilesResource.ListRequest listRequest = driveService.Files.List();
            listRequest.PageSize = 1000;
            listRequest.Fields = "nextPageToken, files(id, name)";

            // List files.
            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute()
                .Files;
            return files.Select(f => (f.Name, f.Id)).ToList();
        }
    }
}