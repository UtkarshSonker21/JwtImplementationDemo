using AutoMapper;
using JwtImplementationDemo.Dto;
using JwtImplementationDemo.Entities;

namespace JwtImplementationDemo.Helper.AutoMapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            // entity -> Dto
            CreateMap<User, UserModel>();


            // Dto -> entity
            CreateMap<UserModel, User>();
        }
    }
}
