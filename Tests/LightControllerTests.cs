using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SmartLightAPI.Controllers;

public class LightControllerTests
{
    private readonly LightController _controller;

    public LightControllerTests()
    {
        _controller = new LightController();
    }

    [Fact]
    public void Status_ReturnsOkWithCurrentState()
    {
        var result = _controller.Status();

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;

        okResult.Value.Should().BeEquivalentTo(new { isOn = false, brightness = 10 });
    }

    [Fact]
    public void Power_On_SetsIsOnToTrue()
    {
        var result = _controller.Power("on");
        var statusResult = _controller.Status().As<OkObjectResult>();

        result.Should().BeOfType<OkResult>();
        statusResult.Value.Should().BeEquivalentTo(new { isOn = true, brightness = 10 });
    }

    [Fact]
    public void Power_Off_SetsIsOnToFalse()
    {
        _controller.Power("on");

        var result = _controller.Power("off");
        var statusResult = _controller.Status().As<OkObjectResult>();

        result.Should().BeOfType<OkResult>();
        statusResult.Value.Should().BeEquivalentTo(new { isOn = false, brightness = 10 });
    }

    [Fact]
    public void Power_InvalidState_ReturnsBadRequest()
    {
        var result = _controller.Power("toggle");

        result.Should().BeOfType<BadRequestObjectResult>()
              .Which.Value.Should().Be("state must be 'on' or 'off'");
    }

    [Theory]
    [InlineData(10)]
    [InlineData(100)]
    public void Brightness_ValidLevel_SetsBrightness(int level)
    {
        var result = _controller.Brightness(level);
        var statusResult = _controller.Status().As<OkObjectResult>();

        result.Should().BeOfType<OkResult>();
        statusResult.Value.Should().BeEquivalentTo(new { isOn = false, brightness = level });
    }

    [Theory]
    [InlineData(-10)]
    [InlineData(105)]
    public void Brightness_InvalidLevel_ReturnsBadRequest(int invalidLevel)
    {
        var result = _controller.Brightness(invalidLevel);

        result.Should().BeOfType<BadRequestObjectResult>()
              .Which.Value.Should().Be("0..100");
    }
}