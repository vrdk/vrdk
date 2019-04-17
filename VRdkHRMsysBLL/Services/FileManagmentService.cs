using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using VRdkHRMsysBLL.Interfaces;

namespace VRdkHRMsysBLL.Services
{
    public class FileManagmentService : IFileManagmentService
    {
        private readonly IConfiguration _configuration;
        public FileManagmentService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task UploadDefaultUserPhoto(string fileName)
        {
            var defaultPhoto = await DownloadFileAsync("default.png", "photos");
            await UploadFileInBlocks(defaultPhoto, $"{fileName}.png", "photos");
        }

        public async Task UploadUserPhoto(IFormFile file, string containerName, string fileName)
        {
            
            await UploadFile(file, containerName, fileName);
        }

        public async Task UploadSickLeaveFileAsync(IFormFile file, string containerName)
        {
                await UploadFile(file, containerName);
        }

        public async Task<string[]> GetSickLeaveFilesAsync(string id)
        {
            CloudBlobContainer cloudBlobContainer = await GetContainerReference(id);
            var files = await cloudBlobContainer.ListBlobsSegmentedAsync(null);
            var result = new List<string>();
            foreach (var file in files.Results)
            {
                result.Add(((CloudBlockBlob)file).Name);
            }

            return result.ToArray();
        }

        public async Task<byte[]> DownloadFileAsync(string fileName, string containerName)
        {
            var file = await DownloadFileInBlocks(fileName, containerName);
            return file;
        }

        private byte[] ConvertToByteArray(IFormFile fileToConvert)
        {
            byte[] content;
            using (MemoryStream ms = new MemoryStream())
            {
                fileToConvert.CopyTo(ms);
                content = ms.ToArray();
            }

            return content;
        }

        private async Task UploadFile(IFormFile file, string containerName, string fileName = null)
        {
            var byteFile = ConvertToByteArray(file);
            if(fileName == null)
            {
                await UploadFileInBlocks(byteFile, file.FileName, containerName);
            }
            else
            {
                await UploadFileInBlocks(byteFile, $"{fileName}.png", containerName);
            }
        }

        private async Task<byte[]> DownloadFileInBlocks(string fileName, string containerName)
        {
            CloudBlobContainer cloudBlobContainer = await GetContainerReference(containerName);
            CloudBlockBlob blob = cloudBlobContainer.GetBlockBlobReference(Path.GetFileName(fileName));
            SharedAccessBlobPolicy a = new SharedAccessBlobPolicy
            {
                Permissions = SharedAccessBlobPermissions.Read
            };

            int blockSize = 1024 * 1024;

            await blob.FetchAttributesAsync();
            long fileSize = blob.Properties.Length;

            byte[] blobContents = new byte[fileSize];
            int position = 0;

            while (fileSize > 0)
            {
                int blockLength = (int)Math.Min(blockSize, fileSize);

                await blob.DownloadRangeToByteArrayAsync(blobContents, position, position, blockLength);

                position += blockLength;
                fileSize -= blockSize;
            }

            return blobContents;
        }

        private async Task UploadFileInBlocks(byte[] file, string fileName, string containerName)
        {
            CloudBlobContainer cloudBlobContainer = await GetContainerReference(containerName);
            CloudBlockBlob blob = cloudBlobContainer.GetBlockBlobReference(Path.GetFileName(fileName));

            await blob.DeleteIfExistsAsync();

            List<string> blockIDs = new List<string>();

            int blockSize = 5 * 1024 * 1024;

            long fileSize = file.Length;

            int blockId = 0;

            while (fileSize > 0)
            {
                int blockLength = (int)Math.Min(blockSize, fileSize);

                string blockIdEncoded = GetBase64BlockId(blockId);
                blockIDs.Add(blockIdEncoded);

                byte[] bytesToUpload = new byte[blockLength];
                Array.Copy(file, blockId * blockSize, bytesToUpload, 0, blockLength);
                using (MemoryStream memoryStream = new MemoryStream(bytesToUpload, 0, blockLength))
                {
                    await blob.PutBlockAsync(blockIdEncoded, memoryStream, null);
                }

                blockId++;
                fileSize -= blockLength;
            }

            await blob.PutBlockListAsync(blockIDs);
        }

        private string GetBase64BlockId(int blockId)
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}", blockId.ToString("0000000"))));
        }

        private async Task<CloudBlobContainer> GetContainerReference(string containerName)
        {
            CloudStorageAccount storageAccount = new CloudStorageAccount(
             new StorageCredentials(
             _configuration["AzureStorageSettings:account"],
             _configuration["AzureStorageSettings:key"]), true);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);
            await container.CreateIfNotExistsAsync();
            return container;
        }
    }
}
