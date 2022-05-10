using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace AnimalLibrary.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };


        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("GetAll")]
        public IEnumerable<TaxonomicRankType> GetAll(int id)
        {
            Log.Information($"In {nameof(TaxonomicRankTypeController)}.{nameof(GetAll)}");
            try
            {
                DAL.TaxonomicRankTypeDal taxonomicRankTypeDal = new(System.Configuration.ConfigurationManager.ConnectionStrings["BloggingDatabase"].ConnectionString);
                return taxonomicRankTypeDal.GetAll();
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error in {nameof(GetAll)}");
                throw;
            }
            //return Json(JsonSerializer.Serialize(trt));
        }

        public IEnumerable<TaxonomicRankType> GetAllTaxonomicRankType()
        {
            DAL.TaxonomicRankTypeDal taxonomicRankTypeDal = new(System.Configuration.ConfigurationManager.ConnectionStrings["BloggingDatabase"].ConnectionString);
            return taxonomicRankTypeDal.GetAll();
        }
    }
}