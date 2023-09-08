using LearnAuthentication.Models;
using Microsoft.AspNetCore.Mvc;

namespace LearnAuthentication.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<LoginController> _logger;
    private readonly IAuthModel _authModel;

    public LoginController(
        ILogger<LoginController> logger,
        IAuthModel authModel
    )
    {
        _logger = logger;
        _authModel = authModel;
    }

    [HttpPost()]
    public async Task<IActionResult> LoginAsync([FromForm] string userName, [FromForm] string password)
    {
        try
        {
            return new JsonResult(await _authModel.Login(userName, password));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}