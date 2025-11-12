using Mapster;
using MediatR;
using Totvs.ATS.Application.Commands.Candidatos;
using Totvs.ATS.Domain.Entities;
using Totvs.ATS.Domain.Exceptions;
using Totvs.ATS.Domain.Interfaces;

namespace Totvs.ATS.Application.Handlers.Candidatos;

public class CreateCandidatoHandler(ICandidatoRepository candidatoRepository)
    : IRequestHandler<CreateCandidatoCommand, string>
{
    public async Task<string> Handle(CreateCandidatoCommand request, CancellationToken cancellationToken)
    {
        var candidato = request.Adapt<Candidato>();
        var candidatoJaExiste = await candidatoRepository.FindAsync(c => c.Cpf == candidato.Cpf);
        if (candidatoJaExiste.Any())
        {
            throw new BusinessRuleException("Candidato já cadastrado");
        }
        await candidatoRepository.AddAsync(candidato);
        return candidato.Id.ToString();
    }
}