using IgrejaSocial.Domain.Models;
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

        public AuthController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
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
