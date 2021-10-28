using Aigamo.Otsuki.Messages.Core;
using FluentAssertions;
using Xunit;

namespace Aigamo.Otsuki.Messages.Tests.Core;

public class RequestNameTableOperationsMessageSerializerTests
{
	private static IEnumerable<object?[]> TestData()
	{
		yield return new object?[]
		{
				new byte[]
				{
					0xCB, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
				},
				new RequestNameTableOperationsMessage
				{
					Version = 2,
				},
		};
	}

	[Theory]
	[MemberData(nameof(TestData))]
	internal void Deserialize(byte[] data, RequestNameTableOperationsMessage expected)
	{
		var message = RequestNameTableOperationsMessageSerializer.Default.Deserialize(data);
		message.PacketType.Should().Be(expected.PacketType);
		message.Version.Should().Be(expected.Version);
		message.VersionNotUsed.Should().Be(expected.VersionNotUsed);
	}

	[Theory]
	[MemberData(nameof(TestData))]
	internal void Serialize(byte[] expected, RequestNameTableOperationsMessage message)
	{
		RequestNameTableOperationsMessageSerializer.Default.Serialize(message).Should().Equal(expected);
	}
}
