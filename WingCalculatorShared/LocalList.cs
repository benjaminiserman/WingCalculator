namespace WingCalculatorShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class LocalList
{
	private readonly Dictionary<int, INode> _nodes = new();

	public LocalList(ICollection<INode> nodes) => _nodes = nodes.Select((n, i) => (n, i)).ToDictionary(x => x.i, x => x.n);

	public INode this[int x]
	{
		get => Get(x);
		set => Set(x, value);
	}

	public void Set(int x, INode a)
	{
		if (_nodes.ContainsKey(x)) _nodes[x] = a;
		else _nodes.Add(x, a);
	}

	public INode Get(int x)
	{
		if (_nodes.ContainsKey(x)) return _nodes[x];
		else
		{
			_nodes.Add(x, new ConstantNode(0));
			return _nodes[x];
		}
	}

	public int Count => _nodes.Count;
}
