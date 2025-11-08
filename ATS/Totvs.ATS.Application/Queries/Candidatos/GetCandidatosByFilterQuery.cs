using MediatR;
using Totvs.ATS.Application.DTOs.Candidatos;
using Totvs.ATS.Application.Filters.Candidatos;

namespace Totvs.ATS.Application.Queries.Candidatos;

public record GetCandidatosByFilterQuery(CandidatoFilter Filtro) : IRequest<IEnumerable<CandidatoDTO>>;