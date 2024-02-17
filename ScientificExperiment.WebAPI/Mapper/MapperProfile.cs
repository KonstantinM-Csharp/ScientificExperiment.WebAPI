using AutoMapper;
using DAL.Entities;
using System.Globalization;
using ScientificExperiment.WebAPI.Models;
using ScientificExperiment.WebAPI.Services;

namespace ScientificExperiment.WebAPI.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<ResultModel, Result>().ReverseMap();

            CreateMap<ValueModel, Value>().ReverseMap();

            CreateMap<FileModel, DAL.Entities.File>();
        }
    }
}
