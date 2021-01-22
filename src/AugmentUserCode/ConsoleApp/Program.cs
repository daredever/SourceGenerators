using System;

namespace ConsoleApp
{
	class Program
	{
		static void Main(string[] args)
		{
			var userClass = new UserClass();
			userClass.UserMethod();
			userClass.PartialUserMethod("message1");
			Console.WriteLine(userClass.PartialUserMethodWithResult("message2"));
		}
	}
	
	public partial class UserClass
	{
		public void UserMethod()
		{
			// call into a generated method inside the class
			this.GeneratedMethod();
		}
		
		// The declaration of partial method
		public partial void PartialUserMethod(string message);		
		
		// The declaration of partial method
		public partial string PartialUserMethodWithResult(string message);
	}
}