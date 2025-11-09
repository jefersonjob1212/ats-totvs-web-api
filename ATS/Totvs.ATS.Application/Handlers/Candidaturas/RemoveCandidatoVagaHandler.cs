using MediatR;
using Totvs.ATS.Application.Commands.Candidaturas;
using Totvs.ATS.Domain.Exceptions;
using Totvs.ATS.Domain.Interfaces;

namespace Totvs.ATS.Application.Handlers.Candidaturas;

public class RemoveCandidatoVagaHandler(IVagaRepository vagaRepository,
    ICandidatoRepository candidatoRepository,
    ICandidaturaRepository candidaturaRepository) : IRequestHandler<RemoveCandidatoVagaCommand, string>
{
    public async Task<string> Handle(RemoveCandidatoVagaCommand request, CancellationToken cancellationToken)
    {
        var vaga = await vagaRepository.FindByIdAsync(request.VagaId);
        if (vaga == null)
            throw new NotFoundException("Vaga não cadastrada");
        if (vaga.Encerrada)
            throw new BusinessRuleException("Vaga foi encerrada.");
        
        var candidato = await candidatoRepository.FindByIdAsync(request.CandidatoId);
        if (candidato == null)
            throw new NotFoundException("Usuário não cadastrado");
        
        var candidatura = (await candidaturaRepository
            .FindAsync(c => c.CandidatoId == request.CandidatoId && c.VagaId == request.VagaId))
            .FirstOrDefault();
        if (candidatura == null)
            throw new NotFoundException("Candidato não está concorrendo a esta vaga.");
        await candidaturaRepository.DeleteAsync(candidatura.Id);
        return "Candidatura removida com sucesso";
    }
}