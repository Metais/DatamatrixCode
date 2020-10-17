using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatamatrixCode
{
	public static class ErrorPrompt
	{
        public static void errorPrompt(string msg, params object[] args)
        {
            Console.WriteLine("\n" + msg, args);
            Console.WriteLine("\nPress any key to continue . . .");
            Console.Read();
            Environment.Exit(-1);
        }
    }
}
