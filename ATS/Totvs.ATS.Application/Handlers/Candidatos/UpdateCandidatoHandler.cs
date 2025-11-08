using Mapster;
using MediatR;
using Totvs.ATS.Application.Commands.Candidatos;
using Totvs.ATS.Application.DTOs.Candidatos;
using Totvs.ATS.Domain.Entities;
using Totvs.ATS.Domain.Interfaces;

namespace Totvs.ATS.Application.Handlers.Candidatos;

public class UpdateCandidatoHandler(ICandidatoRepository candidatoRepository) : IRequestHandler<UpdateCandidatoCommand, CandidatoDTO>
{
    public async Task<CandidatoDTO> Handle(UpdateCandidatoCommand request, CancellationToken cancellationToken)
    {
        var candidato = request.Adapt<Candidato>();
        await candidatoRepository.UpdateAsync(candidato.Id, candidato);
        return candidato.Adapt<CandidatoDTO>();
    }
}