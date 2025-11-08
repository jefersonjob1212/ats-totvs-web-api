using MediatR;
using Totvs.ATS.Application.DTOs.Vagas;
using Totvs.ATS.Application.Filters.Vagas;
using Totvs.ATS.Domain.Enums;

namespace Totvs.ATS.Application.Queries.Vagas;

public record GetVagaByFilterQuery(VagaFilter Filter) : IRequest<IEnumerable<VagaDTO>>;