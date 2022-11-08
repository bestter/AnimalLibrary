using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using Serilog;

namespace AnimalLibrary.DAL
{
    public class SpecieDal
    {
        public string ConnectionString { get; set; }

        /// <summary>
        /// Specie's taxonomic rank
        /// </summary>
        private List<TaxonomicRank> TaxonomicRanks;

        private SpecieDal(string connectionString)
        {
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            Log.Debug($"{nameof(connectionString)}: {connectionString}");
            TaxonomicRanks = new List<TaxonomicRank>();
        }

        private async Task<SpecieDal> InitializeAsync(CancellationToken cancellationToken)
        {
            var taxonomicRanks = await GetTaxonomicRankAsync(cancellationToken);
            TaxonomicRanks = taxonomicRanks.ToList();
            return this;
        }

        public async static Task<SpecieDal> CreateAsync(string connectionString, CancellationToken cancellationToken)
        {
            var ret = new SpecieDal(connectionString);
            return await ret.InitializeAsync(cancellationToken);
        }

        private async Task<IEnumerable<TaxonomicRank>> GetTaxonomicRankAsync(CancellationToken cancellationToken)
        {
            TaxonomicRankDal taxonomicRankDal = await TaxonomicRankDal.CreateAsync(ConnectionString, cancellationToken);
            return await taxonomicRankDal.GetAllAsync(cancellationToken);
        }

        public async Task<List<Specie>> GetSpeciesAsync(CancellationToken cancellationToken)
        {
            List<Specie> species = new ();

            const string sql = @"SELECT [SpecieId]
            ,[Name]
            ,[LatinName]
            ,[Description]
            ,[ParentTaxonomicRankID]
            FROM [AnimalDirectoy].[dbo].[Specie]
            ORDER BY [Name], [SpecieId]";

            using SqlConnection connection = new(
                       ConnectionString);
            SqlCommand command = new(
                sql, connection);
            await connection.OpenAsync(cancellationToken);
            using SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);
            if (reader.HasRows)
            {
                while (await reader.ReadAsync(cancellationToken))
                {
                    var id = await reader.GetFieldValueAsync<int>(reader.GetOrdinal("SpecieId"), cancellationToken);

                    var name = await reader.GetFieldValueAsync<string>(reader.GetOrdinal("Name"), cancellationToken);

                    var latinName = await reader.GetFieldValueAsync<string>(reader.GetOrdinal("LatinName"), cancellationToken);

                    string? description = null;
                    var descriptionPos = reader.GetOrdinal("Description");
                    if (!await reader.IsDBNullAsync(descriptionPos, cancellationToken))
                    {
                        description = await reader.GetFieldValueAsync<string>(descriptionPos, cancellationToken);
                    }

                    var parentTaxonomicRankID = await reader.GetFieldValueAsync<int>(reader.GetOrdinal("ParentTaxonomicRankID"), cancellationToken);

                    TaxonomicRank parentTaxonomicRank = TaxonomicRanks.First(t => t.TaxonomicRankID == parentTaxonomicRankID);

                    Specie specie = new(id, name, latinName, description ?? string.Empty, parentTaxonomicRank);
                    species.Add(specie);
                }
            }
            else
            {
                Log.Warning("No TaxonomicRankType found!");
            }

            return species;
        }

        public async Task<int> UpdateAsync(Specie specie, CancellationToken cancellationToken)
        {
            if (specie is null)
            {
                throw new ArgumentNullException(nameof(specie));
            }
            int specieId = 0;

            using SqlConnection connection = new(
                       ConnectionString);
            await connection.OpenAsync(cancellationToken);
            SqlTransaction transaction;
            using (transaction = connection.BeginTransaction())
            {
                try
                {

                    const string insertString = @"
                UPDATE [dbo].[Specie]
                    SET [Name] = @Name
                        ,[LatinName] = @LatinName
                        ,[Description] = @Description
                        ,[ParentTaxonomicRankID] = @ParentTaxonomicRankID
                        WHERE [SpecieId] = @Id;
                SELECT CAST(SCOPE_IDENTITY() AS INT) AS [SpecieID];";
                    using SqlCommand command = new(
                        insertString, connection, transaction);

                    command.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar, 50).Value = specie.Name;
                    
                    var paramLatinName = new SqlParameter("@LatinName", System.Data.SqlDbType.NVarChar, 100);
                    if (specie.LatinName != null && !string.IsNullOrWhiteSpace(specie.LatinName))
                    {
                        paramLatinName.Value = specie.LatinName.Trim();
                    }
                    else
                    {
                        paramLatinName.SqlValue = DBNull.Value;
                    }
                    command.Parameters.Add(paramLatinName);

                    var paramDescription = new SqlParameter("@Description", System.Data.SqlDbType.NVarChar, -1);
                    if (specie.Description != null)
                    {
                        paramDescription.Value = specie.Description;
                    }
                    else
                    {
                        paramDescription.SqlValue = DBNull.Value;
                    }
                    command.Parameters.Add(paramDescription);

                    var paramParentTaxonomicRankID = new SqlParameter("@ParentTaxonomicRankID", System.Data.SqlDbType.Int);
                    if (specie.ParentTaxonomicRank?.TaxonomicRankID > 0)
                    {
                        paramParentTaxonomicRankID.Value = specie.ParentTaxonomicRank?.TaxonomicRankID;
                    }
                    else
                    {
                        paramParentTaxonomicRankID.SqlValue = DBNull.Value;
                    }
                    command.Parameters.Add(paramParentTaxonomicRankID);

                    command.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = specie.Id;

                    using (SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken))
                    {
                        if (reader.HasRows)
                        {
                            if (await reader.ReadAsync(cancellationToken))
                            {
                                specieId = await reader.GetFieldValueAsync<int>(reader.GetOrdinal("SpecieID"), cancellationToken);
                            }
                        }
                        else
                        {
                            Log.Warning($"Cannot insert {nameof(Specie)}");
                        }
                    }
                    await transaction.CommitAsync(cancellationToken);

                }
                catch
                {
                    specieId = -1;
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }

            return specieId;
        }

        public async Task<int> InsertAsync(Specie specie, CancellationToken cancellationToken)
        {
            if (specie is null)
            {
                throw new ArgumentNullException(nameof(specie));
            }
            int specieId = 0;

            using SqlConnection connection = new(
                       ConnectionString);
            await connection.OpenAsync(cancellationToken);
            SqlTransaction transaction;
            using (transaction = connection.BeginTransaction())
            {
                try
                {

                    const string insertString = @"
            INSERT INTO [dbo].[Specie]
                ([Name]
                ,[LatinName]
                ,[Description]
                ,[ParentTaxonomicRankID])
            VALUES
            (@Name
            ,@LatinName
            ,@Description
            ,@ParentTaxonomicRankID);
            SELECT CAST(SCOPE_IDENTITY() AS INT) AS [SpecieID];";
                    using SqlCommand command = new(
                        insertString, connection, transaction);

                    command.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar, 50).Value = specie.Name.Trim();

                    var paramLatinName = new SqlParameter("@LatinName", System.Data.SqlDbType.NVarChar, 100);
                    if (specie.LatinName != null && !string.IsNullOrWhiteSpace(specie.LatinName))
                    {
                        paramLatinName.Value = specie.LatinName.Trim();
                    }
                    else
                    {
                        paramLatinName.SqlValue = DBNull.Value;
                    }
                    command.Parameters.Add(paramLatinName);

                    var paramDescription = new SqlParameter("@Description", System.Data.SqlDbType.NVarChar, -1);
                    if (specie.Description != null && !string.IsNullOrWhiteSpace(specie.Description))
                    {
                        paramDescription.Value = specie.Description.Trim();
                    }
                    else
                    {
                        paramDescription.SqlValue = DBNull.Value;
                    }
                    command.Parameters.Add(paramDescription);

                    var paramParentTaxonomicRankID = new SqlParameter("@ParentTaxonomicRankID", System.Data.SqlDbType.Int);
                    if (specie.ParentTaxonomicRank?.TaxonomicRankID != null)
                    {
                        paramParentTaxonomicRankID.Value = specie.ParentTaxonomicRank?.TaxonomicRankID;
                    }
                    else
                    {
                        paramParentTaxonomicRankID.SqlValue = DBNull.Value;
                    }
                    command.Parameters.Add(paramParentTaxonomicRankID);
                                        

                    using (SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken))
                    {
                        if (reader.HasRows)
                        {
                            if (await reader.ReadAsync(cancellationToken))
                            {
                                specieId = await reader.GetFieldValueAsync<int>(reader.GetOrdinal("SpecieID"), cancellationToken);
                            }
                        }
                        else
                        {
                            Log.Warning($"Cannot insert {nameof(Specie)}");
                        }
                    }
                    await transaction.CommitAsync(cancellationToken);

                }
                catch
                {
                    specieId = -1;
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }

            return specieId;
        }
    }
}
