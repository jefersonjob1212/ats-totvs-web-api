using FluentAssertions;
using NSubstitute;
using Totvs.ATS.Application.Commands.Vagas;
using Totvs.ATS.Application.Handlers.Vagas;
using Totvs.ATS.Domain.Interfaces;

namespace Totvs.ATS.UnitTests.Application.Commands.Vagas;

public class DeleteVagaHandlerTests
{
    private readonly IVagaRepository _vagaRepository = Substitute.For<IVagaRepository>();
    private readonly DeleteVagaHandler _handler;

    public DeleteVagaHandlerTests()
    {
        _handler = new DeleteVagaHandler(_vagaRepository);
    }

    [Fact(DisplayName = "Deve deletar a vaga com sucesso")]
    public async Task Handle_DeveDeletarVagaComSucesso()
    {
        // Arrange
        var command = new DeleteVagaCommand("123");
        _vagaRepository.DeleteAsync(command.Id).Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be("Vaga excluída com sucesso");
        await _vagaRepository.Received(1).DeleteAsync(command.Id);
    }
}