using Microsoft.AspNetCore.Mvc;

namespace SmartLightAPI.Controllers
{
    [ApiController]
    [Route("light")]
    public class LightController : ControllerBase
    {
        private static readonly object _lock = new object();
        private static bool _isOn = false;
        private static int _brightness = 50;

        [HttpGet("status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult Status()
        {
            return Ok(new { isOn = _isOn, brightness = _brightness});
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

        [HttpPost("brightness/{level:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Brightness(int level)
        {
            lock (_lock)
            {
                if (level < 0 || level > 100) return BadRequest("0..100");
                _brightness = level;
                return Ok();
            }
        }

        [HttpGet("health")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Health() => Ok("OK");
    }
}
