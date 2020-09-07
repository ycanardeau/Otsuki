﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Aigamo.Otsuki.Messages.Core;
using FluentAssertions;
using Xunit;

namespace Aigamo.Otsuki.Messages.Tests.Core
{
	public class PlayerConnectInfoMessageSerializerTests
	{
		private static IEnumerable<object?[]> TestData()
		{
			yield return new object?[]
			{
				new byte[]
				{
					0xC1, 0x00, 0x00, 0x00,
					0x04, 0x00, 0x00, 0x00,
					0x08, 0x00, 0x00, 0x00,
					0x60, 0x00, 0x00, 0x00,
					0x14, 0x00, 0x00, 0x00,
					0x00, 0x00, 0x00, 0x00,
					0x00, 0x00, 0x00, 0x00,
					0x00, 0x00, 0x00, 0x00,
					0x00, 0x00, 0x00, 0x00,
					0x00, 0x00, 0x00, 0x00,
					0x00, 0x00, 0x00, 0x00,
					0x00, 0x00, 0x00, 0x00,
					0x00, 0x00, 0x00, 0x00,
					0x23, 0x81, 0xBE, 0x94, 0xAB, 0xA1, 0xFB, 0x48, 0xA2, 0xE7, 0x23, 0x85, 0x9E, 0x65, 0x89, 0x36,
					0xDA, 0x80, 0xEF, 0x61, 0x1B, 0x69, 0x47, 0x42, 0x9A, 0xDD, 0x1C, 0x7B, 0xED, 0x2B, 0xC1, 0x3E,
					0x58, 0x00, 0x00, 0x00,
					0x08, 0x00, 0x00, 0x00,
					0x07, 0x02, 0x08, 0xFE, 0x41, 0x34, 0xEF, 0x3D,
					0x54, 0x00, 0x65, 0x00, 0x73, 0x00, 0x74, 0x00, 0x20, 0x00, 0x55, 0x00, 0x73, 0x00, 0x65, 0x00, 0x72, 0x00, 0x00, 0x00
				},
				new PlayerConnectInfoMessage
				{
					Flags = ObjectType.Peer,
					DnetVersion = (DnetVersion)8,
					GuidInstance = new Guid("94be8123-a1ab-48fb-a2e7-23859e658936"),
					GuidApplication = new Guid("61ef80da-691b-4247-9add-1c7bed2bc13e"),
					AlternateAddresses = new[]
					{
						new AlternateAddress(ImmutableIPAddress.Parse("65.52.239.61"), 2302),
					}.ToImmutableArray(),
					Url = string.Empty,
					ConnectData = ImmutableArray<byte>.Empty,
					Password = string.Empty,
					Data = ImmutableArray<byte>.Empty,
					Name = "Test User",
				},
			};

			yield return new object?[]
			{
				new byte[]
				{
					0xC1, 0x00, 0x00, 0x00,
					0x04, 0x00, 0x00, 0x00,
					0x07, 0x00, 0x00, 0x00,
					0x07, 0x01, 0x00, 0x00,
					0x16, 0x00, 0x00, 0x00,
					0xEF, 0x00, 0x00, 0x00,
					0x18, 0x00, 0x00, 0x00,
					0xDD, 0x00, 0x00, 0x00,
					0x12, 0x00, 0x00, 0x00,
					0xD1, 0x00, 0x00, 0x00,
					0x0C, 0x00, 0x00, 0x00,
					0x74, 0x00, 0x00, 0x00,
					0x5D, 0x00, 0x00, 0x00,
					0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
					0xC4, 0xE2, 0x4D, 0xEB, 0xB2, 0x79, 0x4A, 0x48, 0xA1, 0xA8, 0x7A, 0xA7, 0x5F, 0x55, 0x45, 0xD4,
					0x58, 0x00, 0x00, 0x00,
					0x1C, 0x00, 0x00, 0x00,
					0x13, 0x17, 0x08, 0xFE, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01,
					0x07, 0x02, 0x08, 0xFE, 0x7F, 0x00, 0x00, 0x01,
					0x78, 0x2D, 0x64, 0x69, 0x72, 0x65, 0x63, 0x74, 0x70, 0x6C, 0x61, 0x79, 0x3A, 0x2F, 0x70, 0x72, 0x6F, 0x76, 0x69, 0x64, 0x65, 0x72, 0x3D, 0x25, 0x37, 0x42, 0x45, 0x42, 0x46, 0x45, 0x37, 0x42, 0x41, 0x30, 0x2D, 0x36, 0x32, 0x38, 0x44, 0x2D, 0x31, 0x31, 0x44, 0x32, 0x2D, 0x41, 0x45, 0x30, 0x46, 0x2D, 0x30, 0x30, 0x36, 0x30, 0x39, 0x37, 0x42, 0x30, 0x31, 0x34, 0x31, 0x31, 0x25, 0x37, 0x44, 0x3B, 0x68, 0x6F, 0x73, 0x74, 0x6E, 0x61, 0x6D, 0x65, 0x3D, 0x31, 0x2E, 0x32, 0x2E, 0x33, 0x2E, 0x34, 0x3B, 0x70, 0x6F, 0x72, 0x74, 0x3D, 0x32, 0x33, 0x30, 0x32, 0x00,
					0x48, 0x65, 0x6C, 0x6C, 0x6F, 0x20, 0x57, 0x6F, 0x72, 0x6C, 0x64, 0x21,
					0x50, 0x00, 0x61, 0x00, 0x73, 0x00, 0x73, 0x00, 0x77, 0x00, 0x6F, 0x00, 0x72, 0x00, 0x64, 0x00, 0x00, 0x00,
					0x48, 0x00, 0x65, 0x00, 0x6C, 0x00, 0x6C, 0x00, 0x6F, 0x00, 0x20, 0x00, 0x57, 0x00, 0x6F, 0x00, 0x72, 0x00, 0x6C, 0x00, 0x64, 0x00, 0x21, 0x00,
					0x54, 0x00, 0x65, 0x00, 0x73, 0x00, 0x74, 0x00, 0x50, 0x00, 0x6C, 0x00, 0x61, 0x00, 0x79, 0x00, 0x65, 0x00, 0x72, 0x00, 0x00, 0x00,
				},
				new PlayerConnectInfoMessage
				{
					Flags = ObjectType.Peer,
					DnetVersion = DnetVersion.DirectX90,
					GuidInstance = Guid.Empty,
					GuidApplication = new Guid("eb4de2c4-79b2-484a-a1a8-7aa75f5545d4"),
					AlternateAddresses = new[]
					{
						new AlternateAddress(ImmutableIPAddress.IPv6Loopback, 2302),
						new AlternateAddress(ImmutableIPAddress.Loopback, 2302),
					}.ToImmutableArray(),
					Url = new Address("1.2.3.4", 2302).Url,
					ConnectData = Encoding.ASCII.GetBytes("Hello World!").ToImmutableArray(),
					Password = "Password",
					Data = Encoding.Unicode.GetBytes("Hello World!").ToImmutableArray(),
					Name = "TestPlayer",
				},
			};
		}

		[Theory]
		[MemberData(nameof(TestData))]
		internal void Deserialize(byte[] data, PlayerConnectInfoMessage expected)
		{
			var message = PlayerConnectInfoMessageSerializer.Default.Deserialize(data);
			message.PacketType.Should().Be(expected.PacketType);
			message.Flags.Should().Be(expected.Flags);
			message.DnetVersion.Should().Be(expected.DnetVersion);
			message.NameSize.Should().Be(expected.NameSize);
			message.DataSize.Should().Be(expected.DataSize);
			message.PasswordSize.Should().Be(expected.PasswordSize);
			message.ConnectDataSize.Should().Be(expected.ConnectDataSize);
			message.UrlSize.Should().Be(expected.UrlSize);
			message.GuidInstance.Should().Be(expected.GuidInstance);
			message.GuidApplication.Should().Be(expected.GuidApplication);
			message.AlternateAddressDataSize.Should().Be(expected.AlternateAddressDataSize);
			message.AlternateAddresses.SelectMany(a => a.ToByteArray()).ToArray().Should().Equal(expected.AlternateAddresses.SelectMany(a => a.ToByteArray()).ToArray());
			message.Url.Should().Be(expected.Url);
			message.ConnectData.ToArray().Should().Equal(expected.ConnectData.ToArray());
			message.Password.Should().Be(expected.Password);
			message.Data.ToArray().Should().Equal(expected.Data.ToArray());
			message.Name.Should().Be(expected.Name);
		}

		[Theory]
		[MemberData(nameof(TestData))]
		internal void Serialize(byte[] expected, PlayerConnectInfoMessage message)
		{
			PlayerConnectInfoMessageSerializer.Default.Serialize(message).Should().Equal(expected);
		}
	}
}
