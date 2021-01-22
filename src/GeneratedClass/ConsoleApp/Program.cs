namespace ConsoleApp
{
	class Program
	{
		static void Main(string[] args)
		{
			var userClass = new UserClass();
			userClass.UserMethod();
		}
	}

	public class UserClass
	{
		public void UserMethod()
		{
			// call into a generated method
			GeneratedNamespace.GeneratedClass.GeneratedMethod();
		}
	}
}