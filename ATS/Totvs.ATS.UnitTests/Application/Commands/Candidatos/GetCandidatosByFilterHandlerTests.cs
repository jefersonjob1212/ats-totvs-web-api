using FluentAssertions;
using NSubstitute;
using Totvs.ATS.Application.Filters.Candidatos;
using Totvs.ATS.Application.Handlers.Candidatos;
using Totvs.ATS.Application.Queries.Candidatos;
using Totvs.ATS.Domain.Entities;
using Totvs.ATS.Domain.Enums;
using Totvs.ATS.Domain.Interfaces;

namespace Totvs.ATS.UnitTests.Application.Commands.Candidatos;

public class GetCandidatosByFilterHandlerTests
{
    private readonly ICandidatoRepository _candidatoRepository;
    private readonly GetCandidatosByFilterHandler _handler;

    public GetCandidatosByFilterHandlerTests()
    {
        _candidatoRepository = Substitute.For<ICandidatoRepository>();
        _handler = new GetCandidatosByFilterHandler(_candidatoRepository);
    }

    [Fact(DisplayName = "Deve retornar candidatos que correspondem ao filtro de nome")]
    public async Task Deve_Retornar_Candidatos_Que_Contem_Nome()
    {
        // Arrange
        var candidatos = new List<Candidato>
        {
            new() { Id = "1", Nome = "Maria Silva", Email = "maria@email.com" },
            new() { Id = "2", Nome = "João Souza", Email = "joao@email.com" },
            new() { Id = "3", Nome = "Mariana Costa", Email = "mariana@email.com" }
        };

        var filtro = new CandidatoFilter { Nome = "Mari" };
        var query = new GetCandidatosByFilterQuery(filtro);

        // Configura o mock para retornar apenas candidatos cujo nome contém "Mari"
        _candidatoRepository.FindAsync(Arg.Any<System.Linq.Expressions.Expression<System.Func<Candidato, bool>>>())
            .Returns(ci => candidatos.Where(c => c.Nome.ToLower().Contains("mari")));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Select(x => x.Nome).Should().Contain(new[] { "Maria Silva", "Mariana Costa" });

        await _candidatoRepository.Received(1)
            .FindAsync(Arg.Any<System.Linq.Expressions.Expression<System.Func<Candidato, bool>>>());
    }

    [Fact(DisplayName = "Deve retornar candidatos filtrados por email")]
    public async Task Deve_Retornar_Candidatos_Por_Email()
    {
        // Arrange
        var candidatos = new List<Candidato>
        {
            new() { Id = "1", Nome = "Pedro Lima", Email = "pedro@email.com" },
            new() { Id = "2", Nome = "Paulo Lima", Email = "paulo@email.com" }
        };

        var filtro = new CandidatoFilter { Email = "pedro" };
        var query = new GetCandidatosByFilterQuery(filtro);

        _candidatoRepository.FindAsync(Arg.Any<System.Linq.Expressions.Expression<System.Func<Candidato, bool>>>())
            .Returns(ci => candidatos.Where(c => c.Email.ToLower().Contains("pedro")));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().ContainSingle();
        result.First().Email.Should().Be("pedro@email.com");
    }

    [Fact(DisplayName = "Deve retornar candidatos filtrados por sexo")]
    public async Task Deve_Retornar_Candidatos_Por_Sexo()
    {
        // Arrange
        var candidatos = new List<Candidato>
        {
            new() { Id = "1", Nome = "Ana", Sexo = SexoEnum.Feminino },
            new() { Id = "2", Nome = "Bruno", Sexo = SexoEnum.Masculino }
        };

        var filtro = new CandidatoFilter { Sexo = SexoEnum.Feminino };
        var query = new GetCandidatosByFilterQuery(filtro);

        _candidatoRepository.FindAsync(Arg.Any<System.Linq.Expressions.Expression<System.Func<Candidato, bool>>>())
            .Returns(ci => candidatos.Where(c => c.Sexo == SexoEnum.Feminino));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(1);
        result.First().Nome.Should().Be("Ana");
    }

    [Fact(DisplayName = "Deve retornar lista vazia se nenhum candidato corresponder ao filtro")]
    public async Task Deve_Retornar_Lista_Vazia_Se_Nenhum_Candidato_Encontrado()
    {
        // Arrange
        var filtro = new CandidatoFilter { Nome = "Inexistente" };
        var query = new GetCandidatosByFilterQuery(filtro);

        _candidatoRepository.FindAsync(Arg.Any<System.Linq.Expressions.Expression<System.Func<Candidato, bool>>>())
            .Returns(Enumerable.Empty<Candidato>());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }
}