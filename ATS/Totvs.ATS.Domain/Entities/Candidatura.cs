using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Totvs.ATS.Domain.Entities;

public class Candidatura
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonRepresentation(BsonType.ObjectId)]
    public string CandidatoId { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string VagaId { get; set; }

    public DateTime DataCandidatura { get; set; }
}