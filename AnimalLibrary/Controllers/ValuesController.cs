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

        #region TaxonomicRankType
        [HttpGet]
        public TaxonomicRankType Get()
        {
            Log.Information($"In {nameof(ValuesController)}.{nameof(Get)}");
            try
            {
                TaxonomicRankType taxonomicRankType = new(1, "test", "test", 0);
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
        #endregion

        #region TaxonomicRank
        [HttpGet("GetTaxonomicRank")]
        [Route("GetTaxonomicRank")]
        public TaxonomicRank GetTaxonomicRank()
        {
            Log.Information($"In {nameof(ValuesController)}.{nameof(GetTaxonomicRank)}");
            try
            {
                TaxonomicRank taxonomicRank = new(-1,"test", null, null);
                return taxonomicRank;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error in {nameof(GetTaxonomicRank)}");
                throw;
            }
        }

        [Route("GetAllTaxonomicRank")]
        public IEnumerable<TaxonomicRank> GetAllTaxonomicRank(int id)
        {
            Log.Information($"In {nameof(ValuesController)}.{nameof(GetAll)}");
            try
            {
                DAL.TaxonomicRankDal taxonomicRankDal = new(ConnectionString);
                return taxonomicRankDal.GetAll();
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error in {nameof(GetAllTaxonomicRank)}");
                throw;
            }
        }

        [Route("GetEmptyTaxonomicRank")]
        public TaxonomicRank GetEmptyTaxonomicRank()
        {
            TaxonomicRankType taxonomicRankType = new(1, "test", "test",null);
            return new TaxonomicRank(0, "Empty", taxonomicRankType, 0);
        }

        [HttpPost]
        [Route("UpdateTaxonomicRank")]
        public bool UpdateTaxonomicRank(TaxonomicRank taxonomicRank)
        {
            try
            {
                DAL.TaxonomicRankDal taxonomicRankDal = new(ConnectionString);
                taxonomicRankDal.Update(taxonomicRank);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error in {nameof(UpdateTaxonomicRank)}");
                throw;
            }
        }

        [HttpPost]
        [Route("InsertTaxonomicRank")]
        public bool InsertTaxonomicRank(TaxonomicRank taxonomicRank)
        {
            try
            {
                DAL.TaxonomicRankDal taxonomicRankDal = new(ConnectionString);
                var id = taxonomicRankDal.Insert(taxonomicRank);
                return id>0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error in {nameof(UpdateTaxonomicRank)}");
                throw;
            }
        }
        #endregion
    }
}
