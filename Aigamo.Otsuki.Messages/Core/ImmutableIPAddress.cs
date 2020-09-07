using System;
using System.Collections.Immutable;
using System.Net;
using System.Net.Sockets;

namespace Aigamo.Otsuki.Messages.Core
{
	[Immutable]
	internal readonly struct ImmutableIPAddress : IEquatable<ImmutableIPAddress>
	{
		public static readonly ImmutableIPAddress Any = IPAddress.Any.ToImmutableIPAddress();
		public static readonly ImmutableIPAddress Loopback = IPAddress.Loopback.ToImmutableIPAddress();
		public static readonly ImmutableIPAddress Broadcast = IPAddress.Broadcast.ToImmutableIPAddress();
		public static readonly ImmutableIPAddress None = Broadcast;

		public static readonly ImmutableIPAddress IPv6Any = IPAddress.IPv6Any.ToImmutableIPAddress();
		public static readonly ImmutableIPAddress IPv6Loopback = IPAddress.IPv6Loopback.ToImmutableIPAddress();
		public static readonly ImmutableIPAddress IPv6None = IPAddress.IPv6None.ToImmutableIPAddress();

		private readonly string? _value;

		public ImmutableIPAddress(string value) => _value = value;

		public IImmutableList<byte> AddressBytes => ToIPAddress().GetAddressBytes().ToImmutableArray();
		public AddressFamily AddressFamily => ToIPAddress().AddressFamily;

		public static bool operator ==(ImmutableIPAddress left, ImmutableIPAddress right) => left.Equals(right);
		public static bool operator !=(ImmutableIPAddress left, ImmutableIPAddress right) => !left.Equals(right);

		public static ImmutableIPAddress Parse(string value) => IPAddress.Parse(value).ToImmutableIPAddress();

		public bool Equals(ImmutableIPAddress other) => _value == other._value;
		public override bool Equals(object? obj) => obj is ImmutableIPAddress other && Equals(other);

		public override int GetHashCode() => HashCode.Combine(_value);

		public override string ToString() => _value!;

		public IPAddress ToIPAddress() => IPAddress.Parse(_value!);
	}

	internal static class IPAddressExtensions
	{
		public static ImmutableIPAddress ToImmutableIPAddress(this IPAddress value) => new(value.ToString());
	}
}
