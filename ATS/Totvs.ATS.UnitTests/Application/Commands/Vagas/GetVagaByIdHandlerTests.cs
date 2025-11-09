using FluentAssertions;
using NSubstitute;
using Totvs.ATS.Application.Commands.Vagas;
using Totvs.ATS.Application.Queries.Vagas;
using Totvs.ATS.Domain.Entities;
using Totvs.ATS.Domain.Enums;
using Totvs.ATS.Domain.Interfaces;

namespace Totvs.ATS.UnitTests.Application.Commands.Vagas;

public class GetVagaByIdHandlerTests
{
    private readonly IVagaRepository _vagaRepository = Substitute.For<IVagaRepository>();
    private readonly GetVagaByIdHander _handler;

    public GetVagaByIdHandlerTests()
    {
        _handler = new GetVagaByIdHander(_vagaRepository);
    }

    [Fact(DisplayName = "Deve retornar a vaga correspondente ao Id informado")]
    public async Task Handle_DeveRetornarVagaPorId()
    {
        // Arrange
        var vaga = new Vaga
        {
            Id = "123",
            Titulo = "Desenvolvedor .NET",
            Descricao = "Trabalhar em sistemas corporativos",
            Localizacao = "Remoto",
            TipoVaga = TipoVagaEnum.Remoto,
            Encerrada = false
        };

        _vagaRepository.FindByIdAsync(vaga.Id).Returns(vaga);

        var query = new GetVagaByIdQuery(vaga.Id);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(vaga.Id);
        result.Titulo.Should().Be(vaga.Titulo);
        result.Encerrada.Should().BeFalse();

        await _vagaRepository.Received(1).FindByIdAsync(vaga.Id);
    }

    [Fact(DisplayName = "Deve retornar null quando a vaga não for encontrada")]
    public async Task Handle_DeveRetornarNull_QuandoVagaNaoEncontrada()
    {
        // Arrange
        _vagaRepository.FindByIdAsync("999").Returns((Vaga?)null);
        var query = new GetVagaByIdQuery("999");

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
        await _vagaRepository.Received(1).FindByIdAsync("999");
    }
}