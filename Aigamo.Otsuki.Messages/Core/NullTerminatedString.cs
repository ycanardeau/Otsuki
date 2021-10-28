using System.Collections.Immutable;
using System.Text;

namespace Aigamo.Otsuki.Messages.Core;

internal abstract class NullTerminatedString<T> : IEquatable<T> where T : NullTerminatedString<T>
{
	private readonly IImmutableList<byte> _value;
	private readonly Encoding _encoding;

	protected NullTerminatedString(Encoding encoding) : this(Array.Empty<byte>(), encoding) { }

	protected NullTerminatedString(byte[] value, Encoding encoding)
	{
		_value = value.ToImmutableArray();
		_encoding = encoding;
	}

	protected NullTerminatedString(string? value, Encoding encoding) : this(!string.IsNullOrEmpty(value) ? encoding.GetBytes(value + '\0') : Array.Empty<byte>(), encoding) { }

	public int Length => _value.Count;

	public byte[] ToByteArray() => _value.ToArray();

	public override string ToString() => _encoding.GetString(ToByteArray()).TrimEnd('\0');

	public bool Equals(T? other) => ToString().Equals(other?.ToString());

	public override bool Equals(object? obj) => obj switch
	{
		T other => Equals(other),
		string other => ToString().Equals(other),
		_ => false,
	};

	public override int GetHashCode() => ToString().GetHashCode();
}

internal sealed class NullTerminatedAsciiString : NullTerminatedString<NullTerminatedAsciiString>
{
	public static readonly NullTerminatedAsciiString Empty = new NullTerminatedAsciiString();

	public NullTerminatedAsciiString() : base(Encoding.ASCII) { }
	public NullTerminatedAsciiString(byte[] value) : base(value, Encoding.ASCII) { }
	public NullTerminatedAsciiString(string? value) : base(value, Encoding.ASCII) { }

	public static implicit operator NullTerminatedAsciiString(byte[] value) => new NullTerminatedAsciiString(value);
	public static implicit operator NullTerminatedAsciiString(string? value) => new NullTerminatedAsciiString(value);

	public static implicit operator string(NullTerminatedAsciiString value) => value.ToString();

	public static bool operator ==(NullTerminatedAsciiString left, NullTerminatedAsciiString right) => left.Equals(right);
	public static bool operator !=(NullTerminatedAsciiString left, NullTerminatedAsciiString right) => !left.Equals(right);

	public override bool Equals(object? obj) => base.Equals(obj);

	public override int GetHashCode() => base.GetHashCode();
}

internal sealed class NullTerminatedUnicodeString : NullTerminatedString<NullTerminatedUnicodeString>
{
	public static readonly NullTerminatedUnicodeString Empty = new NullTerminatedUnicodeString();

	public NullTerminatedUnicodeString() : base(Encoding.Unicode) { }
	public NullTerminatedUnicodeString(byte[] value) : base(value, Encoding.Unicode) { }
	public NullTerminatedUnicodeString(string? value) : base(value, Encoding.Unicode) { }

	public static implicit operator NullTerminatedUnicodeString(byte[] value) => new NullTerminatedUnicodeString(value);
	public static implicit operator NullTerminatedUnicodeString(string? value) => new NullTerminatedUnicodeString(value);

	public static implicit operator string(NullTerminatedUnicodeString value) => value.ToString();

	public static bool operator ==(NullTerminatedUnicodeString left, NullTerminatedUnicodeString right) => left.Equals(right);
	public static bool operator !=(NullTerminatedUnicodeString left, NullTerminatedUnicodeString right) => !left.Equals(right);

	public override bool Equals(object? obj) => base.Equals(obj);

	public override int GetHashCode() => base.GetHashCode();
}
