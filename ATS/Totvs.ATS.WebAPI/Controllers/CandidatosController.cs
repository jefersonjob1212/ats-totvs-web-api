using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Totvs.ATS.Application.Commands.Candidatos;
using Totvs.ATS.Application.DTOs.Candidatos;
using Totvs.ATS.Application.Filters.Candidatos;
using Totvs.ATS.Application.Queries.Candidatos;

namespace Totvs.ATS.WebAPI.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CandidatosController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Recupera um candidato com Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<CandidatoDTO>> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetCandidatoByIdQuery(id),  cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>
    /// Recupera candidatos com filtros informados
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CandidatoDTO>>> GetAsync([FromQuery] CandidatoFilter filter, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetCandidatosByFilterQuery(filter), cancellationToken);
        return !result.Any() ? NotFound() : Ok(result);
    }

    /// <summary>
    /// Cadastra um novo candidato
    /// </summary>
    /// <param name="candidato"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<string>> PostAsync([FromBody] CandidatoCriarEditarDTO candidato, CancellationToken cancellationToken)
    {
        var command = candidato.Adapt<CreateCandidatoCommand>();
        var result = await mediator.Send(command, cancellationToken);
        return Created("", result);
    }
    
    /// <summary>
    /// Atualiza os dados de um candidato
    /// </summary>
    /// <param name="id"></param>
    /// <param name="candidato"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<CandidatoDTO>> PutAsync([FromRoute] string id, [FromBody] CandidatoCriarEditarDTO candidato, CancellationToken cancellationToken)
    {
        var command = new UpdateCandidatoCommand(
            id, candidato.Cpf, candidato.Nome, candidato.Email, candidato.Telefone, candidato.Sexo);
        var result = await mediator.Send(command, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>
    /// Remove o candidato
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] string id, CancellationToken cancellationToken)
    {
        var command = new DeleteCandidatoCommand(id);
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }
}