using FluentAssertions;
using NSubstitute;
using Totvs.ATS.Application.Commands.Vagas;
using Totvs.ATS.Application.Handlers.Vagas;
using Totvs.ATS.Domain.Entities;
using Totvs.ATS.Domain.Enums;
using Totvs.ATS.Domain.Exceptions;
using Totvs.ATS.Domain.Interfaces;

namespace Totvs.ATS.UnitTests.Application.Commands.Vagas;

public class EncerrarVagaHandlerTests
{
    private readonly IVagaRepository _vagaRepository = Substitute.For<IVagaRepository>();
    private readonly EncerrarVagaHandler _handler;

    public EncerrarVagaHandlerTests()
    {
        _handler = new EncerrarVagaHandler(_vagaRepository);
    }

    [Fact(DisplayName = "Deve encerrar uma vaga com sucesso")]
    public async Task Handle_DeveEncerrarVagaComSucesso()
    {
        // Arrange
        var vaga = new Vaga
        {
            Id = "123",
            Titulo = "Desenvolvedor .NET",
            Descricao = "Atuar em projetos backend",
            Localizacao = "Remoto",
            TipoVaga = TipoVagaEnum.Remoto,
            Encerrada = false
        };

        _vagaRepository.FindByIdAsync(vaga.Id).Returns(vaga);

        var command = new EncerrarVagaCommand(vaga.Id);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(vaga.Id);
        result.Encerrada.Should().BeTrue();
        result.Titulo.Should().Be(vaga.Titulo);

        await _vagaRepository.Received(1)
            .UpdateAsync(vaga.Id, Arg.Is<Vaga>(v => v.Encerrada));
    }

    [Fact(DisplayName = "Deve lançar NotFoundException quando a vaga não existe")]
    public async Task Handle_DeveLancarNotFoundException_QuandoVagaNaoExiste()
    {
        // Arrange
        _vagaRepository.FindByIdAsync("999").Returns((Vaga?)null);
        var command = new EncerrarVagaCommand("999");

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage("Vaga não encontrada");

        await _vagaRepository.DidNotReceive()
            .UpdateAsync(Arg.Any<string>(), Arg.Any<Vaga>());
    }
}