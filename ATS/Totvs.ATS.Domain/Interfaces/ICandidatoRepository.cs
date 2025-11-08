using MongoDB.Driver;
using Totvs.ATS.Domain.Entities;

namespace Totvs.ATS.Domain.Interfaces;

public interface ICandidatoRepository : IMongoDbRepositoryBase<Candidato>
{
}