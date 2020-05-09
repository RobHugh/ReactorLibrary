namespace ReactorTestApplication
{
    partial class FormTestApplication
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
            this.panelAddresses = new System.Windows.Forms.Panel();
            this.comboBoxLocalAddresses = new System.Windows.Forms.ComboBox();
            this.labelRemotePort = new System.Windows.Forms.Label();
            this.labelLocalPort = new System.Windows.Forms.Label();
            this.labelRemoteAddress = new System.Windows.Forms.Label();
            this.labelLocalAddress = new System.Windows.Forms.Label();
            this.textBoxRemotePort = new System.Windows.Forms.TextBox();
            this.textBoxRemoteAddress = new System.Windows.Forms.TextBox();
            this.textBoxLocalPort = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonStart = new System.Windows.Forms.Button();
            this.labelTestType = new System.Windows.Forms.Label();
            this.comboBoxTestType = new System.Windows.Forms.ComboBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.listViewDataOutput = new System.Windows.Forms.ListView();
            this.logLevel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.logClass = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.logMethod = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.logMessage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panelSendMessage = new System.Windows.Forms.Panel();
            this.buttonSend = new System.Windows.Forms.Button();
            this.textBoxSendMessage = new System.Windows.Forms.TextBox();
            this.labelSendMessage = new System.Windows.Forms.Label();
            this.panelAddresses.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panelSendMessage.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelAddresses
            // 
            this.panelAddresses.Controls.Add(this.comboBoxLocalAddresses);
            this.panelAddresses.Controls.Add(this.labelRemotePort);
            this.panelAddresses.Controls.Add(this.labelLocalPort);
            this.panelAddresses.Controls.Add(this.labelRemoteAddress);
            this.panelAddresses.Controls.Add(this.labelLocalAddress);
            this.panelAddresses.Controls.Add(this.textBoxRemotePort);
            this.panelAddresses.Controls.Add(this.textBoxRemoteAddress);
            this.panelAddresses.Controls.Add(this.textBoxLocalPort);
            this.panelAddresses.Location = new System.Drawing.Point(10, 10);
            this.panelAddresses.Name = "panelAddresses";
            this.panelAddresses.Size = new System.Drawing.Size(375, 230);
            this.panelAddresses.TabIndex = 0;
            // 
            // comboBoxLocalAddresses
            // 
            this.comboBoxLocalAddresses.FormattingEnabled = true;
            this.comboBoxLocalAddresses.Location = new System.Drawing.Point(92, 23);
            this.comboBoxLocalAddresses.Name = "comboBoxLocalAddresses";
            this.comboBoxLocalAddresses.Size = new System.Drawing.Size(245, 21);
            this.comboBoxLocalAddresses.TabIndex = 8;
            // 
            // labelRemotePort
            // 
            this.labelRemotePort.AutoSize = true;
            this.labelRemotePort.Location = new System.Drawing.Point(245, 122);
            this.labelRemotePort.Name = "labelRemotePort";
            this.labelRemotePort.Size = new System.Drawing.Size(25, 13);
            this.labelRemotePort.TabIndex = 7;
            this.labelRemotePort.Text = "port";
            // 
            // labelLocalPort
            // 
            this.labelLocalPort.AutoSize = true;
            this.labelLocalPort.Location = new System.Drawing.Point(51, 62);
            this.labelLocalPort.Name = "labelLocalPort";
            this.labelLocalPort.Size = new System.Drawing.Size(25, 13);
            this.labelLocalPort.TabIndex = 6;
            this.labelLocalPort.Text = "port";
            // 
            // labelRemoteAddress
            // 
            this.labelRemoteAddress.AutoSize = true;
            this.labelRemoteAddress.Location = new System.Drawing.Point(1, 122);
            this.labelRemoteAddress.Name = "labelRemoteAddress";
            this.labelRemoteAddress.Size = new System.Drawing.Size(85, 13);
            this.labelRemoteAddress.TabIndex = 5;
            this.labelRemoteAddress.Text = "Remote Address";
            // 
            // labelLocalAddress
            // 
            this.labelLocalAddress.AutoSize = true;
            this.labelLocalAddress.Location = new System.Drawing.Point(12, 26);
            this.labelLocalAddress.Name = "labelLocalAddress";
            this.labelLocalAddress.Size = new System.Drawing.Size(74, 13);
            this.labelLocalAddress.TabIndex = 4;
            this.labelLocalAddress.Text = "Local Address";
            // 
            // textBoxRemotePort
            // 
            this.textBoxRemotePort.Location = new System.Drawing.Point(286, 119);
            this.textBoxRemotePort.Name = "textBoxRemotePort";
            this.textBoxRemotePort.Size = new System.Drawing.Size(52, 20);
            this.textBoxRemotePort.TabIndex = 3;
            this.textBoxRemotePort.Text = "55555";
            // 
            // textBoxRemoteAddress
            // 
            this.textBoxRemoteAddress.Location = new System.Drawing.Point(92, 119);
            this.textBoxRemoteAddress.Name = "textBoxRemoteAddress";
            this.textBoxRemoteAddress.Size = new System.Drawing.Size(135, 20);
            this.textBoxRemoteAddress.TabIndex = 2;
            this.textBoxRemoteAddress.Text = "192.168.1.169";
            // 
            // textBoxLocalPort
            // 
            this.textBoxLocalPort.Location = new System.Drawing.Point(92, 59);
            this.textBoxLocalPort.Name = "textBoxLocalPort";
            this.textBoxLocalPort.Size = new System.Drawing.Size(52, 20);
            this.textBoxLocalPort.TabIndex = 1;
            this.textBoxLocalPort.Text = "55555";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.buttonStop);
            this.panel2.Controls.Add(this.buttonStart);
            this.panel2.Controls.Add(this.labelTestType);
            this.panel2.Controls.Add(this.comboBoxTestType);
            this.panel2.Location = new System.Drawing.Point(395, 10);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(375, 230);
            this.panel2.TabIndex = 1;
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(167, 88);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(160, 28);
            this.buttonStop.TabIndex = 3;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(167, 54);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(160, 28);
            this.buttonStart.TabIndex = 2;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // labelTestType
            // 
            this.labelTestType.AutoSize = true;
            this.labelTestType.Location = new System.Drawing.Point(36, 21);
            this.labelTestType.Name = "labelTestType";
            this.labelTestType.Size = new System.Drawing.Size(96, 13);
            this.labelTestType.TabIndex = 1;
            this.labelTestType.Text = "Reactor Test Type";
            // 
            // comboBoxTestType
            // 
            this.comboBoxTestType.FormattingEnabled = true;
            this.comboBoxTestType.Items.AddRange(new object[] {
            "Tcp Acceptor Test",
            "Tcp Connector Test",
            "Tcp Keep Alive Acceptor Test",
            "Tcp Keep Alive Connector Test",
            "Udp Broadcast Test",
            "Udp Multicast Test",
            "Udp P2P Test",
            "Tcp Acceptor Stream Test",
            "Tcp Connector Stream Test"});
            this.comboBoxTestType.Location = new System.Drawing.Point(167, 18);
            this.comboBoxTestType.Name = "comboBoxTestType";
            this.comboBoxTestType.Size = new System.Drawing.Size(160, 21);
            this.comboBoxTestType.TabIndex = 0;
            this.comboBoxTestType.SelectedIndexChanged += new System.EventHandler(this.comboBoxTestType_SelectedIndexChanged);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.listViewDataOutput);
            this.panel3.Location = new System.Drawing.Point(10, 247);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1370, 425);
            this.panel3.TabIndex = 2;
            // 
            // listViewDataOutput
            // 
            this.listViewDataOutput.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.logLevel,
            this.logClass,
            this.logMethod,
            this.logMessage});
            this.listViewDataOutput.Location = new System.Drawing.Point(4, 4);
            this.listViewDataOutput.Name = "listViewDataOutput";
            this.listViewDataOutput.Size = new System.Drawing.Size(1360, 415);
            this.listViewDataOutput.TabIndex = 0;
            this.listViewDataOutput.UseCompatibleStateImageBehavior = false;
            this.listViewDataOutput.View = System.Windows.Forms.View.Details;
            // 
            // logLevel
            // 
            this.logLevel.Text = "Level";
            this.logLevel.Width = 50;
            // 
            // logClass
            // 
            this.logClass.Text = "Class";
            this.logClass.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.logClass.Width = 200;
            // 
            // logMethod
            // 
            this.logMethod.Text = "Method";
            this.logMethod.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.logMethod.Width = 200;
            // 
            // logMessage
            // 
            this.logMessage.Text = "Message";
            this.logMessage.Width = 900;
            // 
            // panelSendMessage
            // 
            this.panelSendMessage.Controls.Add(this.buttonSend);
            this.panelSendMessage.Controls.Add(this.textBoxSendMessage);
            this.panelSendMessage.Controls.Add(this.labelSendMessage);
            this.panelSendMessage.Location = new System.Drawing.Point(777, 10);
            this.panelSendMessage.Name = "panelSendMessage";
            this.panelSendMessage.Size = new System.Drawing.Size(597, 230);
            this.panelSendMessage.TabIndex = 3;
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(443, 52);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(138, 23);
            this.buttonSend.TabIndex = 2;
            this.buttonSend.Text = "Send";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // textBoxSendMessage
            // 
            this.textBoxSendMessage.Location = new System.Drawing.Point(103, 18);
            this.textBoxSendMessage.Name = "textBoxSendMessage";
            this.textBoxSendMessage.Size = new System.Drawing.Size(478, 20);
            this.textBoxSendMessage.TabIndex = 1;
            // 
            // labelSendMessage
            // 
            this.labelSendMessage.AutoSize = true;
            this.labelSendMessage.Location = new System.Drawing.Point(19, 20);
            this.labelSendMessage.Name = "labelSendMessage";
            this.labelSendMessage.Size = new System.Drawing.Size(78, 13);
            this.labelSendMessage.TabIndex = 0;
            this.labelSendMessage.Text = "Send Message";
            // 
            // FormTestApplication
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1384, 678);
            this.Controls.Add(this.panelSendMessage);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panelAddresses);
            this.Name = "FormTestApplication";
            this.Text = "Reactor Test Application";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormTestApplication_FormClosing);
            this.Load += new System.EventHandler(this.FormTestApplication_Load);
            this.panelAddresses.ResumeLayout(false);
            this.panelAddresses.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panelSendMessage.ResumeLayout(false);
            this.panelSendMessage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelAddresses;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ListView listViewDataOutput;
        private System.Windows.Forms.Label labelRemotePort;
        private System.Windows.Forms.Label labelLocalPort;
        private System.Windows.Forms.Label labelRemoteAddress;
        private System.Windows.Forms.Label labelLocalAddress;
        private System.Windows.Forms.TextBox textBoxRemotePort;
        private System.Windows.Forms.TextBox textBoxRemoteAddress;
        private System.Windows.Forms.TextBox textBoxLocalPort;
        private System.Windows.Forms.Label labelTestType;
        private System.Windows.Forms.ComboBox comboBoxTestType;
        private System.Windows.Forms.ColumnHeader logLevel;
        private System.Windows.Forms.ColumnHeader logClass;
        private System.Windows.Forms.ColumnHeader logMethod;
        private System.Windows.Forms.ColumnHeader logMessage;
        private System.Windows.Forms.ComboBox comboBoxLocalAddresses;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Panel panelSendMessage;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.TextBox textBoxSendMessage;
        private System.Windows.Forms.Label labelSendMessage;
    }
}

