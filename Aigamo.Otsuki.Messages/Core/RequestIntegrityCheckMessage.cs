// Comments from: https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/91b0cfb1-9a46-476a-b3f0-e34a10554ce5

using System.Text;

namespace Aigamo.Otsuki.Messages.Core;

/// <summary>
/// The DN_REQ_INTEGRITY_CHECK packet requests that a <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_96048ee4-02d7-484e-a53b-3b8ed355251d">host</see> determine whether a target client is still in the <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_cb5007f6-e2af-44f6-abe1-c8a5ff856254">game session</see>.
/// </summary>
[Immutable]
public sealed record RequestIntegrityCheckMessage : ICoreMessage
{
	/// <summary>
	/// A 32-bit field that contains the packet type.
	/// </summary>
	public PacketType PacketType { get; } = PacketType.RequestIntegrityCheck;

	/// <summary>
	/// A 32-bit field that contains the context for the request operation. Values for the <b>dwReqContext</b> field SHOULD be ignored by the recipient.
	/// </summary>
	public int RequestContext { get; init; }

	/// <summary>
	/// A 32-bit field that contains the identifier of the selected target peer for the host to validate. For more information, see section <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/65b0f61c-4f93-42c9-953f-2299e686b497">2.2.7</see>.
	/// </summary>
	public Dpnid DpnidTarget { get; init; }

	public RequestIntegrityCheckMessage() { }

	public override string ToString()
	{
		var builder = new StringBuilder();
		builder.AppendLine($"{nameof(RequestIntegrityCheckMessage)}:");
		builder.AppendLine($"\t{nameof(RequestContext)}: {RequestContext}");
		builder.AppendLine($"\t{nameof(DpnidTarget)}: {DpnidTarget}");
		return builder.ToString();
	}
}

internal class RequestIntegrityCheckMessageSerializer : ICoreMessageSerializer<RequestIntegrityCheckMessage>
{
	public static RequestIntegrityCheckMessageSerializer Default { get; } = new();

	public virtual RequestIntegrityCheckMessage? Read(BinaryReader reader)
	{
		var packetType = (PacketType)reader.ReadInt32();
		if (packetType != PacketType.RequestIntegrityCheck)
			return null;

		var requestContext = reader.ReadInt32();
		var dpnidTarget = new Dpnid(reader.ReadInt32());

		return new()
		{
			RequestContext = requestContext,
			DpnidTarget = dpnidTarget,
		};
	}

	public virtual void Write(BinaryWriter writer, RequestIntegrityCheckMessage message)
	{
		writer.Write((int)message.PacketType);
		writer.Write(message.RequestContext);
		writer.Write(message.DpnidTarget.Value);
	}

	public virtual RequestIntegrityCheckMessage? Deserialize(byte[] data)
	{
		using var stream = new MemoryStream(data);
		using var reader = new BinaryReader(stream);
		return Read(reader);
	}

	public virtual byte[] Serialize(RequestIntegrityCheckMessage message)
	{
		using var stream = new MemoryStream();
		using var writer = new BinaryWriter(stream);
		Write(writer, message);
		return stream.ToArray();
	}
}
