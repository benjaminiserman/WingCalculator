namespace WingCalculator.Forms;
using System.Windows.Forms;
using WingCalc;

public partial class ViewerForm : Form
{
	private Solver _solver;

	public ViewerForm()
	{
		InitializeComponent();
	}

	public void RefreshEntries(Solver solver)
	{
		_solver = solver;

		RefreshEntries();
	}

	public void RefreshEntries()
	{
		variableView.Items.Clear();

		var allcaps = allcapsCheck.Checked;
		var pointers = pointersCheck.Checked;
		var zeros = zerosCheck.Checked;

		foreach ((var key, var val) in _solver.GetValues())
		{
			if (key != "NAN" && double.TryParse(key, out _)) // NAN gets parsed as a double
			{
				if (pointers)
				{
					AddItem(key, val, zeros);
				}
			}
			else if (key.All(c => char.IsUpper(c)))
			{
				if (allcaps)
				{
					AddItem(key, val, true); // true because allcaps should be shown even if == 0
				}
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

	protected override void OnFormClosing(FormClosingEventArgs e)
	{
		Hide();
		e.Cancel = true; // this cancels the close event.
	}

	private void allcapsCheck_CheckedChanged(object sender, EventArgs e) => RefreshEntries();
	private void pointersCheck_CheckedChanged(object sender, EventArgs e) => RefreshEntries();
	private void zerosCheck_CheckedChanged(object sender, EventArgs e) => RefreshEntries();
}
