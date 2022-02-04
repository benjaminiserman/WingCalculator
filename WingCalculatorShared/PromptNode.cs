namespace WingCalculatorShared;
using InputHandler;

internal record PromptNode(IAssignable A, INode B, Solver Solver) : INode
{
	public double Solve()
	{
		int mode = (int)B.Solve();

		switch (mode)
		{
			case 0:
			{
				double x = Input.Get(s => double.Parse(s.Replace("_", string.Empty)), Solver.ReadLine, Solver.WriteError, (_, _) => "Enter a double.");
				A.Assign(new ConstantNode(x));
				return x;
			}
			case 1:
			{
				if (A is PointerNode pointerNode)
				{
					int start = (int)pointerNode.Address;

					string s = Solver.ReadLine();

					for (int i = 0; i < s.Length; i++)
					{
						pointerNode.Solver.SetVariable((start + i).ToString(), s[i]);
					}

					pointerNode.Solver.SetVariable(s.Length.ToString(), 0); // add null terminator

					return s.Length;
				}
				else throw new Exception("ASCII Prompt mode can only be assigned to a pointer node!");
			}
			case 2:
			{
				string bin = Input.GetCheck(s => !s.Any(c => !"01_".Contains(c)), Solver.ReadLine, Solver.WriteError, getMessage: _ => "Enter a binary number. Allowed characters are 0, 1, and _.");

				int x = 0;

				foreach (char c in bin.Where(a => a != '_'))
				{
					x <<= 1;
					x += c == '1' ? 1 : 0;
				}
				
				A.Assign(new ConstantNode(x));

				return x;
			}
			default:
			{
				throw new Exception($"Prompt mode #{mode} is not implemented.");
			}
		}
	}
}