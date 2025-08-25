using JwtImplementationDemo.Dto;
using JwtImplementationDemo.Entities;

namespace JwtImplementationDemo.Interface
{
    public interface IUserRepository
    {
        Task<ResponseModel> GetAllUsers();


        Task<ResponseModel> GetById(int id);


        Task<ResponseModel> AddUser(UserModel user);


        Task<ResponseModel> DeleteUser(int id);


        Task<ResponseModel> UpdateUser(UserModel user , int id);


        //Task<UserModel?> GetMyProfile(int id);

    }
}
