using MediatR;
using Totvs.ATS.Application.Commands.Candidaturas;
using Totvs.ATS.Domain.Entities;
using Totvs.ATS.Domain.Exceptions;
using Totvs.ATS.Domain.Interfaces;

namespace Totvs.ATS.Application.Handlers.Candidaturas;

public class VincularCandidatoVagaHandler(
    IVagaRepository vagaRepository,
    ICandidatoRepository candidatoRepository,
    ICandidaturaRepository candidaturaRepository) : IRequestHandler<VincularCandidatoVagaCommand, string>
{
    public async Task<string> Handle(VincularCandidatoVagaCommand request, CancellationToken cancellationToken)
    {
        var vaga = await vagaRepository.FindByIdAsync(request.VagaId);
        if (vaga == null)
            throw new NotFoundException("Vaga não cadastrada");
        if (vaga.Encerrada)
            throw new BusinessRuleException("Vaga foi encerrada.");
        
        var candidato = await candidatoRepository.FindByIdAsync(request.CandidatoId);
        if (candidato == null)
            throw new NotFoundException("Usuário não cadastrado");

        var candidatura = new Candidatura
        {
            VagaId = vaga.Id,
            CandidatoId = candidato.Id
        };
        await candidaturaRepository.AddAsync(candidatura);
        return candidatura.Id;
    }
}