using MediatR;
using Totvs.ATS.Application.DTOs.Candidatos;

namespace Totvs.ATS.Application.Queries.Candidatos;

public record GetCandidatoByCpfQuery(string Cpf) : IRequest<CandidatoDTO>;