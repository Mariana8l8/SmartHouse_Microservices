using Microsoft.AspNetCore.Mvc;
using SmartAppMain.Models;
using SmartAppMain.Services;
using System.Text.Json;

namespace SmartAppMain.Controllers
{
    [ApiController]
    [Route("main")]
    public class MainController : ControllerBase
    {
        private readonly IoTFacade _facade;
        public MainController(IoTFacade facade) { _facade = facade; }

        [HttpGet("status-all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public IActionResult StatusAll()
        {
            var speakerF = _facade.GetString("speaker", "speaker/status");
            var lightF = _facade.GetString("light", "light/status");
            var curtainsF = _facade.GetString("curtains", "curtains/status");
            return Ok(new { speaker = speakerF, light = lightF, curtains = curtainsF });
        }

        [HttpPost("toggle-light")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public IActionResult ToggleLight()
        {
            var json = _facade.GetString("light", "light/status");
            var status = System.Text.Json.JsonSerializer.Deserialize<LightStatus>(json);
            var newState = (status != null && status.isOn) ? "off" : "on";
            var response = _facade.Post("light", $"light/power/{newState}");
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Light service error");

            return Ok(new { toggledTo = newState });
        }

        [HttpPost("toggle-speaker")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public IActionResult ToggleSpeaker()
        {
            var json = _facade.GetString("speaker", "speaker/status");
            var status = JsonSerializer.Deserialize<SpeakerStatus>(json);
            var newState = (status != null && status.isOn) ? "off" : "on";
            var response = _facade.Post("speaker", $"speaker/power/{newState}");
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Speaker service error");

            return Ok(new { toggledTo = newState });
        }

        [HttpPost("toggle-curtains")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public IActionResult ToggleCurtains()
        {
            var json = _facade.GetString("curtains", "curtains/status");
            var status = JsonSerializer.Deserialize<CurtainsStatus>(json);
            var newState = (status != null && status.isOpen) ? "close" : "open";
            var response = _facade.Post("curtains", $"curtains/power/{newState}");
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Curtains service error");

            return Ok(new { toggledTo = newState });
        }
    }
}
