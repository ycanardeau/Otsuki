namespace Aigamo.Otsuki.Messages.Core
{
	public readonly struct Dpnid : IEquatable<Dpnid>, IFormattable
	{
		public static readonly Dpnid Empty;

		public int Value { get; }

		public Dpnid(int value) => Value = value;
		public Dpnid(int index, int version, Guid guidInstance) => Value = ((index & 0xFFFFF) + (version << 20)) ^ BitConverter.ToInt32(guidInstance.ToByteArray(), 0);

		public static bool operator ==(Dpnid left, Dpnid right) => left.Equals(right);
		public static bool operator !=(Dpnid left, Dpnid right) => !left.Equals(right);

		public override string ToString() => Value.ToString();
		public string ToString(string? format, IFormatProvider? provider) => Value.ToString(format, provider);

		public bool Equals(Dpnid other) => Value == other.Value;
		public override bool Equals(object? obj) => obj is Dpnid other && Equals(other);

		public override int GetHashCode() => HashCode.Combine(Value);
	}
}
