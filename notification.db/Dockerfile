# Используйте платформу SDK для сборки
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Копируем исходники
COPY --link . .

# Сборка и публикация проекта
RUN dotnet publish api -c Release -o /app/publish

# Используйте минимальный runtime-образ для выполнения приложения
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

# Копируем опубликованные файлы
COPY --from=build /app/publish .

# Устанавливаем команду запуска
ENTRYPOINT ["dotnet", "api.dll"]