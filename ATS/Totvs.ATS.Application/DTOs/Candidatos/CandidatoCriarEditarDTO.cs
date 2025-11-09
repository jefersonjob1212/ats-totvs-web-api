using Totvs.ATS.Domain.Enums;

namespace Totvs.ATS.Application.DTOs.Candidatos;

public class CandidatoCriarEditarDTO
{
    public string Cpf { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    public SexoEnum Sexo { get; set; }
}