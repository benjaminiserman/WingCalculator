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
			this.historyView = new HistoryView();
			this.errorLabel = new System.Windows.Forms.Label();
			this.darkModeButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// omnibox
			// 
			this.omnibox.Location = new System.Drawing.Point(333, 9);
			this.omnibox.Multiline = true;
			this.omnibox.Name = "omnibox";
			this.omnibox.Size = new System.Drawing.Size(350, 45);
			this.omnibox.TabIndex = 0;
			this.omnibox.TextChanged += new System.EventHandler(this.omnibox_TextChanged);
			// 
			// bin_button
			// 
			this.bin_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.bin_button.Location = new System.Drawing.Point(333, 380);
			this.bin_button.Name = "bin_button";
			this.bin_button.Size = new System.Drawing.Size(83, 58);
			this.bin_button.TabIndex = 1;
			this.bin_button.Text = "BIN";
			this.bin_button.UseVisualStyleBackColor = true;
			this.bin_button.Click += new System.EventHandler(this.bin_button_Click);
			// 
			// pi_button
			// 
			this.pi_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.pi_button.Location = new System.Drawing.Point(422, 380);
			this.pi_button.Name = "pi_button";
			this.pi_button.Size = new System.Drawing.Size(83, 58);
			this.pi_button.TabIndex = 2;
			this.pi_button.Text = "π";
			this.pi_button.UseVisualStyleBackColor = true;
			this.pi_button.Click += new System.EventHandler(this.pi_button_Click);
			// 
			// clr_button
			// 
			this.clr_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.clr_button.Location = new System.Drawing.Point(511, 380);
			this.clr_button.Name = "clr_button";
			this.clr_button.Size = new System.Drawing.Size(83, 58);
			this.clr_button.TabIndex = 3;
			this.clr_button.Text = "CLR";
			this.clr_button.UseVisualStyleBackColor = true;
			this.clr_button.Click += new System.EventHandler(this.clr_button_Click);
			// 
			// exe_button
			// 
			this.exe_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.exe_button.Location = new System.Drawing.Point(600, 380);
			this.exe_button.Name = "exe_button";
			this.exe_button.Size = new System.Drawing.Size(83, 58);
			this.exe_button.TabIndex = 4;
			this.exe_button.Text = "EXE";
			this.exe_button.UseVisualStyleBackColor = true;
			this.exe_button.Click += new System.EventHandler(this.exe_button_Click);
			// 
			// ac_button
			// 
			this.ac_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.ac_button.Location = new System.Drawing.Point(600, 316);
			this.ac_button.Name = "ac_button";
			this.ac_button.Size = new System.Drawing.Size(83, 58);
			this.ac_button.TabIndex = 8;
			this.ac_button.Text = "AC";
			this.ac_button.UseVisualStyleBackColor = true;
			this.ac_button.Click += new System.EventHandler(this.ac_button_Click);
			// 
			// ans_button
			// 
			this.ans_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.ans_button.Location = new System.Drawing.Point(511, 316);
			this.ans_button.Name = "ans_button";
			this.ans_button.Size = new System.Drawing.Size(83, 58);
			this.ans_button.TabIndex = 7;
			this.ans_button.Text = "ANS";
			this.ans_button.UseVisualStyleBackColor = true;
			this.ans_button.Click += new System.EventHandler(this.ans_button_Click);
			// 
			// tau_button
			// 
			this.tau_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.tau_button.Location = new System.Drawing.Point(422, 316);
			this.tau_button.Name = "tau_button";
			this.tau_button.Size = new System.Drawing.Size(83, 58);
			this.tau_button.TabIndex = 6;
			this.tau_button.Text = "τ";
			this.tau_button.UseVisualStyleBackColor = true;
			this.tau_button.Click += new System.EventHandler(this.tau_button_Click);
			// 
			// hex_button
			// 
			this.hex_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.hex_button.Location = new System.Drawing.Point(333, 316);
			this.hex_button.Name = "hex_button";
			this.hex_button.Size = new System.Drawing.Size(83, 58);
			this.hex_button.TabIndex = 5;
			this.hex_button.Text = "HEX";
			this.hex_button.UseVisualStyleBackColor = true;
			this.hex_button.Click += new System.EventHandler(this.hex_button_Click);
			// 
			// cbrt_button
			// 
			this.cbrt_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.cbrt_button.Location = new System.Drawing.Point(600, 252);
			this.cbrt_button.Name = "cbrt_button";
			this.cbrt_button.Size = new System.Drawing.Size(83, 58);
			this.cbrt_button.TabIndex = 12;
			this.cbrt_button.Text = "cbrt";
			this.cbrt_button.UseVisualStyleBackColor = true;
			this.cbrt_button.Click += new System.EventHandler(this.cbrt_button_Click);
			// 
			// sqrt_button
			// 
			this.sqrt_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.sqrt_button.Location = new System.Drawing.Point(511, 252);
			this.sqrt_button.Name = "sqrt_button";
			this.sqrt_button.Size = new System.Drawing.Size(83, 58);
			this.sqrt_button.TabIndex = 11;
			this.sqrt_button.Text = "sqrt";
			this.sqrt_button.UseVisualStyleBackColor = true;
			this.sqrt_button.Click += new System.EventHandler(this.sqrt_button_Click);
			// 
			// e_button
			// 
			this.e_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.e_button.Location = new System.Drawing.Point(422, 252);
			this.e_button.Name = "e_button";
			this.e_button.Size = new System.Drawing.Size(83, 58);
			this.e_button.TabIndex = 10;
			this.e_button.Text = "e";
			this.e_button.UseVisualStyleBackColor = true;
			this.e_button.Click += new System.EventHandler(this.e_button_Click);
			// 
			// dec_button
			// 
			this.dec_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.dec_button.Location = new System.Drawing.Point(333, 252);
			this.dec_button.Name = "dec_button";
			this.dec_button.Size = new System.Drawing.Size(83, 58);
			this.dec_button.TabIndex = 9;
			this.dec_button.Text = "DEC";
			this.dec_button.UseVisualStyleBackColor = true;
			this.dec_button.Click += new System.EventHandler(this.dec_button_Click);
			// 
			// txt_button
			// 
			this.txt_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.txt_button.Location = new System.Drawing.Point(333, 188);
			this.txt_button.Name = "txt_button";
			this.txt_button.Size = new System.Drawing.Size(83, 58);
			this.txt_button.TabIndex = 16;
			this.txt_button.Text = "TXT";
			this.txt_button.UseVisualStyleBackColor = true;
			this.txt_button.Click += new System.EventHandler(this.txt_button_Click);
			// 
			// arctan_button
			// 
			this.arctan_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.arctan_button.Location = new System.Drawing.Point(600, 188);
			this.arctan_button.Name = "arctan_button";
			this.arctan_button.Size = new System.Drawing.Size(83, 58);
			this.arctan_button.TabIndex = 15;
			this.arctan_button.Text = "atan";
			this.arctan_button.UseVisualStyleBackColor = true;
			this.arctan_button.Click += new System.EventHandler(this.arctan_button_Click);
			// 
			// arccos_button
			// 
			this.arccos_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.arccos_button.Location = new System.Drawing.Point(511, 188);
			this.arccos_button.Name = "arccos_button";
			this.arccos_button.Size = new System.Drawing.Size(83, 58);
			this.arccos_button.TabIndex = 14;
			this.arccos_button.Text = "acos";
			this.arccos_button.UseVisualStyleBackColor = true;
			this.arccos_button.Click += new System.EventHandler(this.arccos_button_Click);
			// 
			// pow_button
			// 
			this.pow_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.pow_button.Location = new System.Drawing.Point(600, 124);
			this.pow_button.Name = "pow_button";
			this.pow_button.Size = new System.Drawing.Size(83, 58);
			this.pow_button.TabIndex = 20;
			this.pow_button.Text = "pow";
			this.pow_button.UseVisualStyleBackColor = true;
			this.pow_button.Click += new System.EventHandler(this.pow_button_Click);
			// 
			// tan_button
			// 
			this.tan_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.tan_button.Location = new System.Drawing.Point(511, 124);
			this.tan_button.Name = "tan_button";
			this.tan_button.Size = new System.Drawing.Size(83, 58);
			this.tan_button.TabIndex = 19;
			this.tan_button.Text = "tan";
			this.tan_button.UseVisualStyleBackColor = true;
			this.tan_button.Click += new System.EventHandler(this.tan_button_Click);
			// 
			// cos_button
			// 
			this.cos_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.cos_button.Location = new System.Drawing.Point(422, 124);
			this.cos_button.Name = "cos_button";
			this.cos_button.Size = new System.Drawing.Size(83, 58);
			this.cos_button.TabIndex = 18;
			this.cos_button.Text = "cos";
			this.cos_button.UseVisualStyleBackColor = true;
			this.cos_button.Click += new System.EventHandler(this.cos_button_Click);
			// 
			// sin_button
			// 
			this.sin_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.sin_button.Location = new System.Drawing.Point(333, 124);
			this.sin_button.Name = "sin_button";
			this.sin_button.Size = new System.Drawing.Size(83, 58);
			this.sin_button.TabIndex = 17;
			this.sin_button.Text = "sin";
			this.sin_button.UseVisualStyleBackColor = true;
			this.sin_button.Click += new System.EventHandler(this.sin_button_Click);
			// 
			// log_button
			// 
			this.log_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.log_button.Location = new System.Drawing.Point(600, 60);
			this.log_button.Name = "log_button";
			this.log_button.Size = new System.Drawing.Size(83, 58);
			this.log_button.TabIndex = 24;
			this.log_button.Text = "log";
			this.log_button.UseVisualStyleBackColor = true;
			this.log_button.Click += new System.EventHandler(this.log_button_Click);
			// 
			// ln_button
			// 
			this.ln_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.ln_button.Location = new System.Drawing.Point(511, 60);
			this.ln_button.Name = "ln_button";
			this.ln_button.Size = new System.Drawing.Size(83, 58);
			this.ln_button.TabIndex = 23;
			this.ln_button.Text = "ln";
			this.ln_button.UseVisualStyleBackColor = true;
			this.ln_button.Click += new System.EventHandler(this.ln_button_Click);
			// 
			// mac_button
			// 
			this.mac_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.mac_button.Location = new System.Drawing.Point(422, 60);
			this.mac_button.Name = "mac_button";
			this.mac_button.Size = new System.Drawing.Size(83, 58);
			this.mac_button.TabIndex = 22;
			this.mac_button.Text = "MAC";
			this.mac_button.UseVisualStyleBackColor = true;
			this.mac_button.Click += new System.EventHandler(this.mac_button_Click);
			// 
			// var_button
			// 
			this.var_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.var_button.Location = new System.Drawing.Point(333, 60);
			this.var_button.Name = "var_button";
			this.var_button.Size = new System.Drawing.Size(83, 58);
			this.var_button.TabIndex = 21;
			this.var_button.Text = "VAR";
			this.var_button.UseVisualStyleBackColor = true;
			this.var_button.Click += new System.EventHandler(this.var_button_Click);
			// 
			// arcsin_button
			// 
			this.arcsin_button.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.arcsin_button.Location = new System.Drawing.Point(422, 188);
			this.arcsin_button.Name = "arcsin_button";
			this.arcsin_button.Size = new System.Drawing.Size(83, 58);
			this.arcsin_button.TabIndex = 13;
			this.arcsin_button.Text = "asin";
			this.arcsin_button.UseVisualStyleBackColor = true;
			this.arcsin_button.Click += new System.EventHandler(this.arcsin_button_Click);
			// 
			// historyView
			// 
			this.historyView.FormattingEnabled = true;
			this.historyView.HorizontalScrollbar = true;
			this.historyView.ItemHeight = 25;
			this.historyView.Location = new System.Drawing.Point(12, 9);
			this.historyView.Name = "historyView";
			this.historyView.Size = new System.Drawing.Size(315, 429);
			this.historyView.TabIndex = 25;
			this.historyView.SelectedIndexChanged += new System.EventHandler(this.historyView_SelectedIndexChanged);
			// 
			// errorLabel
			// 
			this.errorLabel.AutoSize = true;
			this.errorLabel.ForeColor = System.Drawing.Color.Red;
			this.errorLabel.Location = new System.Drawing.Point(12, 439);
			this.errorLabel.MaximumSize = new System.Drawing.Size(500, 0);
			this.errorLabel.Name = "errorLabel";
			this.errorLabel.Size = new System.Drawing.Size(0, 25);
			this.errorLabel.TabIndex = 26;
			// 
			// darkModeButton
			// 
			this.darkModeButton.BackgroundImage = global::WingCalculatorForms.Properties.Resources.night_mode;
			this.darkModeButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.darkModeButton.Location = new System.Drawing.Point(617, 443);
			this.darkModeButton.Name = "darkModeButton";
			this.darkModeButton.Size = new System.Drawing.Size(50, 50);
			this.darkModeButton.TabIndex = 27;
			this.darkModeButton.UseVisualStyleBackColor = true;
			this.darkModeButton.Click += new System.EventHandler(this.darkModeButton_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(693, 502);
			this.Controls.Add(this.darkModeButton);
			this.Controls.Add(this.errorLabel);
			this.Controls.Add(this.historyView);
			this.Controls.Add(this.log_button);
			this.Controls.Add(this.ln_button);
			this.Controls.Add(this.mac_button);
			this.Controls.Add(this.var_button);
			this.Controls.Add(this.pow_button);
			this.Controls.Add(this.tan_button);
			this.Controls.Add(this.cos_button);
			this.Controls.Add(this.sin_button);
			this.Controls.Add(this.txt_button);
			this.Controls.Add(this.arctan_button);
			this.Controls.Add(this.arccos_button);
			this.Controls.Add(this.arcsin_button);
			this.Controls.Add(this.cbrt_button);
			this.Controls.Add(this.sqrt_button);
			this.Controls.Add(this.e_button);
			this.Controls.Add(this.dec_button);
			this.Controls.Add(this.ac_button);
			this.Controls.Add(this.ans_button);
			this.Controls.Add(this.tau_button);
			this.Controls.Add(this.hex_button);
			this.Controls.Add(this.exe_button);
			this.Controls.Add(this.clr_button);
			this.Controls.Add(this.pi_button);
			this.Controls.Add(this.bin_button);
			this.Controls.Add(this.omnibox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MainForm";
			this.Text = "WingCalculator";
			this.ResumeLayout(false);
			this.PerformLayout();

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
}
