namespace Project_step1_server
{
    partial class ServerForm
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
            this.Port = new System.Windows.Forms.Label();
            this.port_textBox = new System.Windows.Forms.TextBox();
            this.listen_button = new System.Windows.Forms.Button();
            this.logs_richTextBox = new System.Windows.Forms.RichTextBox();
            this.path_textBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.browse_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Port
            // 
            this.Port.AutoSize = true;
            this.Port.BackColor = System.Drawing.Color.Transparent;
            this.Port.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.Port.Location = new System.Drawing.Point(96, 20);
            this.Port.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Port.Name = "Port";
            this.Port.Size = new System.Drawing.Size(42, 20);
            this.Port.TabIndex = 0;
            this.Port.Text = "Port:";
            // 
            // port_textBox
            // 
            this.port_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.port_textBox.Location = new System.Drawing.Point(149, 20);
            this.port_textBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.port_textBox.Name = "port_textBox";
            this.port_textBox.Size = new System.Drawing.Size(95, 23);
            this.port_textBox.TabIndex = 1;
            this.port_textBox.Text = "1999";
            // 
            // listen_button
            // 
            this.listen_button.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.listen_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.listen_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.listen_button.Location = new System.Drawing.Point(562, 20);
            this.listen_button.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.listen_button.Name = "listen_button";
            this.listen_button.Size = new System.Drawing.Size(124, 69);
            this.listen_button.TabIndex = 2;
            this.listen_button.Text = "Listen";
            this.listen_button.UseVisualStyleBackColor = false;
            this.listen_button.Click += new System.EventHandler(this.listen_button_Click);
            // 
            // logs_richTextBox
            // 
            this.logs_richTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.logs_richTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.logs_richTextBox.ForeColor = System.Drawing.Color.Black;
            this.logs_richTextBox.Location = new System.Drawing.Point(18, 109);
            this.logs_richTextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.logs_richTextBox.Name = "logs_richTextBox";
            this.logs_richTextBox.Size = new System.Drawing.Size(670, 425);
            this.logs_richTextBox.TabIndex = 3;
            this.logs_richTextBox.Text = "";
            // 
            // path_textBox
            // 
            this.path_textBox.Enabled = false;
            this.path_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.path_textBox.ForeColor = System.Drawing.Color.Transparent;
            this.path_textBox.Location = new System.Drawing.Point(149, 57);
            this.path_textBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.path_textBox.Name = "path_textBox";
            this.path_textBox.Size = new System.Drawing.Size(312, 23);
            this.path_textBox.TabIndex = 4;
            this.path_textBox.Text = "C:\\Users\\pc\\Desktop\\ati";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(11, 58);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "File upload Path:";
            // 
            // browse_button
            // 
            this.browse_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.browse_button.Location = new System.Drawing.Point(472, 52);
            this.browse_button.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.browse_button.Name = "browse_button";
            this.browse_button.Size = new System.Drawing.Size(71, 32);
            this.browse_button.TabIndex = 6;
            this.browse_button.Text = "Browse:";
            this.browse_button.UseVisualStyleBackColor = true;
            this.browse_button.Click += new System.EventHandler(this.browse_button_Click);
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(713, 566);
            this.Controls.Add(this.browse_button);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.path_textBox);
            this.Controls.Add(this.logs_richTextBox);
            this.Controls.Add(this.listen_button);
            this.Controls.Add(this.port_textBox);
            this.Controls.Add(this.Port);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "ServerForm";
            this.Text = "Cloud Server";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Port;
        private System.Windows.Forms.TextBox port_textBox;
        private System.Windows.Forms.Button listen_button;
        private System.Windows.Forms.RichTextBox logs_richTextBox;
        private System.Windows.Forms.TextBox path_textBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button browse_button;
    }
}

