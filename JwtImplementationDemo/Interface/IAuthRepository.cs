using JwtImplementationDemo.Dto;

namespace JwtImplementationDemo.Interface
{
    public interface IAuthRepository
    {
        Task<ResponseModel> Login(LoginModel model);
    }
}
