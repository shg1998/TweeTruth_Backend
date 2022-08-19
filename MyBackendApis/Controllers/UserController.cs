using Common.Exceptions;
using Data.Contracts;
using Entities.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyBackendApis.Models;
using Service.Services;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebFrameworks.Api;
using WebFrameworks.Filters;

namespace MyBackendApis.Controllers
{
    [Route("api/[controller]")]
    [ApiResultFilter]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserController> _logger;
        private readonly IJwtService _jwtService;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;

        public UserController(IUserRepository userRepository, ILogger<UserController> logger, IJwtService jwtService,
            UserManager<User> userManager, RoleManager<Role> roleManager, SignInManager<User> signInManager)
        {
            this._userRepository = userRepository;
            this._logger = logger;
            this._jwtService = jwtService;
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._signInManager = signInManager;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<User>>> Get(CancellationToken cancellationToken)
        {
            //var userName = HttpContext.User.Identity.GetUserName();
            //userName = HttpContext.User.Identity.Name;
            //var userId = HttpContext.User.Identity.GetUserId();
            //var userIdInt = HttpContext.User.Identity.GetUserId<int>();
            //var phone = HttpContext.User.Identity.FindFirstValue(ClaimTypes.MobilePhone);
            //var role = HttpContext.User.Identity.FindFirstValue(ClaimTypes.Role);

            var users = await _userRepository.TableNoTracking.ToListAsync(cancellationToken);
            return Ok(users);
        }

        [HttpGet("{id:int}")]
        public async Task<ApiResult<User>> Get(int id, CancellationToken cancellationToken)
        {
            var user2 = await _userManager.FindByIdAsync(id.ToString());
            var role = await _roleManager.FindByNameAsync("Admin");

            var user = await _userRepository.GetByIdAsync(cancellationToken, id);
            if (user == null)
                return NotFound();

            await _userManager.UpdateSecurityStampAsync(user);
            //await userRepository.UpdateSecuirtyStampAsync(user, cancellationToken);

            return user;
        }

        [HttpGet("[action]")]
        [AllowAnonymous]
        public async Task<string> Token(string username, string password, CancellationToken cancellationToken)
        {
            //var user = await userRepository.GetByUserAndPass(username, password, cancellationToken);
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                throw new BadRequestException("نام کاربری یا رمز عبور اشتباه است");
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
            if (!isPasswordValid)
                throw new BadRequestException("نام کاربری یا رمز عبور اشتباه است");


            //if (user == null)
            //    throw new BadRequestException("نام کاربری یا رمز عبور اشتباه است");

            var jwt = await _jwtService.GenerateAsync(user);
            return jwt;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiResult<User>> Create(UserDto userDto, CancellationToken cancellationToken)
        {
            //logger.LogError("متد Create فراخوانی شد");
            //HttpContext.RiseError(new Exception("متد Create فراخوانی شد"));

            //var exists = await userRepository.TableNoTracking.AnyAsync(p => p.UserName == userDto.UserName);
            //if (exists)
            //    return BadRequest("نام کاربری تکراری است");


            var user = new User
            {
                Age = userDto.Age,
                FullName = userDto.FullName,
                Gender = userDto.Gender,
                UserName = userDto.UserName,
                Email = userDto.Email
            };
            var result = await _userManager.CreateAsync(user, userDto.Password);

            var result2 = await _roleManager.CreateAsync(new Role
            {
                Name = "Admin",
                Description = "admin role"
            });

            var result3 = await _userManager.AddToRoleAsync(user, "Admin");

            //await userRepository.AddAsync(user, userDto.Password, cancellationToken);
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
