using AutoMapper;
using Demo.Api.Models;
using Demo.Application.Models;

namespace Demo.Api.Mapper
{
    public class DemoProfile : Profile
    {
        public DemoProfile()
        {
            CreateMap<ProductModel, ProductApiModel>().ReverseMap();
            CreateMap<CategoryModel, CategoryApiModel>().ReverseMap();
        }
    }
}
