using System.Net;
using System.Net.Sockets;

namespace Aigamo.Otsuki.Messages.Core
{
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

		public static ImmutableIPEndPoint Parse(string value) => IPEndPoint.Parse(value).ToImmutableIPEndPoint();

		public bool Equals(ImmutableIPEndPoint other) => _value == other._value;
		public override bool Equals(object? obj) => obj is ImmutableIPEndPoint other && Equals(other);

		public override int GetHashCode() => HashCode.Combine(_value);

		public override string ToString() => _value!;

		public IPEndPoint ToIPEndPoint() => IPEndPoint.Parse(_value!);
	}

	internal static class IPEndPointExtensions
	{
		public static ImmutableIPEndPoint ToImmutableIPEndPoint(this IPEndPoint value) => new(value.ToString());
	}
}
