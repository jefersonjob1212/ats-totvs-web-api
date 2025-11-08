using Mapster;
using MediatR;
using Totvs.ATS.Application.DTOs.Vagas;
using Totvs.ATS.Application.Queries.Vagas;
using Totvs.ATS.Domain.Interfaces;

namespace Totvs.ATS.Application.Handlers.Vagas;

public class GetVagaByIdHander(IVagaRepository vagaRepository) : IRequestHandler<GetVagaByIdQuery, VagaDTO>
{
    public async Task<VagaDTO> Handle(GetVagaByIdQuery request, CancellationToken cancellationToken)
    {
        var vaga = await vagaRepository.FindByIdAsync(request.Id);
        return vaga.Adapt<VagaDTO>();
    }
}