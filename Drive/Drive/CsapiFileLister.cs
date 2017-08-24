using System;
using System.Collections.Generic;
using System.Linq;
using com.TheDisappointedProgrammer.IOCC;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;

namespace com.TheDisappointedProgrammer.Drive
{
    [Bean]
    public class CsapiFileLister : IFileLister
    {
        [BeanReference]
        private ILogger logger;
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

        private void Log(IList<File> files)
        {
            foreach (var file in files)
            {
                logger.LogLine($"{file.Name} {file.Id}");
            }
        }
    }
}