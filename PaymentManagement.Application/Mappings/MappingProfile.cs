using AutoMapper;
using PaymentManagement.Application.DTO;
using PaymentManagement.Domain;

namespace PaymentManagement.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, CreateUserDto>().ReverseMap();

            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<Customer, CreateCustomerDto>().ReverseMap();

            CreateMap<Company, CompanyDto>().ReverseMap();
            CreateMap<Company, CreateCompanyDto>().ReverseMap();

            CreateMap<Invoice, InvoiceDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ReverseMap();

            CreateMap<Invoice, CreateInvoiceDto>().ReverseMap();

            CreateMap<Payment, PaymentDto>().ReverseMap();
            CreateMap<Payment, CreatePaymentDto>().ReverseMap();

            CreateMap<PaymentMethod, PaymentMethodDto>().ReverseMap();
            CreateMap<PaymentMethod, CreatePaymentMethodDto>().ReverseMap();
        }
    }
}
