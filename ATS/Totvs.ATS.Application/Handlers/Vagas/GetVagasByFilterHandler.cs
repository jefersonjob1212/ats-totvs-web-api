using System.Linq.Expressions;
using Mapster;
using MediatR;
using Totvs.ATS.Application.DTOs.Vagas;
using Totvs.ATS.Application.Filters.Vagas;
using Totvs.ATS.Application.Queries.Vagas;
using Totvs.ATS.Domain.Entities;
using Totvs.ATS.Domain.Interfaces;

namespace Totvs.ATS.Application.Handlers.Vagas;

public class GetVagasByFilterHandler(IVagaRepository vagaRepository) : IRequestHandler<GetVagaByFilterQuery, IEnumerable<VagaDTO>>
{
    public async Task<IEnumerable<VagaDTO>> Handle(GetVagaByFilterQuery request, CancellationToken cancellationToken)
    {
        var expression = BuilderFilter(request.Filter);
        var candidatos = await vagaRepository.FindAsync(expression);
        return candidatos.Adapt<IEnumerable<VagaDTO>>();
    }

    private static Expression<Func<Vaga, bool>> BuilderFilter(VagaFilter filter)
    {
        return v =>
            (string.IsNullOrEmpty(filter.Titulo) || v.Titulo.ToLower().Contains(filter.Titulo.ToLower())) &&
            (string.IsNullOrEmpty(filter.Localizacao)  || v.Localizacao.ToLower().Contains(filter.Localizacao.ToLower())) &&
            (!filter.TipoVaga.HasValue || v.TipoVagaEnum == filter.TipoVaga.Value);
    }
}