using MediatR;
using Totvs.ATS.Application.Commands.Vagas;
using Totvs.ATS.Domain.Interfaces;

namespace Totvs.ATS.Application.Handlers.Vagas;

public class DeleteVagaHandler(IVagaRepository vagaRepository) : IRequestHandler<DeleteVagaCommand, string>
{
    public async Task<string> Handle(DeleteVagaCommand request, CancellationToken cancellationToken)
    {
        await vagaRepository.DeleteAsync(request.Id);
        return "Vaga excluída com sucesso";
    }
}