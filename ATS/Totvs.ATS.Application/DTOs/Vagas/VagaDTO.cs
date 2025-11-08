using Totvs.ATS.Domain.Enums;

namespace Totvs.ATS.Application.DTOs.Vagas;

public class VagaDTO
{
    public string Id { get; set; }
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public string Localizacao { get; set; }
    public DateTime DataPublicacao { get; set; }
    public TipoVagaEnum TipoVagaEnum { get; set; }
}