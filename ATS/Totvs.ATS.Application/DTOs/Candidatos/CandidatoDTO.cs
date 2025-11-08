using Totvs.ATS.Domain.Enums;

namespace Totvs.ATS.Application.DTOs.Candidatos;

public class CandidatoDTO
{
    public string Id { get; set; } = string.Empty;
    public string Nome { get; set; }  = string.Empty;
    public string Email { get; set; }
    public string Telefone { get; set; }
    public SexoEnum Sexo { get; set; }
}