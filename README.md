# ats-totvs-web-api
Desafio técnico para vaga de Desenvolvedor Fullstack na Totvs

## Executar a aplicação
1 - Docker

Para rodar no ambiente do Docker, rode os seguintes comandos na pasta ATS do projeto
```bash
docker build -t ats-totvs-web-api .
docker run -d -p 5000:5000 --name ats-totvs-web-api ats-totvs-web-api
```

2 - Ambiente local
Para rodar localmente, basta rodar o projeto em uma IDE de preferência, ou via dotnet cli

```bash
cd ATS/Totvs.ATS.WebAPI
dotnet run
```

## Executar os testes unitários
Os testes estão dentro da pastas Totvs.ATS.Infrastructure.Tests (testes integrados) e Totvs.ATS.UnitTests (testes unitários). Basta rodar em uma IDE de preferência ou via dotnet cli

```bash
cd ATS/Totvs.ATS.Infrastructure.Tests
dotnet test
```
```bash
cd ATS/Totvs.ATS.UnitTests
dotnet test
```

## Base de Dados
A base de Dados é a MongoDB Atlas, um banco de dados na nuvem de fácil gerenciamento. O banco está com permissão de uma semana para acesso de qualquer IP.