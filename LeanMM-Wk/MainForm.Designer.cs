namespace LeanMM_Wk
{
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
            label1 = new Label();
            textBoxServer = new TextBox();
            label2 = new Label();
            textBoxLocalFolder = new TextBox();
            label3 = new Label();
            textBoxUser = new TextBox();
            label4 = new Label();
            buttonConnect = new Button();
            listBoxConsole = new ListBox();
            textBoxPass = new TextBox();
            buttonPickFolder = new Button();
            folderBrowserDialog = new FolderBrowserDialog();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(50, 15);
            label1.TabIndex = 0;
            label1.Text = "Servidor";
            // 
            // textBoxServer
            // 
            textBoxServer.Location = new Point(144, 6);
            textBoxServer.Name = "textBoxServer";
            textBoxServer.Size = new Size(269, 23);
            textBoxServer.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 38);
            label2.Name = "label2";
            label2.Size = new Size(79, 15);
            label2.TabIndex = 2;
            label2.Text = "Carpeta Local";
            // 
            // textBoxLocalFolder
            // 
            textBoxLocalFolder.Location = new Point(144, 35);
            textBoxLocalFolder.Name = "textBoxLocalFolder";
            textBoxLocalFolder.Size = new Size(234, 23);
            textBoxLocalFolder.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 67);
            label3.Name = "label3";
            label3.Size = new Size(47, 15);
            label3.TabIndex = 5;
            label3.Text = "Usuario";
            // 
            // textBoxUser
            // 
            textBoxUser.Location = new Point(144, 64);
            textBoxUser.Name = "textBoxUser";
            textBoxUser.Size = new Size(269, 23);
            textBoxUser.TabIndex = 6;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 96);
            label4.Name = "label4";
            label4.Size = new Size(67, 15);
            label4.TabIndex = 7;
            label4.Text = "Contraseña";
            // 
            // buttonConnect
            // 
            buttonConnect.Location = new Point(338, 122);
            buttonConnect.Name = "buttonConnect";
            buttonConnect.Size = new Size(75, 23);
            buttonConnect.TabIndex = 9;
            buttonConnect.Text = "Conectar";
            buttonConnect.UseVisualStyleBackColor = true;
            buttonConnect.Click += buttonConnect_Click;
            // 
            // listBoxConsole
            // 
            listBoxConsole.FormattingEnabled = true;
            listBoxConsole.Location = new Point(12, 179);
            listBoxConsole.Name = "listBoxConsole";
            listBoxConsole.Size = new Size(401, 259);
            listBoxConsole.TabIndex = 10;
            // 
            // textBoxPass
            // 
            textBoxPass.Location = new Point(144, 93);
            textBoxPass.Name = "textBoxPass";
            textBoxPass.PasswordChar = '*';
            textBoxPass.Size = new Size(269, 23);
            textBoxPass.TabIndex = 8;
            // 
            // buttonPickFolder
            // 
            buttonPickFolder.Location = new Point(384, 35);
            buttonPickFolder.Name = "buttonPickFolder";
            buttonPickFolder.Size = new Size(29, 23);
            buttonPickFolder.TabIndex = 4;
            buttonPickFolder.Text = "...";
            buttonPickFolder.UseVisualStyleBackColor = true;
            buttonPickFolder.Click += buttonPickFolder_Click;
            // 
            // MainForm
            // 
            AcceptButton = buttonConnect;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(425, 450);
            Controls.Add(buttonPickFolder);
            Controls.Add(listBoxConsole);
            Controls.Add(buttonConnect);
            Controls.Add(textBoxPass);
            Controls.Add(textBoxUser);
            Controls.Add(label4);
            Controls.Add(textBoxLocalFolder);
            Controls.Add(label3);
            Controls.Add(textBoxServer);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MainForm";
            Text = "LeanMM";
            Load += MainForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox textBoxServer;
        private Label label2;
        private TextBox textBoxLocalFolder;
        private Label label3;
        private TextBox textBoxUser;
        private Label label4;
        private Button buttonConnect;
        private ListBox listBoxConsole;
        private TextBox textBoxPass;
        private Button buttonPickFolder;
        private FolderBrowserDialog folderBrowserDialog;
    }
}
