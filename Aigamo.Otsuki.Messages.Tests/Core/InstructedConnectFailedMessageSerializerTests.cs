using Aigamo.Otsuki.Messages.Core;
using FluentAssertions;
using Xunit;

namespace Aigamo.Otsuki.Messages.Tests.Core;

public class InstructedConnectFailedMessageSerializerTests
{
	private static IEnumerable<object?[]> TestData()
	{
		yield return new object?[]
		{
				new byte[]
				{
				},
				new InstructedConnectFailedMessage
				{
				},
		};
	}

	[Theory(Skip = "Not implemneted")]
	[MemberData(nameof(TestData))]
	internal void Deserialize(byte[] data, InstructedConnectFailedMessage expected)
	{
		var message = InstructedConnectFailedMessageSerializer.Default.Deserialize(data);
		message.PacketType.Should().Be(expected.PacketType);
		message.Dpnid.Should().Be(expected.Dpnid);
	}

	[Theory(Skip = "Not implemneted")]
	[MemberData(nameof(TestData))]
	internal void Serialize(byte[] expected, InstructedConnectFailedMessage message)
	{
		InstructedConnectFailedMessageSerializer.Default.Serialize(message).Should().Equal(expected);
	}
}
