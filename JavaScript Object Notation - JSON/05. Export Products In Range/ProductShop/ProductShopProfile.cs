using AutoMapper;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile() 
        {
            //User
            this.CreateMap<ImportUserDTO, User>();

            //Product
            this.CreateMap<ImportProductDTO, Product>();
            this.CreateMap<Product, ExportProductInRangeDTO>()
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.Price, opt => opt.MapFrom(s => s.Price))
                .ForMember(d => d.Seller, opt => opt.MapFrom(s => $"{s.Seller.FirstName} {s.Seller.LastName}"));

            //Category
            this.CreateMap<ImportCatergoryDTO, Category>();

            //CategoryProduct
            this.CreateMap<ImportCategoriesProductDTO, CategoryProduct>();
        }
    }
}
