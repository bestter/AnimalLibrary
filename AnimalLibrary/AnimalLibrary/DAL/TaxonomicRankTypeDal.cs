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

        public IEnumerable<TaxonomicRankType> GetAll()
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
            connection.Open();
            using SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {                    
                    taxonomicRankTypes.Add(GetFromSqlDataReader(reader));
                }
            }
            else
            {
                Log.Warning("No TaxonomicRankType found!");
            }
            return taxonomicRankTypes;
        }

        public TaxonomicRankType? Get(int id)
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
                connection.Open();
                using SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        taxonomicRankType = GetFromSqlDataReader(reader);
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

            using (SqlCommand command = new(
                queryString, connection))
            {
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
        }


        private static TaxonomicRankType GetFromSqlDataReader(SqlDataReader reader)
        {
            if (reader.IsClosed)
                throw new NotSupportedException($"{nameof(SqlDataReader)} {nameof(reader)} is closed");

            var taxonomicRankTypeID = reader.GetInt32(reader.GetOrdinal("TaxonomicRankTypeID"));
            var name = reader.GetString(reader.GetOrdinal("Name"));
            var nameFr = reader.GetString(reader.GetOrdinal("NameFr"));
            var ordParentTaxonomicRankTypeID = reader.GetOrdinal("ParentTaxonomicRankTypeID");
            int? parentTaxonomicRankTypeID;
            if (!reader.IsDBNull(ordParentTaxonomicRankTypeID))
            {
                parentTaxonomicRankTypeID = reader.GetInt32(ordParentTaxonomicRankTypeID);
            }
            else
            {
                parentTaxonomicRankTypeID = null;
            }

            TaxonomicRankType taxonomicRankType = new()
            { TaxonomicRankTypeID = taxonomicRankTypeID, Name = name, NameFr = nameFr, ParentTaxonomicRankTypeID = parentTaxonomicRankTypeID };
            return taxonomicRankType;
        }
    }
}
