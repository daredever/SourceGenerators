using System;

namespace ConsoleApp
{
	class Program
	{
		static void Main(string[] args)
		{
			var user = new GeneratedNamespace.User();
			Console.WriteLine(user.GetType());

			var person = new GeneratedNamespace.Person();
			Console.WriteLine(person.GetType());
		}
	}
}