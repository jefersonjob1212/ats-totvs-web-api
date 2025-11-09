using FluentAssertions;
using NSubstitute;
using Totvs.ATS.Application.Commands.Candidatos;
using Totvs.ATS.Application.Handlers.Candidatos;
using Totvs.ATS.Domain.Interfaces;

namespace Totvs.ATS.UnitTests.Application.Commands.Candidatos;

public class DeleteCandidatoHandlerTests
{
    private readonly ICandidatoRepository _candidatoRepository;
    private readonly DeleteCandidatoHandler _handler;

    public DeleteCandidatoHandlerTests()
    {
        _candidatoRepository = Substitute.For<ICandidatoRepository>();
        _handler = new DeleteCandidatoHandler(_candidatoRepository);
    }

    [Fact(DisplayName = "Deve deletar candidato e retornar mensagem de sucesso")]
    public async Task Deve_Deletar_Candidato_Retornar_Sucesso()
    {
        // Arrange
        var candidatoId = "123";
        var command = new DeleteCandidatoCommand(candidatoId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _candidatoRepository
            .Received(1)
            .DeleteAsync(candidatoId);

        result.Should().Be("Candidato excluído com sucesso");
    }
}