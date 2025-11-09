using System.Linq.Expressions;
using NSubstitute;
using Totvs.ATS.Application.Commands.Candidaturas;
using Totvs.ATS.Application.Handlers.Candidaturas;
using Totvs.ATS.Domain.Entities;
using Totvs.ATS.Domain.Exceptions;
using Totvs.ATS.Domain.Interfaces;

namespace Totvs.ATS.UnitTests.Application.Commands.Candidaturas;

public class RemoveCandidatoVagaHandlerTests
{
    private readonly IVagaRepository _vagaRepository = Substitute.For<IVagaRepository>();
    private readonly ICandidatoRepository _candidatoRepository = Substitute.For<ICandidatoRepository>();
    private readonly ICandidaturaRepository _candidaturaRepository = Substitute.For<ICandidaturaRepository>();

    private readonly RemoveCandidatoVagaHandler _handler;

    public RemoveCandidatoVagaHandlerTests()
    {
        _handler = new RemoveCandidatoVagaHandler(
            _vagaRepository,
            _candidatoRepository,
            _candidaturaRepository);
    }

    [Fact]
    public async Task Handle_DeveRemoverCandidatura_ComSucesso()
    {
        // Arrange
        var command = new RemoveCandidatoVagaCommand(
            VagaId: Guid.NewGuid().ToString(), 
            CandidatoId: Guid.NewGuid().ToString());

        var vaga = new Vaga { Id = command.VagaId, Titulo = "Dev .NET", Encerrada = false };
        var candidato = new Candidato { Id = command.CandidatoId, Nome = "João" };
        var candidatura = new Candidatura { Id = Guid.NewGuid().ToString(), CandidatoId = candidato.Id, VagaId = vaga.Id };

        _vagaRepository.FindByIdAsync(command.VagaId).Returns(vaga);
        _candidatoRepository.FindByIdAsync(command.CandidatoId).Returns(candidato);
        _candidaturaRepository
            .FindAsync(Arg.Any<Expression<Func<Candidatura, bool>>>())
            .Returns(new List<Candidatura> { candidatura });

        // Act
        var resultado = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal("Candidatura removida com sucesso", resultado);
        await _candidaturaRepository.Received(1).DeleteAsync(candidatura.Id);
    }

    [Fact]
    public async Task Handle_DeveLancar_NotFoundException_QuandoVagaNaoEncontrada()
    {
        // Arrange
        var command = new RemoveCandidatoVagaCommand(
            VagaId: Guid.NewGuid().ToString(), 
            CandidatoId: Guid.NewGuid().ToString());
        _vagaRepository.FindByIdAsync(command.VagaId).Returns((Vaga?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_DeveLancar_BusinessRuleException_QuandoVagaEncerrada()
    {
        // Arrange
        var command = new RemoveCandidatoVagaCommand(
            VagaId: Guid.NewGuid().ToString(), 
            CandidatoId: Guid.NewGuid().ToString());
        var vaga = new Vaga { Id = command.VagaId, Encerrada = true };
        _vagaRepository.FindByIdAsync(command.VagaId).Returns(vaga);

        // Act & Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_DeveLancar_NotFoundException_QuandoCandidatoNaoEncontrado()
    {
        // Arrange
        var command = new RemoveCandidatoVagaCommand(
            VagaId: Guid.NewGuid().ToString(), 
            CandidatoId: Guid.NewGuid().ToString());
        var vaga = new Vaga { Id = command.VagaId, Encerrada = false };

        _vagaRepository.FindByIdAsync(command.VagaId).Returns(vaga);
        _candidatoRepository.FindByIdAsync(command.CandidatoId).Returns((Candidato?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_DeveLancar_NotFoundException_QuandoCandidaturaNaoEncontrada()
    {
        // Arrange
        var command = new RemoveCandidatoVagaCommand(
            VagaId: Guid.NewGuid().ToString(), 
            CandidatoId: Guid.NewGuid().ToString());
        var vaga = new Vaga { Id = command.VagaId, Encerrada = false };
        var candidato = new Candidato { Id = command.CandidatoId, Nome = "Maria" };

        _vagaRepository.FindByIdAsync(command.VagaId).Returns(vaga);
        _candidatoRepository.FindByIdAsync(command.CandidatoId).Returns(candidato);
        _candidaturaRepository
            .FindAsync(Arg.Any<Expression<Func<Candidatura, bool>>>())
            .Returns(new List<Candidatura>());

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }
}