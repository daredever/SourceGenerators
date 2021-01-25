using System;
using Countries;

namespace ConsoleApp
{
	class Program
	{
		static void Main(string[] args)
		{	
			foreach (var country in DirectoryOfCountries.Europe())
			{
				Console.WriteLine(country.Name);
			}
		}
	}
}