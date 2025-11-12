using System.Linq.Expressions;
using FluentAssertions;
using NSubstitute;
using Totvs.ATS.Application.DTOs.Vagas;
using Totvs.ATS.Application.Filters.Vagas;
using Totvs.ATS.Application.Queries.Vagas;
using Totvs.ATS.Application.Responses;
using Totvs.ATS.Domain.Entities;
using Totvs.ATS.Domain.Enums;
using Totvs.ATS.Domain.Interfaces;

namespace Totvs.ATS.Application.Commands.Vagas;

public class GetVagasByFilterHandlerTests
{
    private readonly IVagaRepository _vagaRepository;
    private readonly GetVagasByFilterHandler _handler;

    public GetVagasByFilterHandlerTests()
    {
        _vagaRepository = Substitute.For<IVagaRepository>();
        _handler = new GetVagasByFilterHandler(_vagaRepository);
    }

    [Fact(DisplayName = "Deve retornar vagas paginadas conforme o filtro informado")]
    public async Task Deve_Retornar_Vagas_Paginadas_Filtradas()
    {
        // Arrange
        var vagas = new List<Vaga>
        {
            new() { Id = "1", Titulo = "Desenvolvedor .NET", Localizacao = "São Paulo", TipoVaga = TipoVagaEnum.Presencial },
            new() { Id = "2", Titulo = "Desenvolvedor Angular", Localizacao = "Remoto", TipoVaga = TipoVagaEnum.Remoto },
            new() { Id = "3", Titulo = "Analista QA", Localizacao = "São Paulo", TipoVaga = TipoVagaEnum.Hibrido }
        };

        _vagaRepository
            .FindAsync(Arg.Any<Expression<Func<Vaga, bool>>>())
            .Returns(vagas);

        var filter = new VagaFilter
        {
            Titulo = "Desenvolvedor",
            Localizacao = "",
            PageNumber = 1,
            PageSize = 2
        };

        var query = new GetVagaByFilterQuery(filter);

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().NotBeNull();
        response.Should().BeOfType<PagedResponse<VagaDTO>>();
        response.Items.Should().HaveCount(2);
        response.TotalItems.Should().Be(3);
        response.PageNumber.Should().Be(1);
        response.PageSize.Should().Be(2);
        response.Items.First().Titulo.Should().Contain("Desenvolvedor");

        await _vagaRepository.Received(1)
            .FindAsync(Arg.Any<Expression<Func<Vaga, bool>>>());
    }

    [Fact(DisplayName = "Deve retornar lista vazia quando não houver correspondência com o filtro")]
    public async Task Deve_Retornar_Vazio_Quando_Nao_Encontrar_Vagas()
    {
        // Arrange
        _vagaRepository
            .FindAsync(Arg.Any<Expression<Func<Vaga, bool>>>())
            .Returns(new List<Vaga>());

        var filter = new VagaFilter
        {
            Titulo = "Inexistente",
            Localizacao = "Marte",
            PageNumber = 1,
            PageSize = 10
        };

        var query = new GetVagaByFilterQuery(filter);

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);

        // Assert
        response.Items.Should().BeEmpty();
        response.TotalPages.Should().Be(0);
        response.PageNumber.Should().Be(1);
        response.PageSize.Should().Be(10);

        await _vagaRepository.Received(1)
            .FindAsync(Arg.Any<Expression<Func<Vaga, bool>>>());
    }
}