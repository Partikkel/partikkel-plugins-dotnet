using NUnit.Framework;
using System;
namespace Partikkel.Validator.Tests
{
	[TestFixture()]
	public class Test
	{
		[Test()]
		public void TestCase()
		{
			var text = System.IO.File.ReadAllText("testfile.txt");
			var validator = new PartikkelValidator();
			var certPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "", "partikkel.pem");
			var result = validator.Validate(text, certPath);
			Assert.IsNotEmpty(result);
		}
	}
}
