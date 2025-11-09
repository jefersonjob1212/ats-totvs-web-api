using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Totvs.ATS.Domain.Enums;

namespace Totvs.ATS.Domain.Entities;

public class Candidato
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public string Cpf { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    
    [BsonRepresentation(BsonType.String)]
    public SexoEnum Sexo { get; set; }

    [BsonIgnore] 
    public List<Candidatura> Candidaturas { get; set; } = new();
}