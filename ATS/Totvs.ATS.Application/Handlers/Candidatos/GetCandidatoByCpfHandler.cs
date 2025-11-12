using Mapster;
using MediatR;
using Totvs.ATS.Application.DTOs.Candidatos;
using Totvs.ATS.Application.Queries.Candidatos;
using Totvs.ATS.Domain.Interfaces;

namespace Totvs.ATS.Application.Handlers.Candidatos;

public class GetCandidatoByCpfHandler(ICandidatoRepository candidatoRepository) : IRequestHandler<GetCandidatoByCpfQuery, CandidatoDTO>
{
    public async Task<CandidatoDTO> Handle(GetCandidatoByCpfQuery request, CancellationToken cancellationToken)
    {
        var candidato = await candidatoRepository.FindAsync(c => c.Cpf == request.Cpf);
        return candidato.FirstOrDefault().Adapt<CandidatoDTO>();
    }
}