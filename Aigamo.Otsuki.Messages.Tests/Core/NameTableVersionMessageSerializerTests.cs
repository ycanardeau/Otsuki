using System.Collections.Generic;
using Aigamo.Otsuki.Messages.Core;
using FluentAssertions;
using Xunit;

namespace Aigamo.Otsuki.Messages.Tests.Core
{
	public class NameTableVersionMessageSerializerTests
	{
		private static IEnumerable<object?[]> TestData()
		{
			yield return new object?[]
			{
				new byte[]
				{
					0xc9, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
				},
				new NameTableVersionMessage
				{
					Version = 4,
				},
			};
		}

		[Theory]
		[MemberData(nameof(TestData))]
		internal void Deserialize(byte[] data, NameTableVersionMessage expected)
		{
			var message = NameTableVersionMessageSerializer.Default.Deserialize(data);
			message.PacketType.Should().Be(expected.PacketType);
			message.Version.Should().Be(expected.Version);
			message.VersionNotUsed.Should().Be(expected.VersionNotUsed);
		}

		[Theory]
		[MemberData(nameof(TestData))]
		internal void Serialize(byte[] expected, NameTableVersionMessage message)
		{
			NameTableVersionMessageSerializer.Default.Serialize(message).Should().Equal(expected);
		}
	}
}
