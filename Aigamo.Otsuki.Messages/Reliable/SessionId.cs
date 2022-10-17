namespace Aigamo.Otsuki.Messages.Reliable;

public readonly record struct SessionId(int Value) : IEquatable<SessionId>, IFormattable
{
	public static readonly SessionId Empty;

	public bool IsEmpty => this == Empty;

	public static SessionId NewSessionId()
	{
		var random = new Random();
		var ret = new SessionId(random.Next(int.MinValue, int.MaxValue));
		return ret != Empty ? ret : NewSessionId();
	}

	public override string ToString() => Value.ToString();
	public string ToString(string? format, IFormatProvider? provider) => Value.ToString(format, provider);
}
