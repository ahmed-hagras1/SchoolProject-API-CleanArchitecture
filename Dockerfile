# استخدام نسخة .NET 8 كبيئة تشغيل (تأكد من مطابقتها لإصدار مشروعك)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# نسخ ملفات المشروع بالكامل لحل الـ Dependencies
COPY ["SchoolProject.API/SchoolProject.API.csproj", "SchoolProject.API/"]
COPY ["SchoolProject.Core/SchoolProject.Core.csproj", "SchoolProject.Core/"]
COPY ["SchoolProject.Data/SchoolProject.Data.csproj", "SchoolProject.Data/"]
COPY ["SchoolProject.Infrastructure/SchoolProject.Infrastructure.csproj", "SchoolProject.Infrastructure/"]
COPY ["SchoolProject.Service/SchoolProject.Service.csproj", "SchoolProject.Service/"]

RUN dotnet restore "SchoolProject.API/SchoolProject.API.csproj"

# نسخ باقي الكود وعمل Build
COPY . .
WORKDIR "/src/SchoolProject.API"
RUN dotnet build "SchoolProject.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SchoolProject.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SchoolProject.API.dll"]