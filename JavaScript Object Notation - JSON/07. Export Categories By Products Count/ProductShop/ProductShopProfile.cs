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

            this.CreateMap<User, ExportUserDTO>()
                .ForMember(d => d.FirstName, opt => opt.MapFrom(s => s.FirstName))
                .ForMember(d => d.LastName, opt => opt.MapFrom(s => s.LastName))
                .ForMember(d => d.SoldProducts, opt => opt.MapFrom(s => s.ProductsSold));

            //Product
            this.CreateMap<ImportProductDTO, Product>();

            this.CreateMap<Product, ExportProductInRangeDTO>()
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.Price, opt => opt.MapFrom(s => s.Price))
                .ForMember(d => d.Seller, opt => opt.MapFrom(s => $"{s.Seller.FirstName} {s.Seller.LastName}"));

            this.CreateMap<Product, ExportSoldProductDTO>()
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.Price, opt => opt.MapFrom(s => s.Price))
                .ForMember(d => d.BuyerFirstName, opt => opt.MapFrom(s => s.Buyer.FirstName))
                .ForMember(d => d.BuyerLastName, opt => opt.MapFrom(s => s.Buyer.LastName));

            //Category
            this.CreateMap<ImportCatergoryDTO, Category>();

            //CategoryProduct
            this.CreateMap<ImportCategoriesProductDTO, CategoryProduct>();
        }
    }
}
