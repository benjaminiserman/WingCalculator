namespace WingCalculatorForms;
using System.Collections.Generic;
using System.Windows.Forms;

internal record WindowStyle(Color BaseColor, Color InnerBaseColor, Color ButtonBackgroundColor, Color TextColor, Color ErrorColor, Color InfoColor, FlatStyle ButtonStyle, FlatStyle MenuButtonStyle, FontFamily TextFont)
{
	public static readonly Color DarkBase = Hex("#121212");

	public static readonly WindowStyle DarkMode = new
	(
		BaseColor: DarkBase,
		InnerBaseColor: ControlPaint.Light(DarkBase, 0.1f),
		ButtonBackgroundColor: ControlPaint.Light(DarkBase, 0.2f),
		TextColor: Color.White,
		ErrorColor: Color.IndianRed,
		InfoColor: Color.LightSkyBlue,
		ButtonStyle: FlatStyle.Popup,
		MenuButtonStyle: FlatStyle.Flat,
		TextFont: new FontFamily("Consolas")
	);

	public static readonly WindowStyle LightMode = new
	(
		BaseColor: Color.FromKnownColor(KnownColor.Control),
		InnerBaseColor: Color.FromKnownColor(KnownColor.Control),
		ButtonBackgroundColor: Color.FromKnownColor(KnownColor.ControlLight),
		TextColor: Color.Black,
		ErrorColor: Color.Red,
		InfoColor: Color.DarkBlue,
		ButtonStyle: FlatStyle.Standard,
		MenuButtonStyle: FlatStyle.Standard,
		TextFont: new FontFamily("Segoe UI")
	);

	public static List<string> MenuNames { get; } = new() { "darkModeButton" };

	public void Apply(Control.ControlCollection collection, Form form)
	{
		foreach (Control c in collection)
		{
			c.Font = new(TextFont, c.Font.Size);

			if (c is Button b)
			{
				c.BackColor = ButtonBackgroundColor;

				if (MenuNames.Contains(b.Name)) b.FlatStyle = MenuButtonStyle;
				else b.FlatStyle = ButtonStyle;
			}
			else if (c is TextBox)
			{
				c.BackColor = InnerBaseColor;
			}
			else if (c is ListBox lb)
			{
				c.BackColor = InnerBaseColor;

				if (lb is HistoryView hv) hv.RefreshEntries();
			}
			else if (c is Panel panel)
			{
				Apply(panel.Controls, form);
			}

			if (c.Name == "errorLabel") c.ForeColor = ErrorColor;
			else c.ForeColor = TextColor;
		}

		form.BackColor = BaseColor;
	}

	private static Color Hex(string s) => ColorTranslator.FromHtml(s);
}
