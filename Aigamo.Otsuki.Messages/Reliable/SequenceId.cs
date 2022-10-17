namespace Aigamo.Otsuki.Messages.Reliable;

public readonly record struct SequenceId(byte Value) : IEquatable<SequenceId>, IComparable<SequenceId>, IFormattable
{
	public static readonly SequenceId Empty;

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

	// Code from: TCP/IP Illustrated, Vol. 2, p. 810
	public int CompareTo(SequenceId other) => Math.Sign((sbyte)(Value - other.Value));
}
