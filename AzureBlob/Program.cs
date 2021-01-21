using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.IO;

namespace AzureBlob
{
    class Program
    {
        static string connString = "DefaultEndpointsProtocol=https;AccountName=azfunctionblobstrg;AccountKey=m1n/Wdgl3ZwCl6Gl7vuaX6WenE1ipl1dAktQUYYXqXYY9e3bBDENbgE6MgD/r8Vt2bdCrGzshUw8Zq+4wm8Xlg==;";
        static string containerName = "azstoragecontainer";
        static string fileName = "mukeshtestfile.txt";
        static string localFilePath = Path.Combine("../data/", fileName);
        
        static void Main(string[] args)
        {
            try
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient(connString);
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                containerClient.CreateIfNotExists(); //Creating Container

                #region Code
                //Check Existence
                //if(!containerClient.Exists())
                //{
                //    containerClient = blobServiceClient.CreateBlobContainer(containerName);
                //}
                #endregion

                File.WriteAllText(localFilePath, "This is a blob");

                BlobClient blobClient = containerClient.GetBlobClient(fileName); //Getting Blob
                
                //Check for blobclient existence
                if(!blobClient.Exists())
                {
                    using (FileStream uploadFileStream = File.OpenRead(localFilePath))
                    {
                        blobClient.UploadAsync(uploadFileStream);
                    }
                }
                
                
                foreach (BlobItem item in containerClient.GetBlobs())
                {
                    BlobClient blbClient = containerClient.GetBlobClient(item.Name);
                    BlobDownloadInfo download = blbClient.Download();
                    string downloadFilePath = Path.Combine("../downloadeddata/", item.Name);

                    using (FileStream downloadFileStream = File.OpenWrite(downloadFilePath))
                    {
                        download.Content.CopyToAsync(downloadFileStream);
                        downloadFileStream.Close();
                    }                   
                }




                

                Console.ReadLine();
               
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}
