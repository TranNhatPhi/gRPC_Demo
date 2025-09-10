using System;
using Grpc.Net.Client;
using Grpc.Core;
using GrpcAuth;

AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

using var channel = GrpcChannel.ForAddress("http://localhost:5229");
var client = new AuthService.AuthServiceClient(channel);

// Register
var reg = await client.RegisterAsync(new RegisterRequest { Email = "user1@example.com", Password = "Pass@123" });
Console.WriteLine($"Register: {reg.Success} - {reg.Message}");

// Login
var login = await client.LoginAsync(new LoginRequest { Email = "user1@example.com", Password = "Pass@123" });
Console.WriteLine($"Login: {login.Success}, token-len: {login.Jwt?.Length ?? 0}");

if (login.Success)
{
    var headers = new Metadata { { "Authorization", $"Bearer {login.Jwt}" } };
    var me = await client.MeAsync(new Empty(), headers);
    Console.WriteLine($"Me: {me.Id} - {me.Email}");
}
