using IgrejaSocial.Domain.Models;
using IgrejaSocial.Domain.Identity;
using IgrejaSocial.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace IgrejaSocial.API.Controllers
{
    /// <summary>
    /// Endpoints de autenticação e contexto do usuário.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Autentica o usuário via email e senha.
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return Unauthorized("Credenciais inválidas.");
            }

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, true, false);
            if (!result.Succeeded)
            {
                return Unauthorized("Credenciais inválidas.");
            }

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(new UserInfoResponse
            {
                Email = user.Email ?? string.Empty,
                Roles = roles.ToArray()
            });
        }

        /// <summary>
        /// Cria um novo usuário e o armazena no Identity.
        /// </summary>
        [HttpPost("register")]
        [Authorize(Roles = RoleNames.Administrador)]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Email e senha são obrigatórios.");
            }

            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser is not null)
            {
                return Conflict("Usuário já existe.");
            }

            var roles = request.Roles?.Length > 0
                ? request.Roles
                : new[] { RoleNames.Voluntario };

            foreach (var role in roles.Distinct())
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    return BadRequest($"Perfil inválido: {role}.");
                }
            }

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                EmailConfirmed = true
            };

            var createResult = await _userManager.CreateAsync(user, request.Password);
            if (!createResult.Succeeded)
            {
                return BadRequest(createResult.Errors.Select(error => error.Description));
            }

            var roleResult = await _userManager.AddToRolesAsync(user, roles.Distinct());
            if (!roleResult.Succeeded)
            {
                return BadRequest(roleResult.Errors.Select(error => error.Description));
            }

            return Created(string.Empty, new UserInfoResponse
            {
                Email = user.Email ?? string.Empty,
                Roles = roles.Distinct().ToArray()
            });
        }

        /// <summary>
        /// Encerra a sessão do usuário autenticado.
        /// </summary>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        /// <summary>
        /// Retorna as informações do usuário autenticado.
        /// </summary>
        [HttpGet("me")]
        public async Task<ActionResult<UserInfoResponse>> Me()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return Unauthorized();
            }

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(new UserInfoResponse
            {
                Email = user.Email ?? string.Empty,
                Roles = roles.ToArray()
            });
        }
    }
}
