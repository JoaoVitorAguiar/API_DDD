using Entities.Entities;
using Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using WebAPIs.Models;
using WebAPIs.Token;

namespace WebAPIs.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    public UserController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [AllowAnonymous]
    [Produces("application/json")]
    [HttpPost("/api/CreateTokenIdentity")]
    public async Task<IActionResult> CreateTokenIdentity([FromBody] Login login)
    {
        if (string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Password))
        {
            return Unauthorized();
        }

        var result = await
            _signInManager.PasswordSignInAsync(login.Email, login.Password, false, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            // Recupera Usuário Logado
            var userCurrent = await _userManager.FindByEmailAsync(login.Email);
            var userId = userCurrent.Id;

            var token = new TokenJWTBuilder()
                .AddSecurityKey(JwtSecurityKey.Create("Secret_Key-12345678"))
            .AddSubject("Empresa - Vitor Tech")
            .AddIssuer("Teste.Securiry.Bearer")
            .AddAudience("Teste.Securiry.Bearer")
            .AddClaim("idUsuario", userId)
            .AddExpiry(60)
            .Builder();

            return Ok(token.value);
        }
        else
        {
            return Unauthorized();
        }
    }


    [AllowAnonymous]
    [Produces("application/json")]
    [HttpPost("/api/AddUserIdentity")]
    public async Task<IActionResult> AddUserIdentity([FromBody] Login login)
    {
        if (string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Password))
            return Ok("Falta alguns dados");


        var user = new ApplicationUser
        {
            UserName = login.Email,
            Email = login.Email,
            CPF = login.Cpf,
            Type = UserType.Base,
        };

        var result = await _userManager.CreateAsync(user, login.Password);

        if (result.Errors.Any())
        {
            return Ok(result.Errors);
        }


        // Geração de Confirmação caso precise
        var userId = await _userManager.GetUserIdAsync(user);
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        // retorno email 
        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        var resultado2 = await _userManager.ConfirmEmailAsync(user, code);

        if (resultado2.Succeeded)
            return Ok("Usuário Adicionado com Sucesso");
        else
            return Ok("Erro ao confirmar usuários");

    }

}

