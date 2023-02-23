namespace NEA4
{
    partial class MatGrapher
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
            this.a1 = new System.Windows.Forms.TextBox();
            this.d1 = new System.Windows.Forms.TextBox();
            this.b1 = new System.Windows.Forms.TextBox();
            this.c1 = new System.Windows.Forms.TextBox();
            this.c2 = new System.Windows.Forms.TextBox();
            this.b2 = new System.Windows.Forms.TextBox();
            this.d2 = new System.Windows.Forms.TextBox();
            this.a2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.InvariantLinesButton = new System.Windows.Forms.Button();
            this.EigenvectorsButton = new System.Windows.Forms.Button();
            this.LinesOfInvariantPointsButton = new System.Windows.Forms.Button();
            this.SwitchButton = new System.Windows.Forms.Button();
            this.MultiplyRightButton = new System.Windows.Forms.Button();
            this.ApplyMatrixButton = new System.Windows.Forms.Button();
            this.InverseLeft = new System.Windows.Forms.Button();
            this.InverseRight = new System.Windows.Forms.Button();
            this.ReflectionButton = new System.Windows.Forms.Button();
            this.TransposeLeft = new System.Windows.Forms.Button();
            this.TransposeRight = new System.Windows.Forms.Button();
            this.RotationButton = new System.Windows.Forms.Button();
            this.EnlargementButton = new System.Windows.Forms.Button();
            this.ShearingButton = new System.Windows.Forms.Button();
            this.fTimer = new System.Windows.Forms.Timer(this.components);
            this.AddButton = new System.Windows.Forms.Button();
            this.FunctionList = new System.Windows.Forms.ListBox();
            this.checkMatrixTimer = new System.Windows.Forms.Timer(this.components);
            this.detA = new System.Windows.Forms.Label();
            this.detB = new System.Windows.Forms.Label();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.ClearButton = new System.Windows.Forms.Button();
            this.kTextbox = new System.Windows.Forms.TextBox();
            this.pTextbox = new System.Windows.Forms.TextBox();
            this.qTextbox = new System.Windows.Forms.TextBox();
            this.piButton = new System.Windows.Forms.Button();
            this.eButton = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.absButton = new System.Windows.Forms.Button();
            this.button16 = new System.Windows.Forms.Button();
            this.sinButton = new System.Windows.Forms.Button();
            this.cosButton = new System.Windows.Forms.Button();
            this.button19 = new System.Windows.Forms.Button();
            this.button20 = new System.Windows.Forms.Button();
            this.UnitSquareButton = new System.Windows.Forms.Button();
            this.TriangleButton = new System.Windows.Forms.Button();
            this.AnimateButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.BoundsTextBox = new System.Windows.Forms.TextBox();
            this.MatrixList = new System.Windows.Forms.ListBox();
            this.ClearMatrix = new System.Windows.Forms.Button();
            this.RemoveMatrix = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.LinesOf = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.InvLine1TextBox = new System.Windows.Forms.TextBox();
            this.InvLine2TextBox = new System.Windows.Forms.TextBox();
            this.LOfInvPointsTextBox = new System.Windows.Forms.TextBox();
            this.vLabel = new System.Windows.Forms.Label();
            this.vTextBox = new System.Windows.Forms.TextBox();
            this.AnimateTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // cartesianChart1
            // 
            this.cartesianChart1.Location = new System.Drawing.Point(37, 22);
            this.cartesianChart1.Name = "cartesianChart1";
            this.cartesianChart1.Size = new System.Drawing.Size(550, 550);
            this.cartesianChart1.TabIndex = 0;
            this.cartesianChart1.Load += new System.EventHandler(this.cartesianChart1_Load);
            // 
            // RPNTextBox
            // 
            this.RPNTextBox.Font = new System.Drawing.Font("Segoe UI", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.RPNTextBox.Location = new System.Drawing.Point(740, 275);
            this.RPNTextBox.Name = "RPNTextBox";
            this.RPNTextBox.Size = new System.Drawing.Size(395, 52);
            this.RPNTextBox.TabIndex = 1;
            this.RPNTextBox.TextChanged += new System.EventHandler(this.RPNTextBox_TextChanged);
            // 
            // BitmapButton
            // 
            this.BitmapButton.Font = new System.Drawing.Font("Segoe UI", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.BitmapButton.Location = new System.Drawing.Point(12, 569);
            this.BitmapButton.Name = "BitmapButton";
            this.BitmapButton.Size = new System.Drawing.Size(300, 50);
            this.BitmapButton.TabIndex = 2;
            this.BitmapButton.Text = "Bitmap";
            this.BitmapButton.UseVisualStyleBackColor = true;
            this.BitmapButton.Click += new System.EventHandler(this.BitmapButton_Click);
            // 
            // FunctionButton
            // 
            this.FunctionButton.Font = new System.Drawing.Font("Segoe UI", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FunctionButton.Location = new System.Drawing.Point(12, 922);
            this.FunctionButton.Name = "FunctionButton";
            this.FunctionButton.Size = new System.Drawing.Size(300, 80);
            this.FunctionButton.TabIndex = 3;
            this.FunctionButton.Text = "Function";
            this.FunctionButton.UseVisualStyleBackColor = true;
            this.FunctionButton.Click += new System.EventHandler(this.FunctionButton_Click);
            // 
            // GridButton
            // 
            this.GridButton.Font = new System.Drawing.Font("Segoe UI", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.GridButton.Location = new System.Drawing.Point(12, 717);
            this.GridButton.Name = "GridButton";
            this.GridButton.Size = new System.Drawing.Size(147, 52);
            this.GridButton.TabIndex = 4;
            this.GridButton.Text = "Grid";
            this.GridButton.UseVisualStyleBackColor = true;
            this.GridButton.Click += new System.EventHandler(this.GridButton_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // RPNInputLabel
            // 
            this.RPNInputLabel.AutoSize = true;
            this.RPNInputLabel.Font = new System.Drawing.Font("Segoe UI", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.RPNInputLabel.Location = new System.Drawing.Point(649, 278);
            this.RPNInputLabel.Name = "RPNInputLabel";
            this.RPNInputLabel.Size = new System.Drawing.Size(68, 46);
            this.RPNInputLabel.TabIndex = 6;
            this.RPNInputLabel.Text = "y =";
            this.RPNInputLabel.Click += new System.EventHandler(this.RPNInputLabel_Click);
            // 
            // a1
            // 
            this.a1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.a1.Location = new System.Drawing.Point(333, 630);
            this.a1.Name = "a1";
            this.a1.Size = new System.Drawing.Size(150, 39);
            this.a1.TabIndex = 8;
            this.a1.Text = "1";
            this.a1.TextChanged += new System.EventHandler(this.a1_TextChanged);
            // 
            // d1
            // 
            this.d1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.d1.Location = new System.Drawing.Point(491, 676);
            this.d1.Name = "d1";
            this.d1.Size = new System.Drawing.Size(150, 39);
            this.d1.TabIndex = 9;
            this.d1.Text = "1";
            this.d1.TextChanged += new System.EventHandler(this.d1_TextChanged);
            // 
            // b1
            // 
            this.b1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.b1.Location = new System.Drawing.Point(491, 630);
            this.b1.Name = "b1";
            this.b1.Size = new System.Drawing.Size(150, 39);
            this.b1.TabIndex = 10;
            this.b1.Text = "0";
            this.b1.TextChanged += new System.EventHandler(this.b1_TextChanged);
            // 
            // c1
            // 
            this.c1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.c1.Location = new System.Drawing.Point(333, 675);
            this.c1.Name = "c1";
            this.c1.Size = new System.Drawing.Size(150, 39);
            this.c1.TabIndex = 11;
            this.c1.Text = "0";
            this.c1.TextChanged += new System.EventHandler(this.c1_TextChanged);
            // 
            // c2
            // 
            this.c2.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.c2.Location = new System.Drawing.Point(647, 675);
            this.c2.Name = "c2";
            this.c2.Size = new System.Drawing.Size(150, 39);
            this.c2.TabIndex = 15;
            this.c2.Text = "0";
            this.c2.TextChanged += new System.EventHandler(this.c2_TextChanged);
            // 
            // b2
            // 
            this.b2.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.b2.Location = new System.Drawing.Point(803, 630);
            this.b2.Name = "b2";
            this.b2.Size = new System.Drawing.Size(150, 39);
            this.b2.TabIndex = 14;
            this.b2.Text = "0";
            this.b2.TextChanged += new System.EventHandler(this.b2_TextChanged);
            // 
            // d2
            // 
            this.d2.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.d2.Location = new System.Drawing.Point(803, 675);
            this.d2.Name = "d2";
            this.d2.Size = new System.Drawing.Size(150, 39);
            this.d2.TabIndex = 13;
            this.d2.Text = "1";
            this.d2.TextChanged += new System.EventHandler(this.d2_TextChanged);
            // 
            // a2
            // 
            this.a2.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.a2.Location = new System.Drawing.Point(647, 630);
            this.a2.Name = "a2";
            this.a2.Size = new System.Drawing.Size(150, 39);
            this.a2.TabIndex = 12;
            this.a2.Text = "1";
            this.a2.TextChanged += new System.EventHandler(this.a2_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(315, 729);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 46);
            this.label1.TabIndex = 16;
            this.label1.Text = "det = ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(647, 729);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 46);
            this.label2.TabIndex = 17;
            this.label2.Text = "det = ";
            // 
            // InvariantLinesButton
            // 
            this.InvariantLinesButton.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.InvariantLinesButton.Location = new System.Drawing.Point(12, 775);
            this.InvariantLinesButton.Name = "InvariantLinesButton";
            this.InvariantLinesButton.Size = new System.Drawing.Size(195, 40);
            this.InvariantLinesButton.TabIndex = 18;
            this.InvariantLinesButton.Text = "Invariant Lines";
            this.InvariantLinesButton.UseVisualStyleBackColor = true;
            this.InvariantLinesButton.Click += new System.EventHandler(this.InvariantLinesButton_Click);
            // 
            // EigenvectorsButton
            // 
            this.EigenvectorsButton.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.EigenvectorsButton.Location = new System.Drawing.Point(12, 625);
            this.EigenvectorsButton.Name = "EigenvectorsButton";
            this.EigenvectorsButton.Size = new System.Drawing.Size(300, 40);
            this.EigenvectorsButton.TabIndex = 19;
            this.EigenvectorsButton.Text = "Eigenvectors";
            this.EigenvectorsButton.UseVisualStyleBackColor = true;
            this.EigenvectorsButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // LinesOfInvariantPointsButton
            // 
            this.LinesOfInvariantPointsButton.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.LinesOfInvariantPointsButton.Location = new System.Drawing.Point(213, 775);
            this.LinesOfInvariantPointsButton.Name = "LinesOfInvariantPointsButton";
            this.LinesOfInvariantPointsButton.Size = new System.Drawing.Size(300, 40);
            this.LinesOfInvariantPointsButton.TabIndex = 20;
            this.LinesOfInvariantPointsButton.Text = "Lines of Invariant Points";
            this.LinesOfInvariantPointsButton.UseVisualStyleBackColor = true;
            this.LinesOfInvariantPointsButton.Click += new System.EventHandler(this.LinesOfInvariantPointsButton_Click);
            // 
            // SwitchButton
            // 
            this.SwitchButton.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.SwitchButton.Location = new System.Drawing.Point(803, 518);
            this.SwitchButton.Name = "SwitchButton";
            this.SwitchButton.Size = new System.Drawing.Size(150, 50);
            this.SwitchButton.TabIndex = 21;
            this.SwitchButton.Text = "Switch";
            this.SwitchButton.UseVisualStyleBackColor = true;
            this.SwitchButton.Click += new System.EventHandler(this.SwitchButton_Click);
            // 
            // MultiplyRightButton
            // 
            this.MultiplyRightButton.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MultiplyRightButton.Location = new System.Drawing.Point(647, 518);
            this.MultiplyRightButton.Name = "MultiplyRightButton";
            this.MultiplyRightButton.Size = new System.Drawing.Size(150, 50);
            this.MultiplyRightButton.TabIndex = 22;
            this.MultiplyRightButton.Text = "Multiply";
            this.MultiplyRightButton.UseVisualStyleBackColor = true;
            this.MultiplyRightButton.Click += new System.EventHandler(this.MultiplyRightButton_Click);
            // 
            // ApplyMatrixButton
            // 
            this.ApplyMatrixButton.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ApplyMatrixButton.Location = new System.Drawing.Point(959, 518);
            this.ApplyMatrixButton.Name = "ApplyMatrixButton";
            this.ApplyMatrixButton.Size = new System.Drawing.Size(150, 50);
            this.ApplyMatrixButton.TabIndex = 23;
            this.ApplyMatrixButton.Text = "Apply";
            this.ApplyMatrixButton.UseVisualStyleBackColor = true;
            this.ApplyMatrixButton.Click += new System.EventHandler(this.ApplyMatrixButton_Click);
            // 
            // InverseLeft
            // 
            this.InverseLeft.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.InverseLeft.Location = new System.Drawing.Point(333, 569);
            this.InverseLeft.Name = "InverseLeft";
            this.InverseLeft.Size = new System.Drawing.Size(150, 50);
            this.InverseLeft.TabIndex = 24;
            this.InverseLeft.Text = "Inverse";
            this.InverseLeft.UseVisualStyleBackColor = true;
            this.InverseLeft.Click += new System.EventHandler(this.InverseLeft_Click);
            // 
            // InverseRight
            // 
            this.InverseRight.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.InverseRight.Location = new System.Drawing.Point(647, 569);
            this.InverseRight.Name = "InverseRight";
            this.InverseRight.Size = new System.Drawing.Size(150, 50);
            this.InverseRight.TabIndex = 25;
            this.InverseRight.Text = "Inverse";
            this.InverseRight.UseVisualStyleBackColor = true;
            this.InverseRight.Click += new System.EventHandler(this.InverseRight_Click);
            // 
            // ReflectionButton
            // 
            this.ReflectionButton.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ReflectionButton.Location = new System.Drawing.Point(958, 615);
            this.ReflectionButton.Name = "ReflectionButton";
            this.ReflectionButton.Size = new System.Drawing.Size(120, 30);
            this.ReflectionButton.TabIndex = 26;
            this.ReflectionButton.Text = "Reflection";
            this.ReflectionButton.UseVisualStyleBackColor = true;
            this.ReflectionButton.Click += new System.EventHandler(this.ReflectionButton_Click);
            // 
            // TransposeLeft
            // 
            this.TransposeLeft.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TransposeLeft.Location = new System.Drawing.Point(491, 569);
            this.TransposeLeft.Name = "TransposeLeft";
            this.TransposeLeft.Size = new System.Drawing.Size(150, 50);
            this.TransposeLeft.TabIndex = 27;
            this.TransposeLeft.Text = "Transpose";
            this.TransposeLeft.UseVisualStyleBackColor = true;
            this.TransposeLeft.Click += new System.EventHandler(this.TransposeLeft_Click);
            // 
            // TransposeRight
            // 
            this.TransposeRight.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TransposeRight.Location = new System.Drawing.Point(803, 569);
            this.TransposeRight.Name = "TransposeRight";
            this.TransposeRight.Size = new System.Drawing.Size(150, 50);
            this.TransposeRight.TabIndex = 28;
            this.TransposeRight.Text = "Transpose";
            this.TransposeRight.UseVisualStyleBackColor = true;
            this.TransposeRight.Click += new System.EventHandler(this.TransposeRight_Click);
            // 
            // RotationButton
            // 
            this.RotationButton.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.RotationButton.Location = new System.Drawing.Point(958, 651);
            this.RotationButton.Name = "RotationButton";
            this.RotationButton.Size = new System.Drawing.Size(120, 30);
            this.RotationButton.TabIndex = 29;
            this.RotationButton.Text = "Rotation";
            this.RotationButton.UseVisualStyleBackColor = true;
            this.RotationButton.Click += new System.EventHandler(this.RotationButton_Click);
            // 
            // EnlargementButton
            // 
            this.EnlargementButton.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.EnlargementButton.Location = new System.Drawing.Point(958, 687);
            this.EnlargementButton.Name = "EnlargementButton";
            this.EnlargementButton.Size = new System.Drawing.Size(120, 30);
            this.EnlargementButton.TabIndex = 30;
            this.EnlargementButton.Text = "Enlargement";
            this.EnlargementButton.UseVisualStyleBackColor = true;
            this.EnlargementButton.Click += new System.EventHandler(this.EnlargementButton_Click);
            // 
            // ShearingButton
            // 
            this.ShearingButton.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ShearingButton.Location = new System.Drawing.Point(958, 579);
            this.ShearingButton.Name = "ShearingButton";
            this.ShearingButton.Size = new System.Drawing.Size(120, 30);
            this.ShearingButton.TabIndex = 31;
            this.ShearingButton.Text = "Shearing";
            this.ShearingButton.UseVisualStyleBackColor = true;
            this.ShearingButton.Click += new System.EventHandler(this.ShearingButton_Click);
            // 
            // fTimer
            // 
            this.fTimer.Enabled = true;
            this.fTimer.Tick += new System.EventHandler(this.fTimer_Tick);
            // 
            // AddButton
            // 
            this.AddButton.Font = new System.Drawing.Font("Segoe UI", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.AddButton.Location = new System.Drawing.Point(958, 184);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(240, 75);
            this.AddButton.TabIndex = 32;
            this.AddButton.Text = "Add";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // FunctionList
            // 
            this.FunctionList.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FunctionList.FormattingEnabled = true;
            this.FunctionList.ItemHeight = 23;
            this.FunctionList.Location = new System.Drawing.Point(648, 12);
            this.FunctionList.Name = "FunctionList";
            this.FunctionList.Size = new System.Drawing.Size(305, 257);
            this.FunctionList.TabIndex = 33;
            this.FunctionList.SelectedIndexChanged += new System.EventHandler(this.FunctionList_SelectedIndexChanged);
            // 
            // checkMatrixTimer
            // 
            this.checkMatrixTimer.Interval = 10;
            this.checkMatrixTimer.Tick += new System.EventHandler(this.checkMatrixTimer_Tick);
            // 
            // detA
            // 
            this.detA.AutoSize = true;
            this.detA.Font = new System.Drawing.Font("Segoe UI", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.detA.Location = new System.Drawing.Point(435, 729);
            this.detA.Name = "detA";
            this.detA.Size = new System.Drawing.Size(0, 46);
            this.detA.TabIndex = 34;
            // 
            // detB
            // 
            this.detB.AutoSize = true;
            this.detB.Font = new System.Drawing.Font("Segoe UI", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.detB.Location = new System.Drawing.Point(767, 729);
            this.detB.Name = "detB";
            this.detB.Size = new System.Drawing.Size(0, 46);
            this.detB.TabIndex = 35;
            // 
            // RemoveButton
            // 
            this.RemoveButton.Font = new System.Drawing.Font("Segoe UI", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.RemoveButton.Location = new System.Drawing.Point(959, 22);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(240, 75);
            this.RemoveButton.TabIndex = 36;
            this.RemoveButton.Text = "Remove";
            this.RemoveButton.UseVisualStyleBackColor = true;
            this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // ClearButton
            // 
            this.ClearButton.Font = new System.Drawing.Font("Segoe UI", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ClearButton.Location = new System.Drawing.Point(959, 103);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(240, 75);
            this.ClearButton.TabIndex = 37;
            this.ClearButton.Text = "Clear";
            this.ClearButton.UseVisualStyleBackColor = true;
            this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // kTextbox
            // 
            this.kTextbox.Font = new System.Drawing.Font("Segoe UI", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.kTextbox.Location = new System.Drawing.Point(700, 386);
            this.kTextbox.Name = "kTextbox";
            this.kTextbox.Size = new System.Drawing.Size(100, 52);
            this.kTextbox.TabIndex = 41;
            this.kTextbox.Text = "1";
            this.kTextbox.TextChanged += new System.EventHandler(this.kTextBox_TextChanged);
            // 
            // pTextbox
            // 
            this.pTextbox.Font = new System.Drawing.Font("Segoe UI", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.pTextbox.Location = new System.Drawing.Point(874, 389);
            this.pTextbox.Name = "pTextbox";
            this.pTextbox.Size = new System.Drawing.Size(100, 52);
            this.pTextbox.TabIndex = 42;
            this.pTextbox.Text = "1";
            this.pTextbox.TextChanged += new System.EventHandler(this.pTextbox_TextChanged);
            // 
            // qTextbox
            // 
            this.qTextbox.Font = new System.Drawing.Font("Segoe UI", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.qTextbox.Location = new System.Drawing.Point(1045, 389);
            this.qTextbox.Name = "qTextbox";
            this.qTextbox.Size = new System.Drawing.Size(100, 52);
            this.qTextbox.TabIndex = 43;
            this.qTextbox.Text = "1";
            this.qTextbox.TextChanged += new System.EventHandler(this.qTextbox_TextChanged);
            // 
            // piButton
            // 
            this.piButton.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.piButton.Location = new System.Drawing.Point(649, 462);
            this.piButton.Name = "piButton";
            this.piButton.Size = new System.Drawing.Size(50, 50);
            this.piButton.TabIndex = 44;
            this.piButton.Text = "𝜋";
            this.piButton.UseVisualStyleBackColor = true;
            this.piButton.Click += new System.EventHandler(this.piButton_Click);
            // 
            // eButton
            // 
            this.eButton.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.eButton.Location = new System.Drawing.Point(1313, 180);
            this.eButton.Name = "eButton";
            this.eButton.Size = new System.Drawing.Size(50, 50);
            this.eButton.TabIndex = 45;
            this.eButton.Text = "e";
            this.eButton.UseVisualStyleBackColor = true;
            this.eButton.Click += new System.EventHandler(this.eButton_Click);
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button4.Location = new System.Drawing.Point(1311, 292);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(50, 50);
            this.button4.TabIndex = 47;
            this.button4.Text = ")";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button5.Location = new System.Drawing.Point(1311, 236);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(50, 50);
            this.button5.TabIndex = 46;
            this.button5.Text = "(";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button7
            // 
            this.button7.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button7.Location = new System.Drawing.Point(1368, 292);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(50, 50);
            this.button7.TabIndex = 49;
            this.button7.Text = "-";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button8.Location = new System.Drawing.Point(1368, 348);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(50, 50);
            this.button8.TabIndex = 48;
            this.button8.Text = "+";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button10
            // 
            this.button10.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button10.Location = new System.Drawing.Point(1313, 124);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(50, 50);
            this.button10.TabIndex = 51;
            this.button10.Text = "/";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // button11
            // 
            this.button11.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button11.Location = new System.Drawing.Point(1311, 68);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(50, 50);
            this.button11.TabIndex = 50;
            this.button11.Text = "*";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // absButton
            // 
            this.absButton.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.absButton.Location = new System.Drawing.Point(1368, 68);
            this.absButton.Name = "absButton";
            this.absButton.Size = new System.Drawing.Size(64, 50);
            this.absButton.TabIndex = 53;
            this.absButton.Text = "abs";
            this.absButton.UseVisualStyleBackColor = true;
            this.absButton.Click += new System.EventHandler(this.button15_Click);
            // 
            // button16
            // 
            this.button16.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button16.Location = new System.Drawing.Point(1368, 236);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(50, 50);
            this.button16.TabIndex = 52;
            this.button16.Text = "^";
            this.button16.UseVisualStyleBackColor = true;
            this.button16.Click += new System.EventHandler(this.button16_Click);
            // 
            // sinButton
            // 
            this.sinButton.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.sinButton.Location = new System.Drawing.Point(1368, 12);
            this.sinButton.Name = "sinButton";
            this.sinButton.Size = new System.Drawing.Size(64, 50);
            this.sinButton.TabIndex = 54;
            this.sinButton.Text = "sin";
            this.sinButton.UseVisualStyleBackColor = true;
            this.sinButton.Click += new System.EventHandler(this.sinButton_Click);
            // 
            // cosButton
            // 
            this.cosButton.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cosButton.Location = new System.Drawing.Point(1368, 124);
            this.cosButton.Name = "cosButton";
            this.cosButton.Size = new System.Drawing.Size(64, 50);
            this.cosButton.TabIndex = 55;
            this.cosButton.Text = "cos";
            this.cosButton.UseVisualStyleBackColor = true;
            this.cosButton.Click += new System.EventHandler(this.cosButton_Click);
            // 
            // button19
            // 
            this.button19.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button19.Location = new System.Drawing.Point(1311, 12);
            this.button19.Name = "button19";
            this.button19.Size = new System.Drawing.Size(50, 50);
            this.button19.TabIndex = 56;
            this.button19.Text = "ln";
            this.button19.UseVisualStyleBackColor = true;
            this.button19.Click += new System.EventHandler(this.button19_Click);
            // 
            // button20
            // 
            this.button20.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button20.Location = new System.Drawing.Point(1368, 180);
            this.button20.Name = "button20";
            this.button20.Size = new System.Drawing.Size(64, 50);
            this.button20.TabIndex = 57;
            this.button20.Text = "log";
            this.button20.UseVisualStyleBackColor = true;
            this.button20.Click += new System.EventHandler(this.button20_Click);
            // 
            // UnitSquareButton
            // 
            this.UnitSquareButton.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.UnitSquareButton.Location = new System.Drawing.Point(12, 671);
            this.UnitSquareButton.Name = "UnitSquareButton";
            this.UnitSquareButton.Size = new System.Drawing.Size(147, 40);
            this.UnitSquareButton.TabIndex = 58;
            this.UnitSquareButton.Text = "Unit Square";
            this.UnitSquareButton.UseVisualStyleBackColor = true;
            this.UnitSquareButton.Click += new System.EventHandler(this.UnitSquareButton_Click);
            // 
            // TriangleButton
            // 
            this.TriangleButton.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TriangleButton.Location = new System.Drawing.Point(165, 671);
            this.TriangleButton.Name = "TriangleButton";
            this.TriangleButton.Size = new System.Drawing.Size(147, 40);
            this.TriangleButton.TabIndex = 59;
            this.TriangleButton.Text = "Triangle";
            this.TriangleButton.UseVisualStyleBackColor = true;
            // 
            // AnimateButton
            // 
            this.AnimateButton.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.AnimateButton.Location = new System.Drawing.Point(1115, 518);
            this.AnimateButton.Name = "AnimateButton";
            this.AnimateButton.Size = new System.Drawing.Size(150, 50);
            this.AnimateButton.TabIndex = 60;
            this.AnimateButton.Text = "Animate";
            this.AnimateButton.UseVisualStyleBackColor = true;
            this.AnimateButton.Click += new System.EventHandler(this.AnimateButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(648, 330);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(138, 46);
            this.label3.TabIndex = 62;
            this.label3.Text = "Bounds:";
            // 
            // BoundsTextBox
            // 
            this.BoundsTextBox.Font = new System.Drawing.Font("Segoe UI", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.BoundsTextBox.Location = new System.Drawing.Point(790, 330);
            this.BoundsTextBox.Name = "BoundsTextBox";
            this.BoundsTextBox.Size = new System.Drawing.Size(108, 52);
            this.BoundsTextBox.TabIndex = 63;
            this.BoundsTextBox.Text = "10";
            this.BoundsTextBox.TextChanged += new System.EventHandler(this.BoundsTextBox_TextChanged);
            // 
            // MatrixList
            // 
            this.MatrixList.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MatrixList.FormattingEnabled = true;
            this.MatrixList.ItemHeight = 23;
            this.MatrixList.Location = new System.Drawing.Point(1084, 574);
            this.MatrixList.Name = "MatrixList";
            this.MatrixList.Size = new System.Drawing.Size(197, 188);
            this.MatrixList.TabIndex = 64;
            // 
            // ClearMatrix
            // 
            this.ClearMatrix.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ClearMatrix.Location = new System.Drawing.Point(1339, 701);
            this.ClearMatrix.Name = "ClearMatrix";
            this.ClearMatrix.Size = new System.Drawing.Size(92, 32);
            this.ClearMatrix.TabIndex = 66;
            this.ClearMatrix.Text = "Clear";
            this.ClearMatrix.UseVisualStyleBackColor = true;
            this.ClearMatrix.Click += new System.EventHandler(this.ClearMatrix_Click);
            // 
            // RemoveMatrix
            // 
            this.RemoveMatrix.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.RemoveMatrix.Location = new System.Drawing.Point(1340, 663);
            this.RemoveMatrix.Name = "RemoveMatrix";
            this.RemoveMatrix.Size = new System.Drawing.Size(92, 32);
            this.RemoveMatrix.TabIndex = 65;
            this.RemoveMatrix.Text = "Remove";
            this.RemoveMatrix.UseVisualStyleBackColor = true;
            this.RemoveMatrix.Click += new System.EventHandler(this.RemoveMatrix_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(655, 409);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 15);
            this.label4.TabIndex = 67;
            this.label4.Text = "K";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(854, 409);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 15);
            this.label5.TabIndex = 68;
            this.label5.Text = "P";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(1025, 409);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(16, 15);
            this.label6.TabIndex = 69;
            this.label6.Text = "Q";
            // 
            // LinesOf
            // 
            this.LinesOf.AutoSize = true;
            this.LinesOf.Location = new System.Drawing.Point(1057, 808);
            this.LinesOf.Name = "LinesOf";
            this.LinesOf.Size = new System.Drawing.Size(54, 15);
            this.LinesOf.TabIndex = 70;
            this.LinesOf.Text = "InvLine1:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(1057, 840);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(54, 15);
            this.label8.TabIndex = 71;
            this.label8.Text = "InvLine2:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(1056, 779);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(78, 15);
            this.label7.TabIndex = 72;
            this.label7.Text = "LOfInvPoints:";
            // 
            // InvLine1TextBox
            // 
            this.InvLine1TextBox.Location = new System.Drawing.Point(1127, 805);
            this.InvLine1TextBox.Name = "InvLine1TextBox";
            this.InvLine1TextBox.Size = new System.Drawing.Size(100, 23);
            this.InvLine1TextBox.TabIndex = 73;
            // 
            // InvLine2TextBox
            // 
            this.InvLine2TextBox.Location = new System.Drawing.Point(1127, 835);
            this.InvLine2TextBox.Name = "InvLine2TextBox";
            this.InvLine2TextBox.Size = new System.Drawing.Size(100, 23);
            this.InvLine2TextBox.TabIndex = 74;
            // 
            // LOfInvPointsTextBox
            // 
            this.LOfInvPointsTextBox.Location = new System.Drawing.Point(1138, 776);
            this.LOfInvPointsTextBox.Name = "LOfInvPointsTextBox";
            this.LOfInvPointsTextBox.Size = new System.Drawing.Size(100, 23);
            this.LOfInvPointsTextBox.TabIndex = 75;
            // 
            // vLabel
            // 
            this.vLabel.AutoSize = true;
            this.vLabel.Font = new System.Drawing.Font("Segoe UI", 21F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.vLabel.Location = new System.Drawing.Point(660, 801);
            this.vLabel.Name = "vLabel";
            this.vLabel.Size = new System.Drawing.Size(34, 38);
            this.vLabel.TabIndex = 77;
            this.vLabel.Text = "V";
            // 
            // vTextBox
            // 
            this.vTextBox.Font = new System.Drawing.Font("Segoe UI", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.vTextBox.Location = new System.Drawing.Point(700, 793);
            this.vTextBox.Name = "vTextBox";
            this.vTextBox.Size = new System.Drawing.Size(100, 52);
            this.vTextBox.TabIndex = 76;
            this.vTextBox.Text = "1";
            this.vTextBox.TextChanged += new System.EventHandler(this.vTextBox_TextChanged);
            // 
            // AnimateTimer
            // 
            this.AnimateTimer.Interval = 1;
            this.AnimateTimer.Tick += new System.EventHandler(this.AnimateTimer_Tick);
            // 
            // MatGrapher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1284, 881);
            this.Controls.Add(this.vLabel);
            this.Controls.Add(this.vTextBox);
            this.Controls.Add(this.LOfInvPointsTextBox);
            this.Controls.Add(this.InvLine2TextBox);
            this.Controls.Add(this.InvLine1TextBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.LinesOf);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ClearMatrix);
            this.Controls.Add(this.RemoveMatrix);
            this.Controls.Add(this.MatrixList);
            this.Controls.Add(this.BoundsTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.AnimateButton);
            this.Controls.Add(this.TriangleButton);
            this.Controls.Add(this.UnitSquareButton);
            this.Controls.Add(this.button20);
            this.Controls.Add(this.button19);
            this.Controls.Add(this.cosButton);
            this.Controls.Add(this.sinButton);
            this.Controls.Add(this.absButton);
            this.Controls.Add(this.button16);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.eButton);
            this.Controls.Add(this.piButton);
            this.Controls.Add(this.qTextbox);
            this.Controls.Add(this.pTextbox);
            this.Controls.Add(this.kTextbox);
            this.Controls.Add(this.ClearButton);
            this.Controls.Add(this.RemoveButton);
            this.Controls.Add(this.detB);
            this.Controls.Add(this.detA);
            this.Controls.Add(this.FunctionList);
            this.Controls.Add(this.AddButton);
            this.Controls.Add(this.ShearingButton);
            this.Controls.Add(this.EnlargementButton);
            this.Controls.Add(this.RotationButton);
            this.Controls.Add(this.TransposeRight);
            this.Controls.Add(this.TransposeLeft);
            this.Controls.Add(this.ReflectionButton);
            this.Controls.Add(this.InverseRight);
            this.Controls.Add(this.InverseLeft);
            this.Controls.Add(this.ApplyMatrixButton);
            this.Controls.Add(this.MultiplyRightButton);
            this.Controls.Add(this.SwitchButton);
            this.Controls.Add(this.LinesOfInvariantPointsButton);
            this.Controls.Add(this.EigenvectorsButton);
            this.Controls.Add(this.InvariantLinesButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.c2);
            this.Controls.Add(this.b2);
            this.Controls.Add(this.d2);
            this.Controls.Add(this.a2);
            this.Controls.Add(this.c1);
            this.Controls.Add(this.b1);
            this.Controls.Add(this.d1);
            this.Controls.Add(this.a1);
            this.Controls.Add(this.RPNInputLabel);
            this.Controls.Add(this.GridButton);
            this.Controls.Add(this.FunctionButton);
            this.Controls.Add(this.BitmapButton);
            this.Controls.Add(this.RPNTextBox);
            this.Controls.Add(this.cartesianChart1);
            this.Name = "MatGrapher";
            this.Text = "MatGrapher";
            this.Load += new System.EventHandler(this.Form1_Load_1);
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
        private TextBox a1;
        private TextBox d1;
        private TextBox b1;
        private TextBox c1;
        private TextBox c2;
        private TextBox b2;
        private TextBox d2;
        private TextBox a2;
        private Label label1;
        private Label label2;
        private Button InvariantLinesButton;
        private Button EigenvectorsButton;
        private Button LinesOfInvariantPointsButton;
        private Button SwitchButton;
        private Button MultiplyRightButton;
        private Button ApplyMatrixButton;
        private Button InverseLeft;
        private Button InverseRight;
        private Button ReflectionButton;
        private Button TransposeLeft;
        private Button TransposeRight;
        private Button RotationButton;
        private Button EnlargementButton;
        private Button ShearingButton;
        private System.Windows.Forms.Timer fTimer;
        private Button AddButton;
        private ListBox FunctionList;
        private System.Windows.Forms.Timer checkMatrixTimer;
        private Label detA;
        private Label detB;
        private Button RemoveButton;
        private Button ClearButton;
        private TextBox kTextbox;
        private TextBox pTextbox;
        private TextBox qTextbox;
        private Button piButton;
        private Button eButton;
        private Button button4;
        private Button button5;
        private Button button7;
        private Button button8;
        private Button button10;
        private Button button11;
        private Button absButton;
        private Button button16;
        private Button sinButton;
        private Button cosButton;
        private Button button19;
        private Button button20;
        private Button UnitSquareButton;
        private Button TriangleButton;
        private Button AnimateButton;
        private Label label3;
        private TextBox BoundsTextBox;
        private ListBox MatrixList;
        private Button ClearMatrix;
        private Button RemoveMatrix;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label LinesOf;
        private Label label8;
        private Label label7;
        private TextBox InvLine1TextBox;
        private TextBox InvLine2TextBox;
        private TextBox LOfInvPointsTextBox;
        private Label vLabel;
        private TextBox vTextBox;
        private System.Windows.Forms.Timer AnimateTimer;
    }
}