using Totvs.ATS.Domain.Entities;
using Totvs.ATS.Domain.Interfaces;
using Totvs.ATS.Infrastructure.Context;

namespace Totvs.ATS.Infrastructure.Repositories;

public class VagaRepository : MongoDbRepositoryBase<Vaga>, IVagaRepository
{
    public VagaRepository(MongoDbContext context) 
        : base(context, "vagas")
    {
    }
}