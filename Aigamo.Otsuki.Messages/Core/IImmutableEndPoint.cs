using System.Net.Sockets;

namespace Aigamo.Otsuki.Messages.Core;

internal interface IImmutableEndPoint
{
	AddressFamily AddressFamily { get; }
}
