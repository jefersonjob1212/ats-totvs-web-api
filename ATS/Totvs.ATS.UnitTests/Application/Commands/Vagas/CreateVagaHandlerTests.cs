using NSubstitute;
using Totvs.ATS.Application.Commands.Vagas;
using Totvs.ATS.Application.Handlers.Vagas;
using Totvs.ATS.Domain.Entities;
using Totvs.ATS.Domain.Enums;
using Totvs.ATS.Domain.Interfaces;

namespace Totvs.ATS.UnitTests.Application.Commands.Vagas;

public class CreateVagaHandlerTests
{
    private readonly IVagaRepository _vagaRepository = Substitute.For<IVagaRepository>();
    private readonly CreateVagaHandler _handler;

    public CreateVagaHandlerTests()
    {
        _handler = new CreateVagaHandler(_vagaRepository);
    }

    [Fact(DisplayName = "Deve criar uma vaga com sucesso")]
    public async Task Handle_DeveCriarVagaComSucesso()
    {
        // Arrange
        var command = new CreateVagaCommand(
            Titulo: "Desenvolvedor .NET",
            Descricao: "Atuar com desenvolvimento de APIs REST",
            Localizacao: "São Paulo-SP",
            TipoVaga: TipoVagaEnum.Remoto);

        _vagaRepository.AddAsync(Arg.Any<Vaga>()).Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(string.IsNullOrEmpty(result));
        await _vagaRepository.Received(1).AddAsync(Arg.Is<Vaga>(v =>
            v.Titulo == command.Titulo &&
            v.Descricao == command.Descricao &&
            v.Localizacao == command.Localizacao &&
            v.TipoVaga == command.TipoVaga));
    }
}