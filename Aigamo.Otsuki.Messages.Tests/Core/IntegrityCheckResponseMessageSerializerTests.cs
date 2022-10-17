using Aigamo.Otsuki.Messages.Core;
using FluentAssertions;
using Xunit;

namespace Aigamo.Otsuki.Messages.Tests.Core;

public class IntegrityCheckResponseMessageSerializerTests
{
	private static IEnumerable<object?[]> TestData()
	{
		yield return new object?[]
		{
				new byte[]
				{
				},
				new IntegrityCheckResponseMessage
				{
				},
		};
	}

	[Theory(Skip = "Not implemneted")]
	[MemberData(nameof(TestData))]
	internal void Deserialize(byte[] data, IntegrityCheckResponseMessage expected)
	{
		var message = IntegrityCheckResponseMessageSerializer.Default.Deserialize(data);
		message.PacketType.Should().Be(expected.PacketType);
		message.DpnidRequesting.Should().Be(expected.DpnidRequesting);
	}

	[Theory(Skip = "Not implemneted")]
	[MemberData(nameof(TestData))]
	internal void Serialize(byte[] expected, IntegrityCheckResponseMessage message)
	{
		IntegrityCheckResponseMessageSerializer.Default.Serialize(message).Should().Equal(expected);
	}
}
