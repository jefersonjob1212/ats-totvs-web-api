using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Totvs.ATS.Domain.Entities;

namespace Totvs.ATS.Infrastructure.Context;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MongoDB");
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(configuration["MongoDBDatabase"]);
    }
    
    public IMongoCollection<Candidato> Candidatos => _database.GetCollection<Candidato>("Candidatos");
    public IMongoCollection<Vaga> Vagas => _database.GetCollection<Vaga>("Vagas");
    public IMongoCollection<Candidatura>  Candidaturas => _database.GetCollection<Candidatura>("Candidaturas");
    
    public IMongoDatabase GetDatabase() => _database;
}