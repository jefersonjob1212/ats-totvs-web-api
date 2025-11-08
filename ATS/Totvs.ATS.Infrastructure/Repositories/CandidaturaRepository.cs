using Totvs.ATS.Domain.Entities;
using Totvs.ATS.Domain.Interfaces;
using Totvs.ATS.Infrastructure.Context;

namespace Totvs.ATS.Infrastructure.Repositories;

public class CandidaturaRepository : MongoDbRepositoryBase<Candidatura>, ICandidaturaRepository
{
    public CandidaturaRepository(MongoDbContext context) 
        : base(context, "candidaturas")
    {
    }
}