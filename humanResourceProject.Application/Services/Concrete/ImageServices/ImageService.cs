using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using humanResourceProject.Application.Services.Abstract.IImageServices;
using humanResourceProject.Domain.Enum;
using humanResourceProject.Models.DTOs;
using Microsoft.Extensions.Configuration;

namespace humanResourceProject.Application.Services.Concrete.ImageServices
{
    public class ImageService : IImageService
    {
        private readonly IConfiguration _configuration;

        public ImageService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<CompanyManagerRegisterDTO> UploadImageToAzure(CompanyManagerRegisterDTO model)
        {
            if (model.UploadPath == null || model.UploadPath.FileName == null)
            {
                //model.ImagePath = _configuration["AzureStorage:StorageUrl"] + "/defaultprofile.jpg";
                model.ImagePath = model.Gender == Gender.Male ? "https://ik.imagekit.io/7ypp4olwr" + "/maledefault.png?tr=h-200,w-200" : "https://ik.imagekit.io/7ypp4olwr" + "/femaledefault.png?tr=h-200,w-200";
                return model;
            }

            var fileExtension = Path.GetExtension(model.UploadPath.FileName);
            if (fileExtension != ".jpg" && fileExtension != ".png" && fileExtension != ".jpeg")
            {
                //model.ImagePath = _configuration["AzureStorage:StorageUrl"] + "/defaultprofile.jpg";
                model.ImagePath = model.Gender == Gender.Male ? "https://ik.imagekit.io/7ypp4olwr" + "/maledefault.png?tr=h-200,w-200" : "https://ik.imagekit.io/7ypp4olwr" + "/femaledefault.png?tr=h-200,w-200";
                return model;
            }

            using MemoryStream fileUploadStream = new MemoryStream();
            await model.UploadPath.CopyToAsync(fileUploadStream);
            fileUploadStream.Position = 0;

            var fileName = Guid.NewGuid().ToString() + fileExtension;

            var connectionString = _configuration["AzureStorage:ConnectionString"];
            var containerName = _configuration["AzureStorage:Container"];

            BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, containerName);


            BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(fileUploadStream, new BlobUploadOptions()
            {
                HttpHeaders = new BlobHttpHeaders()
                {
                    ContentType = "image/bitmap"
                }
            }, cancellationToken: default);

            //model.ImagePath = _configuration["AzureStorage:StorageUrl"] + "/" + fileName;
            model.ImagePath = "https://ik.imagekit.io/7ypp4olwr" + "/" + fileName + "?tr=h-200,w-200";
            return model;

        }
    }
}
