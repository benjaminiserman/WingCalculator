namespace WingCalculator.Forms;

partial class ViewerForm
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
			this.checkboxPanel = new System.Windows.Forms.Panel();
			this.zerosCheck = new System.Windows.Forms.CheckBox();
			this.pointersCheck = new System.Windows.Forms.CheckBox();
			this.allcapsCheck = new System.Windows.Forms.CheckBox();
			this.variableView = new System.Windows.Forms.ListBox();
			this.tableLayoutPanel1.SuspendLayout();
			this.checkboxPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Controls.Add(this.checkboxPanel, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.variableView, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 71.72414F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 123F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(478, 646);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// checkboxPanel
			// 
			this.checkboxPanel.Controls.Add(this.zerosCheck);
			this.checkboxPanel.Controls.Add(this.pointersCheck);
			this.checkboxPanel.Controls.Add(this.allcapsCheck);
			this.checkboxPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.checkboxPanel.Location = new System.Drawing.Point(3, 526);
			this.checkboxPanel.Name = "checkboxPanel";
			this.checkboxPanel.Size = new System.Drawing.Size(472, 117);
			this.checkboxPanel.TabIndex = 1;
			// 
			// zerosCheck
			// 
			this.zerosCheck.AutoSize = true;
			this.zerosCheck.Location = new System.Drawing.Point(7, 83);
			this.zerosCheck.Name = "zerosCheck";
			this.zerosCheck.Size = new System.Drawing.Size(142, 29);
			this.zerosCheck.TabIndex = 2;
			this.zerosCheck.Text = "Include zeros";
			this.zerosCheck.UseVisualStyleBackColor = true;
			this.zerosCheck.CheckedChanged += new System.EventHandler(this.zerosCheck_CheckedChanged);
			// 
			// pointersCheck
			// 
			this.pointersCheck.AutoSize = true;
			this.pointersCheck.Location = new System.Drawing.Point(7, 48);
			this.pointersCheck.Name = "pointersCheck";
			this.pointersCheck.Size = new System.Drawing.Size(165, 29);
			this.pointersCheck.TabIndex = 1;
			this.pointersCheck.Text = "Include pointers";
			this.pointersCheck.UseVisualStyleBackColor = true;
			this.pointersCheck.CheckedChanged += new System.EventHandler(this.pointersCheck_CheckedChanged);
			// 
			// allcapsCheck
			// 
			this.allcapsCheck.AutoSize = true;
			this.allcapsCheck.Location = new System.Drawing.Point(7, 13);
			this.allcapsCheck.Name = "allcapsCheck";
			this.allcapsCheck.Size = new System.Drawing.Size(160, 29);
			this.allcapsCheck.TabIndex = 0;
			this.allcapsCheck.Text = "Include all-caps";
			this.allcapsCheck.UseVisualStyleBackColor = true;
			this.allcapsCheck.CheckedChanged += new System.EventHandler(this.allcapsCheck_CheckedChanged);
			// 
			// variableView
			// 
			this.variableView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.variableView.FormattingEnabled = true;
			this.variableView.ItemHeight = 25;
			this.variableView.Location = new System.Drawing.Point(3, 3);
			this.variableView.Name = "variableView";
			this.variableView.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.variableView.Size = new System.Drawing.Size(472, 517);
			this.variableView.TabIndex = 2;
			// 
			// ViewerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(478, 646);
			this.Controls.Add(this.tableLayoutPanel1);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ViewerForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Variable Viewer";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.checkboxPanel.ResumeLayout(false);
			this.checkboxPanel.PerformLayout();
			this.ResumeLayout(false);

	}

	#endregion

	private TableLayoutPanel tableLayoutPanel1;
	private Panel checkboxPanel;
	private CheckBox zerosCheck;
	private CheckBox pointersCheck;
	private CheckBox allcapsCheck;
	private ListBox variableView;
}
