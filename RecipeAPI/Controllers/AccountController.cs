using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RecipeAPI.DAL;
using RecipeAPI.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RecipeAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private RecipeDbContext _context;
        private readonly ILogger<CategoryController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(RecipeDbContext context, ILogger<CategoryController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [HttpPost(Name = "Register")]
        public async Task<AccountResponse> Register([FromBody]Register model)
        {
            AppUser user = new AppUser
            {
                Firstname = model.FirstName,
                Lastname = model.LastName,
                Email = model.Email,
                UserName = model.UserName,
                PhoneNumber = model.PhoneNumber
            };
            IdentityResult identityResult = await _userManager.CreateAsync(user, model.Password);

            if (identityResult.Succeeded)
            {
                return new AccountResponse { Code = 200 };
            }
            else
            {
                var str = "";
                foreach (var item in identityResult.Errors)
                {
                    str += " " + item.Description;
                }
                return new AccountResponse { Code = 400, Description = str};
            }
        }

        [HttpPost(Name = "Login")]
        public async Task<AccountResponse> Login([FromBody] Login model)
        {
            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, true, true);

            if (signInResult.Succeeded)
            {
                return new AccountResponse { Code = 200 };
            }
            else
            {
                return new AccountResponse { Code = 400, Description = "Username or Password wrong" };
            }

        }
    }
}

