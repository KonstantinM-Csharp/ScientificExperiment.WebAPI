using AutoMapper;
using DAL.Entities;
using System.Globalization;
using WebApi_CSV.Models;
using WebApi_CSV.Services;

namespace WebApi_CSV.Mapper
{
    public class MapperProfile : Profile
    {
        private readonly FileService _fileService;
        public MapperProfile(FileService fileService)
        {
             _fileService=fileService;
        }
        public MapperProfile()
        {
            CreateMap<Result, ResultModel>()
                .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileId.Equals(0) ? null : _fileService.GetFileName(src.FileId)));

            CreateMap<ResultModel, Result>()
               .ForMember(dest => dest.FileId, opt => opt.MapFrom(src => _fileService.GetFileId(src.FileName)));

            CreateMap<Value, ValueModel>()
                .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileId.Equals(0) ? null : _fileService.GetFileName(src.FileId)));

            CreateMap<ValueModel, Value>()
                .ForMember(dest => dest.FileId, opt => opt.MapFrom(src => _fileService.GetFileId(src.FileName)));

            CreateMap<FileModel, DAL.Entities.File>();
        }
    }
}
