using System.Collections.Generic;

namespace WeatherAPI
{
    public class WeatherForecastHolder
    {
        public List<WeatherForecast> Values { get; set; }

        public WeatherForecastHolder()
        {
            Values = new List<WeatherForecast>();
        }
    }
}
