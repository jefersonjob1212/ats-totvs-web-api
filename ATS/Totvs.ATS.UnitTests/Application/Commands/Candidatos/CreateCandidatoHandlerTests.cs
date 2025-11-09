using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Totvs.ATS.Application.Commands.Candidatos;
using Totvs.ATS.Application.Handlers.Candidatos;
using Totvs.ATS.Domain.Entities;
using Totvs.ATS.Domain.Enums;
using Totvs.ATS.Domain.Interfaces;

namespace Totvs.ATS.UnitTests.Application.Commands.Candidatos;

public class CreateCandidatoHandlerTests
{
    private readonly ICandidatoRepository _candidatoRepository;
    private readonly CreateCandidatoHandler _handler;

    public CreateCandidatoHandlerTests()
    {
        _candidatoRepository = Substitute.For<ICandidatoRepository>();
        _handler = new CreateCandidatoHandler(_candidatoRepository);
    }

    [Fact(DisplayName = "Deve criar um candidato e retornar seu Id")]
    public async Task Deve_Criar_Candidato_Retornar_Id()
    {
        // Arrange
        var command = new CreateCandidatoCommand(
            Cpf: "1111111111",
            Nome: "João Silva",
            Email: "joao.silva@email.com",
            Telefone: "11999999999",
            Sexo: SexoEnum.Masculino
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _candidatoRepository
            .Received(1)
            .AddAsync(Arg.Is<Candidato>(c =>
                c.Nome == command.Nome &&
                c.Email == command.Email &&
                c.Telefone == command.Telefone &&
                c.Sexo == command.Sexo));

        result.Should().NotBeNullOrEmpty();
    }
}