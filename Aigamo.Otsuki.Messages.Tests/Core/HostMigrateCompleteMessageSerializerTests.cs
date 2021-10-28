using Aigamo.Otsuki.Messages.Core;
using FluentAssertions;
using Xunit;

namespace Aigamo.Otsuki.Messages.Tests.Core;

public class HostMigrateCompleteMessageSerializerTests
{
	private static IEnumerable<object?[]> TestData()
	{
		yield return new object?[]
		{
				new byte[]
				{
					0xCE, 0x00, 0x00, 0x00,
				},
				new HostMigrateCompleteMessage(),
		};
	}

	[Theory]
	[MemberData(nameof(TestData))]
	internal void Deserialize(byte[] data, HostMigrateCompleteMessage expected)
	{
		var message = HostMigrateCompleteMessageSerializer.Default.Deserialize(data);
		message.PacketType.Should().Be(expected.PacketType);
	}

	[Theory]
	[MemberData(nameof(TestData))]
	internal void Serialize(byte[] expected, HostMigrateCompleteMessage message)
	{
		HostMigrateCompleteMessageSerializer.Default.Serialize(message).Should().Equal(expected);
	}
}
