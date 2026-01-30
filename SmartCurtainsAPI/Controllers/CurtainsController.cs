using Microsoft.AspNetCore.Mvc;

namespace SmartCurtainsAPI.Controllers
{
    [ApiController]
    [Route("curtains")]
    public class CurtainsController : ControllerBase
    {
        private static readonly object _lock = new();
        private static bool _isOpen = false;
        private static int _position = 0; 

        [HttpGet("status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Status()
        {
            lock (_lock) { return Ok(new { isOpen = _isOpen, position = _position }); }
        }

        [HttpPost("power/{state}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Power(string state)
        {
            lock (_lock)
            {
                if (state == "open") { _isOpen = true; _position = 100; }
                else if (state == "close") { _isOpen = false; _position = 0; }
                else return BadRequest("state must be 'open' or 'close'");
                return Ok();
            }
        }

        [HttpPost("position/{value:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Position(int value)
        {
            lock (_lock)
            {
                if (value < 0 || value > 100) return BadRequest("0..100");
                _position = value;
                _isOpen = value > 0;
                return Ok();
            }
        }

        [HttpGet("health")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Health() => Ok("OK");
    }
}
