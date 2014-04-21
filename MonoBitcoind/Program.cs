using System;

namespace MonoBitcoind
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			try {
				Examples.RunExamples();
			} catch (Exception e) {
				System.Console.Out.WriteLine("EXCEPTION: " + e.Message);
				System.Console.Out.Write(e.StackTrace);
				System.Console.Out.WriteLine("");
			}

			Console.WriteLine ("Example run-through is complete. Please manually close.");

			System.Threading.Thread.Sleep(int.MaxValue);
		}
	}
}
