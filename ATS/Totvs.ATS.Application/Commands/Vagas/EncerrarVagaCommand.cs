using MediatR;
using Totvs.ATS.Application.DTOs.Vagas;

namespace Totvs.ATS.Application.Commands.Vagas;

public record EncerrarVagaCommand(string Id) : IRequest<VagaDTO>;