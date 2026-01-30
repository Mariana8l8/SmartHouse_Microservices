using Microsoft.AspNetCore.Mvc;

namespace SmartSpeakerAPI.Controllers
{
    [ApiController]
    [Route("speaker")]
    public class SpeakerController : ControllerBase
    {
        private static readonly object _lock = new();
        private static bool _isOn = false;
        private static int _volume = 0;

        [HttpGet("status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Status()
        {
            lock (_lock)
            {
                return Ok(new { isOn = _isOn, volume = _volume });
            }
        }

        [HttpPost("power/{state}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Power(string state)
        {
            lock (_lock)
            {
                if (state == "on") _isOn = true;
                else if (state == "off") _isOn = false;
                else return BadRequest("state must be 'on' or 'off'");
                return Ok();
            }
        }

        [HttpPost("volume/{level:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Volume(int level)
        {
            lock (_lock)
            {
                if (level < 0 || level > 100) return BadRequest("0...100");
                _volume = level;
                return Ok();
            }
        }

        [HttpGet("health")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Health() => Ok("OK");
    }
}