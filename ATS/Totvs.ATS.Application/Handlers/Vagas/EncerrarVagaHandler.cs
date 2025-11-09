using Mapster;
using MediatR;
using Totvs.ATS.Application.Commands.Vagas;
using Totvs.ATS.Application.DTOs.Vagas;
using Totvs.ATS.Domain.Exceptions;
using Totvs.ATS.Domain.Interfaces;

namespace Totvs.ATS.Application.Handlers.Vagas;

public class EncerrarVagaHandler(IVagaRepository vagaRepository) : IRequestHandler<EncerrarVagaCommand, VagaDTO>
{
    public async Task<VagaDTO> Handle(EncerrarVagaCommand request, CancellationToken cancellationToken)
    {
        var vaga = await vagaRepository.FindByIdAsync(request.Id);
        if (vaga == null)
            throw new NotFoundException("Vaga não encontrada");
        vaga.Encerrada = true;
        await vagaRepository.UpdateAsync(vaga.Id, vaga);
        return vaga.Adapt<VagaDTO>();
    }
}