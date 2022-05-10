using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace AnimalLibrary.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ValuesController : ControllerBase
    {
        /// <summary>
        /// Connection string to blogging
        /// </summary>
        private string ConnectionString { get; set; }

        public ValuesController()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true);
            IConfigurationRoot root = configurationBuilder.Build();
            ConnectionString = root.GetConnectionString("AnimalDirectoryDatabase");
            Log.Logger.Verbose($"{nameof(ConnectionString)}: {ConnectionString}");
        }

        [HttpGet]
        public TaxonomicRankType Get()
        {
            Log.Information($"In {nameof(ValuesController)}.{nameof(Get)}");
            try
            {
                TaxonomicRankType taxonomicRankType = new() { Name = "test", NameFr = "1", ParentTaxonomicRankTypeID = 1, TaxonomicRankTypeID = 2 };
                return taxonomicRankType;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error in {nameof(Get)}");
                throw;
            }
        }
        
        [Route("GetAll")]
        public IEnumerable<TaxonomicRankType> GetAll(int id)
        {
            Log.Information($"In {nameof(ValuesController)}.{nameof(GetAll)}");
            try
            {
                DAL.TaxonomicRankTypeDal taxonomicRankTypeDal = new(ConnectionString);
                return taxonomicRankTypeDal.GetAll();
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error in {nameof(GetAll)}");
                throw;
            }
        }

        [HttpPost]
        [Route("UpdateTaxonomicRankType")]
        public bool UpdateTaxonomicRankType(TaxonomicRankType taxonomicRankType)
        {
            try
            {
                DAL.TaxonomicRankTypeDal taxonomicRankTypeDal = new(ConnectionString);
                taxonomicRankTypeDal.Update(taxonomicRankType);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error in {nameof(UpdateTaxonomicRankType)}");
                throw;
            }
        }
    }
}
