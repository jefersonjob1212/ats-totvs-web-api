using MediatR;
using Totvs.ATS.Domain.Enums;

namespace Totvs.ATS.Application.Commands.Candidatos;

public record CreateCandidatoCommand(
    string Cpf,
    string Nome,
    string Email,
    string Telefone,
    SexoEnum Sexo
) : IRequest<string>;