using System.Diagnostics.CodeAnalysis;

namespace AnimalLibrary
{
    public class TaxonomicRank : IEquatable<TaxonomicRank?>, IComparable, IComparable<TaxonomicRank>
    {
        public TaxonomicRank(int taxonomicRankID, string name, int? taxonomicRankTypeId, int? parentTaxonomicRankID)
        {
            TaxonomicRankID = taxonomicRankID;
            Name = name;
            TaxonomicRankTypeId = taxonomicRankTypeId;
            ParentTaxonomicRankID = parentTaxonomicRankID;
        }
        #region properties
        [NotNull]
        public int TaxonomicRankID { get; }
        
        [NotNull]
        public string Name { get; set; }

        public int? TaxonomicRankTypeId { get; set; }

        public int? ParentTaxonomicRankID { get; set; }
        #endregion

        public int CompareTo(object? obj)
        {
            TaxonomicRank? other = obj as TaxonomicRank;
            if (other == null) return 1;
            return TaxonomicRankID.CompareTo(other.TaxonomicRankID);
        }

        public int CompareTo(TaxonomicRank? other)
        {
            if (other == null) return 1;
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(this, this)) return 1;
            return TaxonomicRankID.CompareTo(other.TaxonomicRankID);            
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as TaxonomicRank);
        }

        public bool Equals(TaxonomicRank? other)
        {
            return other is not null &&
                   TaxonomicRankID == other.TaxonomicRankID;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TaxonomicRankID);
        }

        public static bool operator ==(TaxonomicRank? left, TaxonomicRank? right)
        {
            return EqualityComparer<TaxonomicRank>.Default.Equals(left, right);
        }

        public static bool operator !=(TaxonomicRank? left, TaxonomicRank? right)
        {
            return !(left == right);
        }

        public static bool operator <(TaxonomicRank left, TaxonomicRank right)
        {
            return ReferenceEquals(left, null) ? !ReferenceEquals(right, null) : left.CompareTo(right) < 0;
        }

        public static bool operator <=(TaxonomicRank left, TaxonomicRank right)
        {
            return ReferenceEquals(left, null) || left.CompareTo(right) <= 0;
        }

        public static bool operator >(TaxonomicRank left, TaxonomicRank right)
        {
            return !ReferenceEquals(left, null) && left.CompareTo(right) > 0;
        }

        public static bool operator >=(TaxonomicRank left, TaxonomicRank right)
        {
            return ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.CompareTo(right) >= 0;
        }
    }

    
}
