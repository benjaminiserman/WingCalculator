namespace WingCalculatorForms.History;

partial class PopoutOmnibox
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.cursorLabel = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.cursorLabel, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.textBox1, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(778, 444);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// cursorLabel
			// 
			this.cursorLabel.AutoSize = true;
			this.cursorLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cursorLabel.Location = new System.Drawing.Point(3, 404);
			this.cursorLabel.Name = "cursorLabel";
			this.cursorLabel.Size = new System.Drawing.Size(772, 40);
			this.cursorLabel.TabIndex = 0;
			this.cursorLabel.Text = "TEMPLATE";
			this.cursorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textBox1
			// 
			this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox1.Location = new System.Drawing.Point(3, 3);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(772, 398);
			this.textBox1.TabIndex = 1;
			// 
			// PopoutOmnibox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(778, 444);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "PopoutOmnibox";
			this.ShowIcon = false;
			this.Text = "PopoutOmnibox";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

	}

	#endregion

	private TableLayoutPanel tableLayoutPanel1;
	private Label cursorLabel;
	private TextBox textBox1;
}
