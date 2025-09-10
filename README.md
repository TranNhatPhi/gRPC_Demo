# GrpcAuth_Lg_Resgister

This repository contains a gRPC-based authentication system with separate client and server projects.

## Structure
- `GrpcAuth.Client/`: gRPC client implementation
- `GrpcAuth.Server/`: gRPC server implementation, authentication logic, migrations, and services

## Features
- User authentication via gRPC
- Token-based security
- ASP.NET Core Identity integration
- Entity Framework Core migrations

## Getting Started
1. Build the solution:
   ```powershell
   dotnet build GrpcAuth_Lg_Resgister.sln
   ```
2. Run the server:
   ```powershell
   dotnet run --project GrpcAuth.Server/GrpcAuth.Server.csproj
   ```
3. Run the client:
   ```powershell
   dotnet run --project GrpcAuth.Client/GrpcAuth.Client.csproj
   ```

## Project Details
- **Protos**: gRPC service definitions (`auth.proto`)
- **Security**: Token generation and validation
- **Services**: gRPC service implementations
- **Data**: Entity Framework Core context

## Requirements
- .NET 8.0 SDK
- Visual Studio or VS Code

## License
MIT
