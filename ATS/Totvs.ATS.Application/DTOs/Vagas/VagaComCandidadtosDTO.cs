using Totvs.ATS.Application.DTOs.Candidatos;

namespace Totvs.ATS.Application.DTOs.Vagas;

public class VagaComCandidadtosDTO
{
    public string VagaId { get; set; } = string.Empty;
    public string VagaTitulo { get; set; } = string.Empty;
    public DateTime DataPublicacao { get; set; }
    public List<CandidatoResumoDTO> Candidatos { get; set; } = new();
}