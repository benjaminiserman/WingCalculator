namespace WingCalculatorForms;

partial class MainForm
{
	/// <summary>
	///  Required designer variable.
	/// </summary>
	private System.ComponentModel.IContainer components = null;

	/// <summary>
	///  Clean up any resources being used.
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
	///  Required method for Designer support - do not modify
	///  the contents of this method with the code editor.
	/// </summary>
	private void InitializeComponent()
	{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.omnibox = new System.Windows.Forms.TextBox();
			this.bin_button = new System.Windows.Forms.Button();
			this.pi_button = new System.Windows.Forms.Button();
			this.clr_button = new System.Windows.Forms.Button();
			this.exe_button = new System.Windows.Forms.Button();
			this.ac_button = new System.Windows.Forms.Button();
			this.ans_button = new System.Windows.Forms.Button();
			this.tau_button = new System.Windows.Forms.Button();
			this.hex_button = new System.Windows.Forms.Button();
			this.cbrt_button = new System.Windows.Forms.Button();
			this.sqrt_button = new System.Windows.Forms.Button();
			this.e_button = new System.Windows.Forms.Button();
			this.dec_button = new System.Windows.Forms.Button();
			this.txt_button = new System.Windows.Forms.Button();
			this.arctan_button = new System.Windows.Forms.Button();
			this.arccos_button = new System.Windows.Forms.Button();
			this.pow_button = new System.Windows.Forms.Button();
			this.tan_button = new System.Windows.Forms.Button();
			this.cos_button = new System.Windows.Forms.Button();
			this.sin_button = new System.Windows.Forms.Button();
			this.log_button = new System.Windows.Forms.Button();
			this.ln_button = new System.Windows.Forms.Button();
			this.mac_button = new System.Windows.Forms.Button();
			this.var_button = new System.Windows.Forms.Button();
			this.arcsin_button = new System.Windows.Forms.Button();
			this.historyView = new WingCalculatorForms.HistoryView();
			this.errorLabel = new System.Windows.Forms.Label();
			this.darkModeButton = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.panel1.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// omnibox
			// 
			this.omnibox.Location = new System.Drawing.Point(2, 2);
			this.omnibox.Margin = new System.Windows.Forms.Padding(2);
			this.omnibox.Multiline = true;
			this.omnibox.Name = "omnibox";
			this.omnibox.Size = new System.Drawing.Size(246, 29);
			this.omnibox.TabIndex = 0;
			this.omnibox.TextChanged += new System.EventHandler(this.omnibox_TextChanged);
			// 
			// bin_button
			// 
			this.bin_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.bin_button.Location = new System.Drawing.Point(2, 225);
			this.bin_button.Margin = new System.Windows.Forms.Padding(2);
			this.bin_button.Name = "bin_button";
			this.bin_button.Size = new System.Drawing.Size(58, 35);
			this.bin_button.TabIndex = 1;
			this.bin_button.Text = "BIN";
			this.bin_button.UseVisualStyleBackColor = true;
			this.bin_button.Click += new System.EventHandler(this.bin_button_Click);
			// 
			// pi_button
			// 
			this.pi_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.pi_button.Location = new System.Drawing.Point(64, 225);
			this.pi_button.Margin = new System.Windows.Forms.Padding(2);
			this.pi_button.Name = "pi_button";
			this.pi_button.Size = new System.Drawing.Size(58, 35);
			this.pi_button.TabIndex = 2;
			this.pi_button.Text = "π";
			this.pi_button.UseVisualStyleBackColor = true;
			this.pi_button.Click += new System.EventHandler(this.pi_button_Click);
			// 
			// clr_button
			// 
			this.clr_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.clr_button.Location = new System.Drawing.Point(127, 225);
			this.clr_button.Margin = new System.Windows.Forms.Padding(2);
			this.clr_button.Name = "clr_button";
			this.clr_button.Size = new System.Drawing.Size(58, 35);
			this.clr_button.TabIndex = 3;
			this.clr_button.Text = "CLR";
			this.clr_button.UseVisualStyleBackColor = true;
			this.clr_button.Click += new System.EventHandler(this.clr_button_Click);
			// 
			// exe_button
			// 
			this.exe_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.exe_button.Location = new System.Drawing.Point(189, 225);
			this.exe_button.Margin = new System.Windows.Forms.Padding(2);
			this.exe_button.Name = "exe_button";
			this.exe_button.Size = new System.Drawing.Size(58, 35);
			this.exe_button.TabIndex = 4;
			this.exe_button.Text = "EXE";
			this.exe_button.UseVisualStyleBackColor = true;
			this.exe_button.Click += new System.EventHandler(this.exe_button_Click);
			// 
			// ac_button
			// 
			this.ac_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.ac_button.Location = new System.Drawing.Point(189, 187);
			this.ac_button.Margin = new System.Windows.Forms.Padding(2);
			this.ac_button.Name = "ac_button";
			this.ac_button.Size = new System.Drawing.Size(58, 35);
			this.ac_button.TabIndex = 8;
			this.ac_button.Text = "AC";
			this.ac_button.UseVisualStyleBackColor = true;
			this.ac_button.Click += new System.EventHandler(this.ac_button_Click);
			// 
			// ans_button
			// 
			this.ans_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.ans_button.Location = new System.Drawing.Point(127, 187);
			this.ans_button.Margin = new System.Windows.Forms.Padding(2);
			this.ans_button.Name = "ans_button";
			this.ans_button.Size = new System.Drawing.Size(58, 35);
			this.ans_button.TabIndex = 7;
			this.ans_button.Text = "ANS";
			this.ans_button.UseVisualStyleBackColor = true;
			this.ans_button.Click += new System.EventHandler(this.ans_button_Click);
			// 
			// tau_button
			// 
			this.tau_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.tau_button.Location = new System.Drawing.Point(64, 187);
			this.tau_button.Margin = new System.Windows.Forms.Padding(2);
			this.tau_button.Name = "tau_button";
			this.tau_button.Size = new System.Drawing.Size(58, 35);
			this.tau_button.TabIndex = 6;
			this.tau_button.Text = "τ";
			this.tau_button.UseVisualStyleBackColor = true;
			this.tau_button.Click += new System.EventHandler(this.tau_button_Click);
			// 
			// hex_button
			// 
			this.hex_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.hex_button.Location = new System.Drawing.Point(2, 187);
			this.hex_button.Margin = new System.Windows.Forms.Padding(2);
			this.hex_button.Name = "hex_button";
			this.hex_button.Size = new System.Drawing.Size(58, 35);
			this.hex_button.TabIndex = 5;
			this.hex_button.Text = "HEX";
			this.hex_button.UseVisualStyleBackColor = true;
			this.hex_button.Click += new System.EventHandler(this.hex_button_Click);
			// 
			// cbrt_button
			// 
			this.cbrt_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.cbrt_button.Location = new System.Drawing.Point(189, 148);
			this.cbrt_button.Margin = new System.Windows.Forms.Padding(2);
			this.cbrt_button.Name = "cbrt_button";
			this.cbrt_button.Size = new System.Drawing.Size(58, 35);
			this.cbrt_button.TabIndex = 12;
			this.cbrt_button.Text = "cbrt";
			this.cbrt_button.UseVisualStyleBackColor = true;
			this.cbrt_button.Click += new System.EventHandler(this.cbrt_button_Click);
			// 
			// sqrt_button
			// 
			this.sqrt_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.sqrt_button.Location = new System.Drawing.Point(127, 148);
			this.sqrt_button.Margin = new System.Windows.Forms.Padding(2);
			this.sqrt_button.Name = "sqrt_button";
			this.sqrt_button.Size = new System.Drawing.Size(58, 35);
			this.sqrt_button.TabIndex = 11;
			this.sqrt_button.Text = "sqrt";
			this.sqrt_button.UseVisualStyleBackColor = true;
			this.sqrt_button.Click += new System.EventHandler(this.sqrt_button_Click);
			// 
			// e_button
			// 
			this.e_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.e_button.Location = new System.Drawing.Point(64, 148);
			this.e_button.Margin = new System.Windows.Forms.Padding(2);
			this.e_button.Name = "e_button";
			this.e_button.Size = new System.Drawing.Size(58, 35);
			this.e_button.TabIndex = 10;
			this.e_button.Text = "e";
			this.e_button.UseVisualStyleBackColor = true;
			this.e_button.Click += new System.EventHandler(this.e_button_Click);
			// 
			// dec_button
			// 
			this.dec_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.dec_button.Location = new System.Drawing.Point(2, 148);
			this.dec_button.Margin = new System.Windows.Forms.Padding(2);
			this.dec_button.Name = "dec_button";
			this.dec_button.Size = new System.Drawing.Size(58, 35);
			this.dec_button.TabIndex = 9;
			this.dec_button.Text = "DEC";
			this.dec_button.UseVisualStyleBackColor = true;
			this.dec_button.Click += new System.EventHandler(this.dec_button_Click);
			// 
			// txt_button
			// 
			this.txt_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.txt_button.Location = new System.Drawing.Point(2, 110);
			this.txt_button.Margin = new System.Windows.Forms.Padding(2);
			this.txt_button.Name = "txt_button";
			this.txt_button.Size = new System.Drawing.Size(58, 35);
			this.txt_button.TabIndex = 16;
			this.txt_button.Text = "TXT";
			this.txt_button.UseVisualStyleBackColor = true;
			this.txt_button.Click += new System.EventHandler(this.txt_button_Click);
			// 
			// arctan_button
			// 
			this.arctan_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.arctan_button.Location = new System.Drawing.Point(189, 110);
			this.arctan_button.Margin = new System.Windows.Forms.Padding(2);
			this.arctan_button.Name = "arctan_button";
			this.arctan_button.Size = new System.Drawing.Size(58, 35);
			this.arctan_button.TabIndex = 15;
			this.arctan_button.Text = "atan";
			this.arctan_button.UseVisualStyleBackColor = true;
			this.arctan_button.Click += new System.EventHandler(this.arctan_button_Click);
			// 
			// arccos_button
			// 
			this.arccos_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.arccos_button.Location = new System.Drawing.Point(127, 110);
			this.arccos_button.Margin = new System.Windows.Forms.Padding(2);
			this.arccos_button.Name = "arccos_button";
			this.arccos_button.Size = new System.Drawing.Size(58, 35);
			this.arccos_button.TabIndex = 14;
			this.arccos_button.Text = "acos";
			this.arccos_button.UseVisualStyleBackColor = true;
			this.arccos_button.Click += new System.EventHandler(this.arccos_button_Click);
			// 
			// pow_button
			// 
			this.pow_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.pow_button.Location = new System.Drawing.Point(189, 71);
			this.pow_button.Margin = new System.Windows.Forms.Padding(2);
			this.pow_button.Name = "pow_button";
			this.pow_button.Size = new System.Drawing.Size(58, 35);
			this.pow_button.TabIndex = 20;
			this.pow_button.Text = "pow";
			this.pow_button.UseVisualStyleBackColor = true;
			this.pow_button.Click += new System.EventHandler(this.pow_button_Click);
			// 
			// tan_button
			// 
			this.tan_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.tan_button.Location = new System.Drawing.Point(127, 71);
			this.tan_button.Margin = new System.Windows.Forms.Padding(2);
			this.tan_button.Name = "tan_button";
			this.tan_button.Size = new System.Drawing.Size(58, 35);
			this.tan_button.TabIndex = 19;
			this.tan_button.Text = "tan";
			this.tan_button.UseVisualStyleBackColor = true;
			this.tan_button.Click += new System.EventHandler(this.tan_button_Click);
			// 
			// cos_button
			// 
			this.cos_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.cos_button.Location = new System.Drawing.Point(64, 71);
			this.cos_button.Margin = new System.Windows.Forms.Padding(2);
			this.cos_button.Name = "cos_button";
			this.cos_button.Size = new System.Drawing.Size(58, 35);
			this.cos_button.TabIndex = 18;
			this.cos_button.Text = "cos";
			this.cos_button.UseVisualStyleBackColor = true;
			this.cos_button.Click += new System.EventHandler(this.cos_button_Click);
			// 
			// sin_button
			// 
			this.sin_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.sin_button.Location = new System.Drawing.Point(2, 71);
			this.sin_button.Margin = new System.Windows.Forms.Padding(2);
			this.sin_button.Name = "sin_button";
			this.sin_button.Size = new System.Drawing.Size(58, 35);
			this.sin_button.TabIndex = 17;
			this.sin_button.Text = "sin";
			this.sin_button.UseVisualStyleBackColor = true;
			this.sin_button.Click += new System.EventHandler(this.sin_button_Click);
			// 
			// log_button
			// 
			this.log_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.log_button.Location = new System.Drawing.Point(189, 33);
			this.log_button.Margin = new System.Windows.Forms.Padding(2);
			this.log_button.Name = "log_button";
			this.log_button.Size = new System.Drawing.Size(58, 35);
			this.log_button.TabIndex = 24;
			this.log_button.Text = "log";
			this.log_button.UseVisualStyleBackColor = true;
			this.log_button.Click += new System.EventHandler(this.log_button_Click);
			// 
			// ln_button
			// 
			this.ln_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.ln_button.Location = new System.Drawing.Point(127, 33);
			this.ln_button.Margin = new System.Windows.Forms.Padding(2);
			this.ln_button.Name = "ln_button";
			this.ln_button.Size = new System.Drawing.Size(58, 35);
			this.ln_button.TabIndex = 23;
			this.ln_button.Text = "ln";
			this.ln_button.UseVisualStyleBackColor = true;
			this.ln_button.Click += new System.EventHandler(this.ln_button_Click);
			// 
			// mac_button
			// 
			this.mac_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.mac_button.Location = new System.Drawing.Point(64, 33);
			this.mac_button.Margin = new System.Windows.Forms.Padding(2);
			this.mac_button.Name = "mac_button";
			this.mac_button.Size = new System.Drawing.Size(58, 35);
			this.mac_button.TabIndex = 22;
			this.mac_button.Text = "MAC";
			this.mac_button.UseVisualStyleBackColor = true;
			this.mac_button.Click += new System.EventHandler(this.mac_button_Click);
			// 
			// var_button
			// 
			this.var_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.var_button.Location = new System.Drawing.Point(2, 33);
			this.var_button.Margin = new System.Windows.Forms.Padding(2);
			this.var_button.Name = "var_button";
			this.var_button.Size = new System.Drawing.Size(58, 35);
			this.var_button.TabIndex = 21;
			this.var_button.Text = "VAR";
			this.var_button.UseVisualStyleBackColor = true;
			this.var_button.Click += new System.EventHandler(this.var_button_Click);
			// 
			// arcsin_button
			// 
			this.arcsin_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.arcsin_button.Location = new System.Drawing.Point(64, 110);
			this.arcsin_button.Margin = new System.Windows.Forms.Padding(2);
			this.arcsin_button.Name = "arcsin_button";
			this.arcsin_button.Size = new System.Drawing.Size(58, 35);
			this.arcsin_button.TabIndex = 13;
			this.arcsin_button.Text = "asin";
			this.arcsin_button.UseVisualStyleBackColor = true;
			this.arcsin_button.Click += new System.EventHandler(this.arcsin_button_Click);
			// 
			// historyView
			// 
			this.historyView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.historyView.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
			this.historyView.FormattingEnabled = true;
			this.historyView.HorizontalScrollbar = true;
			this.historyView.ItemHeight = 25;
			this.historyView.Items.AddRange(new object[] {
            "\n\n",
            "\n\n",
            "\n\n",
            "\n\n",
            "\n\n",
            "\n\n",
            "\n\n",
            "\n\n",
            "\n\n",
            "\n\n",
            "\n\n",
            "\n\n",
            "\n\n"});
			this.historyView.Location = new System.Drawing.Point(2, 2);
			this.historyView.Margin = new System.Windows.Forms.Padding(2);
			this.historyView.Name = "historyView";
			this.historyView.Size = new System.Drawing.Size(234, 264);
			this.historyView.TabIndex = 25;
			this.historyView.SelectedIndexChanged += new System.EventHandler(this.HistoryViewIndexChanged);
			// 
			// errorLabel
			// 
			this.errorLabel.AutoSize = true;
			this.errorLabel.ForeColor = System.Drawing.Color.Red;
			this.errorLabel.Location = new System.Drawing.Point(2, 268);
			this.errorLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.errorLabel.MaximumSize = new System.Drawing.Size(350, 0);
			this.errorLabel.Name = "errorLabel";
			this.errorLabel.Size = new System.Drawing.Size(0, 15);
			this.errorLabel.TabIndex = 26;
			// 
			// darkModeButton
			// 
			this.darkModeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.darkModeButton.BackgroundImage = global::WingCalculatorForms.Properties.Resources.night_mode;
			this.darkModeButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.darkModeButton.Location = new System.Drawing.Point(458, 270);
			this.darkModeButton.Margin = new System.Windows.Forms.Padding(2);
			this.darkModeButton.Name = "darkModeButton";
			this.darkModeButton.Size = new System.Drawing.Size(35, 30);
			this.darkModeButton.TabIndex = 27;
			this.darkModeButton.UseVisualStyleBackColor = true;
			this.darkModeButton.Click += new System.EventHandler(this.darkModeButton_Click);
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Controls.Add(this.omnibox);
			this.panel1.Controls.Add(this.bin_button);
			this.panel1.Controls.Add(this.pi_button);
			this.panel1.Controls.Add(this.clr_button);
			this.panel1.Controls.Add(this.log_button);
			this.panel1.Controls.Add(this.exe_button);
			this.panel1.Controls.Add(this.ln_button);
			this.panel1.Controls.Add(this.hex_button);
			this.panel1.Controls.Add(this.mac_button);
			this.panel1.Controls.Add(this.tau_button);
			this.panel1.Controls.Add(this.var_button);
			this.panel1.Controls.Add(this.ans_button);
			this.panel1.Controls.Add(this.pow_button);
			this.panel1.Controls.Add(this.ac_button);
			this.panel1.Controls.Add(this.tan_button);
			this.panel1.Controls.Add(this.dec_button);
			this.panel1.Controls.Add(this.cos_button);
			this.panel1.Controls.Add(this.e_button);
			this.panel1.Controls.Add(this.sin_button);
			this.panel1.Controls.Add(this.sqrt_button);
			this.panel1.Controls.Add(this.txt_button);
			this.panel1.Controls.Add(this.cbrt_button);
			this.panel1.Controls.Add(this.arctan_button);
			this.panel1.Controls.Add(this.arcsin_button);
			this.panel1.Controls.Add(this.arccos_button);
			this.panel1.Location = new System.Drawing.Point(241, 3);
			this.panel1.MinimumSize = new System.Drawing.Size(251, 262);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(251, 262);
			this.panel1.TabIndex = 28;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.43525F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 257F));
			this.tableLayoutPanel1.Controls.Add(this.errorLabel, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.historyView, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.darkModeButton, 1, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(495, 302);
			this.tableLayoutPanel1.TabIndex = 29;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(495, 302);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.Margin = new System.Windows.Forms.Padding(2);
			this.MaximizeBox = false;
			this.MinimumSize = new System.Drawing.Size(511, 341);
			this.Name = "MainForm";
			this.Text = "WingCalculator";
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

	}

	#endregion

	private TextBox omnibox;
	private Button bin_button;
	private Button pi_button;
	private Button clr_button;
	private Button exe_button;
	private Button ac_button;
	private Button ans_button;
	private Button tau_button;
	private Button hex_button;
	private Button cbrt_button;
	private Button sqrt_button;
	private Button e_button;
	private Button dec_button;
	private Button txt_button;
	private Button arctan_button;
	private Button arccos_button;
	private Button tan_button;
	private Button cos_button;
	private Button sin_button;
	private Button log_button;
	private Button ln_button;
	private Button mac_button;
	private Button var_button;
	private Button arcsin_button;
	private Button pow_button;
	private HistoryView historyView;
	private Label errorLabel;
	private Button darkModeButton;
	private Panel panel1;
	private TableLayoutPanel tableLayoutPanel1;
}
