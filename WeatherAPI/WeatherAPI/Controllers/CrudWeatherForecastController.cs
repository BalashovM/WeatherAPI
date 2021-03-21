using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace WeatherAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CrudWeatherForecastController : ControllerBase
    {
        private readonly WeatherForecastHolder _holder;
        public CrudWeatherForecastController(WeatherForecastHolder holder)
        {
            this._holder = holder;
        }
        
        [HttpPost("create")]
        public IActionResult Create([FromQuery] DateTime date, [FromQuery] int temperatureC)
        {
            WeatherForecast weatherForecast = new WeatherForecast
            {
                Date = date,
                TemperatureC = temperatureC
            };

            _holder.Values.Add(weatherForecast);
            return Ok();
        }

        [HttpGet("read")]
        public IActionResult Read([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo)
        {
            if (dateTo == DateTime.MinValue)
                dateTo = DateTime.MaxValue;

            return Ok(_holder.Values.Where(w => (w.Date >= dateFrom && w.Date <= dateTo)).ToList());
        }

        [HttpPut("update")]
        public IActionResult Update([FromQuery] DateTime date, [FromQuery] int newTemperatureC)
        {
            foreach (var value in _holder.Values)
            {
                if (date == value.Date)
                {
                    value.TemperatureC = newTemperatureC; 
                }
            }

            return Ok();
        }

        [HttpDelete("delete")]
        public IActionResult Delete([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo)
        {
            for(int i = _holder.Values.Count-1; i>=0; i--)
            {
                if (_holder.Values[i].Date >= dateFrom && _holder.Values[i].Date <= dateTo)
                {
                    _holder.Values.RemoveAt(i);
                }
            }

            return Ok();
        }
    }
}
