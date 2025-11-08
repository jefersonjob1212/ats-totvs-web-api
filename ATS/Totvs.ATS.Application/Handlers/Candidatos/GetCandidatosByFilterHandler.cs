using System.Linq.Expressions;
using Mapster;
using MediatR;
using Totvs.ATS.Application.DTOs.Candidatos;
using Totvs.ATS.Application.Filters.Candidatos;
using Totvs.ATS.Application.Queries.Candidatos;
using Totvs.ATS.Domain.Entities;
using Totvs.ATS.Domain.Interfaces;

namespace Totvs.ATS.Application.Handlers.Candidatos;

public class GetCandidatosByFilterHandler(ICandidatoRepository candidatoRepository) : IRequestHandler<GetCandidatosByFilterQuery, IEnumerable<CandidatoDTO>>
{
    public async Task<IEnumerable<CandidatoDTO>> Handle(GetCandidatosByFilterQuery request, CancellationToken cancellationToken)
    {
        var expression = BuilderFilter(request.Filtro);
        var candidatos = await candidatoRepository.FindAsync(expression);
        return candidatos.Adapt<IEnumerable<CandidatoDTO>>();
    }

    private static Expression<Func<Candidato, bool>> BuilderFilter(CandidatoFilter filtro)
    {
        return c => 
            (string.IsNullOrEmpty(filtro.Nome) || c.Nome.ToLower().Contains(filtro.Nome.ToLower())) &&
            (string.IsNullOrEmpty(filtro.Email) || c.Email.ToLower().Contains(filtro.Email.ToLower())) &&
            (!filtro.Sexo.HasValue || c.Sexo == filtro.Sexo.Value);
    }
}