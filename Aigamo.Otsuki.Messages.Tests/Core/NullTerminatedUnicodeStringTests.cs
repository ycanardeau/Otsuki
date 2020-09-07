using System;
using System.Collections.Generic;
using Aigamo.Otsuki.Messages.Core;
using FluentAssertions;
using Xunit;

namespace Aigamo.Otsuki.Messages.Tests.Core
{
	public class NullTerminatedUnicodeStringTests
	{
		private static readonly byte[] _testByteArray = new byte[]
		{
			0x48, 0x00, 0x65, 0x00, 0x6C, 0x00, 0x6C, 0x00, 0x6F, 0x00, 0x20, 0x00, 0x57, 0x00, 0x6F, 0x00, 0x72, 0x00, 0x6C, 0x00, 0x64, 0x00, 0x21, 0x00, 0x00, 0x00,
		};
		private static readonly string _testString = "Hello World!";

		private static IEnumerable<object?[]> Ctor_ByteArray_TestData()
		{
			yield return new object?[] { _testByteArray, _testString };
			yield return new object?[] { Array.Empty<byte>(), string.Empty };
		}

		[Theory]
		[MemberData(nameof(Ctor_ByteArray_TestData))]
		public void Ctor_ByteArray(byte[] value, string expected)
		{
			new NullTerminatedUnicodeString(value).Should().Be(expected);
		}

		[Fact]
		public void Ctor_String()
		{
			new NullTerminatedUnicodeString(_testString).ToString().Should().Be(_testString);
		}

		private static IEnumerable<object?[]> Equals_TestData()
		{
			yield return new object?[] { _testString, _testString, true };
			yield return new object?[] { _testString, new NullTerminatedUnicodeString("Hello World!"), true };
			yield return new object?[] { _testString, "Hello World!", true };
			yield return new object?[] { _testString, string.Empty, false };

			yield return new object?[] { _testString, new object(), false };
			yield return new object?[] { _testString, null, false };
		}

		[Theory]
		[MemberData(nameof(Equals_TestData))]
		internal void EqualsTest(NullTerminatedUnicodeString string1, object obj, bool expected)
		{
			if (obj is NullTerminatedUnicodeString string2)
			{
				string1.Equals(string2).Should().Be(expected);
				(string1 == string2).Should().Be(expected);
				(string1 != string2).Should().Be(!expected);
				string1.GetHashCode().Equals(string2.GetHashCode()).Should().Be(expected);
			}
			string1.Equals(obj).Should().Be(expected);
		}

		private static IEnumerable<object?[]> ToByteArray_TestData()
		{
			yield return new object?[] { _testString, _testByteArray };
			yield return new object?[] { string.Empty, Array.Empty<byte>() };
		}

		[Theory]
		[MemberData(nameof(ToByteArray_TestData))]
		public void ToByteArray(string value, byte[] expected)
		{
			new NullTerminatedUnicodeString(value).ToByteArray().Should().Equal(expected);
		}
	}
}
