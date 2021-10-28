// Code from: https://larryparkerdotnet.wordpress.com/2009/08/25/documenting-thread-safety/

namespace Aigamo.Otsuki.Messages
{
	/// <summary>
	/// Indicates the synchronization object that guards this item.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field)]
	internal sealed class GuardedByAttribute : Attribute
	{
		/// <summary>
		/// Initializes a new <see cref="GuardedByAttribute"/> object
		/// and sets the <see cref="SyncObjectNames"/> property.
		/// </summary>
		/// <param name="syncObjectNames">The name of the sync object.</param>
		public GuardedByAttribute(params string[] syncObjectNames)
		{
			SyncObjectNames = syncObjectNames;
		}

		/// <summary>
		/// Gets or sets the name of the synchronization object that guards this item.
		/// </summary>
		public string[] SyncObjectNames { get; set; }
	}
}
