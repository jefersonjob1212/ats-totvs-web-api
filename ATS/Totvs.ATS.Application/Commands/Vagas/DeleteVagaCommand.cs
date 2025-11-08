using MediatR;

namespace Totvs.ATS.Application.Commands.Vagas;

public record DeleteVagaCommand(string Id) : IRequest<string>;