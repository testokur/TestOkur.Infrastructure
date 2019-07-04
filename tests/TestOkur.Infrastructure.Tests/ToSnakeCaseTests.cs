using FluentAssertions;
using TestOkur.Infrastructure.Extensions;
using Xunit;

namespace TestOkur.Infrastructure.Tests
{
	public class ToSnakeCaseTests
	{
		[Theory]
		[InlineData(null)]
		[InlineData("")]
		public void When_StringIsEmptyOrNull_Then_StringShouldReturn(string input)
		{
			input.ToSnakeCase().Should().Be(input);
		}

		[Theory]
		[InlineData("UserTable","user_table")]
		[InlineData("FooBar", "foo_bar")]
		public void When_StringIsConvertable_Then_ShouldBeConverted(
			string input,
			string expected)
		{
			input.ToSnakeCase().Should().Be(expected);
		}
	}
}
