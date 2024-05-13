FROM mcr.microsoft.com/dotnet/aspnet:6.0.18-alpine3.18
WORKDIR /app
COPY publish .
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
EXPOSE 80
ENTRYPOINT ["dotnet", "AcademyGestionGeneral.dll"]