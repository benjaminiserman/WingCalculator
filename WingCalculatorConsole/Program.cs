using WingCalculatorShared;
using System.Text;
using InputHandler;

Solver solver = new();

Console.WriteLine("Enter Math:");

while (true)
{
	try
	{
		string s = Console.ReadLine();

		double ans = solver.Solve(s);
		solver.SetVariable("ANS", ans);

		Console.WriteLine($"> Solution: {ans}");
	}
	catch (Exception e)
	{
		Console.WriteLine(e);
	}
}