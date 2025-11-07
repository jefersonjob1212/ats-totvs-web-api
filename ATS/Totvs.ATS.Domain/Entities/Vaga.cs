using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Totvs.ATS.Domain.Enums;

namespace Totvs.ATS.Domain.Entities;

public class Vaga
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public int Id { get; set; }
    
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public string Localizacao { get; set; }
    public DateTime DataPublicacao { get; set; }
    
    [BsonRepresentation(BsonType.String)] 
    public TipoVaga TipoVaga { get; set; }
}