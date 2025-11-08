using Totvs.ATS.Domain.Enums;

namespace Totvs.ATS.Application.Filters.Vagas;

public class VagaFilter
{
    public string? Titulo { get; set; }
    public string? Localizacao { get; set; }
    public TipoVagaEnum? TipoVaga { get; set; }
}