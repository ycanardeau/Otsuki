using Aigamo.Otsuki.Messages.Core;
using FluentAssertions;
using Xunit;

namespace Aigamo.Otsuki.Messages.Tests.Core
{
	public class AlternateAddressTests
	{
		private static IEnumerable<object?[]> TestData()
		{
			yield return new object?[]
			{
				new byte[]
				{
					7, 0x02, 0x09, 0x06, 127, 0, 0, 1,
				},
				new AlternateAddress(ImmutableIPAddress.Loopback, 2310),
			};

			yield return new object?[]
			{
				new byte[]
				{
					19, 0x17, 0x09, 0x06, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
				},
				new AlternateAddress(ImmutableIPAddress.IPv6Loopback, 2310),
			};
		}

		[Theory]
		[MemberData(nameof(TestData))]
		internal void FromByteArray(byte[] data, AlternateAddress expected)
		{
			var message = AlternateAddress.FromByteArray(data);
			message.Size.Should().Be(expected.Size);
			message.Family.Should().Be(expected.Family);
			message.Port.Should().Be(expected.Port);
			expected.Address.Equals(message.Address).Should().BeTrue();
		}

		[Theory]
		[MemberData(nameof(TestData))]
		internal void ToByteArray(byte[] expected, AlternateAddress address)
		{
			address.ToByteArray().Should().Equal(expected);
		}
	}
}
