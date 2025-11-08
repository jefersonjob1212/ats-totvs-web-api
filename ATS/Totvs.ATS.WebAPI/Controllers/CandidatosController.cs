using MediatR;
using Microsoft.AspNetCore.Mvc;
using Totvs.ATS.Application.Commands.Candidatos;
using Totvs.ATS.Application.DTOs.Candidatos;
using Totvs.ATS.Application.Filters.Candidatos;
using Totvs.ATS.Application.Queries.Candidatos;

namespace Totvs.ATS.WebAPI.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CandidatosController : ControllerBase
{
    private readonly IMediator _mediator;

    public CandidatosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
        var result = await _mediator.Send(new GetCandidatoByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync([FromQuery] CandidatoFilter filter)
    {
        var result = await _mediator.Send(new GetCandidatosByFilterQuery(filter));
        return !result.Any() ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] CandidatoCriarEditarDTO candidato)
    {
        var command = new CreateCandidatoCommand(candidato.Nome, candidato.Email, candidato.Telefone, candidato.Sexo);
        var result = await _mediator.Send(command);
        return Created("", result);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsync([FromRoute] string id, [FromBody] CandidatoCriarEditarDTO candidato)
    {
        var command = new UpdateCandidatoCommand(id, candidato.Nome, candidato.Email, candidato.Telefone, candidato.Sexo);
        var result = await _mediator.Send(command);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] string id)
    {
        var command = new DeleteCandidatoCommand(id);
        var result = await _mediator.Send(command);
        return NoContent();
    }
}