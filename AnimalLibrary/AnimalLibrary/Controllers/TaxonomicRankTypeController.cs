using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using System.Text.Json;
using Serilog;
using Serilog.Exceptions;

namespace AnimalLibrary.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaxonomicRankTypeController : ControllerBase
    {

        [HttpGet]
        public IEnumerable<TaxonomicRankType> Get()
        {
            Log.Information($"In {nameof(TaxonomicRankTypeController)}.{nameof(Get)}");
            try
            {
                DAL.TaxonomicRankTypeDal taxonomicRankTypeDal = new(System.Configuration.ConfigurationManager.ConnectionStrings["BloggingDatabase"].ConnectionString);
                return taxonomicRankTypeDal.GetAll();
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error in {nameof(Get)}");
                throw;
            }
            //return Json(JsonSerializer.Serialize(trt));
        }

        [HttpGet("GetAll/id={id}")]
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

        [HttpPost]
        public void UpdateTaxonomicRankType(TaxonomicRankType taxonomicRankType)
        {
            try
            {
                DAL.TaxonomicRankTypeDal taxonomicRankTypeDal = new(System.Configuration.ConfigurationManager.ConnectionStrings["BloggingDatabase"].ConnectionString);
                taxonomicRankTypeDal.Update(taxonomicRankType);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error in {nameof(UpdateTaxonomicRankType)}");
                throw;
            }
        }


    }
}
