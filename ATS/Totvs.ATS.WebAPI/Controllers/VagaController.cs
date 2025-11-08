using MediatR;
using Microsoft.AspNetCore.Mvc;
using Totvs.ATS.Application.Commands.Vagas;
using Totvs.ATS.Application.DTOs.Vagas;
using Totvs.ATS.Application.Filters.Vagas;
using Totvs.ATS.Application.Queries.Vagas;

namespace Totvs.ATS.WebAPI.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class VagaController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAsync([FromQuery] VagaFilter request)
    {
        var command = new GetVagaByFilterQuery(request);
        var result = await mediator.Send(command);
        return result.Any() ? Ok(result) : NotFound();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync([FromRoute] string id)
    {
        var command = new GetVagaByIdQuery(id);
        var result = await mediator.Send(command);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] CriarEditarVagaDTO request)
    {
        var command =
            new CreateVagaCommand(request.Titulo, request.Descricao, request.Localizacao, request.TipoVagaEnum);
        var result = await mediator.Send(command);
        return Created("", result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsync([FromRoute] string id, [FromBody] CriarEditarVagaDTO request)
    {
        var command = new UpdateVagaCommand(id, request.Titulo, request.Descricao, request.Localizacao,
            request.TipoVagaEnum);
        var result = await mediator.Send(command);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] string id)
    {
        var command = new DeleteVagaCommand(id);
        var result = await mediator.Send(command);
        return NoContent();
    }
}
