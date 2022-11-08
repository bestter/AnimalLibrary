using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;

namespace AnimalLibrary
{
    /// <summary>
    /// Specie
    /// </summary>
    public class Specie
    {
        public Specie([NotNull] int id,[NotNull] string name, string latinName, string description, TaxonomicRank parentTaxonomicRank)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Id = id;
            _name = name;
            LatinName = latinName;
            Description = description;
            ParentTaxonomicRank = parentTaxonomicRank;
        }

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        private string _name;

        /// <summary>
        /// Name
        /// </summary>
        public string Name { 
            get { return _name; }
            set { 
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(nameof(value));
                }
                _name = value; }
        }

        /// <summary>
        /// Latin name
        /// </summary>
        public string LatinName { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Taxonomix rank
        /// </summary>
        /// <remarks>Must be a specie</remarks>
        public TaxonomicRank ParentTaxonomicRank { get; }


    }
}
