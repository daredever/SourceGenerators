using System;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var userClass = new UserClass();	
            userClass.UserMethod();
        }
    }
    
    public partial class UserClass
    {
	    public void UserMethod()
	    {
		    // call into a generated method
		    GeneratedNamespace.GeneratedClass.GeneratedMethod();
	    }
    }
}
