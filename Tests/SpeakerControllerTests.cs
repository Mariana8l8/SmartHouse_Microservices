using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SmartSpeakerAPI.Controllers;

public class SpeakerControllerTests
{
    private readonly SpeakerController _controller;

    public SpeakerControllerTests()
    {
        _controller = new SpeakerController();
    }

    [Fact]
    public void Status_ReturnsOkWithCurrentState()
    {
        var result = _controller.Status();

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;

        okResult.Value.Should().BeEquivalentTo(new { isOn = false, volume = 0 });
    }

    [Fact]
    public void Power_On_SetsIsOnToTrue()
    {
        var result = _controller.Power("on");
        var statusResult = _controller.Status().As<OkObjectResult>();

        result.Should().BeOfType<OkResult>();
        statusResult.Value.Should().BeEquivalentTo(new { isOn = true, volume = 1 });
    }

    [Fact]
    public void Power_Off_SetsIsOnToFalse()
    {
        _controller.Power("on");

        var result = _controller.Power("off");
        var statusResult = _controller.Status().As<OkObjectResult>();

        result.Should().BeOfType<OkResult>();
        statusResult.Value.Should().BeEquivalentTo(new { isOn = false, volume = 0 });
    }

    [Fact]
    public void Power_InvalidState_ReturnsBadRequest()
    {
        var result = _controller.Power("pause");

        result.Should().BeOfType<BadRequestObjectResult>()
              .Which.Value.Should().Be("state must be 'on' or 'off'");
    }


    [Theory]
    [InlineData(75)]
    [InlineData(1)]
    public void Volume_ValidLevel_SetsVolume(int level)
    {
        var result = _controller.Volume(level);
        var statusResult = _controller.Status().As<OkObjectResult>();

        result.Should().BeOfType<OkResult>();
        statusResult.Value.Should().BeEquivalentTo(new { isOn = false, volume = level });
    }

    [Theory]
    [InlineData(-5)]
    [InlineData(110)]
    public void Volume_InvalidLevel_ReturnsBadRequest(int invalidLevel)
    {
        var result = _controller.Volume(invalidLevel);

        result.Should().BeOfType<BadRequestObjectResult>()
              .Which.Value.Should().Be("0...100");
    }
}