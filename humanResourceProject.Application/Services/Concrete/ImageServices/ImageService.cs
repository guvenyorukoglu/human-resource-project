using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using humanResourceProject.Application.Services.Abstract.IAppUserServices;
using humanResourceProject.Application.Services.Abstract.IBaseServices;
using humanResourceProject.Application.Services.Abstract.IImageServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.Enum;
using humanResourceProject.Models.DTOs;
using humanResourceProject.Presentation.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace humanResourceProject.Application.Services.Concrete.ImageServices
{
    public class ImageService : IImageService
    {
        private readonly IConfiguration _configuration;
        private readonly IBaseReadService<AppUser> _appUserReadService;
        private readonly IBaseWriteService<AppUser> _appUserWriteService;

        public ImageService(IConfiguration configuration, IBaseReadService<AppUser> appUserReadService, IBaseWriteService<AppUser> appUserWriteService)
        {
            _configuration = configuration;
            _appUserReadService = appUserReadService;
            _appUserWriteService = appUserWriteService;
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
        public async Task<IdentityResult> UpdateProfileImage(UpdateProfileImageDTO model)
        {
            AppUser user = await _appUserReadService.GetSingleDefault(x => x.Id == model.Id);

            var fileExtension = Path.GetExtension(model.UploadPath.FileName);
            
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

            user.ImagePath = "https://ik.imagekit.io/7ypp4olwr" + "/" + fileName + "?tr=h-200,w-200";
            user.UpdateDate = DateTime.Now;
            user.Status = Status.Modified;

            var result = await _appUserWriteService.Update(user);
            if (result)
                return IdentityResult.Success;
            else
                return IdentityResult.Failed();

        }
    }
}
