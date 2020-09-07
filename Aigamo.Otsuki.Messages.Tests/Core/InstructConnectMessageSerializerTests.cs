using System.Collections.Generic;
using Aigamo.Otsuki.Messages.Core;
using FluentAssertions;
using Xunit;

namespace Aigamo.Otsuki.Messages.Tests.Core
{
	public class InstructConnectMessageSerializerTests
	{
		private static IEnumerable<object?[]> TestData()
		{
			yield return new object?[]
			{
				new byte[]
				{
					0xc6, 0x00, 0x00, 0x00, 0x11, 0x77, 0xeb, 0x0f, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
				},
				new InstructConnectMessage
				{
					Dpnid = new Dpnid(0x0FEB7711),
					Version = 4,
				},
			};
		}

		[Theory]
		[MemberData(nameof(TestData))]
		internal void Deserialize(byte[] data, InstructConnectMessage expected)
		{
			var message = InstructConnectMessageSerializer.Default.Deserialize(data);
			message.PacketType.Should().Be(expected.PacketType);
			message.Dpnid.Should().Be(expected.Dpnid);
			message.Version.Should().Be(expected.Version);
			message.VersionNotUsed.Should().Be(expected.VersionNotUsed);
		}

		[Theory]
		[MemberData(nameof(TestData))]
		internal void Serialize(byte[] expected, InstructConnectMessage message)
		{
			InstructConnectMessageSerializer.Default.Serialize(message).Should().Equal(expected);
		}
	}
}
