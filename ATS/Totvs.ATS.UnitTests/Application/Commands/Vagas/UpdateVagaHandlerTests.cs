using FluentAssertions;
using NSubstitute;
using Totvs.ATS.Application.Commands.Vagas;
using Totvs.ATS.Application.Handlers.Vagas;
using Totvs.ATS.Domain.Entities;
using Totvs.ATS.Domain.Enums;
using Totvs.ATS.Domain.Interfaces;

namespace Totvs.ATS.UnitTests.Application.Commands.Vagas;

public class UpdateVagaHandlerTests
{
    private readonly IVagaRepository _vagaRepository = Substitute.For<IVagaRepository>();
    private readonly UpdateVagaHandler _handler;

    public UpdateVagaHandlerTests()
    {
        _handler = new UpdateVagaHandler(_vagaRepository);
    }

    [Fact(DisplayName = "Deve atualizar uma vaga e retornar o DTO atualizado")]
    public async Task Handle_DeveAtualizarVagaERetornarDTO()
    {
        // Arrange
        var command = new UpdateVagaCommand(
            Id: "1",
            Titulo: "Desenvolvedor .NET Pleno",
            Descricao: "Atuar com APIs e microserviços",
            Localizacao: "Remoto",
            TipoVaga: TipoVagaEnum.Remoto);

        _vagaRepository.UpdateAsync(command.Id, Arg.Any<Vaga>())
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(command.Id);
        result.Titulo.Should().Be(command.Titulo);
        result.Localizacao.Should().Be(command.Localizacao);
        result.TipoVaga.Should().Be(command.TipoVaga);

        await _vagaRepository.Received(1)
            .UpdateAsync(command.Id, Arg.Is<Vaga>(v =>
                v.Titulo == command.Titulo &&
                v.Localizacao == command.Localizacao &&
                v.TipoVaga == command.TipoVaga));
    }
}