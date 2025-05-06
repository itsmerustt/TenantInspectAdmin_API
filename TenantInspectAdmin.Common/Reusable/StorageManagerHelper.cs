using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Azure.Storage.Blobs.Specialized;

namespace TenantInspectAdmin.Common.Reusable
{
    public class StorageManagerHelper
    {

        private readonly IConfiguration _configuration;
        private static string _storageAccountString;
        private static string _storageAccountName;
        private static string _storageAccountKey;

        public StorageManagerHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            _storageAccountString = _configuration.GetConnectionString("StorageAccountString");
            _storageAccountName = _configuration.GetConnectionString("StorageAccountName");
            _storageAccountKey = _configuration.GetConnectionString("StorageAccountKey");
        }
        public static BlobResult Upload(string ContainerName, string ApplicationGuid, string BlobName, string FileName, byte[] Data, string contentType, string storageAccountString)
        {
            BlobResult result = new BlobResult();

            BlobContainerClient container = new BlobContainerClient(storageAccountString, ContainerName);

            container.CreateIfNotExists(PublicAccessType.None);

            string extension = System.IO.Path.GetExtension(FileName);



            string fileName = String.Format("{0}/{1}{2}", ApplicationGuid.ToLower(), Guid.NewGuid().ToString().Substring(0, 5) + "-" + BlobName, extension);


            BlobClient blob = container.GetBlobClient(fileName);

            Dictionary<string, string> metaInfo = new Dictionary<string, string>();
            metaInfo.Add("filename", FileName);


            try
            {
                Stream stream = new MemoryStream(Data);

                blob.Upload(stream, new BlobHttpHeaders { ContentType = contentType }, conditions: null);

                blob.SetMetadata(metaInfo);
                result.Successfull = true;
                result.FileName = fileName;
                result.AbsuloteUri = blob.Uri.AbsolutePath;
            }
            catch (Exception exc)
            {
                result.Successfull = false;
                result.ErrorMessage = exc.Message;
            }



            return result;

        }


        public static BlobResult UploadText(string ContainerName, string BlobName, string FileName, byte[] Data, string contentType)
        {
            BlobResult result = new BlobResult();

            BlobContainerClient container = new BlobContainerClient(_storageAccountString, ContainerName);

            container.CreateIfNotExists(PublicAccessType.None);

            string extension = System.IO.Path.GetExtension(FileName);



            string fileName = String.Format("{0}/{1}{2}", BlobName, FileName + "-" + Guid.NewGuid().ToString().Substring(0, 5) + "-" + BlobName, extension);


            BlobClient blob = container.GetBlobClient(fileName);

            Dictionary<string, string> metaInfo = new Dictionary<string, string>();
            metaInfo.Add("filename", FileName);


            try
            {
                Stream stream = new MemoryStream(Data);

                blob.Upload(stream, new BlobHttpHeaders { ContentType = contentType }, conditions: null);

                blob.SetMetadata(metaInfo);
                result.Successfull = true;
                result.FileName = fileName;
                result.AbsuloteUri = blob.Uri.AbsolutePath;
            }
            catch (Exception exc)
            {
                result.Successfull = false;
                result.ErrorMessage = exc.Message;
            }



            return result;

        }

        public static BlobResult Upload(string ContainerName, string ApplicationGuid, string BlobName, string FileName, System.IO.Stream FileStream)
        {
            BlobResult result = new BlobResult();

            BlobContainerClient container = new BlobContainerClient(_storageAccountString, ContainerName);

            container.CreateIfNotExists(PublicAccessType.None);

            string extension = System.IO.Path.GetExtension(FileName);


            string fileName = String.Format("{0}/{1}{2}", ApplicationGuid.ToLower(), Guid.NewGuid().ToString().Substring(0, 5) + "-" + BlobName, extension);


            BlobClient blob = container.GetBlobClient(fileName);

            Dictionary<string, string> metaInfo = new Dictionary<string, string>();
            metaInfo.Add("filename", FileName);


            try
            {

                blob.Upload(FileStream, true);

                blob.SetMetadata(metaInfo);
                result.Successfull = true;
                result.FileName = fileName;
                result.AbsuloteUri = blob.Uri.AbsolutePath;
            }
            catch (Exception exc)
            {
                result.Successfull = false;
                result.ErrorMessage = exc.Message;
            }



            return result;

        }

        public static BlobResult Upload(string ContainerName, string ApplicationGuid, string BlobName, string FileName, System.IO.Stream FileStream, string contentType)
        {
            BlobResult result = new BlobResult();

            BlobContainerClient container = new BlobContainerClient(_storageAccountString, ContainerName);

            container.CreateIfNotExists(PublicAccessType.None);

            string extension = System.IO.Path.GetExtension(FileName);


            string fileName = String.Format("{0}/{1}{2}", ApplicationGuid.ToLower(), Guid.NewGuid().ToString().Substring(0, 5) + "-" + BlobName, extension);


            BlobClient blob = container.GetBlobClient(fileName);

            Dictionary<string, string> metaInfo = new Dictionary<string, string>();
            metaInfo.Add("filename", FileName);


            try
            {

                blob.Upload(FileStream, true);

                blob.SetMetadata(metaInfo);
                blob.SetHttpHeaders(new BlobHttpHeaders
                {
                    ContentType = contentType,

                });
                result.Successfull = true;
                result.FileName = fileName;
                result.AbsuloteUri = blob.Uri.AbsolutePath;
            }
            catch (Exception exc)
            {
                result.Successfull = false;
                result.ErrorMessage = exc.Message;
            }



            return result;

        }

        public static BlobResult UploadPublicAccess(string ContainerName, string ApplicationGuid, string BlobName, string FileName, System.IO.Stream FileStream)
        {
            BlobResult result = new BlobResult();
            BlobContainerClient container = new BlobContainerClient(_storageAccountString, ContainerName);
            container.CreateIfNotExists(PublicAccessType.Blob);
            string extension = System.IO.Path.GetExtension(FileName);
            string fileName = String.Format("{0}/{1}{2}", ApplicationGuid.ToLower(), Guid.NewGuid().ToString().Substring(0, 5) + "-" + BlobName, extension);
            BlobClient blob = container.GetBlobClient(fileName);
            Dictionary<string, string> metaInfo = new Dictionary<string, string>();
            metaInfo.Add("filename", FileName);
            try
            {
                blob.Upload(FileStream);
                blob.SetMetadata(metaInfo);
                result.Successfull = true;
                result.FileName = fileName;
                result.AbsuloteUri = blob.Uri.AbsolutePath;
            }
            catch (Exception exc)
            {
                result.Successfull = false;
                result.ErrorMessage = exc.Message;
            }
            return result;
        }
        /// <summary>
        /// Get Url to Access a blob temporarly for 15 mins
        /// </summary>
        /// <param name="ContainerName">The Main Container Name (Inspection Order ID)</param>
        /// <param name="Uri">The File Referene Randomly generated for the file</param>
        /// <param name="storedPolicyName"></param>
        /// <returns></returns>
        public static Uri GetServiceSasUriForBlob(string ContainerName, string Uri, string _storageAccountString, string storedPolicyName = null)
        {
            BlobContainerClient container = new BlobContainerClient(_storageAccountString, ContainerName);

            BlobClient blobClient = container.GetBlobClient(HttpUtility.UrlDecode(Uri.Substring(ContainerName.Length + 2)));

            // Check whether this BlobClient object has been authorized with Shared Key.
            if (blobClient.CanGenerateSasUri)
            {
                // Create a SAS token that's valid for one hour.
                BlobSasBuilder sasBuilder = new BlobSasBuilder()
                {
                    BlobContainerName = blobClient.GetParentBlobContainerClient().Name,
                    BlobName = blobClient.Name,
                    Resource = "b"
                };

                if (storedPolicyName == null)
                {
                    sasBuilder.ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(15);
                    sasBuilder.SetPermissions(BlobSasPermissions.Read);
                }
                else
                {
                    sasBuilder.Identifier = storedPolicyName;
                }

                Uri sasUri = blobClient.GenerateSasUri(sasBuilder);
                Console.WriteLine("SAS URI for blob is: {0}", sasUri);
                Console.WriteLine();

                return sasUri;
            }
            else
            {
                Console.WriteLine(@"BlobClient must be authorized with Shared Key 
                          credentials to create a service SAS.");
                return null;
            }
        }
    }
}
