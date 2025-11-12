using System.Linq.Expressions;
using FluentAssertions;
using NSubstitute;
using Totvs.ATS.Application.Handlers.Candidatos;
using Totvs.ATS.Application.Queries.Candidatos;
using Totvs.ATS.Domain.Entities;
using Totvs.ATS.Domain.Interfaces;

namespace Totvs.ATS.UnitTests.Application.Commands.Candidatos;

public class GetCandidatoByCpfHandlerTests
{
    private readonly ICandidatoRepository _candidatoRepository = Substitute.For<ICandidatoRepository>();
    private readonly GetCandidatoByCpfHandler _handler;

    public GetCandidatoByCpfHandlerTests()
    {
        _handler = new GetCandidatoByCpfHandler(_candidatoRepository);
    }

    [Fact(DisplayName = "Deve retornar candidato pelo CPF com sucesso")]
    public async Task Deve_Retornar_Candidato_Pelo_CPF_Com_Sucesso()
    {
        // Arrange
        var cpf = "12345678900";
        var candidato = new Candidato
        {
            Id = "1",
            Nome = "Jeferson",
            Cpf = cpf,
            Email = "jeferson@email.com"
        };

        _candidatoRepository.FindAsync(Arg.Any<Expression<Func<Candidato, bool>>>())
            .Returns(new List<Candidato> { candidato });

        var query = new GetCandidatoByCpfQuery(cpf);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Cpf.Should().Be(cpf);
        result.Nome.Should().Be("Jeferson");
        result.Email.Should().Be("jeferson@email.com");
    }

    [Fact(DisplayName = "Deve retornar null se candidato não existir")]
    public async Task Deve_Retornar_Null_Se_Candidato_Nao_Existir()
    {
        // Arrange
        var cpf = "00000000000";
        _candidatoRepository.FindAsync(Arg.Any<Expression<Func<Candidato, bool>>>())
            .Returns(new List<Candidato>());

        var query = new GetCandidatoByCpfQuery(cpf);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
}