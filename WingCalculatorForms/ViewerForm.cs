namespace WingCalculatorForms;
using System.Windows.Forms;
using WingCalculatorShared;

public partial class ViewerForm : Form
{
	public ViewerForm()
	{
		InitializeComponent();
	}

	public void Refresh(Solver solver)
	{
		variableView.Items.Clear();

		bool allcaps = allcapsCheck.Checked;
		bool pointers = pointersCheck.Checked;
		bool zeros = zerosCheck.Checked;
		
		foreach ((string key, double val) in solver.GetValues())
		{
			if (double.TryParse(key, out _))
			{
				if (pointers) AddItem(key, val, zeros);
			}
			else if (key.All(c => char.IsUpper(c)))
			{
				if (allcaps) AddItem(key, val, zeros);
			}
			else
			{
				AddItem(key, val, zeros);
			}
		}
	}

	private void AddItem(string key, double val, bool zeros)
	{
		if (val != 0 || zeros)
		{
			variableView.Items.Add($"${key} = {val}");
		}
	}
}
