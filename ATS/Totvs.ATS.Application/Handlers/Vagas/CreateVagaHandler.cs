using Mapster;
using MediatR;
using Totvs.ATS.Application.Commands.Vagas;
using Totvs.ATS.Domain.Entities;
using Totvs.ATS.Domain.Interfaces;

namespace Totvs.ATS.Application.Handlers.Vagas;

public class CreateVagaHandler(IVagaRepository vagaRepository) : IRequestHandler<CreateVagaCommand, string>
{
    public async Task<string> Handle(CreateVagaCommand request, CancellationToken cancellationToken)
    {
        var vaga = request.Adapt<Vaga>();
        await vagaRepository.AddAsync(vaga);
        return vaga.Id;
    }
}