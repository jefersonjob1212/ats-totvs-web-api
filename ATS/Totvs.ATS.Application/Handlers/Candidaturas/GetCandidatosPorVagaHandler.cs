using MediatR;
using Totvs.ATS.Application.DTOs.Candidatos;
using Totvs.ATS.Application.DTOs.Vagas;
using Totvs.ATS.Application.Queries.Candidaturas;
using Totvs.ATS.Domain.Interfaces;

namespace Totvs.ATS.Application.Handlers.Candidaturas;

public class GetCandidatosPorVagaHandler(
    IVagaRepository vagaRepository,
    ICandidatoRepository candidatoRepository,
    ICandidaturaRepository candidaturaRepository) : IRequestHandler<GetCandidatosPorVagaQuery, VagaComCandidadtosDTO>
{
    public async Task<VagaComCandidadtosDTO?> Handle(GetCandidatosPorVagaQuery request, CancellationToken cancellationToken)
    {
        var vaga = await vagaRepository.FindByIdAsync(request.VagaId);
        if (vaga == null)
            return null;
        
        var candidaturas = await candidaturaRepository.FindAsync(c => c.VagaId == vaga.Id);
        var candidatos = new List<CandidatoResumoDTO>();
        foreach (var candidatura in candidaturas)
        {
            var candidato = await candidatoRepository.FindByIdAsync(candidatura.CandidatoId);
            if (candidato != null)
            {
                candidatos.Add(new CandidatoResumoDTO
                {
                    Id = candidato.Id,
                    Nome = candidato.Nome,
                    Email = candidato.Email,
                    Telefone = candidato.Telefone
                });
            }
        }

        return new VagaComCandidadtosDTO
        {
            VagaId = vaga.Id,
            VagaTitulo = vaga.Titulo,
            DataPublicacao = vaga.DataPublicacao,
            Candidatos = candidatos
        };
    }
}