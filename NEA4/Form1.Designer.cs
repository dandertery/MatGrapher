namespace NEA4
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.cartesianChart1 = new LiveChartsCore.SkiaSharpView.WinForms.CartesianChart();
            this.RPNTextBox = new System.Windows.Forms.TextBox();
            this.BitmapButton = new System.Windows.Forms.Button();
            this.FunctionButton = new System.Windows.Forms.Button();
            this.GridButton = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.RPNInputLabel = new System.Windows.Forms.Label();
            this.DisplayButton = new System.Windows.Forms.Button();
            this.a1 = new System.Windows.Forms.TextBox();
            this.d1 = new System.Windows.Forms.TextBox();
            this.b1 = new System.Windows.Forms.TextBox();
            this.c1 = new System.Windows.Forms.TextBox();
            this.c2 = new System.Windows.Forms.TextBox();
            this.b2 = new System.Windows.Forms.TextBox();
            this.d2 = new System.Windows.Forms.TextBox();
            this.a2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cartesianChart1
            // 
            this.cartesianChart1.Location = new System.Drawing.Point(37, 22);
            this.cartesianChart1.Name = "cartesianChart1";
            this.cartesianChart1.Size = new System.Drawing.Size(862, 706);
            this.cartesianChart1.TabIndex = 0;
            // 
            // RPNTextBox
            // 
            this.RPNTextBox.Location = new System.Drawing.Point(353, 887);
            this.RPNTextBox.Name = "RPNTextBox";
            this.RPNTextBox.Size = new System.Drawing.Size(100, 23);
            this.RPNTextBox.TabIndex = 1;
            // 
            // BitmapButton
            // 
            this.BitmapButton.Location = new System.Drawing.Point(67, 749);
            this.BitmapButton.Name = "BitmapButton";
            this.BitmapButton.Size = new System.Drawing.Size(150, 46);
            this.BitmapButton.TabIndex = 2;
            this.BitmapButton.Text = "Bitmap";
            this.BitmapButton.UseVisualStyleBackColor = true;
            // 
            // FunctionButton
            // 
            this.FunctionButton.Location = new System.Drawing.Point(223, 749);
            this.FunctionButton.Name = "FunctionButton";
            this.FunctionButton.Size = new System.Drawing.Size(150, 46);
            this.FunctionButton.TabIndex = 3;
            this.FunctionButton.Text = "Function";
            this.FunctionButton.UseVisualStyleBackColor = true;
            // 
            // GridButton
            // 
            this.GridButton.Location = new System.Drawing.Point(379, 749);
            this.GridButton.Name = "GridButton";
            this.GridButton.Size = new System.Drawing.Size(150, 46);
            this.GridButton.TabIndex = 4;
            this.GridButton.Text = "Grid";
            this.GridButton.UseVisualStyleBackColor = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // RPNInputLabel
            // 
            this.RPNInputLabel.AutoSize = true;
            this.RPNInputLabel.Location = new System.Drawing.Point(309, 890);
            this.RPNInputLabel.Name = "RPNInputLabel";
            this.RPNInputLabel.Size = new System.Drawing.Size(24, 15);
            this.RPNInputLabel.TabIndex = 6;
            this.RPNInputLabel.Text = "y =";
            // 
            // DisplayButton
            // 
            this.DisplayButton.Location = new System.Drawing.Point(489, 874);
            this.DisplayButton.Name = "DisplayButton";
            this.DisplayButton.Size = new System.Drawing.Size(150, 46);
            this.DisplayButton.TabIndex = 7;
            this.DisplayButton.Text = "Display";
            this.DisplayButton.UseVisualStyleBackColor = true;
            this.DisplayButton.Click += new System.EventHandler(this.DisplayButton_Click);
            // 
            // a1
            // 
            this.a1.Location = new System.Drawing.Point(1053, 520);
            this.a1.Name = "a1";
            this.a1.Size = new System.Drawing.Size(100, 23);
            this.a1.TabIndex = 8;
            this.a1.Text = "1";
            this.a1.TextChanged += new System.EventHandler(this.a1_TextChanged);
            // 
            // d1
            // 
            this.d1.Location = new System.Drawing.Point(1159, 549);
            this.d1.Name = "d1";
            this.d1.Size = new System.Drawing.Size(100, 23);
            this.d1.TabIndex = 9;
            this.d1.Text = "1";
            this.d1.TextChanged += new System.EventHandler(this.d1_TextChanged);
            // 
            // b1
            // 
            this.b1.Location = new System.Drawing.Point(1159, 520);
            this.b1.Name = "b1";
            this.b1.Size = new System.Drawing.Size(100, 23);
            this.b1.TabIndex = 10;
            this.b1.Text = "0";
            this.b1.TextChanged += new System.EventHandler(this.b1_TextChanged);
            // 
            // c1
            // 
            this.c1.Location = new System.Drawing.Point(1053, 549);
            this.c1.Name = "c1";
            this.c1.Size = new System.Drawing.Size(100, 23);
            this.c1.TabIndex = 11;
            this.c1.Text = "0";
            this.c1.TextChanged += new System.EventHandler(this.c1_TextChanged);
            // 
            // c2
            // 
            this.c2.Location = new System.Drawing.Point(1318, 549);
            this.c2.Name = "c2";
            this.c2.Size = new System.Drawing.Size(100, 23);
            this.c2.TabIndex = 15;
            this.c2.Text = "0";
            this.c2.TextChanged += new System.EventHandler(this.c2_TextChanged);
            // 
            // b2
            // 
            this.b2.Location = new System.Drawing.Point(1424, 520);
            this.b2.Name = "b2";
            this.b2.Size = new System.Drawing.Size(100, 23);
            this.b2.TabIndex = 14;
            this.b2.Text = "0";
            this.b2.TextChanged += new System.EventHandler(this.b2_TextChanged);
            // 
            // d2
            // 
            this.d2.Location = new System.Drawing.Point(1424, 549);
            this.d2.Name = "d2";
            this.d2.Size = new System.Drawing.Size(100, 23);
            this.d2.TabIndex = 13;
            this.d2.Text = "1";
            this.d2.TextChanged += new System.EventHandler(this.d2_TextChanged);
            // 
            // a2
            // 
            this.a2.Location = new System.Drawing.Point(1318, 520);
            this.a2.Name = "a2";
            this.a2.Size = new System.Drawing.Size(100, 23);
            this.a2.TabIndex = 12;
            this.a2.Text = "1";
            this.a2.TextChanged += new System.EventHandler(this.a2_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1110, 319);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 15);
            this.label1.TabIndex = 16;
            this.label1.Text = "det = ";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1670, 1102);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.c2);
            this.Controls.Add(this.b2);
            this.Controls.Add(this.d2);
            this.Controls.Add(this.a2);
            this.Controls.Add(this.c1);
            this.Controls.Add(this.b1);
            this.Controls.Add(this.d1);
            this.Controls.Add(this.a1);
            this.Controls.Add(this.DisplayButton);
            this.Controls.Add(this.RPNInputLabel);
            this.Controls.Add(this.GridButton);
            this.Controls.Add(this.FunctionButton);
            this.Controls.Add(this.BitmapButton);
            this.Controls.Add(this.RPNTextBox);
            this.Controls.Add(this.cartesianChart1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private LiveChartsCore.SkiaSharpView.WinForms.CartesianChart cartesianChart1;
        private TextBox RPNTextBox;
        private Button BitmapButton;
        private Button FunctionButton;
        private Button GridButton;
        private ContextMenuStrip contextMenuStrip1;
        private Label RPNInputLabel;
        private Button DisplayButton;
        private TextBox a1;
        private TextBox d1;
        private TextBox b1;
        private TextBox c1;
        private TextBox c2;
        private TextBox b2;
        private TextBox d2;
        private TextBox a2;
        private Label label1;
    }
}