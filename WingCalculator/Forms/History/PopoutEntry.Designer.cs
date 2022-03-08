namespace WingCalculator.Forms.History;

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
			this.omniBox = new WingCalculator.Forms.Omnibox();
			this.bottomTable = new System.Windows.Forms.TableLayoutPanel();
			this.exeButton = new System.Windows.Forms.Button();
			this.editToggle = new System.Windows.Forms.CheckBox();
			this.cursorLabel = new System.Windows.Forms.Label();
			this.tableLayout.SuspendLayout();
			this.bottomTable.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayout
			// 
			this.tableLayout.ColumnCount = 1;
			this.tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayout.Controls.Add(this.omniBox, 0, 0);
			this.tableLayout.Controls.Add(this.bottomTable, 0, 1);
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
			this.omniBox.ReadOnly = true;
			this.omniBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.omniBox.Size = new System.Drawing.Size(372, 98);
			this.omniBox.TabIndex = 0;
			// 
			// bottomTable
			// 
			this.bottomTable.ColumnCount = 3;
			this.bottomTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.bottomTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.bottomTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.bottomTable.Controls.Add(this.exeButton, 2, 0);
			this.bottomTable.Controls.Add(this.editToggle, 1, 0);
			this.bottomTable.Controls.Add(this.cursorLabel, 0, 0);
			this.bottomTable.Dock = System.Windows.Forms.DockStyle.Fill;
			this.bottomTable.Location = new System.Drawing.Point(3, 107);
			this.bottomTable.Name = "bottomTable";
			this.bottomTable.RowCount = 1;
			this.bottomTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.bottomTable.Size = new System.Drawing.Size(372, 34);
			this.bottomTable.TabIndex = 1;
			// 
			// exeButton
			// 
			this.exeButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.exeButton.Location = new System.Drawing.Point(282, 3);
			this.exeButton.Name = "exeButton";
			this.exeButton.Size = new System.Drawing.Size(87, 28);
			this.exeButton.TabIndex = 0;
			this.exeButton.Text = "EXE";
			this.exeButton.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.exeButton.UseVisualStyleBackColor = true;
			// 
			// editToggle
			// 
			this.editToggle.AutoSize = true;
			this.editToggle.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.editToggle.Dock = System.Windows.Forms.DockStyle.Right;
			this.editToggle.Location = new System.Drawing.Point(208, 3);
			this.editToggle.Name = "editToggle";
			this.editToggle.Size = new System.Drawing.Size(68, 28);
			this.editToggle.TabIndex = 1;
			this.editToggle.Text = "Edit";
			this.editToggle.UseVisualStyleBackColor = true;
			// 
			// cursorLabel
			// 
			this.cursorLabel.AutoSize = true;
			this.cursorLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cursorLabel.Location = new System.Drawing.Point(3, 0);
			this.cursorLabel.Name = "cursorLabel";
			this.cursorLabel.Size = new System.Drawing.Size(180, 34);
			this.cursorLabel.TabIndex = 2;
			this.cursorLabel.Text = "Line: 1, Col: 1";
			this.cursorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// PopoutEntry
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(378, 144);
			this.Controls.Add(this.tableLayout);
			this.KeyPreview = true;
			this.MinimumSize = new System.Drawing.Size(375, 150);
			this.Name = "PopoutEntry";
			this.ShowIcon = false;
			this.Text = "PopoutEntry";
			this.tableLayout.ResumeLayout(false);
			this.tableLayout.PerformLayout();
			this.bottomTable.ResumeLayout(false);
			this.bottomTable.PerformLayout();
			this.ResumeLayout(false);

	}

	#endregion

	private TableLayoutPanel tableLayout;
	private Omnibox omniBox;
	private TableLayoutPanel bottomTable;
	private Button exeButton;
	private CheckBox editToggle;
	private Label cursorLabel;
}
