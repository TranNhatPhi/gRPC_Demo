
namespace GrpcAuth;
using Grpc.Core;                          
using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Identity;      
public class AuthGrpcService : AuthService.AuthServiceBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly GrpcAuth.Server.Security.TokenService _tokenService;

    public AuthGrpcService(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        GrpcAuth.Server.Security.TokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    public override async Task<AuthReply> Register(RegisterRequest request, ServerCallContext context)
    {
        var existing = await _userManager.FindByEmailAsync(request.Email);
        if (existing != null)
            return new AuthReply { Success = false, Message = "Email already exists" };

        var user = new IdentityUser { UserName = request.Email, Email = request.Email };
        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var msg = string.Join("; ", result.Errors.Select(e => e.Description));
            return new AuthReply { Success = false, Message = msg };
        }

        // (optional) add role, send confirm email, etc.

        return new AuthReply { Success = true, Message = "Registered" };
    }

    public override async Task<AuthReply> Login(LoginRequest request, ServerCallContext context)
    {
    var user = await _userManager.FindByEmailAsync(request.Email);
    if (user == null)
        return new AuthReply { Success = false, Message = "Invalid credentials" };

    var check = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
    if (!check.Succeeded)
        return new AuthReply { Success = false, Message = "Invalid credentials" };

    // Bỏ GetRolesAsync
    var jwt = _tokenService.Create(user, null);

    return new AuthReply { Success = true, Message = "OK", Jwt = jwt };
    }
    [Authorize] // cần Bearer token
    public override Task<UserDto> Me(Empty request, ServerCallContext context)
    {
        var http = context.GetHttpContext();
        var userId = http.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "";
        var email  = http.User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email)?.Value
                     ?? http.User.FindFirst("email")?.Value ?? "";

        return Task.FromResult(new UserDto { Id = userId, Email = email });
    }
}
