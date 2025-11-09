using MediatR;

namespace Totvs.ATS.Application.Commands.Candidaturas;

public record RemoveCandidatoVagaCommand(
    string CandidatoId,
    string VagaId) : IRequest<string>;