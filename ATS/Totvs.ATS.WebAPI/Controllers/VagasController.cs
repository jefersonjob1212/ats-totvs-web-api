using MediatR;
using Microsoft.AspNetCore.Mvc;
using Totvs.ATS.Application.Commands.Vagas;
using Totvs.ATS.Application.DTOs.Vagas;
using Totvs.ATS.Application.Filters.Vagas;
using Totvs.ATS.Application.Queries.Vagas;

namespace Totvs.ATS.WebAPI.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class VagasController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Busca as vagas com filtros informados
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<VagaDTO>>> GetAsync([FromQuery] VagaFilter request, CancellationToken cancellationToken)
    {
        var query = new GetVagaByFilterQuery(request);
        var result = await mediator.Send(query,  cancellationToken);
        return result.Items.Any() ? Ok(result) : NotFound();
    }

    /// <summary>
    /// Busca a vaga pelo identificador único
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VagaDTO>> GetAsync([FromRoute] string id, CancellationToken cancellationToken)
    {
        var query = new GetVagaByIdQuery(id);
        var result = await mediator.Send(query, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>
    /// Cadastra uma nova vaga
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<string>> PostAsync([FromBody] CriarEditarVagaDTO request, CancellationToken cancellationToken)
    {
        var command =
            new CreateVagaCommand(request.Titulo, request.Descricao, request.Localizacao, request.TipoVaga);
        var result = await mediator.Send(command, cancellationToken);
        return Created("", new {id = result});
    }

    /// <summary>
    /// Atualiza uma vaga
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VagaDTO>> PutAsync([FromRoute] string id, [FromBody] CriarEditarVagaDTO request, CancellationToken cancellationToken)
    {
        var command = new UpdateVagaCommand(id, request.Titulo, request.Descricao, request.Localizacao,
            request.TipoVaga);
        var result = await mediator.Send(command, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>
    /// Remove uma vaga
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync([FromRoute] string id, CancellationToken cancellationToken)
    {
        var command = new DeleteVagaCommand(id);
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Realiza o encerramento da vaga
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPatch("{id}/encerrar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EncerradoAsync([FromRoute] string id, CancellationToken cancellationToken)
    {
        var command = new EncerrarVagaCommand(id);
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}
