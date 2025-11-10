using System.Linq.Expressions;
using FluentAssertions;
using NSubstitute;
using Totvs.ATS.Application.DTOs.Candidatos;
using Totvs.ATS.Application.Filters.Candidatos;
using Totvs.ATS.Application.Handlers.Candidatos;
using Totvs.ATS.Application.Queries.Candidatos;
using Totvs.ATS.Application.Responses;
using Totvs.ATS.Domain.Entities;
using Totvs.ATS.Domain.Enums;
using Totvs.ATS.Domain.Interfaces;

namespace Totvs.ATS.UnitTests.Application.Commands.Candidatos;

public class GetCandidatosByFilterHandlerTests
{
    private readonly ICandidatoRepository _repository;
    private readonly GetCandidatosByFilterHandler _handler;

    public GetCandidatosByFilterHandlerTests()
    {
        _repository = Substitute.For<ICandidatoRepository>();
        _handler = new GetCandidatosByFilterHandler(_repository);
    }

    [Fact(DisplayName = "Deve retornar candidatos filtrados e paginados corretamente")]
    public async Task Handle_DeveRetornarCandidatosPaginados()
    {
        // Arrange
        var candidatos = new List<Candidato>
        {
            new() { Id = "1", Nome = "Ana", Email = "ana@teste.com", Sexo = SexoEnum.Feminino },
            new() { Id = "2", Nome = "Bruno", Email = "bruno@teste.com", Sexo = SexoEnum.Masculino },
            new() { Id = "3", Nome = "Carlos", Email = "carlos@teste.com", Sexo = SexoEnum.Masculino }
        };

        // Retorna a lista simulada para qualquer expressão
        _repository.FindAsync(Arg.Any<Expression<Func<Candidato, bool>>>())
            .Returns(candidatos);

        var filtro = new CandidatoFilter
        {
            Nome = "",
            PageNumber = 1,
            PageSize = 2
        };

        var query = new GetCandidatosByFilterQuery(filtro);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<PagedResponse<CandidatoDTO>>();
        result.Items.Should().HaveCount(2); // primeira página, 2 itens
        result.TotalItems.Should().Be(3);
        result.TotalPages.Should().Be(2);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(2);
    }

    [Fact(DisplayName = "Deve retornar lista vazia quando não encontrar candidatos")]
    public async Task Handle_DeveRetornarListaVazia_QuandoNaoEncontrar()
    {
        // Arrange
        _repository.FindAsync(Arg.Any<Expression<Func<Candidato, bool>>>())
            .Returns(new List<Candidato>());

        var filtro = new CandidatoFilter
        {
            Nome = "Inexistente",
            PageNumber = 1,
            PageSize = 10
        };

        var query = new GetCandidatosByFilterQuery(filtro);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
        result.TotalItems.Should().Be(0);
        result.TotalPages.Should().Be(0);
    }
}