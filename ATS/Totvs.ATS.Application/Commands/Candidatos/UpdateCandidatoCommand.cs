using MediatR;
using Totvs.ATS.Application.DTOs.Candidatos;
using Totvs.ATS.Domain.Enums;

namespace Totvs.ATS.Application.Commands.Candidatos;

public record UpdateCandidatoCommand(
    string Id,
    string Cpf,
    string Nome,
    string Email,
    string Telefone,
    SexoEnum Sexo
) : IRequest<CandidatoDTO>;