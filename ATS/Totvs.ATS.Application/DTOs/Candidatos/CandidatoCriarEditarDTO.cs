using Totvs.ATS.Domain.Enums;

namespace Totvs.ATS.Application.DTOs.Candidatos;

public class CandidatoCriarEditarDTO
{
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    public SexoEnum Sexo { get; set; }
}