using Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Google.Apis.Requests.BatchRequest;

namespace UploadDrive
{
    public class Extend
    {
        public static string UploadFileToGoogleDrive(string folderId, string filePath)
        {
            string pathToLog = Directory.GetParent(filePath).Parent.ToString();
            
            string mess = "";
            var service = GetService();

            var fileMetaData = new Google.Apis.Drive.v3.Data.File()
            {
                Name = Path.GetFileName(filePath),
                Parents = new List<string> { folderId },

            };

            // upload file
            FilesResource.CreateMediaUpload request;
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                request = service.Files.Create(fileMetaData, stream, "");
                request.Fields = "id";
                var response = request.Upload();
                if (response.Status != UploadStatus.Completed)
                {
                
                }
                else
                {

                  var uploadFile = request.ResponseBody;
                    mess = $"File {fileMetaData.Name} successfull sent {response.BytesSent / 1024} kb to drive";
                }
            }
            return mess;
        }
        public static DriveService GetService()
        {
            string credentialPath = "D:\\YTB\\c1_UseDriveAPI\\UploadDrive\\i-save-your-life-f82e1991f7c0.json";
            GoogleCredential credential;
            using (var stream = new FileStream(credentialPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(new[]
                {
                    DriveService.ScopeConstants.DriveFile
                });

            }
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Google Drive Upload Console App"
            });
            return service;

        }
        public static void DeleteFile(string fileId)
        {
            var service = GetService();

            try
            {
                var request = service.Files.Delete(fileId);

                string result = request.Execute(); // Send the delete request


                // Check if the deletion was successful
                if (string.IsNullOrEmpty(result))
                {
                    //Console.WriteLine("File deleted successfully!");
                }
                else
                {
                    Console.WriteLine("Error deleting file: " + result);
                }
            }
            catch (GoogleApiException e)
            {
         
            }

        }

        public static IEnumerable<Google.Apis.Drive.v3.Data.File> GetLisFiles(string folder)
        {
            var service = GetService();

            var fileList = service.Files.List();
            fileList.Q = $"mimeType!='application/vnd.google-apps.folder' and '{folder}' in parents";
            fileList.Fields = "nextPageToken, files(id, name, size, mimeType,createdTime)";

            var result = new List<Google.Apis.Drive.v3.Data.File>();
            string pageToken = null;
            do
            {
                fileList.PageToken = pageToken;
                var filesResult = fileList.Execute();
                var files = filesResult.Files;
                pageToken = filesResult.NextPageToken;
                result.AddRange(files);
            } while (pageToken != null);


            return result;
        }



    }

}
