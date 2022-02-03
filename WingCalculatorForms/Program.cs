namespace WingCalculatorForms;

using WingCalculatorShared;
using System.Runtime.InteropServices;

internal static class Program
{
	/// <summary>
	///  The main entry point for the application.
	/// </summary>
	[STAThread]
	static void Main()
	{
		// To customize application configuration such as set high DPI settings or default font,
		// see https://aka.ms/applicationconfiguration.
		ApplicationConfiguration.Initialize();
		//Application.Run(new Form1());

		AllocConsole();

		Solver solver = new();

		while (true)
		{
			try
			{
				string s = Console.ReadLine();

				/*Console.WriteLine(string.Join('\n', Tokenizer.Tokenize(s)));
				Console.WriteLine();*/
				
				double ans = solver.Solve(s);
				solver.SetVariable("ANS", ans);

				Console.WriteLine($"> Solution: {ans}");
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}
	}

	[DllImport("kernel32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	static extern bool AllocConsole();
}