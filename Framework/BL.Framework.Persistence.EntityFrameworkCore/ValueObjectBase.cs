using System.Collections.Generic;
using System.Linq;

namespace BL.Framework.Persistence.EntityFrameworkCore
{
	public abstract class ValueObjectBase
	{
		public int Id { get; set; }

		protected static bool EqualOperator(ValueObjectBase left, ValueObjectBase right)
		{
			if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
			{
				return false;
			}

			return ReferenceEquals(left, null) || left.Equals(right);
		}

		protected static bool NotEqualOperator(ValueObjectBase left, ValueObjectBase right)
		{
			return !(EqualOperator(left, right));
		}

		protected abstract IEnumerable<object> GetEqualityComponents();

		public override bool Equals(object obj)
		{
			if (obj == null || obj.GetType() != GetType())
			{
				return false;
			}

			ValueObjectBase other = (ValueObjectBase)obj;
			IEnumerator<object> thisValues = GetEqualityComponents().GetEnumerator();
			IEnumerator<object> otherValues = other.GetEqualityComponents().GetEnumerator();

			while (thisValues.MoveNext() && otherValues.MoveNext())
			{
				if (ReferenceEquals(thisValues.Current, null) ^ ReferenceEquals(otherValues.Current, null))
				{
					return false;
				}

				if (thisValues.Current != null && !thisValues.Current.Equals(otherValues.Current))
				{
					return false;
				}
			}

			return !thisValues.MoveNext() && !otherValues.MoveNext();
		}

		public override int GetHashCode()
		{
			return GetEqualityComponents()
				.Select(x => x != null ? x.GetHashCode() : 0)
				.Aggregate((x, y) => x ^ y);
		}
	}
}
