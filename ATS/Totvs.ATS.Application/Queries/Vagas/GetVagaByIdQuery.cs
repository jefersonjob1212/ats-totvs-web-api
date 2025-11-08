using MediatR;
using Totvs.ATS.Application.DTOs.Vagas;

namespace Totvs.ATS.Application.Queries.Vagas;

public record GetVagaByIdQuery(string Id) : IRequest<VagaDTO>;