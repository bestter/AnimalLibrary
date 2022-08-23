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
        
        //[Route("GetAll")]
        //public IEnumerable<TaxonomicRankType> GetAll(int id)
        //{
        //    Log.Information($"In {nameof(ValuesController)}.{nameof(GetAll)}");
        //    try
        //    {
        //        DAL.TaxonomicRankTypeDal taxonomicRankTypeDal = new(ConnectionString);
        //        return taxonomicRankTypeDal.GetAll();
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, $"Error in {nameof(GetAll)}");
        //        throw;
        //    }
        //}
        
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

        [Route("GetAllTaxonomicRankAsync")]
        public async Task<IEnumerable<TaxonomicRank>> GetAllTaxonomicRankAsync(int id, CancellationToken cancellationToken)
        {
            Log.Information($"In {nameof(ValuesController)}.{nameof(GetAllTaxonomicRankAsync)}");
            try
            {
                DAL.TaxonomicRankDal taxonomicRankDal = await DAL.TaxonomicRankDal.CreateAsync(ConnectionString, cancellationToken);
                return await taxonomicRankDal.GetAllAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error in {nameof(GetAllTaxonomicRankAsync)}");
                throw;
            }
        }

        [Route("GetAllTaxonomicRankTypeAsync")]
        public async Task<IEnumerable<TaxonomicRankType>> GetAllTaxonomicRankTypeAsync(int id, CancellationToken cancellationToken)
        {
            Log.Information($"In {nameof(ValuesController)}.{nameof(GetAllTaxonomicRankTypeAsync)}");
            try
            {
                DAL.TaxonomicRankTypeDal taxonomicRankTypeDal = new(ConnectionString);
                return await taxonomicRankTypeDal.GetAllTaxonomicRankTypeAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error in {nameof(GetAllTaxonomicRankTypeAsync)}");
                throw;
            }
        }

        [Route("GetEmptyTaxonomicRank")]
        public TaxonomicRank GetEmptyTaxonomicRank()
        {
            TaxonomicRankType taxonomicRankType = new(1, "test", "test",null);
            return new TaxonomicRank(0, "Empty", taxonomicRankType.TaxonomicRankTypeID, 0);
        }

        [HttpPost]
        [Route("UpdateTaxonomicRankAsync")]
        public async Task<bool> UpdateTaxonomicRankAsync(TaxonomicRank taxonomicRank, CancellationToken cancellationToken)
        {
            try
            {
                DAL.TaxonomicRankDal taxonomicRankDal = await DAL.TaxonomicRankDal.CreateAsync(ConnectionString, cancellationToken);
                   return await taxonomicRankDal.UpdateAsync(taxonomicRank, cancellationToken);                
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error in {nameof(UpdateTaxonomicRankAsync)}");
                throw;
            }
        }

        [HttpPost]
        [Route("InsertTaxonomicRankAsync")]
        public async Task<bool> InsertTaxonomicRankAsync(TaxonomicRank taxonomicRank, CancellationToken cancellationToken)
        {
            try
            {
                DAL.TaxonomicRankDal taxonomicRankDal = await DAL.TaxonomicRankDal.CreateAsync(ConnectionString, cancellationToken);
                var id = await taxonomicRankDal.InsertAsync(taxonomicRank, cancellationToken);
                return id>0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error in {nameof(InsertTaxonomicRankAsync)}");
                throw;
            }
        }
        #endregion
    }
}
