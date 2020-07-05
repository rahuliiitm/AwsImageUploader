using ImageUploader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageUpload.Services
{
    public interface IImageUploaderStorageService
    {
        Task<string> AddAsync(ImageUploadModel model);
        Task ConfirmAsync(ConfirmModel model);
        Task<ImageUploadModel> GetByIdAsync(string id);
        Task<bool> CheckHealthAsync();
        Task<List<ImageUploadModel>> GetAllAsync();
    }
}
