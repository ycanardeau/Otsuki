// Comments from: https://docs.microsoft.com/en-us/previous-versions/ms833500(v=msdn.10)?redirectedfrom=MSDN

using System.Net;
using System.Web;

namespace Aigamo.Otsuki.Messages.Core;

/// <summary>
/// The Address object contains generic addressing methods used to create and manipulate addresses for Microsoft® DirectPlay®.
/// </summary>
internal class Address
{
	public static readonly Guid ServiceProviderTcpIp = new Guid("ebfe7ba0-628d-11d2-ae0f-006097b01411");

	public static string KeyHostname = "hostname";
	public static string KeyPort = "port";
	public static string KeyProvider = "provider";

	private readonly Dictionary<string, object> _components = new Dictionary<string, object>();

	public Address() { }

	public Address(IPAddress address)
	{
		ServiceProvider = ServiceProviderTcpIp;
		AddComponent(KeyHostname, address.ToString());
	}

	public Address(IPEndPoint address)
	{
		ServiceProvider = ServiceProviderTcpIp;
		AddComponent(KeyHostname, address.Address.ToString());
		AddComponent(KeyPort, address.Port);
	}

	public Address(string hostname, int port)
	{
		ServiceProvider = ServiceProviderTcpIp;
		AddComponent(KeyHostname, hostname);
		AddComponent(KeyPort, port);
	}

	/// <summary>
	/// Retrieves or sets the service provider globally unique identifier (GUID) in the <see cref="Address"/> object.
	/// </summary>
	public Guid ServiceProvider
	{
		get => GetComponentGuid(KeyProvider);
		set => AddComponent(KeyProvider, value);
	}

	/// <summary>
	/// Retrieves the number of components in the <see cref="Address"/> object.
	/// </summary>
	public int NumberComponents => _components.Count();

	/// <summary>
	/// Retrieves or sets the Microsoft DirectPlay address URL string represented by this <see cref="Address"/> object.
	/// </summary>
	public string Url
	{
		get
		{
			static string ComponentToString(object value) => value switch
			{
				string o => Uri.EscapeDataString(o),
				int o => o.ToString(),
				Guid o => Uri.EscapeDataString(o.ToString("B").ToUpper()),
				byte[] o => HttpUtility.UrlEncode(o).ToUpper(),
				_ => throw new ArgumentException(),
			};

			return $"x-directplay:/{string.Join(';', _components.OrderByDescending(c => c.Key == KeyProvider).Select(c => $"{c.Key}={ComponentToString(c.Value)}").ToArray())}";
		}
		set
		{
			if (!value.StartsWith("x-directplay:/"))
				throw new UriFormatException();

			static object StringToComponent(string value)
			{
				if (int.TryParse(value, out var intValue))
					return intValue;

				if (Guid.TryParseExact(Uri.UnescapeDataString(value), "B", out var guid))
					return guid;

				return Uri.UnescapeDataString(value);
			}

			try
			{
				_components.Clear();
				var components = value.Substring("x-directplay:/".Length).Split(';').Select(x => x.Split('=')).Select(x => (Key: x[0], Value: x[1]));
				foreach (var (k, v) in components)
					_components.Add(k, StringToComponent(v));
			}
			catch
			{
				throw new UriFormatException();
			}
		}
	}

	/// <summary>
	/// Adds a component to the address. If the component is part of the address, it is replaced by the new value in this call.Values are specified in native formats when making this call. Therefore, the parameter should be a recast reference to a variable that holds the data in the native format. For example, if the component is a , the parameter should be a recast reference to a . This method validates that the predefined component types are the right format.
	/// </summary>
	public void AddComponent(string keyName, byte[] value) => _components[keyName] = value;

	/// <summary>
	/// Adds a component to the address. If the component is part of the address, it is replaced by the new value in this call.Values are specified in native formats when making this call. Therefore, the parameter should be a recast reference to a variable that holds the data in the native format. For example, if the component is a , the parameter should be a recast reference to a . This method validates that the predefined component types are the right format.
	/// </summary>
	public void AddComponent(string keyName, Guid value) => _components[keyName] = value;

	/// <summary>
	/// Adds a component to the address. If the component is part of the address, it is replaced by the new value in this call.Values are specified in native formats when making this call. Therefore, the parameter should be a recast reference to a variable that holds the data in the native format. For example, if the component is a , the parameter should be a recast reference to a . This method validates that the predefined component types are the right format.
	/// </summary>
	public void AddComponent(string keyName, int iValue) => _components[keyName] = iValue;

	/// <summary>
	/// Adds a component to the address. If the component is part of the address, it is replaced by the new value in this call.Values are specified in native formats when making this call. Therefore, the parameter should be a recast reference to a variable that holds the data in the native format. For example, if the component is a , the parameter should be a recast reference to a . This method validates that the predefined component types are the right format.
	/// </summary>
	public void AddComponent(string keyName, string value) => _components[keyName] = value;

	/// <summary>
	/// Retrieves information on the component at the specified key. Values for the component are retrieved in their native format. If the component key is not found, DPNERR_DOESNOTEXIST is returned.The value of the component is retrieved in its native format. Therefore, if the component's value is a , a is retrieved by this call. So buffer size = 4 and should be a recast .
	/// </summary>
	public byte[] GetComponentBinary(string keyName) => (byte[])_components[keyName];

	/// <summary>
	/// Retrieves information on the component at the specified key. Values for the component are retrieved in their native format. If the component key is not found, DPNERR_DOESNOTEXIST is returned.The value of the component is retrieved in its native format. Therefore, if the component's value is a , a is retrieved by this call. So buffer size = 4 and should be a recast .
	/// </summary>
	public Guid GetComponentGuid(string keyName) => (Guid)_components[keyName];

	/// <summary>
	/// Retrieves information on the component at the specified key. Values for the component are retrieved in their native format. If the component key is not found, DPNERR_DOESNOTEXIST is returned.The value of the component is retrieved in its native format. Therefore, if the component's value is a , a is retrieved by this call. So buffer size = 4 and should be a recast .
	/// </summary>
	public int GetComponentInteger(string keyName) => (int)_components[keyName];

	/// <summary>
	/// Retrieves information on the component at the specified key. Values for the component are retrieved in their native format. If the component key is not found, DPNERR_DOESNOTEXIST is returned.The value of the component is retrieved in its native format. Therefore, if the component's value is a , a is retrieved by this call. So buffer size = 4 and should be a recast .
	/// </summary>
	public string GetComponentString(string keyName) => (string)_components[keyName];

	public bool Equals(Address address) => Url == address.Url;
	public override bool Equals(object? obj) => obj is Address other && Equals(other);

	public override int GetHashCode() => Url.GetHashCode();
}
