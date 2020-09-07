using System.Collections.Generic;
using Aigamo.Otsuki.Messages.Core;
using FluentAssertions;
using Xunit;

namespace Aigamo.Otsuki.Messages.Tests.Core
{
	public class TerminateSessionMessageSerializerTests
	{
		private static IEnumerable<object?[]> TestData()
		{
			yield return new object?[]
			{
				new byte[]
				{
				},
				new TerminateSessionMessage
				{
				},
			};
		}

		[Theory(Skip = "Not implemneted")]
		[MemberData(nameof(TestData))]
		internal void Deserialize(byte[] data, TerminateSessionMessage expected)
		{
			var message = TerminateSessionMessageSerializer.Default.Deserialize(data);
			message.PacketType.Should().Be(expected.PacketType);
			message.TerminateData.Should().Equal(expected.TerminateData);
		}

		[Theory(Skip = "Not implemneted")]
		[MemberData(nameof(TestData))]
		internal void Serialize(byte[] expected, TerminateSessionMessage message)
		{
			TerminateSessionMessageSerializer.Default.Serialize(message).Should().Equal(expected);
		}
	}
}
