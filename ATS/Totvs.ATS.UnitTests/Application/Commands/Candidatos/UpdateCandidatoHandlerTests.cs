using FluentAssertions;
using NSubstitute;
using Totvs.ATS.Application.Commands.Candidatos;
using Totvs.ATS.Application.DTOs.Candidatos;
using Totvs.ATS.Application.Handlers.Candidatos;
using Totvs.ATS.Domain.Entities;
using Totvs.ATS.Domain.Enums;
using Totvs.ATS.Domain.Interfaces;

namespace Totvs.ATS.UnitTests.Application.Commands.Candidatos;

public class UpdateCandidatoHandlerTests
{
    private readonly ICandidatoRepository _candidatoRepository;
    private readonly UpdateCandidatoHandler _handler;

    public UpdateCandidatoHandlerTests()
    {
        _candidatoRepository = Substitute.For<ICandidatoRepository>();
        _handler = new UpdateCandidatoHandler(_candidatoRepository);
    }

    [Fact(DisplayName = "Deve atualizar um candidato e retornar o DTO correspondente")]
    public async Task Deve_Atualizar_Candidato_E_Retornar_DTO()
    {
        // Arrange
        var command = new UpdateCandidatoCommand(
            Id: "123",
            Cpf: "1111111111",
            Nome: "João Silva",
            Email: "joao.silva@email.com",
            Telefone: "99999-9999",
            Sexo: SexoEnum.Masculino
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _candidatoRepository.Received(1)
            .UpdateAsync(Arg.Is<string>(id => id == "123"),
                         Arg.Is<Candidato>(c =>
                             c.Nome == "João Silva" &&
                             c.Email == "joao.silva@email.com" &&
                             c.Sexo == SexoEnum.Masculino));

        result.Should().NotBeNull();
        result.Should().BeOfType<CandidatoDTO>();
        result.Nome.Should().Be("João Silva");
        result.Email.Should().Be("joao.silva@email.com");
    }

    [Fact(DisplayName = "Deve chamar UpdateAsync mesmo se alguns campos estiverem nulos")]
    public async Task Deve_Chamar_UpdateMesmo_Com_Campos_Nulos()
    {
        // Arrange
        var command = new UpdateCandidatoCommand(
            Id: "456",
            Cpf: "1111111111",
            Nome: null,
            Email: "teste@email.com",
            Telefone: null,
            Sexo: SexoEnum.NaoInformado
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _candidatoRepository.Received(1)
            .UpdateAsync(Arg.Is("456"), Arg.Any<Candidato>());

        result.Should().NotBeNull();
        result.Email.Should().Be("teste@email.com");
    }
}