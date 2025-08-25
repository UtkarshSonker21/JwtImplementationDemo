using JwtImplementationDemo.Helper.Exceptions;
using AutoMapper;
using JwtImplementationDemo.Dto;
using JwtImplementationDemo.Entities;
using JwtImplementationDemo.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace JwtImplementationDemo.Repository
{
    public class UserRepository:IUserRepository
    {
        private readonly ParcticeContext _context;
        private readonly IMapper _mapper;

        public UserRepository(ParcticeContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<ResponseModel> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();

            //var userList = users.Select(x => new UserModel
            //{
            //    UserId = x.UserId,
            //    UserName = x.UserName,
            //    Name = x.UserName,
            //    Role = x.Role,
            //    Password = x.Password,
            //    Contact = x.Contact
            //}).ToList();

            var userList = _mapper.Map<List<UserModel>>(users);

            return new ResponseModel
            {
                Result = userList,
            };
        }



        public async Task<ResponseModel> GetById(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync( x => x.UserId == id);

            if (user == null)
            {
                throw new NotFoundException($"User with {id} not found");
            }

            //var userModel = new UserModel
            //{
            //    UserId = user.UserId,
            //    UserName = user.UserName,
            //    Name = user.UserName,
            //    Role = user.Role,
            //    Password = user.Password,
            //    Contact = user.Contact
            //};

            var userModel = _mapper.Map<UserModel>(user);

            return new ResponseModel
            {
                Result = userModel,
            };
        }



        public async Task<ResponseModel> AddUser(UserModel userModel)
        {

            var user = _mapper.Map<User>(userModel);
                
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();


            return new ResponseModel
            {
                Message = "User created successfully.",
            };
        }



        public async Task<ResponseModel> UpdateUser(UserModel userModel, int id)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.UserId == id);

            if (existingUser == null) 
            {
                throw new NotFoundException($"User with {id} not found");
            }

            existingUser.Name = userModel.Name;
            existingUser.UserName = userModel.UserName;
            existingUser.Password = userModel.Password;
            existingUser.Contact = userModel.Contact;

            await _context.SaveChangesAsync();

            return new ResponseModel
            {
                Message = "User updated successfully.",
            };
        }



        public async Task<ResponseModel> DeleteUser(int id)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.UserId == id);

            if (existingUser == null)
            {
                throw new NotFoundException($"User with {id} not found");
            }

            _context.Remove(existingUser);
            await _context.SaveChangesAsync();

            return new ResponseModel
            {
                Message = "User deleted successfully.",
            };
        }

    
    }
}
