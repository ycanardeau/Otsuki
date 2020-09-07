﻿using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Aigamo.Otsuki.Messages.Core;
using FluentAssertions;
using Xunit;

namespace Aigamo.Otsuki.Messages.Tests.Core
{
	public class AckNameTableOperationsMessageSerializerTests
	{
		private static IEnumerable<object?[]> TestData()
		{
			yield return new object?[]
			{
				new byte[]
				{
					0xCC, 0x00, 0x00, 0x00,
					0x05, 0x00, 0x00, 0x00,

					0xC6, 0x00, 0x00, 0x00,
					0x16, 0x01, 0x00, 0x00,
					0x0C, 0x00, 0x00, 0x00,

					0xD0, 0x00, 0x00, 0x00,
					0x4C, 0x00, 0x00, 0x00,
					0xCA, 0x00, 0x00, 0x00,

					0xC6, 0x00, 0x00, 0x00,
					0x40, 0x00, 0x00, 0x00,
					0x0C, 0x00, 0x00, 0x00,

					0x00, 0x00, 0x00, 0x00,
					0x00, 0x00, 0x00, 0x00,
					0x00, 0x00, 0x00, 0x00,

					0x00, 0x00, 0x00, 0x00,
					0x00, 0x00, 0x00, 0x00,
					0x00, 0x00, 0x00, 0x00,

					0x05, 0x00, 0x50, 0x00, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
					0x05, 0x00, 0x50, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x11, 0x01, 0x00, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00, 0x94, 0x00, 0x00, 0x00, 0x36, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x30, 0x00, 0x00, 0x00, 0x64, 0x00, 0x00, 0x00, 0x78, 0x2D, 0x64, 0x69, 0x72, 0x65, 0x63, 0x74, 0x70, 0x6C, 0x61, 0x79, 0x3A, 0x2F, 0x70, 0x72, 0x6F, 0x76, 0x69, 0x64, 0x65, 0x72, 0x3D, 0x25, 0x37, 0x42, 0x45, 0x42, 0x46, 0x45, 0x37, 0x42, 0x41, 0x30, 0x2D, 0x36, 0x32, 0x38, 0x44, 0x2D, 0x31, 0x31, 0x44, 0x32, 0x2D, 0x41, 0x45, 0x30, 0x46, 0x2D, 0x30, 0x30, 0x36, 0x30, 0x39, 0x37, 0x42, 0x30, 0x31, 0x34, 0x31, 0x31, 0x25, 0x37, 0x44, 0x3B, 0x68, 0x6F, 0x73, 0x74, 0x6E, 0x61, 0x6D, 0x65, 0x3D, 0x31, 0x39, 0x32, 0x2E, 0x31, 0x36, 0x38, 0x2E, 0x31, 0x31, 0x2E, 0x31, 0x30, 0x31, 0x3B, 0x70, 0x6F, 0x72, 0x74, 0x3D, 0x32, 0x33, 0x30, 0x33, 0x00, 0x31, 0x00, 0x34, 0x00, 0x2E, 0x00, 0x30, 0x00, 0x31, 0x00, 0x3A, 0x00, 0x34, 0x00, 0x35, 0x00, 0x3A, 0x00, 0x33, 0x00, 0x32, 0x00, 0x2E, 0x00, 0x33, 0x00, 0x35, 0x00, 0x39, 0x00, 0x30, 0x00, 0x30, 0x00, 0x30, 0x00, 0x30, 0x00, 0x20, 0x00, 0x28, 0x00, 0x50, 0x00, 0x65, 0x00, 0x65, 0x00, 0x72, 0x00, 0x29, 0x00, 0x00, 0x00,
					0x03, 0x00, 0x30, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
				},
				new AckNameTableOperationsMessage
				{
					Entries = new ICoreMessage?[]
					{
						new InstructConnectMessage
						{
							Dpnid = new Dpnid(3145731),
							Version = 4,
						},
						new AddPlayerMessage
						{
							Dpnid = new Dpnid(5242885),
							DpnidOwner = Dpnid.Empty,
							Peer = true,
							Connecting = true,
							Indicated = true,
							Version = 5,
							DnetClientVersion = DnetVersion.DirectX90,
							Url = new Address("192.168.11.101", 2303).Url,
							Data = ImmutableArray<byte>.Empty,
							Name = "14.01:45:32.3590000 (Peer)",
						},
						new InstructConnectMessage
						{
							Dpnid = new Dpnid(5242885),
							Version = 6,
						},
						null,
						null,
					}.ToImmutableArray(),
				},
			};
		}

		[Theory]
		[MemberData(nameof(TestData))]
		internal void Deserialize(byte[] data, AckNameTableOperationsMessage expected)
		{
			var message = AckNameTableOperationsMessageSerializer.Default.Deserialize(data);
			message.PacketType.Should().Be(expected.PacketType);
			message.NumEntries.Should().Be(expected.NumEntries);
			message.EntriesInternal.SelectMany(e => e.ToByteArray()).ToArray().Should().Equal(expected.EntriesInternal.SelectMany(e => e.ToByteArray()).ToArray());
		}

		[Theory]
		[MemberData(nameof(TestData))]
		internal void Serialize(byte[] expected, AckNameTableOperationsMessage message)
		{
			AckNameTableOperationsMessageSerializer.Default.Serialize(message).Should().Equal(expected);
		}
	}
}
