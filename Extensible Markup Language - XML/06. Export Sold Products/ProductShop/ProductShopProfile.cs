using AutoMapper;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;

namespace ProductShop;

public class ProductShopProfile : Profile
{
    public ProductShopProfile()
    {
        //User
        this.CreateMap<ImportUserDTO, User>();

        this.CreateMap<User, ExportUserDTO>()
            .ForMember(d => d.SoldProducts, opt => opt.MapFrom(s => s.ProductsSold));

        //Product
        this.CreateMap<ImportProductDTO, Product>();

        this.CreateMap<Product, ExportProductDTO>()
            //.ForMember(d => d.Buyer, opt => opt.MapFrom(src => src.Buyer != null ? $"{src.Buyer.FirstName} {src.Buyer.LastName}" : null))
            .ForMember(d => d.Price, opt => opt.MapFrom(s => decimal.Parse(s.Price.ToString("0.##"))));


        //Category
        this.CreateMap<ImportCategoryDTO, Category>();

        //CategoryProduct
        this.CreateMap<ImportCategoryProductDTO, CategoryProduct>();
    }
}
