using Aigamo.Otsuki.Messages.Core;
using FluentAssertions;
using Xunit;

namespace Aigamo.Otsuki.Messages.Tests.Core;

public class ConnectAttemptFailedMessageSerializerTests
{
	private static IEnumerable<object?[]> TestData()
	{
		yield return new object?[]
		{
				new byte[]
				{
				},
				new ConnectAttemptFailedMessage
				{
				},
		};
	}

	[Theory(Skip = "Not implemneted")]
	[MemberData(nameof(TestData))]
	internal void Deserialize(byte[] data, ConnectAttemptFailedMessage expected)
	{
		var message = ConnectAttemptFailedMessageSerializer.Default.Deserialize(data);
		message.PacketType.Should().Be(expected.PacketType);
		message.Dpnid.Should().Be(expected.Dpnid);
	}

	[Theory(Skip = "Not implemneted")]
	[MemberData(nameof(TestData))]
	internal void Serialize(byte[] expected, ConnectAttemptFailedMessage message)
	{
		ConnectAttemptFailedMessageSerializer.Default.Serialize(message).Should().Equal(expected);
	}
}
