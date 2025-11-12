# ===========================
# Etapa 1: Build
# ===========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia apenas os .csproj de cada projeto para otimizar cache
COPY ["Totvs.ATS.WebAPI/Totvs.ATS.WebAPI.csproj", "Totvs.ATS.WebAPI/"]
COPY ["Totvs.ATS.Domain/Totvs.ATS.Domain.csproj", "Totvs.ATS.Domain/"]
COPY ["Totvs.ATS.Application/Totvs.ATS.Application.csproj", "Totvs.ATS.Application/"]
COPY ["Totvs.ATS.Infrastructure/Totvs.ATS.Infrastructure.csproj", "Totvs.ATS.Infrastructure/"]

# Restaura dependências
RUN dotnet restore "Totvs.ATS.WebAPI/Totvs.ATS.WebAPI.csproj"

# Copia todo o código da solução
COPY . .

# Publica a API
WORKDIR "/src/Totvs.ATS.WebAPI"
RUN dotnet publish "Totvs.ATS.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ===========================
# Etapa 2: Runtime
# ===========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Define porta da API
ENV ASPNETCORE_URLS=http://+:5000

# Copia o build publicado
COPY --from=build /app/publish .

# Expõe a porta 5000
EXPOSE 5000

# Define variável de ambiente para MongoDB Atlas
ENV MONGO_CONNECTION="mongodb+srv://jefersonjob_db_user:Iwxoqppm7bL6Ig9M@clusterats.ajtocwa.mongodb.net/?appName=ClusterAts"
ENV MONGO_DATABASE="ats"

# Inicia a API
ENTRYPOINT ["dotnet", "Totvs.ATS.WebAPI.dll"]
