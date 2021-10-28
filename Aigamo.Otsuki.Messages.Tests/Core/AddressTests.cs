using System.Net;
using Aigamo.Otsuki.Messages.Core;
using FluentAssertions;
using Xunit;

namespace Aigamo.Otsuki.Messages.Tests.Core;

public class AddressTests
{
	[Fact]
	public void Ctor_IpAddress()
	{
		var address = new Address(IPAddress.Loopback);
		address.GetComponentString(Address.KeyHostname).Should().Be(IPAddress.Loopback.ToString());
		address.Url.Should().Be($"x-directplay:/provider=%7BEBFE7BA0-628D-11D2-AE0F-006097B01411%7D;hostname={IPAddress.Loopback}");
	}

	[Fact]
	public void Ctor_IpEndPoint()
	{
		var address = new Address(new IPEndPoint(IPAddress.Loopback, 2310));
		address.GetComponentString(Address.KeyHostname).Should().Be(IPAddress.Loopback.ToString());
		address.Url.Should().Be($"x-directplay:/provider=%7BEBFE7BA0-628D-11D2-AE0F-006097B01411%7D;hostname={IPAddress.Loopback};port=2310");
		address.GetComponentInteger(Address.KeyPort).Should().Be(2310);
	}

	[Fact]
	public void Ctor_String_Int()
	{
		var address = new Address("localhost", 2310);
		address.GetComponentString(Address.KeyHostname).Should().Be("localhost");
		address.GetComponentInteger(Address.KeyPort).Should().Be(2310);
		address.Url.Should().Be("x-directplay:/provider=%7BEBFE7BA0-628D-11D2-AE0F-006097B01411%7D;hostname=localhost;port=2310");
	}

	/*[Fact]
	public void Ctor_NullString()
	{
		Assert.Throws<NullReferenceException>(() => new Address(default!, 2310));
	}*/

	[Fact]
	public void Url()
	{
		var address = new Address();
		address.Url = "x-directplay:/provider=%7BEBFE7BA0-628D-11D2-AE0F-006097B01411%7D;hostname=localhost;port=2310";
		address.GetComponentGuid(Address.KeyProvider).Should().Be(Address.ServiceProviderTcpIp);
		address.GetComponentString(Address.KeyHostname).Should().Be("localhost");
		address.GetComponentInteger(Address.KeyPort).Should().Be(2310);
	}

	[Fact]
	public void Url_Invalid_ThrowsInvalidUrlException()
	{
		var address = new Address();
		address.Invoking(address => address.Url = "abcdef").Should().Throw<UriFormatException>();
		address.Invoking(address => address.Url = "x-directplay:/=;").Should().Throw<UriFormatException>();
	}

	/*[Fact]
	public void AddComponent_NullString_EmptyByteArray_ThrowsIndexOutOfRangeException()
	{
		var address = new Address();
		var ex = Assert.Throws<IndexOutOfRangeException>(() => address.AddComponent(default!, Array.Empty<byte>()));
	}*/

	/*[Fact]
	public void AddComponent_NullString_ByteArray_ThrowsDirectPlayException()
	{
		var address = new Address();
		var ex = Assert.Throws<DirectPlayException>(() => address.AddComponent(default!, new byte[] { 0 }));
		ex.ResultCode.Should().Be(ResultCode.InvalidPointer);
	}*/

	/*[Fact]
	public void AddComponent_NullString_Guid_ThrowsDirectPlayException()
	{
		var address = new Address();
		var ex = Assert.Throws<DirectPlayException>(() => address.AddComponent(default!, Guid.Empty));
		ex.ResultCode.Should().Be(ResultCode.InvalidPointer);
	}*/

	/*[Fact]
	public void AddComponent_NullString_String_ThrowsDirectPlayException()
	{
		var address = new Address();
		var ex = Assert.Throws<DirectPlayException>(() => address.AddComponent(default!, "localhost"));
		ex.ResultCode.Should().Be(ResultCode.InvalidPointer);
	}*/

	/*[Fact]
	public void AddComponent_NullString_Int_ThrowsDirectPlayException()
	{
		var address = new Address();
		var ex = Assert.Throws<DirectPlayException>(() => address.AddComponent(default!, 2310));
		ex.ResultCode.Should().Be(ResultCode.InvalidPointer);
	}*/

	/*[Fact]
	public void AddComponent_String_NullString_ThrowsNullReferenceException()
	{
		var address = new Address();
		Assert.Throws<NullReferenceException>(() => address.AddComponent(Address.KeyHostname, default(string)!));
	}*/

	[Theory]
	[InlineData(-1)]
	[InlineData(int.MaxValue)]
	public void AddComponent_Int(int value)
	{
		var address = new Address();
		address.AddComponent(Address.KeyPort, value);
		address.GetComponentInteger(Address.KeyPort).Should().Be(value);
	}

	/*[Fact]
	public void GetComponentBinary_String_ThrowsDoesNotExistException()
	{
		var address = new Address();
		Assert.Throws<DoesNotExistException>(() => address.GetComponentBinary(string.Empty));
	}*/

	/*[Fact]
	public void GetComponentGuid_String_ThrowsDoesNotExistException()
	{
		var address = new Address();
		Assert.Throws<DoesNotExistException>(() => address.GetComponentGuid(Address.KeyProvider));
	}*/

	/*[Fact]
	public void GetComponentString_String_ThrowsDoesNotExistException()
	{
		var address = new Address();
		Assert.Throws<DoesNotExistException>(() => address.GetComponentString(Address.KeyHostname));
	}*/

	/*[Fact]
	public void GetComponentInteger_String_ThrowsDoesNotExistException()
	{
		var address = new Address();
		Assert.Throws<DoesNotExistException>(() => address.GetComponentInteger(Address.KeyPort));
	}*/

	/*[Fact]
	public void GetComponentBinary_NullString_ThrowsArgumentException()
	{
		var address = new Address();
		Assert.Throws<ArgumentException>(() => address.GetComponentBinary(default!));
	}*/

	/*[Fact]
	public void GetComponentGuid_NullString_ThrowsArgumentException()
	{
		var address = new Address();
		Assert.Throws<ArgumentException>(() => address.GetComponentGuid(default!));
	}*/

	/*[Fact]
	public void GetComponentString_NullString_ThrowsArgumentException()
	{
		var address = new Address();
		Assert.Throws<ArgumentException>(() => address.GetComponentString(default!));
	}*/

	/*[Fact]
	public void GetComponentInteger_NullString_ThrowsArgumentException()
	{
		var address = new Address();
		Assert.Throws<ArgumentException>(() => address.GetComponentInteger(default!));
	}*/

	[Fact]
	public void EqualsTest()
	{
		var address1 = new Address();
		address1.AddComponent(Address.KeyProvider, Address.ServiceProviderTcpIp);
		address1.AddComponent(Address.KeyHostname, "localhost");
		address1.AddComponent(Address.KeyPort, 2310);

		var address2 = new Address();
		address2.AddComponent(Address.KeyPort, 2310);
		address2.AddComponent(Address.KeyHostname, "localhost");
		address2.AddComponent(Address.KeyProvider, Address.ServiceProviderTcpIp);

		var address3 = new Address();
		address3.AddComponent(Address.KeyHostname, "localhost");
		address3.AddComponent(Address.KeyPort, 2310);
		address3.AddComponent(Address.KeyProvider, Address.ServiceProviderTcpIp);

		address1.Equals(address2).Should().BeFalse();
		address1.Equals(address3).Should().BeTrue();
		address2.Equals(address3).Should().BeFalse();
	}
}
