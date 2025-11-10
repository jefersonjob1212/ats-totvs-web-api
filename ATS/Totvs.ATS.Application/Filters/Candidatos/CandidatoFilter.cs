using Totvs.ATS.Domain.Enums;

namespace Totvs.ATS.Application.Filters.Candidatos;

public class CandidatoFilter : FilterBase
{
    public string? Nome { get; set; }
    public string? Email { get; set; }
    public SexoEnum? Sexo { get; set; }
}