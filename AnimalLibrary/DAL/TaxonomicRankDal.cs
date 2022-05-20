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
            taxonomicRankTypeDal.GetAll();
            return taxonomicRankTypes;
        }

        public IEnumerable<TaxonomicRank> GetAll()
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
            connection.Open();
            using SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    taxonomicRanks.Add(GetFromSqlDataReader(reader));
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

        public void Update(TaxonomicRank taxonomicRank)
        {
            if (taxonomicRank is null)
            {
                throw new ArgumentNullException(nameof(taxonomicRank));
            }

            const string queryString = @"
            UPDATE [dbo].[TaxonomicRank]
                SET [Name] = @Name
                ,[TaxonomicRankTypeID] = @TaxonomicRankTypeID
                ,[ParentTaxonomicRankID] = @ParentTaxonomicRankID
            WHERE [TaxonomicRankID]  = @TaxonomicRankID";
            using SqlConnection connection = new(
                       ConnectionString);
            List<TaxonomicRankType> taxonomicRankTypes = new();

            using SqlCommand command = new(
                queryString, connection);
            connection.Open();
            command.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar, 256).Value = taxonomicRank.Name;

            var paramTaxonomicRankTypeID = new SqlParameter("@TaxonomicRankTypeID", System.Data.SqlDbType.Int);
            if (taxonomicRank.TaxonomicRankType != null)
            {
                paramTaxonomicRankTypeID.Value = taxonomicRank.TaxonomicRankType.TaxonomicRankTypeID;
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

            command.Parameters.Add("@TaxonomicRankID", System.Data.SqlDbType.Int).Value = taxonomicRank.TaxonomicRankID;
            command.ExecuteNonQuery();
        }

        public int Insert(TaxonomicRank taxonomicRank)
        {
            if (taxonomicRank is null)
            {
                throw new ArgumentNullException(nameof(taxonomicRank));
            }
            int taxonomicRankId = 0;

            using SqlConnection connection = new(
                       ConnectionString);
            const string insertString = @"
            INSERT INTO [dbo].[TaxonomicRank]
                ([Name]
                ,[TaxonomicRankTypeID]
                ,[ParentTaxonomicRankID])
            VALUES
                (@Name
                ,@TaxonomicRankTypeID
                ,@ParentTaxonomicRankID)
            SELECT SCOPE_IDENTITY() [TaxonomicRankID];";
            using SqlCommand command = new(
                insertString, connection);
            connection.Open();

            command.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar, 256).Value = taxonomicRank.Name;
            var paramTaxonomicRankTypeID = new SqlParameter("@TaxonomicRankTypeID", System.Data.SqlDbType.Int);
            if (taxonomicRank.TaxonomicRankType?.TaxonomicRankTypeID != null)
            {
                paramTaxonomicRankTypeID.Value = taxonomicRank.TaxonomicRankType.TaxonomicRankTypeID;
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
            using SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                if (reader.Read())
                {
                    taxonomicRankId = reader.GetInt32(reader.GetOrdinal("TaxonomicRankID"));
                }
            }
            else
            {
                Log.Warning($"Cannot insert {nameof(TaxonomicRank)}");
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

            var taxonomicRankType = taxonomicRankTypes.FirstOrDefault(t => t.TaxonomicRankTypeID == taxonomicRankTypeID);

            TaxonomicRank taxonomicRank = new(taxonomicRankID, name, taxonomicRankType, parentTaxonomicRankID);
            return taxonomicRank;
        }
    }
}
