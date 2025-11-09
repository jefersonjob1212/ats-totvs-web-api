using FluentAssertions;
using MongoDB.Bson;
using Totvs.ATS.Domain.Entities;
using Totvs.ATS.Domain.Enums;

namespace Totvs.ATS.UnitTests.Domain;

public class CandidatoTestes
{
    [Fact(DisplayName = "Deve criar candidato com Id gerado automaticamente")]
    public void DeveCriarCandidatoComIdGeradoAutomaticamente()
    {
        // Act
        var candidato = new Candidato
        {
            Nome = "Jeferson Job Ribeiro",
            Email = "jeferson@teste.com",
            Telefone = "47999999999",
            Cpf = "12345678900",
            Sexo = SexoEnum.Masculino
        };

        // Assert
        candidato.Id.Should().NotBeNullOrEmpty("o Id deve ser gerado automaticamente");
        ObjectId.TryParse(candidato.Id, out _).Should().BeTrue("o Id deve ser um ObjectId válido do MongoDB");

        candidato.Nome.Should().Be("Jeferson Job Ribeiro");
        candidato.Email.Should().Be("jeferson@teste.com");
        candidato.Telefone.Should().Be("47999999999");
        candidato.Cpf.Should().Be("12345678900");
        candidato.Sexo.Should().Be(SexoEnum.Masculino);
        candidato.Candidaturas.Should().NotBeNull().And.BeEmpty("a lista de candidaturas deve ser inicializada vazia");
    }

    [Fact(DisplayName = "Deve conter os atributos de mapeamento MongoDB corretos")]
    public void DeveConterAtributosDeMapeamentoMongo()
    {
        // Arrange
        var tipo = typeof(Candidato);

        // Act
        var idProp = tipo.GetProperty(nameof(Candidato.Id));
        var sexoProp = tipo.GetProperty(nameof(Candidato.Sexo));
        var candidaturasProp = tipo.GetProperty(nameof(Candidato.Candidaturas));

        // Assert
        idProp.Should().NotBeNull();
        idProp!.GetCustomAttributes(false)
            .Should().Contain(a => a.GetType().Name == "BsonIdAttribute");

        idProp.GetCustomAttributes(false)
            .Should().Contain(a => a.GetType().Name == "BsonRepresentationAttribute");

        sexoProp.Should().NotBeNull();
        sexoProp!.GetCustomAttributes(false)
            .Should().Contain(a => a.GetType().Name == "BsonRepresentationAttribute");

        candidaturasProp.Should().NotBeNull();
        candidaturasProp!.GetCustomAttributes(false)
            .Should().Contain(a => a.GetType().Name == "BsonIgnoreAttribute");
    }

    [Fact(DisplayName = "Deve permitir adicionar candidaturas à lista")]
    public void DevePermitirAdicionarCandidaturas()
    {
        // Arrange
        var candidato = new Candidato();

        // Act
        candidato.Candidaturas.Add(new Candidatura());
        candidato.Candidaturas.Add(new Candidatura());

        // Assert
        candidato.Candidaturas.Should().HaveCount(2);
    }
}