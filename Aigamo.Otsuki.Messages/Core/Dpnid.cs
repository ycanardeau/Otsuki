namespace Aigamo.Otsuki.Messages.Core;

public readonly record struct Dpnid(int Value) : IEquatable<Dpnid>, IFormattable
{
	public static readonly Dpnid Empty;

	public Dpnid(int index, int version, Guid guidInstance)
		: this(((index & 0xFFFFF) + (version << 20)) ^ BitConverter.ToInt32(guidInstance.ToByteArray(), 0)) { }

	public override string ToString() => Value.ToString();
	public string ToString(string? format, IFormatProvider? provider) => Value.ToString(format, provider);
}
