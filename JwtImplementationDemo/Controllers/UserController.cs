using JwtImplementationDemo.Dto;
using JwtImplementationDemo.Helper.Exceptions;
using JwtImplementationDemo.Helper.Filters;
using JwtImplementationDemo.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JwtImplementationDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;


        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }



        [HttpGet("GetAllUsers")]
        [Authorize]
        [ServiceFilter(typeof(CachingFilter))]
        public async Task<ActionResult<ResponseModel>> GetAllUsers()
        {
            var response = await _userRepository.GetAllUsers();

            return Ok(response);
        }



        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<ResponseModel>> GetUserById(int id)
        {
            var response = await _userRepository.GetById(id);

            return Ok(response);
        }



        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseModel>> AddUser(UserModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _userRepository.AddUser(user);

            return Ok(response);
        }



        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseModel>> UpdateUser(UserModel user, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _userRepository.UpdateUser(user,id);

            return Ok(response);
        }



        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseModel>> DeleteUset(int id)
        {
            var response = await _userRepository.DeleteUser(id);

            return Ok(response);
        }



        [HttpGet("GetMyProfile")]
        [Authorize]
        public async Task<ActionResult<ResponseModel>> GetMyProfile()
        {
            var userId = User.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedException("UserId claim is missing.");

                
            var response = await _userRepository.GetById(Convert.ToInt32(userId));

            return Ok(response);
        }




    }
}
