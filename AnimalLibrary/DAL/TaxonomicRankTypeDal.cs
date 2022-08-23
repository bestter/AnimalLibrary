using Microsoft.Data.SqlClient;
using Serilog;
using Microsoft.Extensions.Configuration;

namespace AnimalLibrary.DAL
{
    public class TaxonomicRankTypeDal
    {
        public string ConnectionString { get; set; }

        public TaxonomicRankTypeDal(string connectionString)
        {
            ConnectionString = connectionString;
            Log.Debug($"{nameof(connectionString)}: {connectionString}");
        }

        public async Task<List<TaxonomicRankType>> GetAllTaxonomicRankTypeAsync(CancellationToken cancellationToken)
        {            
            const string queryString = @"SELECT [TaxonomicRankTypeID] 
                ,[Name]
                ,[NameFr]
                ,[ParentTaxonomicRankTypeID]
                FROM [AnimalDirectoy].[dbo].[TaxonomicRankType]
                ORDER BY ISNULL([ParentTaxonomicRankTypeID], 0), [TaxonomicRankTypeID]";

            using SqlConnection connection = new(
                       ConnectionString);
            List<TaxonomicRankType> taxonomicRankTypes = new();

            SqlCommand command = new(
                queryString, connection);
            await connection.OpenAsync(cancellationToken);
            using SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);
            if (reader.HasRows)
            {
                while (await reader.ReadAsync(cancellationToken))
                {                    
                    taxonomicRankTypes.Add(await GetFromSqlDataReaderAsync(reader, cancellationToken));
                }
            }
            else
            {
                Log.Warning("No TaxonomicRankType found!");
            }
            return taxonomicRankTypes;
        }

        public async Task<TaxonomicRankType?> GetAsync(int id, CancellationToken cancellationToken)
        {
            TaxonomicRankType? taxonomicRankType = null;
            const string queryString = @"SELECT [TaxonomicRankTypeID] 
                ,[Name]
                ,[NameFr]
                ,[ParentTaxonomicRankTypeID]
                FROM [AnimalDirectoy].[dbo].[TaxonomicRankType]
                WHERE TaxonomicRankTypeID = @TaxonomicRankTypeID";

            using SqlConnection connection = new(
                       ConnectionString);
            List<TaxonomicRankType> taxonomicRankTypes = new();

            using (SqlCommand command = new(
                queryString, connection))
            {
                await connection.OpenAsync(cancellationToken);
                using SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync(cancellationToken))
                    {
                        taxonomicRankType = await GetFromSqlDataReaderAsync(reader, cancellationToken);
                    }
                }
                else
                {
                    Log.Warning($"No TaxonomicRankType with {nameof(id)} {id} found!");
                }
            }
            return taxonomicRankType;
        }

        public void Update(TaxonomicRankType taxonomicRankType)
        {
            if (taxonomicRankType is null)
            {
                throw new ArgumentNullException(nameof(taxonomicRankType));
            }

            const string queryString = @"UPDATE [dbo].[TaxonomicRankType]
                   SET [Name] = @Name
                      ,[NameFr] = @NameFr
                      ,[ParentTaxonomicRankTypeID] = @ParentTaxonomicRankTypeID
                 WHERE [TaxonomicRankTypeID] = @TaxonomicRankTypeID;";
            using SqlConnection connection = new(
                       ConnectionString);
            List<TaxonomicRankType> taxonomicRankTypes = new();

            using SqlCommand command = new(
                queryString, connection);
            connection.Open();
            command.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar, 256).Value = taxonomicRankType.Name;
            command.Parameters.Add("@NameFr", System.Data.SqlDbType.NVarChar, 256).Value = taxonomicRankType.NameFr;

            var paramParentTaxonomicRankTypeID = new SqlParameter("@ParentTaxonomicRankTypeID", System.Data.SqlDbType.Int);
            if (taxonomicRankType.ParentTaxonomicRankTypeID.HasValue)
            {
                paramParentTaxonomicRankTypeID.Value = taxonomicRankType.ParentTaxonomicRankTypeID.Value;
            }
            else
            {
                paramParentTaxonomicRankTypeID.SqlValue = DBNull.Value;
            }

            command.Parameters.Add(paramParentTaxonomicRankTypeID);
            command.Parameters.Add("@TaxonomicRankTypeID", System.Data.SqlDbType.Int).Value = taxonomicRankType.TaxonomicRankTypeID;
            command.ExecuteNonQuery();
        }


        private static async Task<TaxonomicRankType> GetFromSqlDataReaderAsync(SqlDataReader reader, CancellationToken cancellationToken)
        {
            if (reader.IsClosed)
                throw new NotSupportedException($"{nameof(SqlDataReader)} {nameof(reader)} is closed");

            var taxonomicRankTypeID = await reader.GetFieldValueAsync<int>(reader.GetOrdinal("TaxonomicRankTypeID"), cancellationToken);
            var name = await reader.GetFieldValueAsync<string>(reader.GetOrdinal("Name"), cancellationToken);
            var nameFr = await reader.GetFieldValueAsync<string>(reader.GetOrdinal("NameFr"), cancellationToken);
            var ordParentTaxonomicRankTypeID = reader.GetOrdinal("ParentTaxonomicRankTypeID");
            int? parentTaxonomicRankTypeID;
            if (! await reader.IsDBNullAsync(ordParentTaxonomicRankTypeID, cancellationToken))
            {
                parentTaxonomicRankTypeID = await reader.GetFieldValueAsync<int>(ordParentTaxonomicRankTypeID);
            }
            else
            {
                parentTaxonomicRankTypeID = null;
            }

            TaxonomicRankType taxonomicRankType = new (taxonomicRankTypeID, name, nameFr, parentTaxonomicRankTypeID);
            return taxonomicRankType;
        }
    }
}
