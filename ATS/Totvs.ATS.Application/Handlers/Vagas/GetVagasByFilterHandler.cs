using System.Linq.Expressions;
using Mapster;
using MediatR;
using Totvs.ATS.Application.DTOs.Vagas;
using Totvs.ATS.Application.Filters.Vagas;
using Totvs.ATS.Application.Queries.Vagas;
using Totvs.ATS.Application.Responses;
using Totvs.ATS.Domain.Entities;
using Totvs.ATS.Domain.Interfaces;

namespace Totvs.ATS.Application.Commands.Vagas;

public class GetVagasByFilterHandler(IVagaRepository vagaRepository) : IRequestHandler<GetVagaByFilterQuery, PagedResponse<VagaDTO>>
{
    public async Task<PagedResponse<VagaDTO>> Handle(GetVagaByFilterQuery request, CancellationToken cancellationToken)
    {
        var filter = request.Filter;
        var expression = BuilderFilter(filter);
        var vagas = await vagaRepository.FindAsync(expression);
        var total = vagas.Count();
        var paged = vagas
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToList();
        
        var dto = paged.Adapt<IEnumerable<VagaDTO>>();
        
        return new PagedResponse<VagaDTO>(
            dto,
            filter.PageNumber,
            filter.PageSize,
            total);
    }

    private static Expression<Func<Vaga, bool>> BuilderFilter(VagaFilter filter)
    {
        return v =>
            (string.IsNullOrEmpty(filter.Titulo) || v.Titulo.ToLower().Contains(filter.Titulo.ToLower())) &&
            (string.IsNullOrEmpty(filter.Localizacao) || v.Localizacao.ToLower().Contains(filter.Localizacao.ToLower())) &&
            (!filter.TipoVaga.HasValue || v.TipoVaga == filter.TipoVaga.Value) &&
            (!filter.SomenteAtivas || v.Encerrada == false);
    }
}