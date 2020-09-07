// Code from: https://larryparkerdotnet.wordpress.com/2009/08/25/documenting-thread-safety/

using System;

namespace Aigamo.Otsuki.Messages
{
	/// <summary>
	/// Indicates the items that this synchronization object guards.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field)]
	internal sealed class GuardsAttribute : Attribute
	{
		/// <summary>
		/// Initializes a new <see cref="GuardsAttribute"/> object
		/// and sets the <see cref="Items"/> property.
		/// </summary>
		/// <param name="syncObjectName">The name of the sync object.</param>
		public GuardsAttribute(params string[] items)
		{
			Items = items;
		}

		/// <summary>
		/// Gets or sets a comma-separated list of items guarded by this synchronization object.
		/// </summary>
		public string[] Items { get; set; }
	}
}
