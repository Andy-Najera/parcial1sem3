namespace proyecto1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            textBoxConsultaIA = new TextBox();
            buttonConsultar = new Button();
            textBoxResultadoAI = new TextBox();
            label1 = new Label();
            label2 = new Label();
            buttonWord = new Button();
            buttonPp = new Button();
            label3 = new Label();
            pictureBox1 = new PictureBox();
            buttonSalir = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // textBoxConsultaIA
            // 
            textBoxConsultaIA.Location = new Point(507, 95);
            textBoxConsultaIA.Margin = new Padding(3, 4, 3, 4);
            textBoxConsultaIA.Multiline = true;
            textBoxConsultaIA.Name = "textBoxConsultaIA";
            textBoxConsultaIA.Size = new Size(435, 32);
            textBoxConsultaIA.TabIndex = 0;
            textBoxConsultaIA.TextChanged += textBoxConsultaIA_TextChanged;
            // 
            // buttonConsultar
            // 
            buttonConsultar.BackgroundImage = (Image)resources.GetObject("buttonConsultar.BackgroundImage");
            buttonConsultar.BackgroundImageLayout = ImageLayout.Zoom;
            buttonConsultar.FlatStyle = FlatStyle.Flat;
            buttonConsultar.Location = new Point(948, 95);
            buttonConsultar.Margin = new Padding(3, 4, 3, 4);
            buttonConsultar.Name = "buttonConsultar";
            buttonConsultar.Size = new Size(39, 32);
            buttonConsultar.TabIndex = 1;
            buttonConsultar.Text = " ";
            buttonConsultar.UseVisualStyleBackColor = true;
            buttonConsultar.Click += buttonConsultar_Click;
            // 
            // textBoxResultadoAI
            // 
            textBoxResultadoAI.Cursor = Cursors.SizeAll;
            textBoxResultadoAI.Location = new Point(25, 47);
            textBoxResultadoAI.Margin = new Padding(2, 3, 2, 3);
            textBoxResultadoAI.Multiline = true;
            textBoxResultadoAI.Name = "textBoxResultadoAI";
            textBoxResultadoAI.ReadOnly = true;
            textBoxResultadoAI.ScrollBars = ScrollBars.Vertical;
            textBoxResultadoAI.Size = new Size(477, 513);
            textBoxResultadoAI.TabIndex = 6;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(672, 68);
            label1.Name = "label1";
            label1.Size = new Size(89, 20);
            label1.TabIndex = 9;
            label1.Text = "Consultar AI";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(199, 24);
            label2.Name = "label2";
            label2.Size = new Size(83, 20);
            label2.TabIndex = 10;
            label2.Text = "Respuesta: ";
            // 
            // buttonWord
            // 
            buttonWord.Anchor = AnchorStyles.None;
            buttonWord.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            buttonWord.BackgroundImage = (Image)resources.GetObject("buttonWord.BackgroundImage");
            buttonWord.BackgroundImageLayout = ImageLayout.Zoom;
            buttonWord.FlatStyle = FlatStyle.Flat;
            buttonWord.Location = new Point(586, 390);
            buttonWord.Name = "buttonWord";
            buttonWord.Size = new Size(107, 97);
            buttonWord.TabIndex = 11;
            buttonWord.UseVisualStyleBackColor = true;
            // 
            // buttonPp
            // 
            buttonPp.BackgroundImage = (Image)resources.GetObject("buttonPp.BackgroundImage");
            buttonPp.BackgroundImageLayout = ImageLayout.Zoom;
            buttonPp.FlatStyle = FlatStyle.Flat;
            buttonPp.Location = new Point(804, 390);
            buttonPp.Name = "buttonPp";
            buttonPp.Size = new Size(107, 97);
            buttonPp.TabIndex = 12;
            buttonPp.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(644, 367);
            label3.Name = "label3";
            label3.Size = new Size(203, 20);
            label3.TabIndex = 13;
            label3.Text = "Convierte tu investigacion en:";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(-140, -54);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(1146, 666);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 14;
            pictureBox1.TabStop = false;
            // 
            // buttonSalir
            // 
            buttonSalir.BackColor = Color.Snow;
            buttonSalir.BackgroundImage = (Image)resources.GetObject("buttonSalir.BackgroundImage");
            buttonSalir.BackgroundImageLayout = ImageLayout.Zoom;
            buttonSalir.FlatStyle = FlatStyle.Flat;
            buttonSalir.ForeColor = Color.White;
            buttonSalir.Location = new Point(861, 537);
            buttonSalir.Name = "buttonSalir";
            buttonSalir.Size = new Size(126, 51);
            buttonSalir.TabIndex = 15;
            buttonSalir.UseVisualStyleBackColor = false;
            buttonSalir.Click += buttonSalir_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(999, 600);
            Controls.Add(buttonSalir);
            Controls.Add(label3);
            Controls.Add(buttonPp);
            Controls.Add(buttonWord);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(textBoxResultadoAI);
            Controls.Add(buttonConsultar);
            Controls.Add(textBoxConsultaIA);
            Controls.Add(pictureBox1);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBoxConsultaIA;
        private Button buttonConsultar;
        private TextBox textBoxResultadoAI;
        private Label label1;
        private Label label2;
        private Button buttonWord;
        private Button buttonPp;
        private Label label3;
        private PictureBox pictureBox1;
        private Button buttonSalir;
    }
}
