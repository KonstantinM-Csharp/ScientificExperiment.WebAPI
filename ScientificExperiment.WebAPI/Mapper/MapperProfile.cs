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
            CreateMap<ResultModel, Result>().ReverseMap();

            CreateMap<ValueModel, Value>().ReverseMap();

            CreateMap<FileModel, DAL.Entities.File>();
        }
    }
}
