using Aigamo.Otsuki.Messages.Reliable;
using FluentAssertions;
using Xunit;

namespace Aigamo.Otsuki.Messages.Tests.Reliable;

public class SequenceIdTests
{
	private static readonly SequenceId _testSequenceId = new(255);

	[Fact]
	public void Empty()
	{
		SequenceId.Empty.Should().Be(new SequenceId(0));
	}

	private static IEnumerable<object?[]> CompareTo_TestData()
	{
		yield return new object?[] { _testSequenceId, _testSequenceId, 0 };

		yield return new object?[] { new SequenceId(0), new SequenceId(128), -1/* REVIEW */ };
		yield return new object?[] { new SequenceId(128), new SequenceId(0), -1 };

		yield return new object?[] { new SequenceId(0), new SequenceId(255), 1 };
		yield return new object?[] { new SequenceId(255), new SequenceId(0), -1 };

		yield return new object?[] { new SequenceId(1), new SequenceId(255), 1 };
		yield return new object?[] { new SequenceId(255), new SequenceId(1), -1 };

		yield return new object?[] { new SequenceId(1), new SequenceId(0), 1 };
		yield return new object?[] { new SequenceId(0), new SequenceId(1), -1 };

		yield return new object?[] { new SequenceId(127), new SequenceId(0), 1 };
		yield return new object?[] { new SequenceId(0), new SequenceId(127), -1 };

		yield return new object?[] { new SequenceId(127), new SequenceId(1), 1 };
		yield return new object?[] { new SequenceId(1), new SequenceId(127), -1 };

		yield return new object?[] { new SequenceId(128), new SequenceId(1), 1 };
		yield return new object?[] { new SequenceId(1), new SequenceId(128), -1 };

		yield return new object?[] { new SequenceId(128), new SequenceId(127), 1 };
		yield return new object?[] { new SequenceId(127), new SequenceId(128), -1 };

		yield return new object?[] { new SequenceId(255), new SequenceId(127), -1/* REVIEW */ };
		yield return new object?[] { new SequenceId(127), new SequenceId(255), -1 };

		yield return new object?[] { new SequenceId(255), new SequenceId(128), 1 };
		yield return new object?[] { new SequenceId(128), new SequenceId(255), -1 };
	}

	[Theory]
	[MemberData(nameof(CompareTo_TestData))]
	internal void CompareTo(SequenceId sequenceId1, object obj, int expected)
	{
		if (obj is SequenceId sequenceId2)
			Math.Sign(sequenceId1.CompareTo(sequenceId2)).Should().Be(expected);
	}

	private static IEnumerable<object?[]> Equals_TestData()
	{
		yield return new object?[] { _testSequenceId, _testSequenceId, true };
		yield return new object?[] { _testSequenceId, new SequenceId(255), true };
		yield return new object?[] { _testSequenceId, SequenceId.Empty, false };

		yield return new object?[] { new SequenceId(1), new SequenceId(1), true };
		yield return new object?[] { new SequenceId(1), new SequenceId(0), false };

		yield return new object?[] { _testSequenceId, new object(), false };
		yield return new object?[] { _testSequenceId, null, false };
	}

	[Theory]
	[MemberData(nameof(Equals_TestData))]
	internal void EqualsTest(SequenceId sequenceId1, object obj, bool expected)
	{
		if (obj is SequenceId sequenceId2)
		{
			sequenceId1.Equals(sequenceId2).Should().Be(expected);
			(sequenceId1 == sequenceId2).Should().Be(expected);
			(sequenceId1 != sequenceId2).Should().Be(!expected);
			sequenceId1.GetHashCode().Equals(sequenceId2.GetHashCode()).Should().Be(expected);
		}
		sequenceId1.Equals(obj).Should().Be(expected);
	}

	[Fact]
	public void Increment()
	{
		var sequenceId1 = new SequenceId(byte.MaxValue);
		(++sequenceId1).Should().Be(SequenceId.Empty);

		var sequenceId2 = SequenceId.Empty;
		(++sequenceId2).Should().Be(new SequenceId(1));
	}

	[Fact]
	public void ToStringTest()
	{
		_testSequenceId.ToString().Should().Be(_testSequenceId.Value.ToString());
		$"{_testSequenceId:X}".Should().Be($"{_testSequenceId.Value:X}");
	}
}
