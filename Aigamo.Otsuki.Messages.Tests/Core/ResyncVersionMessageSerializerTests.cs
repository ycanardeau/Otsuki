using Aigamo.Otsuki.Messages.Core;
using FluentAssertions;
using Xunit;

namespace Aigamo.Otsuki.Messages.Tests.Core;

public class ResyncVersionMessageSerializerTests
{
	private static IEnumerable<object?[]> TestData()
	{
		yield return new object?[]
		{
				new byte[]
				{
					0xca, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
				},
				new ResyncVersionMessage
				{
					Version = 4,
				},
		};
	}

	[Theory]
	[MemberData(nameof(TestData))]
	internal void Deserialize(byte[] data, ResyncVersionMessage expected)
	{
		var message = ResyncVersionMessageSerializer.Default.Deserialize(data);
		message.PacketType.Should().Be(expected.PacketType);
		message.Version.Should().Be(expected.Version);
		message.VersionNotUsed.Should().Be(expected.VersionNotUsed);
	}

	[Theory]
	[MemberData(nameof(TestData))]
	internal void Serialize(byte[] expected, ResyncVersionMessage message)
	{
		ResyncVersionMessageSerializer.Default.Serialize(message).Should().Equal(expected);
	}
}
