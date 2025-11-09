using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Totvs.ATS.Domain.Entities;
using Totvs.ATS.Domain.Enums;
using Totvs.ATS.Infrastructure.Context;
using Totvs.ATS.Infrastructure.Repositories;

namespace Totvs.ATS.Infrastructure.Tests.Repositories;

public class MongoDbRepositoryBaseTests
{
     private readonly MongoDbRepositoryBase<Vaga> _repository;
    private readonly MongoDbContext _context;

    public MongoDbRepositoryBaseTests()
    {
        // Cria configuração simulando o ambiente real
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.Test.json", optional: false)
            .Build();

        // Cria contexto e repositório
        _context = new MongoDbContext(configuration);
        _repository = new MongoDbRepositoryBase<Vaga>(_context, "VagasTests");
    }

    [Fact(DisplayName = "Deve inserir e buscar uma vaga por ID no MongoDB")]
    public async Task Deve_Inserir_E_Buscar_Vaga_Por_Id()
    {
        // Arrange
        var vaga = new Vaga
        {
            Titulo = "Desenvolvedor .NET",
            Descricao = "APIs REST e MongoDB",
            Localizacao = "Remoto",
            TipoVaga = TipoVagaEnum.Remoto
        };

        // Act
        await _repository.AddAsync(vaga);
        var encontrada = await _repository.FindByIdAsync(vaga.Id);

        // Assert
        encontrada.Should().NotBeNull();
        encontrada!.Titulo.Should().Be("Desenvolvedor .NET");
    }

    [Fact(DisplayName = "Deve atualizar uma vaga existente")]
    public async Task Deve_Atualizar_Vaga()
    {
        var vaga = new Vaga
        {
            Titulo = "QA Engineer",
            Descricao = "Testes automatizados",
            Localizacao = "São Paulo",
            TipoVaga = TipoVagaEnum.Hibrido
        };
        await _repository.AddAsync(vaga);

        vaga.Titulo = "QA Senior";
        await _repository.UpdateAsync(vaga.Id, vaga);

        var atualizada = await _repository.FindByIdAsync(vaga.Id);
        atualizada!.Titulo.Should().Be("QA Senior");
    }

    [Fact(DisplayName = "Deve deletar uma vaga existente")]
    public async Task Deve_Deletar_Vaga()
    {
        var vaga = new Vaga
        {
            Titulo = "DevOps Engineer",
            Descricao = "CI/CD e Kubernetes",
            Localizacao = "Remoto",
            TipoVaga = TipoVagaEnum.Remoto
        };
        await _repository.AddAsync(vaga);

        await _repository.DeleteAsync(vaga.Id);
        var encontrada = await _repository.FindByIdAsync(vaga.Id);

        encontrada.Should().BeNull();
    }

    [Fact(DisplayName = "Deve buscar todas as vagas")]
    public async Task Deve_Buscar_Todas_As_Vagas()
    {
        var vagas = await _repository.GetAllAsync();

        vagas.Should().NotBeNull();
        vagas.Should().BeAssignableTo<IEnumerable<Vaga>>();
    }

    public async Task InitializeAsync()
    {
        // Limpeza antes dos testes
        var db = _context.GetDatabase();
        await db.DropCollectionAsync("VagasTests");
    }

    public async Task DisposeAsync()
    {
        // Limpeza após os testes
        var db = _context.GetDatabase();
        await db.DropCollectionAsync("VagasTests");
    }
}