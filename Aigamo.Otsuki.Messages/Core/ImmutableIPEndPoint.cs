using System.Globalization;
using System.Net;
using System.Net.Sockets;

namespace Aigamo.Otsuki.Messages.Core;

[Immutable]
internal readonly struct ImmutableIPEndPoint : IImmutableEndPoint, IEquatable<ImmutableIPEndPoint>
{
	private readonly string? _value;

	public ImmutableIPEndPoint(string value) => _value = value;

	public AddressFamily AddressFamily => ToIPEndPoint().AddressFamily;
	public ImmutableIPAddress Address => ToIPEndPoint().Address.ToImmutableIPAddress();
	public int Port => ToIPEndPoint().Port;

	public static bool operator ==(ImmutableIPEndPoint left, ImmutableIPEndPoint right) => left.Equals(right);
	public static bool operator !=(ImmutableIPEndPoint left, ImmutableIPEndPoint right) => !left.Equals(right);

	public static ImmutableIPEndPoint Parse(string value) => IPEndPointExtensions.Parse(value).ToImmutableIPEndPoint();

	public bool Equals(ImmutableIPEndPoint other) => _value == other._value;
	public override bool Equals(object? obj) => obj is ImmutableIPEndPoint other && Equals(other);

	public override int GetHashCode() => _value?.GetHashCode() ?? 0;

	public override string ToString() => _value!;

	public IPEndPoint ToIPEndPoint() => IPEndPointExtensions.Parse(_value!);
}

internal static class IPEndPointExtensions
{
	public static ImmutableIPEndPoint ToImmutableIPEndPoint(this IPEndPoint value) => new(value.ToString());

	// netstandard2.0 lacks IPEndPoint.Parse, so this mirrors the .NET Core implementation.
	public static IPEndPoint Parse(string s)
	{
		var addressLength = s.Length;
		var lastColonPos = s.LastIndexOf(':');

		if (lastColonPos > 0)
		{
			if (s[lastColonPos - 1] == ']')
				addressLength = lastColonPos;
			else if (s.Substring(0, lastColonPos).LastIndexOf(':') == -1)
				addressLength = lastColonPos;
		}

		if (IPAddress.TryParse(s.Substring(0, addressLength), out var address))
		{
			uint port = 0;
			if (addressLength == s.Length || (uint.TryParse(s.Substring(addressLength + 1), NumberStyles.None, CultureInfo.InvariantCulture, out port) && port <= IPEndPoint.MaxPort))
				return new IPEndPoint(address, (int)port);
		}

		throw new FormatException("An invalid IPEndPoint was specified.");
	}
}
