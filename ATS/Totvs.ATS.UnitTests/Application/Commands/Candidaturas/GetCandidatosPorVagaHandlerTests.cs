using System.Linq.Expressions;
using FluentAssertions;
using NSubstitute;
using Totvs.ATS.Application.Handlers.Candidaturas;
using Totvs.ATS.Application.Queries.Candidaturas;
using Totvs.ATS.Domain.Entities;
using Totvs.ATS.Domain.Interfaces;

namespace Totvs.ATS.UnitTests.Application.Commands.Candidaturas;

public class GetCandidatosPorVagaHandlerTests
{
    private readonly IVagaRepository _vagaRepository;
    private readonly ICandidatoRepository _candidatoRepository;
    private readonly ICandidaturaRepository _candidaturaRepository;
    private readonly GetCandidatosPorVagaHandler _handler;

    public GetCandidatosPorVagaHandlerTests()
    {
        _vagaRepository = Substitute.For<IVagaRepository>();
        _candidatoRepository = Substitute.For<ICandidatoRepository>();
        _candidaturaRepository = Substitute.For<ICandidaturaRepository>();

        _handler = new GetCandidatosPorVagaHandler(
            _vagaRepository,
            _candidatoRepository,
            _candidaturaRepository);
    }

    [Fact(DisplayName = "Deve retornar vaga com candidatos quando houver candidaturas")]
    public async Task Deve_Retornar_VagaComCandidatos_QuandoExistirem()
    {
        // Arrange
        var vagaId = "vaga123";
        var candidato1 = new Candidato { Id = "cand1", Nome = "Alice", Email = "alice@email.com", Telefone = "1111-1111" };
        var candidato2 = new Candidato { Id = "cand2", Nome = "Bob", Email = "bob@email.com", Telefone = "2222-2222" };

        var vaga = new Vaga { Id = vagaId, Titulo = "Desenvolvedor .NET", DataPublicacao = new DateTime(2025, 1, 1) };
        var candidaturas = new List<Candidatura>
        {
            new Candidatura { VagaId = vagaId, CandidatoId = "cand1" },
            new Candidatura { VagaId = vagaId, CandidatoId = "cand2" }
        };

        _vagaRepository.FindByIdAsync(vagaId).Returns(vaga);
        _candidaturaRepository.FindAsync(Arg.Any<Expression<Func<Candidatura, bool>>>()).Returns(candidaturas);
        _candidatoRepository.FindByIdAsync("cand1").Returns(candidato1);
        _candidatoRepository.FindByIdAsync("cand2").Returns(candidato2);

        var query = new GetCandidatosPorVagaQuery(vagaId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.VagaId.Should().Be(vagaId);
        result.VagaTitulo.Should().Be("Desenvolvedor .NET");
        result.Candidatos.Should().HaveCount(2);
        result.Candidatos.Select(c => c.Nome).Should().Contain(new[] { "Alice", "Bob" });

        await _vagaRepository.Received(1).FindByIdAsync(vagaId);
        await _candidaturaRepository.Received(1).FindAsync(Arg.Any<Expression<Func<Candidatura, bool>>>());
        await _candidatoRepository.Received(2).FindByIdAsync(Arg.Any<string>());
    }

    [Fact(DisplayName = "Deve retornar null quando vaga não for encontrada")]
    public async Task Deve_RetornarNull_QuandoVagaNaoEncontrada()
    {
        // Arrange
        var vagaId = "inexistente";
        _vagaRepository.FindByIdAsync(vagaId).Returns((Vaga?)null);

        var query = new GetCandidatosPorVagaQuery(vagaId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
        await _vagaRepository.Received(1).FindByIdAsync(vagaId);
        await _candidaturaRepository.DidNotReceive().FindAsync(Arg.Any<Expression<Func<Candidatura, bool>>>());
    }

    [Fact(DisplayName = "Deve ignorar candidaturas cujo candidato não exista")]
    public async Task Deve_Ignorar_Candidaturas_Com_CandidatoInexistente()
    {
        // Arrange
        var vagaId = "vaga999";
        var vaga = new Vaga { Id = vagaId, Titulo = "QA Engineer", DataPublicacao = DateTime.UtcNow };
        var candidaturas = new List<Candidatura>
        {
            new Candidatura { VagaId = vagaId, CandidatoId = "cand1" },
            new Candidatura { VagaId = vagaId, CandidatoId = "cand2" }
        };

        _vagaRepository.FindByIdAsync(vagaId).Returns(vaga);
        _candidaturaRepository.FindAsync(Arg.Any<Expression<Func<Candidatura, bool>>>()).Returns(candidaturas);
        _candidatoRepository.FindByIdAsync("cand1").Returns(new Candidato { Id = "cand1", Nome = "Maria" });
        _candidatoRepository.FindByIdAsync("cand2").Returns((Candidato?)null); // candidato não encontrado

        var query = new GetCandidatosPorVagaQuery(vagaId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Candidatos.Should().HaveCount(1);
        result.Candidatos.First().Nome.Should().Be("Maria");
    }
}