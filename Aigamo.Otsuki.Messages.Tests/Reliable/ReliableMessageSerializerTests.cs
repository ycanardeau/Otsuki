using Aigamo.Otsuki.Messages.Reliable;
using FluentAssertions;
using Xunit;

namespace Aigamo.Otsuki.Messages.Tests.Reliable
{
	public class ReliableMessageSerializerTests
	{
		private static IEnumerable<object?[]> TestData()
		{
			static IEnumerable<object?[]> TestDataCore()
			{
				yield return new object?[] { new byte[] { 0 }, null, false };
				yield return new object?[] { new byte[] { 0, 1, 2, 3 }, null, false };
				yield return new object?[] { new byte[] { 1, 2, 3, 4 }, null, false };
			}

			return DataFrameMessageSerializerTests.TestData()
				.Concat(ConnectMessageSerializerTests.TestData().Select(x => x.Concat(new object?[] { false }).ToArray()))
				.Concat(ConnectedMessageSerializerTests.TestData().Select(x => x.Concat(new object?[] { false }).ToArray()))
				.Concat(HardDisconnectMessageSerializerTests.TestData())
				.Concat(SackMessageSerializerTests.TestData())
				.Concat(TestDataCore());
		}

		[Theory]
		[MemberData(nameof(TestData))]
		internal void Deserialize(byte[] data, IReliableMessage? expected, bool enableSigning)
		{
			static byte[]? Serialize(IReliableMessage? message) => message is not null ? ReliableMessageSerializer.Default.Serialize(message) : null;
			Serialize(ReliableMessageSerializer.Default.Deserialize(data)).Should().Equal(Serialize(expected));
		}
	}
}
