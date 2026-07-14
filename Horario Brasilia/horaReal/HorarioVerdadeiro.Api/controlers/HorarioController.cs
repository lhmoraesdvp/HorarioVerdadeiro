using Microsoft.AspNetCore.Mvc;

namespace HorarioVerdadeiro.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HorarioController : ControllerBase
{
    [HttpGet("agora")]
    public IActionResult Agora()
    {
        return Ok(new
        {
            horaBrasilia = DateTime.Now,
            fonte = "Sistema",
            observacao = "Horário temporário até integração com NTP"
        });
    }
}