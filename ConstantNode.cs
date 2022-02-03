namespace WingCalculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal record ConstantNode(double Value) : INode
{
	public double Solve() => Value;
}
