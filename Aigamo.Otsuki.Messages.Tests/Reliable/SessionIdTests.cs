using Aigamo.Otsuki.Messages.Reliable;
using FluentAssertions;
using Xunit;

namespace Aigamo.Otsuki.Messages.Tests.Reliable;

public class SessionIdTests
{
	private static readonly SessionId _testSessionId = new SessionId(0x22AE0764);

	[Fact]
	public void Empty()
	{
		SessionId.Empty.Should().Be(new SessionId(0));
	}

	[Fact]
	public void NewSessionId()
	{
		var sessionId1 = SessionId.NewSessionId();
		sessionId1.Should().NotBe(SessionId.Empty);

		var sessionId2 = SessionId.NewSessionId();
		sessionId2.Should().NotBe(SessionId.Empty);
	}

	private static IEnumerable<object?[]> Equals_TestData()
	{
		yield return new object?[] { _testSessionId, _testSessionId, true };
		yield return new object?[] { _testSessionId, new SessionId(0x22AE0764), true };
		yield return new object?[] { _testSessionId, SessionId.Empty, false };

		yield return new object?[] { new SessionId(1), new SessionId(1), true };
		yield return new object?[] { new SessionId(1), new SessionId(0), false };

		yield return new object?[] { _testSessionId, new object(), false };
		yield return new object?[] { _testSessionId, null, false };
	}

	[Theory]
	[MemberData(nameof(Equals_TestData))]
	internal void EqualsTest(SessionId sessionId1, object obj, bool expected)
	{
		if (obj is SessionId sessionId2)
		{
			sessionId1.Equals(sessionId2).Should().Be(expected);
			(sessionId1 == sessionId2).Should().Be(expected);
			(sessionId1 != sessionId2).Should().Be(!expected);
			sessionId1.GetHashCode().Equals(sessionId2.GetHashCode()).Should().Be(expected);
		}
		sessionId1.Equals(obj).Should().Be(expected);
	}

	[Fact]
	public void ToStringTest()
	{
		_testSessionId.ToString().Should().Be(_testSessionId.Value.ToString());
		$"{_testSessionId:X}".Should().Be($"{_testSessionId.Value:X}");
	}
}
