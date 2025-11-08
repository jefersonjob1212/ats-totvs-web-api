using Mapster;
using Totvs.ATS.Application.Commands.Candidatos;
using Totvs.ATS.Domain.Interfaces;
using Totvs.ATS.Infrastructure.Context;
using Totvs.ATS.Infrastructure.Repositories;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// MongoDB
builder.Services.AddSingleton<MongoDbContext>();

// Repositories
builder.Services.AddScoped<ICandidatoRepository, CandidatoRepository>();
builder.Services.AddScoped<IVagaRepository, VagaRepository>();
builder.Services.AddScoped<ICandidaturaRepository, CandidaturaRepository>();
builder.Services.AddScoped(typeof(IMongoDbRepositoryBase<>), typeof(MongoDbRepositoryBase<>));

// Mapster
builder.Services.AddSingleton(TypeAdapterConfig.GlobalSettings);

// MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateCandidatoCommand).Assembly));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();