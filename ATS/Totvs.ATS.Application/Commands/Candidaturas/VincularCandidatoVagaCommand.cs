using MediatR;

namespace Totvs.ATS.Application.Commands.Candidaturas;

public record VincularCandidatoVagaCommand(
    string CandidatoId,
    string VagaId
    ) : IRequest<string>;