using MediatR;
using Totvs.ATS.Application.Commands.Candidatos;
using Totvs.ATS.Domain.Interfaces;

namespace Totvs.ATS.Application.Handlers.Candidatos;

public class DeleteCandidatoHandler(ICandidatoRepository candidatoRepository) : IRequestHandler<DeleteCandidatoCommand, string>
{
    public async Task<string> Handle(DeleteCandidatoCommand request, CancellationToken cancellationToken)
    {
        await candidatoRepository.DeleteAsync(request.CandidatoId);
        return "Candidato excluído com sucesso";
    }
}