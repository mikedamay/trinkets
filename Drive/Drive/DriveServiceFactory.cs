using System;
using System.IO;
using System.Threading;
using com.TheDisappointedProgrammer.IOCC;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace com.TheDisappointedProgrammer.Drive
{
    [Bean]
    internal class DriveServiceFactory : IFactory
    {
        [BeanReference(Factory = typeof(CredentialsFactory))] private UserCredential credential;
        static string ApplicationName = "Drive API .NET Quickstart";

        public object Execute(BeanFactoryArgs args)
        {
            // Create Drive API service.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });              
            return service;
        }

    }
}