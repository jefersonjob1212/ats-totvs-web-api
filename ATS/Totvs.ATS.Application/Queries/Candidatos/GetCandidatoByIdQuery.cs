using MediatR;
using Totvs.ATS.Application.DTOs.Candidatos;

namespace Totvs.ATS.Application.Queries.Candidatos;

public record GetCandidatoByIdQuery(string Id) : IRequest<CandidatoDTO>;