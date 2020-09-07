using System;
using System.Collections.Generic;
using Aigamo.Otsuki.Messages.Core;
using FluentAssertions;
using Xunit;

namespace Aigamo.Otsuki.Messages.Tests.Core
{
	public class DpnidTests
	{
		private static readonly Guid _testGuid = new Guid("eb4de2c4-79b2-484a-a1a8-7aa75f5545d4");
		private static readonly Dpnid _testDpnid = new Dpnid(index: 2, version: 2, _testGuid);

		[Fact]
		public void Empty()
		{
			Dpnid.Empty.Should().Be(new Dpnid(0));
		}

		[Fact]
		public void Ctor_Int_Int_Guid()
		{
			var dpnid = new Dpnid(index: 5, version: 10, new Guid(0xA1B2C3D4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
			dpnid.Value.Should().Be(unchecked((int)0xA112C3D1));
		}

		private static IEnumerable<object?[]> Equals_TestData()
		{
			yield return new object?[] { _testDpnid, _testDpnid, true };
			yield return new object?[] { _testDpnid, new Dpnid(index: 2, version: 2, _testGuid), true };
			yield return new object?[] { _testDpnid, Dpnid.Empty, false };

			yield return new object?[] { new Dpnid(1), new Dpnid(1), true };
			yield return new object?[] { new Dpnid(1), new Dpnid(0), false };

			yield return new object?[] { _testDpnid, new object(), false };
			yield return new object?[] { _testDpnid, null, false };
		}

		[Theory]
		[MemberData(nameof(Equals_TestData))]
		internal void EqualsTest(Dpnid dpnid1, object obj, bool expected)
		{
			if (obj is Dpnid dpnid2)
			{
				dpnid1.Equals(dpnid2).Should().Be(expected);
				(dpnid1 == dpnid2).Should().Be(expected);
				(dpnid1 != dpnid2).Should().Be(!expected);
				dpnid1.GetHashCode().Equals(dpnid2.GetHashCode()).Should().Be(expected);
			}
			dpnid1.Equals(obj).Should().Be(expected);
		}

		[Fact]
		public void ToStringTest()
		{
			_testDpnid.ToString().Should().Be(_testDpnid.Value.ToString());
			$"{_testDpnid:X}".Should().Be($"{_testDpnid.Value:X}");
		}
	}
}
