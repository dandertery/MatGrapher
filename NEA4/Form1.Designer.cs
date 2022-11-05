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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1670, 1102);
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
    }
}