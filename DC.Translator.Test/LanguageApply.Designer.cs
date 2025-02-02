namespace LanguageApplyTest
{
    partial class LanguageApply
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.comboLangList = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.richTexSource = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.richTextTranslation = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.闭环设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.开启本机闭环ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.停止本机闭环ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label4 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboLangList
            // 
            this.comboLangList.FormattingEnabled = true;
            this.comboLangList.Items.AddRange(new object[] {
            "中国",
            "英国",
            "德国",
            "俄国",
            "韩国",
            "日本",
            "法国",
            "意大利"});
            this.comboLangList.Location = new System.Drawing.Point(206, 76);
            this.comboLangList.Name = "comboLangList";
            this.comboLangList.Size = new System.Drawing.Size(121, 20);
            this.comboLangList.TabIndex = 0;
            this.comboLangList.SelectedIndexChanged += new System.EventHandler(this.comboLangList_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Snow;
            this.label1.ForeColor = System.Drawing.Color.DimGray;
            this.label1.Location = new System.Drawing.Point(31, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(143, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "请选择您想要应用的语言:";
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(353, 75);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(60, 23);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "确定";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // richTexSource
            // 
            this.richTexSource.Location = new System.Drawing.Point(116, 110);
            this.richTexSource.Name = "richTexSource";
            this.richTexSource.Size = new System.Drawing.Size(211, 91);
            this.richTexSource.TabIndex = 3;
            this.richTexSource.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 133);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "原文:";
            // 
            // richTextTranslation
            // 
            this.richTextTranslation.Location = new System.Drawing.Point(116, 220);
            this.richTextTranslation.Name = "richTextTranslation";
            this.richTextTranslation.Size = new System.Drawing.Size(211, 104);
            this.richTextTranslation.TabIndex = 5;
            this.richTextTranslation.Text = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 256);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "译文:";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.设置ToolStripMenuItem,
            this.操作ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(551, 25);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 设置ToolStripMenuItem
            // 
            this.设置ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.闭环设置ToolStripMenuItem});
            this.设置ToolStripMenuItem.Name = "设置ToolStripMenuItem";
            this.设置ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.设置ToolStripMenuItem.Text = "设置";
            // 
            // 闭环设置ToolStripMenuItem
            // 
            this.闭环设置ToolStripMenuItem.Name = "闭环设置ToolStripMenuItem";
            this.闭环设置ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.闭环设置ToolStripMenuItem.Text = "闭环设置";
            // 
            // 操作ToolStripMenuItem
            // 
            this.操作ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.开启本机闭环ToolStripMenuItem,
            this.停止本机闭环ToolStripMenuItem,
            this.退出ToolStripMenuItem});
            this.操作ToolStripMenuItem.Name = "操作ToolStripMenuItem";
            this.操作ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.操作ToolStripMenuItem.Text = "操作";
            // 
            // 开启本机闭环ToolStripMenuItem
            // 
            this.开启本机闭环ToolStripMenuItem.Name = "开启本机闭环ToolStripMenuItem";
            this.开启本机闭环ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.开启本机闭环ToolStripMenuItem.Text = "开启本机闭环";
            // 
            // 停止本机闭环ToolStripMenuItem
            // 
            this.停止本机闭环ToolStripMenuItem.Name = "停止本机闭环ToolStripMenuItem";
            this.停止本机闭环ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.停止本机闭环ToolStripMenuItem.Text = "停止本机闭环";
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.退出ToolStripMenuItem.Text = "退出";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(31, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "本机闭环状态：";
            // 
            // LanguageApply
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 344);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.richTextTranslation);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.richTexSource);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboLangList);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "LanguageApply";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "语言应用测试";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboLangList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.RichTextBox richTexSource;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox richTextTranslation;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 闭环设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 操作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 开启本机闭环ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 停止本机闭环ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.Label label4;
    }
}

