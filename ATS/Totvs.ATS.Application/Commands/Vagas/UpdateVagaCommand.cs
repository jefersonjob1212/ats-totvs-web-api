using MediatR;
using Totvs.ATS.Application.DTOs.Vagas;
using Totvs.ATS.Domain.Enums;

namespace Totvs.ATS.Application.Commands.Vagas;

public record UpdateVagaCommand(
    string Id,
    string Titulo,
    string Descricao,
    string Localizacao,
    TipoVagaEnum TipoVagaEnum
    ) : IRequest<VagaDTO>;