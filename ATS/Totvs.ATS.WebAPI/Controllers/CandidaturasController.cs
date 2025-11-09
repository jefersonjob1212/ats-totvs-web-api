using MediatR;
using Microsoft.AspNetCore.Mvc;
using Totvs.ATS.Application.Commands.Candidaturas;
using Totvs.ATS.Application.Queries.Candidaturas;

namespace Totvs.ATS.WebAPI.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CandidaturasController(IMediator  mediator) : ControllerBase
{
    /// <summary>
    /// Vincula um candidato a uma vaga
    /// </summary>
    /// <param name="vagaId"></param>
    /// <param name="candidatoId"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    [HttpPost("vagas/{vagaId}/candidatos/{candidatoId}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<string>> VincularAsync(string vagaId, string candidatoId, CancellationToken cancellation)
    {
        var result =  await mediator.Send(new VincularCandidatoVagaCommand(candidatoId, vagaId), cancellation);
        return Created("", result);
    }

    /// <summary>
    /// Obtém os candidatos que se cadastraram a esta vaga
    /// </summary>
    /// <param name="vagaId"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    [HttpGet("vaga/{vagaId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetVagaComCandidatosAsync(string vagaId, CancellationToken cancellation)
    {
        var dto = await mediator.Send(new GetCandidatosPorVagaQuery(vagaId));
        return dto == null ? NotFound() : Ok(dto);
    }

    /// <summary>
    /// Remove a candidatura de um candidato a vaga
    /// </summary>
    /// <param name="vagaId"></param>
    /// <param name="candidatoId"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    [HttpDelete("vagas/{vagaId}/candidatos/{candidatoId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<string>> DeleteVagasAsync(string vagaId, string candidatoId, CancellationToken cancellation)
    {
        var result = await mediator.Send(new RemoveCandidatoVagaCommand(candidatoId, vagaId), cancellation);
        return Ok(result);
    }
}