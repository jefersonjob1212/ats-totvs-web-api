using System.Linq.Expressions;
using FluentAssertions;
using NSubstitute;
using Totvs.ATS.Application.Commands.Candidaturas;
using Totvs.ATS.Application.Handlers.Candidaturas;
using Totvs.ATS.Domain.Entities;
using Totvs.ATS.Domain.Exceptions;
using Totvs.ATS.Domain.Interfaces;

namespace Totvs.ATS.UnitTests.Application.Commands.Candidaturas;

public class VincularCandidatoVagaHandlerTests
{
    private readonly IVagaRepository _vagaRepository = Substitute.For<IVagaRepository>();
    private readonly ICandidatoRepository _candidatoRepository = Substitute.For<ICandidatoRepository>();
    private readonly ICandidaturaRepository _candidaturaRepository = Substitute.For<ICandidaturaRepository>();
    private readonly VincularCandidatoVagaHandler _handler;

    public VincularCandidatoVagaHandlerTests()
    {
        _handler = new VincularCandidatoVagaHandler(_vagaRepository, _candidatoRepository, _candidaturaRepository);
    }

    [Fact(DisplayName = "Deve vincular candidato à vaga com sucesso")]
    public async Task Deve_Vincular_Candidato_A_Vaga_Com_Sucesso()
    {
        // Arrange
        var vaga = new Vaga { Id = "1", Titulo = "Dev .NET", Encerrada = false };
        var candidato = new Candidato { Id = "10", Nome = "Jeferson" };

        _vagaRepository.FindByIdAsync(vaga.Id).Returns(vaga);
        _candidatoRepository.FindByIdAsync(candidato.Id).Returns(candidato);
        _candidaturaRepository.FindAsync(Arg.Any<Expression<Func<Candidatura, bool>>>())
            .Returns(new List<Candidatura>()); // sem candidaturas existentes

        var command = new VincularCandidatoVagaCommand(
            VagaId: vaga.Id,
            CandidatoId: candidato.Id
        );

        // Act
        var resultado = await _handler.Handle(command, CancellationToken.None);

        // Assert
        resultado.Should().NotBeNullOrEmpty();
        await _candidaturaRepository.Received(1).AddAsync(Arg.Any<Candidatura>());
    }

    [Fact(DisplayName = "Deve lançar NotFoundException se vaga não existir")]
    public async Task Deve_Lancar_NotFoundException_Se_Vaga_Nao_Existir()
    {
        // Arrange
        _vagaRepository.FindByIdAsync(Arg.Any<string>()).Returns((Vaga?)null);
        var command = new VincularCandidatoVagaCommand(VagaId: "999", CandidatoId: "1");

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("Vaga não cadastrada");
    }

    [Fact(DisplayName = "Deve lançar BusinessRuleException se vaga estiver encerrada")]
    public async Task Deve_Lancar_BusinessRuleException_Se_Vaga_Encerrada()
    {
        // Arrange
        var vaga = new Vaga { Id = "1", Encerrada = true };
        _vagaRepository.FindByIdAsync(vaga.Id).Returns(vaga);
        var command = new VincularCandidatoVagaCommand(VagaId: "1", CandidatoId: "10");

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage("Vaga foi encerrada.");
    }

    [Fact(DisplayName = "Deve lançar NotFoundException se candidato não existir")]
    public async Task Deve_Lancar_NotFoundException_Se_Candidato_Nao_Existir()
    {
        // Arrange
        var vaga = new Vaga { Id = "1", Encerrada = false };
        _vagaRepository.FindByIdAsync(vaga.Id).Returns(vaga);
        _candidatoRepository.FindByIdAsync(Arg.Any<string>()).Returns((Candidato?)null);

        var command = new VincularCandidatoVagaCommand(VagaId: vaga.Id, CandidatoId: "10");

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("Usuário não cadastrado");
    }

    [Fact(DisplayName = "Deve lançar BusinessRuleException se candidato já estiver vinculado à vaga")]
    public async Task Deve_Lancar_BusinessRuleException_Se_Candidato_Ja_Estiver_Vinculado()
    {
        // Arrange
        var vaga = new Vaga { Id = "1", Encerrada = false };
        var candidato = new Candidato { Id = "10", Nome = "Jeferson" };
        var candidaturaExistente = new List<Candidatura>
        {
            new() { VagaId = vaga.Id, CandidatoId = candidato.Id }
        };

        _vagaRepository.FindByIdAsync(vaga.Id).Returns(vaga);
        _candidatoRepository.FindByIdAsync(candidato.Id).Returns(candidato);
        _candidaturaRepository.FindAsync(Arg.Any<Expression<Func<Candidatura, bool>>>()).Returns(candidaturaExistente);

        var command = new VincularCandidatoVagaCommand(VagaId: vaga.Id, CandidatoId: candidato.Id);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage("Candidato já está concorrendo a esta vaga.");
    }
}