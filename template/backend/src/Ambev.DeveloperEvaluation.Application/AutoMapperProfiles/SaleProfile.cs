using AutoMapper;
using Ambev.DeveloperEvaluation.Application.DTOs;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.AutoMapperProfiles
{
    public class SaleProfile : Profile
    {
        public SaleProfile()
        {
            CreateMap<SalePostDTO, SaleEntity>();
            CreateMap<SaleEntity, SaleReadDTO>();
            CreateMap<SaleItemCancelDTO, SaleItemEntity>();
            CreateMap<SaleItemDTO, SaleItemEntity>();
            CreateMap<SalePostDTO, SaleItemEntity>();
            CreateMap<SaleItemPostDTO, SaleItemEntity>();

            CreateMap<SaleItemPostDTO, SaleItemEntity>()
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
              .ForMember(dest => dest.Id, opt => opt.Ignore()) 
              .ForMember(dest => dest.SaleId, opt => opt.Ignore())  
              .ForMember(dest => dest.Sale, opt => opt.Ignore()) 
              .ForMember(dest => dest.Total, opt => opt.Ignore()) 
              .ForMember(dest => dest.Discount, opt => opt.Ignore()) 
              .ForMember(dest => dest.Cancelled, opt => opt.Ignore());



        }
    }
}
