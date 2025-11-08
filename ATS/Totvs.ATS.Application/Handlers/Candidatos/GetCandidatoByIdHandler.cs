using Mapster;
using MediatR;
using Totvs.ATS.Application.DTOs.Candidatos;
using Totvs.ATS.Application.Queries.Candidatos;
using Totvs.ATS.Domain.Interfaces;

namespace Totvs.ATS.Application.Handlers.Candidatos;

public class GetCandidatoByIdHandler(ICandidatoRepository candidatoRepository) : IRequestHandler<GetCandidatoByIdQuery, CandidatoDTO>
{
    public async Task<CandidatoDTO> Handle(GetCandidatoByIdQuery request, CancellationToken cancellationToken)
    {
        var candidato = await candidatoRepository.FindByIdAsync(request.Id);
        return candidato.Adapt<CandidatoDTO>();
    }
}