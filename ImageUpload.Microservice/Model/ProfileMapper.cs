using ImageUpload.Model;
using AutoMapper;
using ImageUploader.Models;

namespace ImageUpload.Model
{
    public class ProfileMapper : Profile
    {
        public ProfileMapper()
        {
            CreateMap<DBModel, ImageUploadModel>().ReverseMap();
        }
    }
}