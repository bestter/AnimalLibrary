using Microsoft.Data.SqlClient;
using Serilog;
using Microsoft.Extensions.Configuration;

namespace AnimalLibrary.DAL
{
    public class TaxonomicRankDal
    {
        public string ConnectionString { get; set; }

        private readonly List<TaxonomicRankType> taxonomicRankTypes;

        public TaxonomicRankDal(string connectionString)
        {
            ConnectionString = connectionString;
            Log.Debug($"{nameof(connectionString)}: {connectionString}");
            taxonomicRankTypes = GetTaxonomicRankTypes();
        }

        private List<TaxonomicRankType> GetTaxonomicRankTypes()
        {
            TaxonomicRankTypeDal taxonomicRankTypeDal = new(ConnectionString);
            return taxonomicRankTypeDal.GetAll();
        }

        public async Task<IEnumerable<TaxonomicRank>> GetAllAsync(CancellationToken cancellationToken)
        {
            const string queryString = @"SELECT 
                [TaxonomicRankID]
                ,[Name]
                ,[TaxonomicRankTypeID]
                ,[ParentTaxonomicRankID]
                FROM [dbo].[TaxonomicRank]
                ORDER BY TaxonomicRankID";

            using SqlConnection connection = new(
                       ConnectionString);
            List<TaxonomicRank> taxonomicRanks = new();

            SqlCommand command = new(
                queryString, connection);
            await connection.OpenAsync(cancellationToken);
            using SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);
            if (reader.HasRows)
            {
                var posTaxonomicRankID = reader.GetOrdinal("TaxonomicRankID");
                var posName = reader.GetOrdinal("Name");
                var posTaxonomicRankTypeID = reader.GetOrdinal("TaxonomicRankTypeID");
                var posParentTaxonomicRankID = reader.GetOrdinal("ParentTaxonomicRankID");

                while (await reader.ReadAsync(cancellationToken))
                {
                    var taxonomicRankID = await reader.GetFieldValueAsync<int>(posTaxonomicRankID, cancellationToken);
                    var name = await reader.GetFieldValueAsync<string>(posName, cancellationToken);
                    var taxonomicRankTypeID = await reader.GetFieldValueAsync<int>(posTaxonomicRankTypeID, cancellationToken);

                    int? parentTaxonomicRankID;                    
                    if (! await reader.IsDBNullAsync(posParentTaxonomicRankID, cancellationToken))
                    {
                        parentTaxonomicRankID = await reader.GetFieldValueAsync<int>(posParentTaxonomicRankID, cancellationToken);
                    }
                    else
                    {
                        parentTaxonomicRankID = null;
                    }

                    var taxonomicRankType = taxonomicRankTypes?.FirstOrDefault(t => t.TaxonomicRankTypeID == taxonomicRankTypeID);

                    TaxonomicRank taxonomicRank = new(taxonomicRankID, name, taxonomicRankType?.TaxonomicRankTypeID ?? 0, parentTaxonomicRankID);
                    taxonomicRanks.Add(taxonomicRank);
                }
            }
            else
            {
                Log.Warning("No TaxonomicRank found!");
            }
            return taxonomicRanks;
        }

        public TaxonomicRank? Get(int id)
        {
            TaxonomicRank? taxonomicRank = null;
            const string queryString = @"SELECT 
                [TaxonomicRankID]
                ,[Name]
                ,[TaxonomicRankTypeID]
                ,[ParentTaxonomicRankID]
                FROM [dbo].[TaxonomicRank]
                WHERE TaxonomicRankID = @TaxonomicRankID";

            using SqlConnection connection = new(
                       ConnectionString);
            List<TaxonomicRank> taxonomicRankTypes = new();

            using (SqlCommand command = new(
                queryString, connection))
            {
                connection.Open();
                using SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        taxonomicRank = GetFromSqlDataReader(reader);
                    }
                }
                else
                {
                    Log.Warning($"No TaxonomicRank with {nameof(id)} {id} found!");
                }
            }
            return taxonomicRank;
        }

        public async Task<bool> UpdateAsync(TaxonomicRank taxonomicRank)
        {
            if (taxonomicRank is null)
            {
                throw new ArgumentNullException(nameof(taxonomicRank));
            }
            if (taxonomicRank.TaxonomicRankID <= 0)
            {
                throw new ArgumentException($"{nameof(taxonomicRank.TaxonomicRankID)} must be provided and higher than zero", nameof(taxonomicRank));
            }

            var returnValue = false;

            const string queryString = @"
            UPDATE [dbo].[TaxonomicRank]
                SET [Name] = @Name
                ,[TaxonomicRankTypeID] = @TaxonomicRankTypeID
                ,[ParentTaxonomicRankID] = @ParentTaxonomicRankID
            WHERE [TaxonomicRankID]  = @TaxonomicRankID";
            using SqlConnection connection = new(
                       ConnectionString);
            await connection.OpenAsync();
            SqlTransaction transaction;
            using (transaction = connection.BeginTransaction())
            {
                try
                {
                    List<TaxonomicRankType> taxonomicRankTypes = new();

                    using SqlCommand command = new(
                        queryString, connection);


                    command.Parameters.Add("@TaxonomicRankID", System.Data.SqlDbType.Int).Value = taxonomicRank.TaxonomicRankID;
                    command.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar, 256).Value = taxonomicRank.Name;


                    var paramTaxonomicRankTypeID = new SqlParameter("@TaxonomicRankTypeID", System.Data.SqlDbType.Int);
                    if (taxonomicRank.TaxonomicRankTypeId != null)
                    {
                        paramTaxonomicRankTypeID.Value = taxonomicRank.TaxonomicRankTypeId;
                    }
                    else
                    {
                        paramTaxonomicRankTypeID.SqlValue = DBNull.Value;
                    }
                    command.Parameters.Add(paramTaxonomicRankTypeID);

                    var paramParentTaxonomicRankID = new SqlParameter("@ParentTaxonomicRankID", System.Data.SqlDbType.Int);
                    if (taxonomicRank.ParentTaxonomicRankID.HasValue)
                    {
                        paramParentTaxonomicRankID.Value = taxonomicRank.ParentTaxonomicRankID.Value;
                    }
                    else
                    {
                        paramParentTaxonomicRankID.SqlValue = DBNull.Value;
                    }
                    command.Parameters.Add(paramParentTaxonomicRankID);

                    returnValue = await command.ExecuteNonQueryAsync() > 0;
                    await transaction.CommitAsync();
                }
                catch
                {
                    returnValue = false;
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            return returnValue;
        }

        public async Task<int> InsertAsync(TaxonomicRank taxonomicRank)
        {
            if (taxonomicRank is null)
            {
                throw new ArgumentNullException(nameof(taxonomicRank));
            }
            int taxonomicRankId = 0;

            using SqlConnection connection = new(
                       ConnectionString);
            await connection.OpenAsync();
            SqlTransaction transaction;
            using (transaction = connection.BeginTransaction())
            {
                try
                {

                    const string insertString = @"
            INSERT INTO [dbo].[TaxonomicRank]
                ([Name]
                ,[TaxonomicRankTypeID]
                ,[ParentTaxonomicRankID])
            VALUES
                (@Name
                ,@TaxonomicRankTypeID
                ,@ParentTaxonomicRankID);
            SELECT CAST(SCOPE_IDENTITY() AS INT) AS [TaxonomicRankID];";
                    using SqlCommand command = new(
                        insertString, connection);


                    command.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar, 256).Value = taxonomicRank.Name;
                    var paramTaxonomicRankTypeID = new SqlParameter("@TaxonomicRankTypeID", System.Data.SqlDbType.Int);
                    if (taxonomicRank.TaxonomicRankTypeId != null)
                    {
                        paramTaxonomicRankTypeID.Value = taxonomicRank.TaxonomicRankTypeId;
                    }
                    else
                    {
                        paramTaxonomicRankTypeID.SqlValue = DBNull.Value;
                    }
                    command.Parameters.Add(paramTaxonomicRankTypeID);

                    var paramParentTaxonomicRankID = new SqlParameter("@ParentTaxonomicRankID", System.Data.SqlDbType.Int);
                    if (taxonomicRank.ParentTaxonomicRankID.HasValue && taxonomicRank.ParentTaxonomicRankID.Value > 0)
                    {
                        paramParentTaxonomicRankID.Value = taxonomicRank.ParentTaxonomicRankID.Value;
                    }
                    else
                    {
                        paramParentTaxonomicRankID.SqlValue = DBNull.Value;
                    }
                    command.Parameters.Add(paramParentTaxonomicRankID);

                    using SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        if (await reader.ReadAsync())
                        {
                            taxonomicRankId = await reader.GetFieldValueAsync<int>(reader.GetOrdinal("TaxonomicRankID"));
                        }
                    }
                    else
                    {
                        Log.Warning($"Cannot insert {nameof(TaxonomicRank)}");
                    }
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }

            return taxonomicRankId;
        }

        private TaxonomicRank GetFromSqlDataReader(SqlDataReader reader)
        {
            if (reader.IsClosed)
                throw new NotSupportedException($"{nameof(SqlDataReader)} {nameof(reader)} is closed");

            var taxonomicRankID = reader.GetInt32(reader.GetOrdinal("TaxonomicRankID"));
            var name = reader.GetString(reader.GetOrdinal("Name"));
            var taxonomicRankTypeID = reader.GetInt32(reader.GetOrdinal("TaxonomicRankTypeID"));

            int? parentTaxonomicRankID;
            var posParentTaxonomicRankID = reader.GetOrdinal("ParentTaxonomicRankID");
            if (!reader.IsDBNull(posParentTaxonomicRankID))
            {
                parentTaxonomicRankID = reader.GetInt32(posParentTaxonomicRankID);
            }
            else
            {
                parentTaxonomicRankID = null;
            }

            var taxonomicRankType = taxonomicRankTypes?.FirstOrDefault(t => t.TaxonomicRankTypeID == taxonomicRankTypeID);

            TaxonomicRank taxonomicRank = new(taxonomicRankID, name, taxonomicRankType?.TaxonomicRankTypeID ?? 0, parentTaxonomicRankID);
            return taxonomicRank;
        }
    }
}
