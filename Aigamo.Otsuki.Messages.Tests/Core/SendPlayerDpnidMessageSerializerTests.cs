using Aigamo.Otsuki.Messages.Core;
using FluentAssertions;
using Xunit;

namespace Aigamo.Otsuki.Messages.Tests.Core;

public class SendPlayerDpnidMessageSerializerTests
{
	private static IEnumerable<object?[]> TestData()
	{
		yield return new object?[]
		{
				new byte[]
				{
				},
				new SendPlayerDpnidMessage
				{
				},
		};
	}

	[Theory(Skip = "Not implemneted")]
	[MemberData(nameof(TestData))]
	internal void Deserialize(byte[] data, SendPlayerDpnidMessage expected)
	{
		var message = SendPlayerDpnidMessageSerializer.Default.Deserialize(data);
		message.PacketType.Should().Be(expected.PacketType);
		message.Dpnid.Should().Be(expected.Dpnid);
	}

	[Theory(Skip = "Not implemneted")]
	[MemberData(nameof(TestData))]
	internal void Serialize(byte[] expected, SendPlayerDpnidMessage message)
	{
		SendPlayerDpnidMessageSerializer.Default.Serialize(message).Should().Equal(expected);
	}
}
