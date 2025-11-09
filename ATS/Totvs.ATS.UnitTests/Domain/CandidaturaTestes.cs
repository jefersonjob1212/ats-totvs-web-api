using FluentAssertions;
using MongoDB.Bson;
using Totvs.ATS.Domain.Entities;

namespace Totvs.ATS.UnitTests.Domain;

public class CandidaturaTestes
{
    [Fact(DisplayName = "Deve criar candidatura com Id gerado automaticamente")]
    public void DeveCriarCandidaturaComIdGeradoAutomaticamente()
    {
        // Act
        var candidatura = new Candidatura
        {
            CandidatoId = ObjectId.GenerateNewId().ToString(),
            VagaId = ObjectId.GenerateNewId().ToString(),
            DataCandidatura = DateTime.UtcNow
        };

        // Assert
        candidatura.Id.Should().NotBeNullOrEmpty("o Id deve ser gerado automaticamente");
        ObjectId.TryParse(candidatura.Id, out _).Should().BeTrue("o Id deve ser um ObjectId válido");

        candidatura.CandidatoId.Should().NotBeNullOrEmpty();
        ObjectId.TryParse(candidatura.CandidatoId, out _).Should().BeTrue("CandidatoId deve ser um ObjectId válido");

        candidatura.VagaId.Should().NotBeNullOrEmpty();
        ObjectId.TryParse(candidatura.VagaId, out _).Should().BeTrue("VagaId deve ser um ObjectId válido");

        candidatura.DataCandidatura.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact(DisplayName = "Deve conter os atributos de mapeamento MongoDB corretos")]
    public void DeveConterAtributosDeMapeamentoMongo()
    {
        // Arrange
        var tipo = typeof(Candidatura);

        // Act
        var idProp = tipo.GetProperty(nameof(Candidatura.Id));
        var candidatoIdProp = tipo.GetProperty(nameof(Candidatura.CandidatoId));
        var vagaIdProp = tipo.GetProperty(nameof(Candidatura.VagaId));

        // Assert
        idProp.Should().NotBeNull();
        idProp!.GetCustomAttributes(false)
            .Should().Contain(a => a.GetType().Name == "BsonIdAttribute");

        idProp.GetCustomAttributes(false)
            .Should().Contain(a => a.GetType().Name == "BsonRepresentationAttribute");

        candidatoIdProp.Should().NotBeNull();
        candidatoIdProp!.GetCustomAttributes(false)
            .Should().Contain(a => a.GetType().Name == "BsonRepresentationAttribute");

        vagaIdProp.Should().NotBeNull();
        vagaIdProp!.GetCustomAttributes(false)
            .Should().Contain(a => a.GetType().Name == "BsonRepresentationAttribute");
    }

    [Fact(DisplayName = "Deve permitir definir propriedades manualmente")]
    public void DevePermitirDefinirPropriedadesManualmente()
    {
        // Arrange
        var id = ObjectId.GenerateNewId().ToString();
        var candidatoId = ObjectId.GenerateNewId().ToString();
        var vagaId = ObjectId.GenerateNewId().ToString();
        var data = new DateTime(2025, 11, 6, 12, 0, 0, DateTimeKind.Utc);

        // Act
        var candidatura = new Candidatura
        {
            Id = id,
            CandidatoId = candidatoId,
            VagaId = vagaId,
            DataCandidatura = data
        };

        // Assert
        candidatura.Id.Should().Be(id);
        candidatura.CandidatoId.Should().Be(candidatoId);
        candidatura.VagaId.Should().Be(vagaId);
        candidatura.DataCandidatura.Should().Be(data);
    }
}