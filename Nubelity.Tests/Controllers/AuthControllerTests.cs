using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Nubelity.API.Controllers;
using Nubelity.Application.Interfaces;
using System.Text.Json;
using Xunit;

namespace Nubelity.Tests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _authService = new();
    [Fact]
    public async Task Login_ShouldReturnOk_WithToken()
    {
        var controller = new AuthController(_authService.Object);

        string fakeToken = "jwt_token_string";

        _authService
            .Setup(x => x.LoginAsync("elmer", "123"))
            .ReturnsAsync(fakeToken);

        var result = await controller.Login("elmer", "123");

        result.Should().BeOfType<OkObjectResult>();

        var ok = result as OkObjectResult;
        ok.Should().NotBeNull();

        var value = ok!.Value; 

        var prop = value!.GetType().GetProperty("token");
        var returnedToken = prop!.GetValue(value) as string;

        returnedToken.Should().Be(fakeToken);
    }
}
