using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Totvs.ATS.Domain.Entities;

public class Candidatura
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public int Id { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public int CandidatoId { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public int VagaId { get; set; }

    public DateTime DataCandidatura { get; set; }
}