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

    [Fact(DisplayName = "Deve vincular candidato a vaga com sucesso")]
    public async Task Handle_DeveVincularCandidatoComSucesso()
    {
        // Arrange
        var vaga = new Vaga { Id = "vaga123", Titulo = "Desenvolvedor", Encerrada = false };
        var candidato = new Candidato { Id = "cand123", Nome = "Maria" };

        _vagaRepository.FindByIdAsync(vaga.Id).Returns(vaga);
        _candidatoRepository.FindByIdAsync(candidato.Id).Returns(candidato);
        _candidaturaRepository.AddAsync(Arg.Any<Candidatura>()).Returns(Task.CompletedTask);

        var command = new VincularCandidatoVagaCommand(candidato.Id, vaga.Id);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(string.IsNullOrEmpty(result));
        await _candidaturaRepository.Received(1).AddAsync(Arg.Is<Candidatura>(c =>
            c.CandidatoId == candidato.Id && c.VagaId == vaga.Id));
    }

    [Fact(DisplayName = "Deve lançar NotFoundException quando a vaga não existe")]
    public async Task Handle_DeveLancarExcecao_QuandoVagaNaoExiste()
    {
        // Arrange
        _vagaRepository.FindByIdAsync("vagaInexistente").Returns((Vaga?)null);

        var command = new VincularCandidatoVagaCommand("cand123", "vagaInexistente");

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));

        await _candidaturaRepository.DidNotReceive().AddAsync(Arg.Any<Candidatura>());
    }

    [Fact(DisplayName = "Deve lançar BusinessRuleException quando a vaga estiver encerrada")]
    public async Task Handle_DeveLancarExcecao_QuandoVagaEncerrada()
    {
        // Arrange
        var vaga = new Vaga { Id = "vaga123", Encerrada = true };
        _vagaRepository.FindByIdAsync(vaga.Id).Returns(vaga);

        var command = new VincularCandidatoVagaCommand("cand123", vaga.Id);

        // Act & Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() => _handler.Handle(command, CancellationToken.None));

        await _candidaturaRepository.DidNotReceive().AddAsync(Arg.Any<Candidatura>());
    }

    [Fact(DisplayName = "Deve lançar NotFoundException quando o candidato não existir")]
    public async Task Handle_DeveLancarExcecao_QuandoCandidatoNaoExiste()
    {
        // Arrange
        var vaga = new Vaga { Id = "vaga123", Encerrada = false };
        _vagaRepository.FindByIdAsync(vaga.Id).Returns(vaga);
        _candidatoRepository.FindByIdAsync("candInexistente").Returns((Candidato?)null);

        var command = new VincularCandidatoVagaCommand("candInexistente", vaga.Id);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));

        await _candidaturaRepository.DidNotReceive().AddAsync(Arg.Any<Candidatura>());
    }
}