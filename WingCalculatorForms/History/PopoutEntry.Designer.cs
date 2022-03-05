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
			this.tableLayout = new System.Windows.Forms.TableLayoutPanel();
			this.omniBox = new System.Windows.Forms.TextBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.exeButton = new System.Windows.Forms.Button();
			this.editToggle = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.tableLayout.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayout
			// 
			this.tableLayout.ColumnCount = 1;
			this.tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayout.Controls.Add(this.omniBox, 0, 0);
			this.tableLayout.Controls.Add(this.tableLayoutPanel1, 0, 1);
			this.tableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayout.Location = new System.Drawing.Point(0, 0);
			this.tableLayout.Name = "tableLayout";
			this.tableLayout.RowCount = 2;
			this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayout.Size = new System.Drawing.Size(378, 144);
			this.tableLayout.TabIndex = 0;
			// 
			// omniBox
			// 
			this.omniBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.omniBox.Location = new System.Drawing.Point(3, 3);
			this.omniBox.Multiline = true;
			this.omniBox.Name = "omniBox";
			this.omniBox.Size = new System.Drawing.Size(372, 98);
			this.omniBox.TabIndex = 0;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
			this.tableLayoutPanel1.Controls.Add(this.exeButton, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.editToggle, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 107);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(372, 34);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// exeButton
			// 
			this.exeButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.exeButton.Location = new System.Drawing.Point(255, 3);
			this.exeButton.Name = "exeButton";
			this.exeButton.Size = new System.Drawing.Size(114, 28);
			this.exeButton.TabIndex = 0;
			this.exeButton.Text = "Execute";
			this.exeButton.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.exeButton.UseVisualStyleBackColor = true;
			// 
			// editToggle
			// 
			this.editToggle.AutoSize = true;
			this.editToggle.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.editToggle.Dock = System.Windows.Forms.DockStyle.Right;
			this.editToggle.Location = new System.Drawing.Point(181, 3);
			this.editToggle.Name = "editToggle";
			this.editToggle.Size = new System.Drawing.Size(68, 28);
			this.editToggle.TabIndex = 1;
			this.editToggle.Text = "Edit";
			this.editToggle.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(166, 34);
			this.label1.TabIndex = 2;
			this.label1.Text = "Line: 0, Col: 0";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// PopoutEntry
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(378, 144);
			this.Controls.Add(this.tableLayout);
			this.MinimumSize = new System.Drawing.Size(350, 100);
			this.Name = "PopoutEntry";
			this.ShowIcon = false;
			this.Text = "PopoutEntry";
			this.tableLayout.ResumeLayout(false);
			this.tableLayout.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

	}

	#endregion

	private TableLayoutPanel tableLayout;
	private TextBox omniBox;
	private TableLayoutPanel tableLayoutPanel1;
	private Button exeButton;
	private CheckBox editToggle;
	private Label label1;
}
