using Totvs.ATS.Domain.Enums;

namespace Totvs.ATS.Application.DTOs.Vagas;

public class CriarEditarVagaDTO
{
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public string Localizacao { get; set; }
    public TipoVagaEnum TipoVaga { get; set; }
}