namespace Aigamo.Otsuki.Messages.Reliable
{
	public readonly struct SequenceId : IEquatable<SequenceId>, IComparable<SequenceId>, IFormattable
	{
		public static readonly SequenceId Empty;

		public byte Value { get; }

		public SequenceId(byte value) => Value = value;

		public static bool operator ==(SequenceId left, SequenceId right) => left.Equals(right);
		public static bool operator !=(SequenceId left, SequenceId right) => !left.Equals(right);

		public static bool operator <(SequenceId left, SequenceId right) => left.CompareTo(right) < 0;
		public static bool operator <=(SequenceId left, SequenceId right) => left.CompareTo(right) <= 0;
		public static bool operator >(SequenceId left, SequenceId right) => left.CompareTo(right) > 0;
		public static bool operator >=(SequenceId left, SequenceId right) => left.CompareTo(right) >= 0;

		public static SequenceId operator ++(SequenceId value) => value + 1;

		public static SequenceId operator +(SequenceId left, byte right) => new((byte)(left.Value + right));
		public static int operator -(SequenceId left, SequenceId right) => left.Value - right.Value;
		public static SequenceId operator -(SequenceId left, byte right) => new((byte)(left.Value - right));

		public override string ToString() => Value.ToString();
		public string ToString(string? format, IFormatProvider? provider) => Value.ToString(format, provider);

		public bool Equals(SequenceId other) => Value == other.Value;
		public override bool Equals(object? obj) => obj is SequenceId other && Equals(other);

		public override int GetHashCode() => HashCode.Combine(Value);

		// Code from: TCP/IP Illustrated, Vol. 2, p. 810
		public int CompareTo(SequenceId other) => Math.Sign((sbyte)(Value - other.Value));
	}
}
