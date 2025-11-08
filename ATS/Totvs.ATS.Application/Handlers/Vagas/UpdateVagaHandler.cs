using Mapster;
using MediatR;
using Totvs.ATS.Application.Commands.Vagas;
using Totvs.ATS.Application.DTOs.Vagas;
using Totvs.ATS.Domain.Entities;
using Totvs.ATS.Domain.Interfaces;

namespace Totvs.ATS.Application.Handlers.Vagas;

public class UpdateVagaHandler(IVagaRepository vagaRepository) : IRequestHandler<UpdateVagaCommand, VagaDTO>
{
    public async Task<VagaDTO> Handle(UpdateVagaCommand request, CancellationToken cancellationToken)
    {
        var vaga = request.Adapt<Vaga>();
        await vagaRepository.UpdateAsync(vaga.Id, vaga);
        return vaga.Adapt<VagaDTO>();
    }
}