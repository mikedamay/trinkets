﻿using System;
using System.IO;
using System.Threading;
using com.TheDisappointedProgrammer.IOCC;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Util.Store;

namespace com.TheDisappointedProgrammer.Drive
{
    [Bean]
    internal class CredentialsFactory : IFactory
    {
        [BeanReference] private ILogger logger = null;
        [BeanReference] private AppContext appContext;

        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/drive-dotnet-quickstart.json
        private static string[] Scopes = {DriveService.Scope.Drive};

        public object Execute(BeanFactoryArgs args)
        {
            UserCredential credential;
            String inputPath = appContext.OsPlatform.GetHomeDirectoryName();
            DriveService service;
            using (var stream =
                new FileStream(Path.Combine(inputPath, "client_id.json"), FileMode.Open, FileAccess.Read))
            {
                string generatedCredentialsPath = Environment.GetFolderPath(
                    Environment.SpecialFolder.Personal);
                generatedCredentialsPath = Path.Combine(generatedCredentialsPath, ".credentials/drive-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(generatedCredentialsPath, true)).Result;
                logger.LogLine("Credential file saved to: " + generatedCredentialsPath);

            }
            return credential;
        }
    }
}