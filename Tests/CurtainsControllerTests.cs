using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SmartCurtainsAPI.Controllers;

public class CurtainsControllerTests
{
    private readonly CurtainsController _controller;

    public CurtainsControllerTests()
    {
        _controller = new CurtainsController();
    }

    [Fact]
    public void Status_ReturnsOkWithInitialState()
    {
        var result = _controller.Status();

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var value = okResult.Value.Should().BeAssignableTo<object>().Subject;

        Assert.NotNull(value);
    }

    [Fact]
    public void Power_Open_SetsStateToOpenAnd100()
    {
        var result = _controller.Power("open");
        var statusResult = _controller.Status().As<OkObjectResult>();

        result.Should().BeOfType<OkResult>();
        statusResult.Value.Should().BeEquivalentTo(new { isOpen = true, position = 100 });
    }

    [Fact]
    public void Power_Close_SetsStateToCloseAnd0()
    {
        _controller.Power("open");

        var result = _controller.Power("close");
        var statusResult = _controller.Status().As<OkObjectResult>();

        result.Should().BeOfType<OkResult>();
        statusResult.Value.Should().BeEquivalentTo(new { isOpen = false, position = 0 });
    }

    [Fact]
    public void Power_InvalidState_ReturnsBadRequest()
    {
        var result = _controller.Power("half");

        result.Should().BeOfType<BadRequestObjectResult>()
              .Which.Value.Should().Be("state must be 'open' or 'close'");
    }

    [Theory]
    [InlineData(50, true)]
    [InlineData(0, false)]
    public void Position_ValidValue_SetsPositionAndIsOpen(int value, bool expectedIsOpen)
    {
        var result = _controller.Position(value);
        var statusResult = _controller.Status().As<OkObjectResult>();

        result.Should().BeOfType<OkResult>();
        statusResult.Value.Should().BeEquivalentTo(new { isOpen = expectedIsOpen, position = value });
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(101)]
    public void Position_InvalidValue_ReturnsBadRequest(int invalidValue)
    {
        var result = _controller.Position(invalidValue);

        result.Should().BeOfType<BadRequestObjectResult>()
              .Which.Value.Should().Be("0..100");
    }

    [Fact]
    public void Health_ReturnsOk()
    {
        var result = _controller.Health();

        result.Should().BeOfType<OkObjectResult>()
              .Which.Value.Should().Be("OK");
    }
}