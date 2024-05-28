using System.IO;
using System.IO.Compression;
using System.Text.Json;
using UploadDrive;

string folderId = "1jWgakpLKDVNOcVyojmjeuhZwAa5yyMAm";
string fileUploadPath = "D:\\YTB\\c1_UseDriveAPI\\Test\\I save your life.txt";

// uplload 
//Extend.UploadFileToGoogleDrive(folderId, fileUploadPath);

//get list file
//Console.WriteLine("====");
//var listFile = Extend.GetLisFiles(folderId);
//foreach (var file in listFile)
//{
//    Console.WriteLine($"{file.Name} {file.Id}");
//}

// delete
var fileId = "14AmpYVy1GpprKXVVZ3zXxpJ3vDrHzzZZ";
Extend.DeleteFile(fileId);