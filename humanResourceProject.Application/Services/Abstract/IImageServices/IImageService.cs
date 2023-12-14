﻿using humanResourceProject.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace humanResourceProject.Application.Services.Abstract.IImageServices
{
    public interface IImageService
    {
        Task<UserRegisterDTO> UploadImageToAzure(UserRegisterDTO model);
    }
}
