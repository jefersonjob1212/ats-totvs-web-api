using MediatR;
using Totvs.ATS.Domain.Enums;

namespace Totvs.ATS.Application.Commands.Vagas;

public record CreateVagaCommand(
    string Titulo,
    string Descricao,
    string Localizacao,
    TipoVagaEnum TipoVaga
) : IRequest<string>;