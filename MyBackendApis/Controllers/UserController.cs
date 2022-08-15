using Data.Repositories;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBackendApis.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Data.Contracts;
using WebFramework.Api;
using WebFramework.Filters;
using Entities.User;

namespace MyBackendApis.Controllers
{
    [Route("api/[controller]")]
    [ApiResultFilter]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> Get(CancellationToken cancellationToken)
        {
            var users = await _userRepository.TableNoTracking.ToListAsync(cancellationToken);
            return Ok(users);
        }

        [HttpGet("{id:int}")]
        public async Task<ApiResult<User>> Get(int id, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(cancellationToken, id);
            if (user == null)
                return NotFound();
            return user;
        }

        [HttpPost]
        public async Task<ApiResult<User>> Create(UserDto userDto, CancellationToken cancellationToken)
        {
            //var exists = await userRepository.TableNoTracking.AnyAsync(p => p.UserName == userDto.UserName);
            //if (exists)
            //    return BadRequest("نام کاربری تکراری است");

            var user = new User
            {
                Age = userDto.Age,
                FullName = userDto.FullName,
                Gender = userDto.Gender,
                UserName = userDto.UserName,
            };
            await _userRepository.AddAsync(user, userDto.Password, cancellationToken);
            return user;
        }

        [HttpPut]
        public async Task<ApiResult> Update(int id, User user, CancellationToken cancellationToken)
        {
            var updateUser = await _userRepository.GetByIdAsync(cancellationToken, id);

            updateUser.UserName = user.UserName;
            updateUser.PasswordHash = user.PasswordHash;
            updateUser.FullName = user.FullName;
            updateUser.Age = user.Age;
            updateUser.Gender = user.Gender;
            updateUser.IsActive = user.IsActive;
            updateUser.LastLoginDate = user.LastLoginDate;

            await _userRepository.UpdateAsync(updateUser, cancellationToken);

            return Ok();
        }

        [HttpDelete]
        public async Task<ApiResult> Delete(int id, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(cancellationToken, id);
            await _userRepository.DeleteAsync(user, cancellationToken);

            return Ok();
        }
    }
}
