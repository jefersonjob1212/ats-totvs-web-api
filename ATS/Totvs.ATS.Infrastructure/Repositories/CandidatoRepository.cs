using MongoDB.Driver;
using Totvs.ATS.Domain.Entities;
using Totvs.ATS.Domain.Interfaces;
using Totvs.ATS.Infrastructure.Context;

namespace Totvs.ATS.Infrastructure.Repositories;

public class CandidatoRepository : MongoDbRepositoryBase<Candidato>, ICandidatoRepository
{
    public CandidatoRepository(MongoDbContext context) 
        : base(context, "candidatos")
    {
    }
}