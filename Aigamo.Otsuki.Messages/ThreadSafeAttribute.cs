// Code from: https://larryparkerdotnet.wordpress.com/2009/08/25/documenting-thread-safety/

namespace Aigamo.Otsuki.Messages
{
	/// <summary>
	/// Represents an attribute that marks a class as thread-safe.
	/// This is only for documentation purposes and does not
	/// have any affect on the code's actual thread-safety.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
	internal sealed class ThreadSafeAttribute : Attribute
	{
	}

}
