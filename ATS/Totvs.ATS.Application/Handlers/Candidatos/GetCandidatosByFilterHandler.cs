using System.Linq.Expressions;
using Mapster;
using MediatR;
using Totvs.ATS.Application.DTOs.Candidatos;
using Totvs.ATS.Application.Filters.Candidatos;
using Totvs.ATS.Application.Queries.Candidatos;
using Totvs.ATS.Application.Responses;
using Totvs.ATS.Domain.Entities;
using Totvs.ATS.Domain.Interfaces;

namespace Totvs.ATS.Application.Handlers.Candidatos;

public class GetCandidatosByFilterHandler(ICandidatoRepository candidatoRepository) : IRequestHandler<GetCandidatosByFilterQuery, PagedResponse<CandidatoDTO>>
{
    public async Task<PagedResponse<CandidatoDTO>> Handle(GetCandidatosByFilterQuery request, CancellationToken cancellationToken)
    {
        var filter = request.Filtro;
        var expression = BuilderFilter(filter);
        var candidatos = await candidatoRepository.FindAsync(expression);
        var total = candidatos.Count();
        var paged = candidatos
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToList();
        
        var dto = paged.Adapt<IEnumerable<CandidatoDTO>>();
        
        return new PagedResponse<CandidatoDTO>(
            dto,
            filter.PageNumber,
            filter.PageSize,
            total);
    }

    private static Expression<Func<Candidato, bool>> BuilderFilter(CandidatoFilter filtro)
    {
        return c => 
            (string.IsNullOrEmpty(filtro.Nome) || c.Nome.ToLower().Contains(filtro.Nome.ToLower())) &&
            (string.IsNullOrEmpty(filtro.Email) || c.Email.ToLower().Contains(filtro.Email.ToLower())) &&
            (!filtro.Sexo.HasValue || c.Sexo == filtro.Sexo.Value);
    }
}