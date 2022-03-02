namespace WingCalculatorForms;
using System.Windows.Forms;
using System.Text.RegularExpressions;

internal partial class PopoutEntry : Form
{
	public int Index { get; set; }

	public PopoutEntry(string s, int index, WindowStyle style)
	{
		Index = index;
		InitializeComponent();
		Update(s);

		style.Apply(Controls, this);

		SizeF guessSize = CreateGraphics().MeasureString(s, Font);
		Height = (int)guessSize.Height;
		Width = (int)guessSize.Width;

		if (Height < MinimumSize.Height) Height = MinimumSize.Height;
		if (Width < MinimumSize.Width) Width = MinimumSize.Width;

		if (Height > 800) Height = 800;
		if (Width > 500) Width = 1000;
	}

	public void Update(string s) => textBox.Text = Regex.Replace(s, "(?<!\r)\n", "\r\n");
}
