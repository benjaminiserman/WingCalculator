namespace WingCalculatorShared;
using System.Collections.Generic;
using System.Linq;
using WingCalculatorShared.Nodes;

internal class LocalList
{
	private readonly Dictionary<string, INode> _nodes = new();
	private readonly Solver _solver;

	public LocalList(Solver solver) => _solver = solver;

	public LocalList(ICollection<INode> nodes, Solver solver)
	{
		_nodes = nodes.Select((n, i) => (n, i)).ToDictionary(x => x.i.ToString(), x => x.n);
		_solver = solver;
	}

	public INode this[string name]
	{
		get => Get(name);
		set => Set(name, value);
	}

	public void Set(string name, INode a)
	{
		if (_nodes.ContainsKey(name)) _nodes[name] = a;
		else _nodes.Add(name, a);
	}

	public INode Get(string name)
	{
		if (_nodes.ContainsKey(name)) return _nodes[name];
		else
		{
			_nodes.Add(name, new ConstantNode(0, _solver));
			return _nodes[name];
		}
	}

	public int Count => _nodes.Count;
}
