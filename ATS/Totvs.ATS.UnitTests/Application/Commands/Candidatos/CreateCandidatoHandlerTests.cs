using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Totvs.ATS.Application.Commands.Candidatos;
using Totvs.ATS.Application.Handlers.Candidatos;
using Totvs.ATS.Domain.Entities;
using Totvs.ATS.Domain.Enums;
using Totvs.ATS.Domain.Exceptions;
using Totvs.ATS.Domain.Interfaces;

namespace Totvs.ATS.UnitTests.Application.Commands.Candidatos;

public class CreateCandidatoHandlerTests
{
    private readonly ICandidatoRepository _candidatoRepository = Substitute.For<ICandidatoRepository>();
    private readonly CreateCandidatoHandler _handler;

    public CreateCandidatoHandlerTests()
    {
        _handler = new CreateCandidatoHandler(_candidatoRepository);
    }

    [Fact(DisplayName = "Deve criar candidato com sucesso")]
    public async Task Deve_Criar_Candidato_Com_Sucesso()
    {
        // Arrange
        var command = new CreateCandidatoCommand
        (
            Nome: "Jeferson",
            Cpf: "12345678900",
            Email: "jeferson@email.com",
            Telefone: "47988776655",
            Sexo: SexoEnum.NaoInformado
        );

        _candidatoRepository.FindAsync(Arg.Any<Expression<Func<Candidato, bool>>>())
            .Returns(new List<Candidato>()); // nenhum candidato com o mesmo CPF

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNullOrEmpty();
        await _candidatoRepository.Received(1).AddAsync(Arg.Any<Candidato>());
    }

    [Fact(DisplayName = "Deve lançar BusinessRuleException se CPF já existir")]
    public async Task Deve_Lancar_BusinessRuleException_Se_CPF_Ja_Existir()
    {
        // Arrange
        var command = new CreateCandidatoCommand
        (
            Nome: "Jeferson",
            Cpf: "12345678900",
            Email: "jeferson@email.com",
            Telefone: "47988776655",
            Sexo: SexoEnum.NaoInformado
        );

        var candidatoExistente = new List<Candidato>
        {
            new() { Id = "1", Nome = "Outro", Cpf = "12345678900", Email = "outro@email.com" }
        };

        _candidatoRepository.FindAsync(Arg.Any<Expression<Func<Candidato, bool>>>())
            .Returns(candidatoExistente);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage("Candidato já cadastrado");

        await _candidatoRepository.DidNotReceive().AddAsync(Arg.Any<Candidato>());
    }
}