namespace WingCalculatorShared;
using System.Collections.Generic;
using System.Linq;
using WingCalculatorShared.Exceptions;

internal class LocalList
{
	private readonly Dictionary<string, INode> _nodes = new();

	public LocalList() { }

	public LocalList(ICollection<INode> nodes)
	{
		_nodes = nodes.Select((n, i) => (n, i)).ToDictionary(x => x.i.ToString(), x => x.n);
	}

	public INode this[string name]
	{
		get => Get(name);
		set => Set(name, value);
	}

	public INode this[double name] => this[name.ToString()];

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
			throw new Exception($"LocalList does not contain element #{name}.");
		}
	}

	public bool Contains(string name) => _nodes.ContainsKey(name);

	public static explicit operator List<INode>(LocalList x)
	{
		List<INode> list = new();
		for (double i = 0; true; i++)
		{
			if (x._nodes.TryGetValue(i.ToString(), out INode node))
			{
				list.Add(node);
			}
			else break;
		}

		return list;
	}

	public int Count => _nodes.Count;
}
