using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Totvs.ATS.Domain.Enums;

namespace Totvs.ATS.Domain.Entities;

public class Vaga
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
    
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public string Localizacao { get; set; }
    public DateTime DataPublicacao { get; set; } = DateTime.Now;
    public bool Encerrada { get; set; } = false;
    
    [BsonRepresentation(BsonType.String)] 
    public TipoVagaEnum TipoVaga { get; set; }
}