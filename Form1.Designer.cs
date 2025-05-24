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
            textBoxConsultaIA = new TextBox();
            buttonConsultar = new Button();
            textBoxResultadoAI = new TextBox();
            label1 = new Label();
            label2 = new Label();
            SuspendLayout();
            // 
            // textBoxConsultaIA
            // 
            textBoxConsultaIA.Location = new Point(88, 54);
            textBoxConsultaIA.Multiline = true;
            textBoxConsultaIA.Name = "textBoxConsultaIA";
            textBoxConsultaIA.Size = new Size(530, 54);
            textBoxConsultaIA.TabIndex = 0;
            // 
            // buttonConsultar
            // 
            buttonConsultar.Location = new Point(639, 54);
            buttonConsultar.Name = "buttonConsultar";
            buttonConsultar.Size = new Size(93, 23);
            buttonConsultar.TabIndex = 1;
            buttonConsultar.Text = "Consultar AI";
            buttonConsultar.UseVisualStyleBackColor = true;
            buttonConsultar.Click += buttonConsultar_Click;
            // 
            // textBoxResultadoAI
            // 
            textBoxResultadoAI.Cursor = Cursors.IBeam;
            textBoxResultadoAI.Location = new Point(88, 153);
            textBoxResultadoAI.Margin = new Padding(2);
            textBoxResultadoAI.Multiline = true;
            textBoxResultadoAI.Name = "textBoxResultadoAI";
            textBoxResultadoAI.ReadOnly = true;
            textBoxResultadoAI.ScrollBars = ScrollBars.Vertical;
            textBoxResultadoAI.Size = new Size(583, 132);
            textBoxResultadoAI.TabIndex = 6;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(88, 25);
            label1.Name = "label1";
            label1.Size = new Size(72, 15);
            label1.TabIndex = 9;
            label1.Text = "Consultar AI";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(94, 127);
            label2.Name = "label2";
            label2.Size = new Size(66, 15);
            label2.TabIndex = 10;
            label2.Text = "Respuesta: ";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(textBoxResultadoAI);
            Controls.Add(buttonConsultar);
            Controls.Add(textBoxConsultaIA);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBoxConsultaIA;
        private Button buttonConsultar;
        private TextBox textBoxResultadoAI;
        private Label label1;
        private Label label2;
    }
}
