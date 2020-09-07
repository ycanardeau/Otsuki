// Comments from: https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/772ec104-55b6-4375-9111-cebd7bca1690

using System.Text;

namespace Aigamo.Otsuki.Messages.Core
{
	/// <summary>
	/// The DN_NAMETABLE_MEMBERSHIP_INFO structure contains information about a <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_d6292f62-e604-4ac7-9b20-87dde6efb93b">name table's</see> <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_51c51c14-7f9d-4c0b-a69c-d3e059bfffac">group</see> and <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_0258d4e2-d4f7-4099-ae0f-a02fad73e824">player</see> memberships. The number of DN_NAMETABLE_MEMBERSHIP_INFO structures in this packet is specified in the <b>dwMembershipCount</b> field.
	/// </summary>
	[Immutable]
	public sealed record NameTableMembershipInfo
	{
		/// <summary>
		/// A 32-bit integer that specifies the <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/8195991d-b7e3-4435-9e9f-2c3ab57eda8c#gt_1a73cb63-9f53-49e9-9f3e-19ce82ab5a6d">DirectPlay</see> identifier for the user. For more information, see section <see href="https://docs.microsoft.com/en-us/openspecs/windows_protocols/mc-dpl8cs/65b0f61c-4f93-42c9-953f-2299e686b497">2.2.7</see>.
		/// </summary>
		public Dpnid DpnidPlayer { get; init; }

		/// <summary>
		/// A 32-bit integer that provides the DirectPlay identifier for the group. For more information, see section 2.2.7.
		/// </summary>
		public Dpnid DpnidGroup { get; init; }

		/// <summary>
		/// A 32-bit integer that specifies the name table version.
		/// </summary>
		public int Version { get; init; }

		/// <summary>
		/// Not used.
		/// </summary>
		internal int VersionNotUsed { get; init; }

		public NameTableMembershipInfo() { }

		public override string ToString()
		{
			var builder = new StringBuilder();
			builder.AppendLine($"{nameof(NameTableMembershipInfo)}:");
			builder.AppendLine($"\t{nameof(DpnidPlayer)}: {DpnidPlayer}");
			builder.AppendLine($"\t{nameof(DpnidGroup)}: {DpnidGroup}");
			builder.AppendLine($"\t{nameof(Version)}: {Version}");
			builder.AppendLine($"\t{nameof(VersionNotUsed)}: {VersionNotUsed}");
			return builder.ToString();
		}
	}
}
