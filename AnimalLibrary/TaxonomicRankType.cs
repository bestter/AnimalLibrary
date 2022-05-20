using System.Diagnostics.CodeAnalysis;

namespace AnimalLibrary
{
    public class TaxonomicRankType : IEquatable<TaxonomicRankType?>
    {
        public TaxonomicRankType(int taxonomicRankTypeID, string? name, string? nameFr, int? parentTaxonomicRankTypeID)
        {
            TaxonomicRankTypeID = taxonomicRankTypeID;
            Name = name;
            NameFr = nameFr;
            ParentTaxonomicRankTypeID = parentTaxonomicRankTypeID;
        }

        #region properties
        [NotNull]
        public int TaxonomicRankTypeID { get; set; }

        public string? Name { get; set; }

        public string? NameFr { get; set; } 

        public int? ParentTaxonomicRankTypeID { get; set; }
        #endregion

        public override bool Equals(object? obj)
        {
            return Equals(obj as TaxonomicRankType);
        }

        public bool Equals(TaxonomicRankType? other)
        {
            return other is not null &&
                   TaxonomicRankTypeID == other.TaxonomicRankTypeID;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TaxonomicRankTypeID);
        }

        public static bool operator ==(TaxonomicRankType? left, TaxonomicRankType? right)
        {
            return EqualityComparer<TaxonomicRankType>.Default.Equals(left, right);
        }

        public static bool operator !=(TaxonomicRankType? left, TaxonomicRankType? right)
        {
            return !(left == right);
        }
    }
}
