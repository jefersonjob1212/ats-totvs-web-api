using FluentAssertions;
using MongoDB.Bson;
using Totvs.ATS.Domain.Entities;
using Totvs.ATS.Domain.Enums;

namespace Totvs.ATS.UnitTests.Domain;

public class VagaTestes
{
    [Fact(DisplayName = "Deve criar vaga com valores padrão corretos")]
    public void DeveCriarVagaComValoresPadrao()
    {
        // Act
        var vaga = new Vaga();

        // Assert
        vaga.Id.Should().NotBeNullOrEmpty("o Id deve ser gerado automaticamente");
        ObjectId.TryParse(vaga.Id, out _).Should().BeTrue("o Id deve ser um ObjectId válido");

        vaga.DataPublicacao.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        vaga.Encerrada.Should().BeFalse("a vaga deve iniciar aberta");
    }

    [Fact(DisplayName = "Deve permitir definir todas as propriedades manualmente")]
    public void DevePermitirDefinirPropriedadesManualmente()
    {
        // Arrange
        var id = ObjectId.GenerateNewId().ToString();
        var titulo = "Desenvolvedor Back-End";
        var descricao = "Responsável por desenvolver APIs REST em .NET";
        var localizacao = "São Paulo - SP";
        var dataPublicacao = new DateTime(2025, 11, 6, 10, 0, 0, DateTimeKind.Local);
        var encerrada = true;
        var tipoVaga = TipoVagaEnum.Presencial;

        // Act
        var vaga = new Vaga
        {
            Id = id,
            Titulo = titulo,
            Descricao = descricao,
            Localizacao = localizacao,
            DataPublicacao = dataPublicacao,
            Encerrada = encerrada,
            TipoVaga = tipoVaga
        };

        // Assert
        vaga.Id.Should().Be(id);
        vaga.Titulo.Should().Be(titulo);
        vaga.Descricao.Should().Be(descricao);
        vaga.Localizacao.Should().Be(localizacao);
        vaga.DataPublicacao.Should().Be(dataPublicacao);
        vaga.Encerrada.Should().BeTrue();
        vaga.TipoVaga.Should().Be(tipoVaga);
    }

    [Fact(DisplayName = "Deve conter atributos de mapeamento MongoDB corretos")]
    public void DeveConterAtributosDeMapeamentoMongo()
    {
        // Arrange
        var tipo = typeof(Vaga);

        // Act
        var idProp = tipo.GetProperty(nameof(Vaga.Id));
        var tipoVagaProp = tipo.GetProperty(nameof(Vaga.TipoVaga));

        // Assert
        idProp.Should().NotBeNull();
        idProp!.GetCustomAttributes(false)
            .Should().Contain(a => a.GetType().Name == "BsonIdAttribute");

        idProp.GetCustomAttributes(false)
            .Should().Contain(a => a.GetType().Name == "BsonRepresentationAttribute");

        tipoVagaProp.Should().NotBeNull();
        tipoVagaProp!.GetCustomAttributes(false)
            .Should().Contain(a => a.GetType().Name == "BsonRepresentationAttribute");
    }

    [Fact(DisplayName = "Deve ser possível alterar o estado de encerramento da vaga")]
    public void DeveAlterarEstadoDeEncerramento()
    {
        // Arrange
        var vaga = new Vaga { Encerrada = false };

        // Act
        vaga.Encerrada = true;

        // Assert
        vaga.Encerrada.Should().BeTrue("o estado de encerramento deve poder ser alterado");
    }
}