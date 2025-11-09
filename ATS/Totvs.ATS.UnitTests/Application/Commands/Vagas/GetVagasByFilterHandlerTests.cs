using System.Linq.Expressions;
using FluentAssertions;
using NSubstitute;
using Totvs.ATS.Application.Filters.Vagas;
using Totvs.ATS.Application.Queries.Vagas;
using Totvs.ATS.Domain.Entities;
using Totvs.ATS.Domain.Enums;
using Totvs.ATS.Domain.Interfaces;

namespace Totvs.ATS.Application.Commands.Vagas;

public class GetVagasByFilterHandlerTests
{
    private readonly IVagaRepository _vagaRepository = Substitute.For<IVagaRepository>();
    private readonly GetVagasByFilterHandler _handler;

    public GetVagasByFilterHandlerTests()
    {
        _handler = new GetVagasByFilterHandler(_vagaRepository);
    }

    [Fact(DisplayName = "Deve retornar as vagas que correspondem ao filtro informado")]
    public async Task Handle_DeveRetornarVagasFiltradas()
    {
        // Arrange
        var vagas = new List<Vaga>
        {
            new Vaga { Id = "1", Titulo = "Desenvolvedor .NET", Localizacao = "Remoto", TipoVaga = TipoVagaEnum.Remoto, Encerrada = false },
            new Vaga { Id = "2", Titulo = "Analista de QA", Localizacao = "São Paulo", TipoVaga = TipoVagaEnum.Presencial, Encerrada = false },
            new Vaga { Id = "3", Titulo = "DevOps Engineer", Localizacao = "Remoto", TipoVaga = TipoVagaEnum.Remoto, Encerrada = false }
        };

        var filter = new VagaFilter
        {
            Titulo = "Desenvolvedor",
            Localizacao = "Remoto"
        };

        _vagaRepository.FindAsync(Arg.Any<Expression<Func<Vaga, bool>>>())
            .Returns(ci => vagas.Where(v =>
                v.Titulo.ToLower().Contains(filter.Titulo.ToLower()) &&
                v.Localizacao.ToLower().Contains(filter.Localizacao.ToLower())));

        var query = new GetVagaByFilterQuery(filter);

        // Act
        var result = (await _handler.Handle(query, CancellationToken.None)).ToList();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().Titulo.Should().Be("Desenvolvedor .NET");
        result.First().Localizacao.Should().Be("Remoto");

        await _vagaRepository.Received(1).FindAsync(Arg.Any<Expression<Func<Vaga, bool>>>());
    }

    [Fact(DisplayName = "Deve retornar lista vazia quando nenhuma vaga corresponder ao filtro")]
    public async Task Handle_DeveRetornarListaVazia_QuandoSemCorrespondencia()
    {
        // Arrange
        _vagaRepository.FindAsync(Arg.Any<Expression<Func<Vaga, bool>>>())
            .Returns(Enumerable.Empty<Vaga>());

        var filter = new VagaFilter { Titulo = "Inexistente" };
        var query = new GetVagaByFilterQuery(filter);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
        await _vagaRepository.Received(1).FindAsync(Arg.Any<Expression<Func<Vaga, bool>>>());
    }
}