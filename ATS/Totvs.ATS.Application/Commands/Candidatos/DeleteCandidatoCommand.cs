using MediatR;

namespace Totvs.ATS.Application.Commands.Candidatos;

public record DeleteCandidatoCommand(string CandidatoId) : IRequest<string>;