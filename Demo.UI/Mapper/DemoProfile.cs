using AutoMapper;
using Demo.Application.Models;
using Demo.UI.ViewModels;

namespace Demo.UI.Mapper
{
    public class DemoProfile : Profile
    {
        public DemoProfile()
        {
            CreateMap<ProductModel, ProductViewModel>().ReverseMap();
            CreateMap<CategoryModel, CategoryViewModel>().ReverseMap();
        }
    }
}
