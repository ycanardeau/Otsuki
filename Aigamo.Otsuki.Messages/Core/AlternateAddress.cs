using System;
using System.Collections.Immutable;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Aigamo.Otsuki.Messages.Core
{
	[Immutable]
	internal sealed record AlternateAddress
	{
		internal byte Size => (byte)(3 + Address.AddressBytes.Count);

		public AddressFamily Family => Address.AddressFamily;

		public int Port { get; init; }

		public ImmutableIPAddress Address { get; init; } = ImmutableIPAddress.Any;

		public AlternateAddress() { }

		public AlternateAddress(ImmutableIPAddress address, int port)
		{
			Address = address;
			Port = port;
		}

		public AlternateAddress(ImmutableIPEndPoint endPoint) : this(endPoint.Address, endPoint.Port) { }

		private AlternateAddress(BinaryReader reader)
		{
			var size = reader.ReadByte();
			var family = (AddressFamily)reader.ReadByte();
			Port = (ushort)IPAddress.NetworkToHostOrder(reader.ReadInt16());
			AddressBytes = family switch
			{
				AddressFamily.InterNetwork => reader.ReadBytes(4).ToImmutableArray(),
				AddressFamily.InterNetworkV6 => reader.ReadBytes(16).ToImmutableArray(),
				_ => throw new InvalidEnumArgumentException()
			};

			if (size != Size)
				throw new ArgumentException();
		}

		private IImmutableList<byte> AddressBytes
		{
			get => Address.AddressBytes;
			init => Address = new IPAddress(value.ToArray()).ToImmutableIPAddress();
		}

		public IImmutableEndPoint EndPoint => new IPEndPoint(Address.ToIPAddress(), Port).ToImmutableIPEndPoint();

		public static AlternateAddress FromBinaryReader(BinaryReader reader) => new AlternateAddress(reader);

		public static AlternateAddress FromByteArray(byte[] buffer)
		{
			using var stream = new MemoryStream(buffer);
			using var reader = new BinaryReader(stream);
			return new AlternateAddress(reader);
		}

		public byte[] ToByteArray()
		{
			var stream = new MemoryStream();
			var writer = new BinaryWriter(stream);

			writer.Write(Size);
			writer.Write((byte)Family);
			writer.Write(IPAddress.HostToNetworkOrder((short)Port));
			writer.Write(AddressBytes.ToArray());

			return stream.ToArray();
		}

		public override string ToString()
		{
			var builder = new StringBuilder();
			builder.AppendLine($"{nameof(AlternateAddress)}:");
			builder.AppendLine($"\t{nameof(Family)}: {Family}");
			builder.AppendLine($"\t{nameof(Port)}: {Port}");
			builder.AppendLine($"\t{nameof(Address)}: {Address}");
			return builder.ToString();
		}
	}
}
