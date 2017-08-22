using System;
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
        [BeanReference] private AppContext appContext;

        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/drive-dotnet-quickstart.json
        private static string[] Scopes = {DriveService.Scope.DriveReadonly};

        public object Execute(BeanFactoryArgs args)
        {
            UserCredential credential;
            String inputPath = appContext.OsPlatform.GetHomeDirectoryName();
            DriveService service;
            using (var stream =
                new FileStream(Path.Combine(inputPath, "client_id.json"), FileMode.Open, FileAccess.Read))
            {
                string credPath = Environment.GetFolderPath(
                    Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/drive-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);

            }
            return credential;
        }
    }
}