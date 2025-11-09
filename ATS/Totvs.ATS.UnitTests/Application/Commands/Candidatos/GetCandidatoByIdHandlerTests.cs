using FluentAssertions;
using NSubstitute;
using Totvs.ATS.Application.Handlers.Candidatos;
using Totvs.ATS.Application.Queries.Candidatos;
using Totvs.ATS.Domain.Entities;
using Totvs.ATS.Domain.Interfaces;

namespace Totvs.ATS.UnitTests.Application.Commands.Candidatos;

public class GetCandidatoByIdHandlerTests
{
    private readonly ICandidatoRepository _candidatoRepository;
    private readonly GetCandidatoByIdHandler _handler;

    public GetCandidatoByIdHandlerTests()
    {
        _candidatoRepository = Substitute.For<ICandidatoRepository>();
        _handler = new GetCandidatoByIdHandler(_candidatoRepository);
    }

    [Fact(DisplayName = "Deve retornar o candidato quando encontrado pelo ID")]
    public async Task Deve_Retornar_CandidatoDTO_Quando_Encontrado()
    {
        // Arrange
        var candidato = new Candidato
        {
            Id = "abc123",
            Nome = "João da Silva",
            Email = "joao@email.com",
            Telefone = "99999-9999"
        };

        _candidatoRepository.FindByIdAsync(candidato.Id)
            .Returns(candidato);

        var query = new GetCandidatoByIdQuery(candidato.Id);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        await _candidatoRepository.Received(1).FindByIdAsync(candidato.Id);
        result.Should().NotBeNull();
        result.Id.Should().Be(candidato.Id);
        result.Nome.Should().Be(candidato.Nome);
        result.Email.Should().Be(candidato.Email);
    }

    [Fact(DisplayName = "Deve retornar nulo se o candidato não for encontrado")]
    public async Task Deve_Retornar_Nulo_Se_Nao_Encontrado()
    {
        // Arrange
        var query = new GetCandidatoByIdQuery("nao-existe");
        _candidatoRepository.FindByIdAsync(query.Id)
            .Returns((Candidato?)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }    
}