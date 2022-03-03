namespace WingCalculatorForms.History;

partial class PopoutEntry
{
	/// <summary>
	/// Required designer variable.
	/// </summary>
	private System.ComponentModel.IContainer components = null;

	/// <summary>
	/// Clean up any resources being used.
	/// </summary>
	/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
	protected override void Dispose(bool disposing)
	{
		if (disposing && (components != null))
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	#region Windows Form Designer generated code

	/// <summary>
	/// Required method for Designer support - do not modify
	/// the contents of this method with the code editor.
	/// </summary>
	private void InitializeComponent()
	{
		this.mainPanel = new System.Windows.Forms.Panel();
		this.textBox = new System.Windows.Forms.TextBox();
		this.mainPanel.SuspendLayout();
		this.SuspendLayout();
		// 
		// mainPanel
		// 
		this.mainPanel.Controls.Add(this.textBox);
		this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
		this.mainPanel.Location = new System.Drawing.Point(0, 0);
		this.mainPanel.Name = "mainPanel";
		this.mainPanel.Size = new System.Drawing.Size(378, 144);
		this.mainPanel.TabIndex = 0;
		// 
		// textBox
		// 
		this.textBox.Dock = System.Windows.Forms.DockStyle.Fill;
		this.textBox.Location = new System.Drawing.Point(0, 0);
		this.textBox.Multiline = true;
		this.textBox.Name = "textBox";
		this.textBox.ReadOnly = true;
		this.textBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.textBox.Size = new System.Drawing.Size(378, 144);
		this.textBox.TabIndex = 0;
		// 
		// PopoutEntry
		// 
		this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
		this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoScroll = true;
		this.ClientSize = new System.Drawing.Size(378, 144);
		this.Controls.Add(this.mainPanel);
		this.MinimumSize = new System.Drawing.Size(350, 100);
		this.Name = "PopoutEntry";
		this.ShowIcon = false;
		this.Text = "PopoutEntry";
		this.mainPanel.ResumeLayout(false);
		this.mainPanel.PerformLayout();
		this.ResumeLayout(false);

	}

	#endregion

	private Panel mainPanel;
	private TextBox textBox;
}
