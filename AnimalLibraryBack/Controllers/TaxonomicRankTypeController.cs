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
        public async Task<IEnumerable<TaxonomicRankType>> GetAsync(CancellationToken cancellationToken)
        {
            Log.Information($"In {nameof(TaxonomicRankTypeController)}.{nameof(GetAsync)}");
            try
            {
                DAL.TaxonomicRankTypeDal taxonomicRankTypeDal = new(System.Configuration.ConfigurationManager.ConnectionStrings["BloggingDatabase"].ConnectionString);
                return await taxonomicRankTypeDal.GetAllTaxonomicRankTypeAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error in {nameof(GetAsync)}");
                throw;
            }
            //return Json(JsonSerializer.Serialize(trt));
        }

        [HttpGet("GetAllAsync/id={id}")]
        public async Task<IEnumerable<TaxonomicRankType>> GetAllAsync(int id, CancellationToken cancellationToken)
        {
            Log.Information($"In {nameof(TaxonomicRankTypeController)}.{nameof(GetAllAsync)}");
            try
            {
                DAL.TaxonomicRankTypeDal taxonomicRankTypeDal = new(System.Configuration.ConfigurationManager.ConnectionStrings["BloggingDatabase"].ConnectionString);
                return await taxonomicRankTypeDal.GetAllTaxonomicRankTypeAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error in {nameof(GetAllAsync)}");
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
