namespace CS408Project_client
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_IP = new System.Windows.Forms.TextBox();
            this.textBox_Port = new System.Windows.Forms.TextBox();
            this.textBox_Username = new System.Windows.Forms.TextBox();
            this.FilePath_textBox = new System.Windows.Forms.TextBox();
            this.button_Browse = new System.Windows.Forms.Button();
            this.logs = new System.Windows.Forms.RichTextBox();
            this.button_Connect = new System.Windows.Forms.Button();
            this.button_Send = new System.Windows.Forms.Button();
            this.button_disconnect = new System.Windows.Forms.Button();
            this.fileName_textbox = new System.Windows.Forms.TextBox();
            this.button_copy = new System.Windows.Forms.Button();
            this.button_download = new System.Windows.Forms.Button();
            this.button_delete = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.button_public = new System.Windows.Forms.Button();
            this.button_rtrvPublic = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.button_folderBrowse = new System.Windows.Forms.Button();
            this.textBox_downPath = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textBox_owner = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.button_publicDown = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(58, 41);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 26);
            this.label1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label2.Location = new System.Drawing.Point(39, 44);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(144, 30);
            this.label2.TabIndex = 1;
            this.label2.Text = "IP Address:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label3.Location = new System.Drawing.Point(71, 18);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 30);
            this.label3.TabIndex = 2;
            this.label3.Text = "Port:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label4.Location = new System.Drawing.Point(41, 70);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(139, 30);
            this.label4.TabIndex = 3;
            this.label4.Text = "Username:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label5.Location = new System.Drawing.Point(24, 157);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(298, 30);
            this.label5.TabIndex = 4;
            this.label5.Text = "Select file path to upload:";
            // 
            // textBox_IP
            // 
            this.textBox_IP.Location = new System.Drawing.Point(112, 42);
            this.textBox_IP.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_IP.Name = "textBox_IP";
            this.textBox_IP.Size = new System.Drawing.Size(97, 32);
            this.textBox_IP.TabIndex = 5;
            this.textBox_IP.Text = "127.0.0.1";
            // 
            // textBox_Port
            // 
            this.textBox_Port.Location = new System.Drawing.Point(112, 18);
            this.textBox_Port.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_Port.Name = "textBox_Port";
            this.textBox_Port.Size = new System.Drawing.Size(55, 32);
            this.textBox_Port.TabIndex = 6;
            this.textBox_Port.Text = "1999";
            // 
            // textBox_Username
            // 
            this.textBox_Username.Location = new System.Drawing.Point(112, 69);
            this.textBox_Username.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_Username.Name = "textBox_Username";
            this.textBox_Username.Size = new System.Drawing.Size(97, 32);
            this.textBox_Username.TabIndex = 7;
            this.textBox_Username.Text = "gorki";
            // 
            // FilePath_textBox
            // 
            this.FilePath_textBox.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.FilePath_textBox.Enabled = false;
            this.FilePath_textBox.Location = new System.Drawing.Point(46, 185);
            this.FilePath_textBox.Margin = new System.Windows.Forms.Padding(2);
            this.FilePath_textBox.Name = "FilePath_textBox";
            this.FilePath_textBox.Size = new System.Drawing.Size(191, 32);
            this.FilePath_textBox.TabIndex = 8;
            // 
            // button_Browse
            // 
            this.button_Browse.Enabled = false;
            this.button_Browse.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_Browse.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.button_Browse.Location = new System.Drawing.Point(198, 152);
            this.button_Browse.Margin = new System.Windows.Forms.Padding(2);
            this.button_Browse.Name = "button_Browse";
            this.button_Browse.Size = new System.Drawing.Size(65, 26);
            this.button_Browse.TabIndex = 9;
            this.button_Browse.Text = "Browse";
            this.button_Browse.UseVisualStyleBackColor = true;
            this.button_Browse.Click += new System.EventHandler(this.button_Browse_Click);
            // 
            // logs
            // 
            this.logs.BackColor = System.Drawing.SystemColors.Control;
            this.logs.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.logs.ForeColor = System.Drawing.Color.Black;
            this.logs.Location = new System.Drawing.Point(307, 11);
            this.logs.Margin = new System.Windows.Forms.Padding(2);
            this.logs.Name = "logs";
            this.logs.Size = new System.Drawing.Size(382, 654);
            this.logs.TabIndex = 10;
            this.logs.Text = "";
            // 
            // button_Connect
            // 
            this.button_Connect.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.button_Connect.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_Connect.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.button_Connect.Location = new System.Drawing.Point(42, 97);
            this.button_Connect.Margin = new System.Windows.Forms.Padding(2);
            this.button_Connect.Name = "button_Connect";
            this.button_Connect.Size = new System.Drawing.Size(103, 31);
            this.button_Connect.TabIndex = 11;
            this.button_Connect.Text = "Connect";
            this.button_Connect.UseVisualStyleBackColor = false;
            this.button_Connect.Click += new System.EventHandler(this.button_Connect_Click);
            // 
            // button_Send
            // 
            this.button_Send.BackColor = System.Drawing.SystemColors.Control;
            this.button_Send.Enabled = false;
            this.button_Send.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_Send.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.button_Send.Location = new System.Drawing.Point(105, 213);
            this.button_Send.Margin = new System.Windows.Forms.Padding(2);
            this.button_Send.Name = "button_Send";
            this.button_Send.Size = new System.Drawing.Size(89, 26);
            this.button_Send.TabIndex = 12;
            this.button_Send.Text = "Upload file";
            this.button_Send.UseVisualStyleBackColor = false;
            this.button_Send.Click += new System.EventHandler(this.button_Send_Click);
            // 
            // button_disconnect
            // 
            this.button_disconnect.BackColor = System.Drawing.Color.DimGray;
            this.button_disconnect.Enabled = false;
            this.button_disconnect.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_disconnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.button_disconnect.ForeColor = System.Drawing.Color.Black;
            this.button_disconnect.Location = new System.Drawing.Point(149, 97);
            this.button_disconnect.Margin = new System.Windows.Forms.Padding(2);
            this.button_disconnect.Name = "button_disconnect";
            this.button_disconnect.Size = new System.Drawing.Size(103, 31);
            this.button_disconnect.TabIndex = 13;
            this.button_disconnect.Text = "Disconnect";
            this.button_disconnect.UseVisualStyleBackColor = false;
            this.button_disconnect.Click += new System.EventHandler(this.button_disconnect_Click);
            // 
            // fileName_textbox
            // 
            this.fileName_textbox.Enabled = false;
            this.fileName_textbox.Location = new System.Drawing.Point(33, 547);
            this.fileName_textbox.Margin = new System.Windows.Forms.Padding(2);
            this.fileName_textbox.Name = "fileName_textbox";
            this.fileName_textbox.Size = new System.Drawing.Size(236, 32);
            this.fileName_textbox.TabIndex = 14;
            // 
            // button_copy
            // 
            this.button_copy.Enabled = false;
            this.button_copy.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.button_copy.Location = new System.Drawing.Point(22, 574);
            this.button_copy.Margin = new System.Windows.Forms.Padding(2);
            this.button_copy.Name = "button_copy";
            this.button_copy.Size = new System.Drawing.Size(83, 26);
            this.button_copy.TabIndex = 15;
            this.button_copy.Text = "Copy";
            this.button_copy.UseVisualStyleBackColor = true;
            this.button_copy.Click += new System.EventHandler(this.button_copy_Click_1);
            // 
            // button_download
            // 
            this.button_download.Enabled = false;
            this.button_download.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.button_download.Location = new System.Drawing.Point(75, 604);
            this.button_download.Margin = new System.Windows.Forms.Padding(2);
            this.button_download.Name = "button_download";
            this.button_download.Size = new System.Drawing.Size(163, 28);
            this.button_download.TabIndex = 16;
            this.button_download.Text = "Download your own file";
            this.button_download.UseVisualStyleBackColor = true;
            this.button_download.Click += new System.EventHandler(this.button_download_Click);
            // 
            // button_delete
            // 
            this.button_delete.Enabled = false;
            this.button_delete.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.button_delete.Location = new System.Drawing.Point(200, 574);
            this.button_delete.Margin = new System.Windows.Forms.Padding(2);
            this.button_delete.Name = "button_delete";
            this.button_delete.Size = new System.Drawing.Size(84, 26);
            this.button_delete.TabIndex = 17;
            this.button_delete.Text = "Delete";
            this.button_delete.UseVisualStyleBackColor = true;
            this.button_delete.Click += new System.EventHandler(this.button_delete_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label6.Location = new System.Drawing.Point(67, 507);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(302, 30);
            this.label6.TabIndex = 18;
            this.label6.Text = "Write a filename to copy, ";
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.button1.Location = new System.Drawing.Point(59, 266);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(166, 24);
            this.button1.TabIndex = 19;
            this.button1.Text = "Retrieve my own file list";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.DimGray;
            this.label7.Location = new System.Drawing.Point(8, 128);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(564, 26);
            this.label7.TabIndex = 20;
            this.label7.Text = "______________________________________________";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.DimGray;
            this.label8.Location = new System.Drawing.Point(8, 316);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(564, 26);
            this.label8.TabIndex = 21;
            this.label8.Text = "______________________________________________";
            // 
            // button_public
            // 
            this.button_public.Enabled = false;
            this.button_public.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.button_public.Location = new System.Drawing.Point(113, 574);
            this.button_public.Margin = new System.Windows.Forms.Padding(2);
            this.button_public.Name = "button_public";
            this.button_public.Size = new System.Drawing.Size(83, 26);
            this.button_public.TabIndex = 22;
            this.button_public.Text = "Publicise";
            this.button_public.UseVisualStyleBackColor = true;
            this.button_public.Click += new System.EventHandler(this.button_public_Click);
            // 
            // button_rtrvPublic
            // 
            this.button_rtrvPublic.Enabled = false;
            this.button_rtrvPublic.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.button_rtrvPublic.Location = new System.Drawing.Point(61, 295);
            this.button_rtrvPublic.Margin = new System.Windows.Forms.Padding(2);
            this.button_rtrvPublic.Name = "button_rtrvPublic";
            this.button_rtrvPublic.Size = new System.Drawing.Size(166, 24);
            this.button_rtrvPublic.TabIndex = 23;
            this.button_rtrvPublic.Text = "Retrieve public file list";
            this.button_rtrvPublic.UseVisualStyleBackColor = true;
            this.button_rtrvPublic.Click += new System.EventHandler(this.button_rtrvPublic_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.DimGray;
            this.label9.Location = new System.Drawing.Point(8, 240);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(564, 26);
            this.label9.TabIndex = 24;
            this.label9.Text = "______________________________________________";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.DimGray;
            this.label10.Location = new System.Drawing.Point(7, 479);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(564, 26);
            this.label10.TabIndex = 25;
            this.label10.Text = "______________________________________________";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label11.Location = new System.Drawing.Point(11, 343);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(390, 30);
            this.label11.TabIndex = 26;
            this.label11.Text = "Select a folder path to download: ";
            // 
            // button_folderBrowse
            // 
            this.button_folderBrowse.Enabled = false;
            this.button_folderBrowse.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.button_folderBrowse.Location = new System.Drawing.Point(220, 338);
            this.button_folderBrowse.Margin = new System.Windows.Forms.Padding(2);
            this.button_folderBrowse.Name = "button_folderBrowse";
            this.button_folderBrowse.Size = new System.Drawing.Size(70, 27);
            this.button_folderBrowse.TabIndex = 27;
            this.button_folderBrowse.Text = "Browse";
            this.button_folderBrowse.UseVisualStyleBackColor = true;
            this.button_folderBrowse.Click += new System.EventHandler(this.button_folderBrowse_Click);
            // 
            // textBox_downPath
            // 
            this.textBox_downPath.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.textBox_downPath.Enabled = false;
            this.textBox_downPath.Location = new System.Drawing.Point(46, 376);
            this.textBox_downPath.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_downPath.Name = "textBox_downPath";
            this.textBox_downPath.Size = new System.Drawing.Size(191, 32);
            this.textBox_downPath.TabIndex = 28;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label12.Location = new System.Drawing.Point(54, 523);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(356, 30);
            this.label12.TabIndex = 29;
            this.label12.Text = "download, publicise or delete: ";
            // 
            // textBox_owner
            // 
            this.textBox_owner.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.textBox_owner.Enabled = false;
            this.textBox_owner.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textBox_owner.Location = new System.Drawing.Point(61, 446);
            this.textBox_owner.Name = "textBox_owner";
            this.textBox_owner.Size = new System.Drawing.Size(155, 32);
            this.textBox_owner.TabIndex = 30;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label13.Location = new System.Drawing.Point(62, 407);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(303, 30);
            this.label13.TabIndex = 31;
            this.label13.Text = "Write the owner of the file";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label14.Location = new System.Drawing.Point(46, 469);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(398, 22);
            this.label14.TabIndex = 32;
            this.label14.Text = "(Leave as empty if you\'d download your own file)";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label15.Location = new System.Drawing.Point(22, 423);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(452, 30);
            this.label15.TabIndex = 33;
            this.label15.Text = "if you request to download a public file:";
            // 
            // button_publicDown
            // 
            this.button_publicDown.Enabled = false;
            this.button_publicDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.button_publicDown.Location = new System.Drawing.Point(75, 637);
            this.button_publicDown.Name = "button_publicDown";
            this.button_publicDown.Size = new System.Drawing.Size(163, 28);
            this.button_publicDown.TabIndex = 34;
            this.button_publicDown.Text = "Download a public file";
            this.button_publicDown.UseVisualStyleBackColor = true;
            this.button_publicDown.Click += new System.EventHandler(this.button_publicDown_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 26F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(707, 685);
            this.Controls.Add(this.button_publicDown);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.textBox_owner);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.textBox_downPath);
            this.Controls.Add(this.button_folderBrowse);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.button_rtrvPublic);
            this.Controls.Add(this.button_public);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button_delete);
            this.Controls.Add(this.button_download);
            this.Controls.Add(this.button_copy);
            this.Controls.Add(this.fileName_textbox);
            this.Controls.Add(this.button_disconnect);
            this.Controls.Add(this.button_Send);
            this.Controls.Add(this.button_Connect);
            this.Controls.Add(this.logs);
            this.Controls.Add(this.button_Browse);
            this.Controls.Add(this.FilePath_textBox);
            this.Controls.Add(this.textBox_Username);
            this.Controls.Add(this.textBox_Port);
            this.Controls.Add(this.textBox_IP);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_IP;
        private System.Windows.Forms.TextBox textBox_Port;
        private System.Windows.Forms.TextBox textBox_Username;
        private System.Windows.Forms.TextBox FilePath_textBox;
        private System.Windows.Forms.Button button_Browse;
        private System.Windows.Forms.RichTextBox logs;
        private System.Windows.Forms.Button button_Connect;
        private System.Windows.Forms.Button button_Send;
        private System.Windows.Forms.Button button_disconnect;
        private System.Windows.Forms.TextBox fileName_textbox;
        private System.Windows.Forms.Button button_copy;
        private System.Windows.Forms.Button button_download;
        private System.Windows.Forms.Button button_delete;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button button_public;
        private System.Windows.Forms.Button button_rtrvPublic;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button button_folderBrowse;
        private System.Windows.Forms.TextBox textBox_downPath;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBox_owner;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button button_publicDown;
    }
}

