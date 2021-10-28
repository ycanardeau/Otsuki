using Aigamo.Otsuki.Messages.Core;
using FluentAssertions;
using Xunit;

namespace Aigamo.Otsuki.Messages.Tests.Core
{
	public class DestroyPlayerMessageSerializerTests
	{
		private static IEnumerable<object?[]> TestData()
		{
			yield return new object?[]
			{
				new byte[]
				{
					0xD1, 0x00, 0x00, 0x00, 0xEE, 0x99, 0x8E, 0x11, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00,
				},
				new DestroyPlayerMessage
				{
					DpnidLeaving = new Dpnid(0x118E99EE),
					Version = 5,
					Reason = DestroyPlayerFlags.ConnectionLost,
				},
			};
		}

		[Theory]
		[MemberData(nameof(TestData))]
		internal void Deserialize(byte[] data, DestroyPlayerMessage expected)
		{
			var message = DestroyPlayerMessageSerializer.Default.Deserialize(data);
			message.PacketType.Should().Be(expected.PacketType);
			message.DpnidLeaving.Should().Be(expected.DpnidLeaving);
			message.Version.Should().Be(expected.Version);
			message.VersionNotUsed.Should().Be(expected.VersionNotUsed);
			message.Reason.Should().Be(expected.Reason);
		}

		[Theory]
		[MemberData(nameof(TestData))]
		internal void Serialize(byte[] expected, DestroyPlayerMessage message)
		{
			DestroyPlayerMessageSerializer.Default.Serialize(message).Should().Equal(expected);
		}
	}
}
