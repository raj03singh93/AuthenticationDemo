using Common.Library.Model;
using Common.Library.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [Route("Token")]
        [HttpGet]
        public string Token()
        {
            JwtTokenGeneratorModel jwtTokenGeneratorModel = new JwtTokenGeneratorModel();
            jwtTokenGeneratorModel.Issuer = "http://localhost:5000";
            jwtTokenGeneratorModel.Audiance = "http://localhost:44344";
            jwtTokenGeneratorModel.ExpireAfter = 3;
            jwtTokenGeneratorModel.Key = "thisisaverylongkeyforsha";
            jwtTokenGeneratorModel.Claims = new List<System.Security.Claims.Claim>();
            jwtTokenGeneratorModel.Claims.Add(new System.Security.Claims.Claim("Id", "1"));
            return JwtTokenGenerator.GenerateJwtToken(jwtTokenGeneratorModel); ;
        }
    }
}
