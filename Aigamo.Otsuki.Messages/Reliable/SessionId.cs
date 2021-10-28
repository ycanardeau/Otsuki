namespace Aigamo.Otsuki.Messages.Reliable
{
	public readonly struct SessionId : IEquatable<SessionId>, IFormattable
	{
		public static readonly SessionId Empty;

		public int Value { get; }

		public SessionId(int value) => Value = value;

		public bool IsEmpty => this == Empty;

		public static bool operator ==(SessionId left, SessionId right) => left.Equals(right);
		public static bool operator !=(SessionId left, SessionId right) => !left.Equals(right);

		public static SessionId NewSessionId()
		{
			var random = new Random();
			var ret = new SessionId(random.Next(int.MinValue, int.MaxValue));
			return ret != Empty ? ret : NewSessionId();
		}

		public override string ToString() => Value.ToString();
		public string ToString(string? format, IFormatProvider? provider) => Value.ToString(format, provider);

		public bool Equals(SessionId other) => Value == other.Value;
		public override bool Equals(object? obj) => obj is SessionId other && Equals(other);

		public override int GetHashCode() => HashCode.Combine(Value);
	}
}
