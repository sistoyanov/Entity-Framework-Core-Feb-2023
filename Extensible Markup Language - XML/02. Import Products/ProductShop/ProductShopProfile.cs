﻿using AutoMapper;
using ProductShop.DTOs.Import;
using ProductShop.Models;

namespace ProductShop;

public class ProductShopProfile : Profile
{
    public ProductShopProfile()
    {
        //User
        this.CreateMap<ImportUserDTO, User>();

        //Product
        this.CreateMap<ImportProductDTO, Product>();
    }
}
