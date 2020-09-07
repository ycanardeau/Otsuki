// Comments from: https://docs.microsoft.com/en-us/previous-versions/windows/desktop/bb320099(v=vs.85)

namespace Aigamo.Otsuki.Messages.Core
{
	/// <summary>
	/// Flag that controls how this method is processed.
	/// </summary>
	public enum DestroyPlayerFlags
	{
		Normal = 1,
		ConnectionLost,
		SessionTerminated,
		HostDestroyedPlayer,
	}
}
