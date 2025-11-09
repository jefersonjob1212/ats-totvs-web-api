using MediatR;
using Totvs.ATS.Application.DTOs.Vagas;

namespace Totvs.ATS.Application.Queries.Candidaturas;

public record GetCandidatosPorVagaQuery(string VagaId) : IRequest<VagaComCandidadtosDTO>;