namespace Test.forms
{
    partial class FormWatchVariants
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWatchVariants));
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxVar = new System.Windows.Forms.ComboBox();
            this.listExercises = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonDelete = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Номер варианта";
            // 
            // comboBoxVar
            // 
            this.comboBoxVar.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxVar.FormattingEnabled = true;
            this.comboBoxVar.Location = new System.Drawing.Point(182, 28);
            this.comboBoxVar.Name = "comboBoxVar";
            this.comboBoxVar.Size = new System.Drawing.Size(121, 24);
            this.comboBoxVar.TabIndex = 1;
            this.comboBoxVar.SelectedIndexChanged += new System.EventHandler(this.comboBoxVar_SelectedIndexChanged);
            // 
            // listExercises
            // 
            this.listExercises.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.listExercises.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listExercises.FullRowSelect = true;
            this.listExercises.Location = new System.Drawing.Point(0, 75);
            this.listExercises.Margin = new System.Windows.Forms.Padding(4);
            this.listExercises.Name = "listExercises";
            this.listExercises.Size = new System.Drawing.Size(780, 344);
            this.listExercises.TabIndex = 2;
            this.listExercises.UseCompatibleStateImageBehavior = false;
            this.listExercises.View = System.Windows.Forms.View.Details;
            this.listExercises.SelectedIndexChanged += new System.EventHandler(this.listExercises_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "№";
            this.columnHeader1.Width = 25;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Формулировка";
            this.columnHeader2.Width = 406;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Правильный ответ";
            this.columnHeader3.Width = 145;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Варианты ответа (для закрытого теста)";
            this.columnHeader4.Width = 316;
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(590, 24);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(177, 30);
            this.buttonDelete.TabIndex = 3;
            this.buttonDelete.Text = "Удалить вариант";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(373, 24);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(159, 30);
            this.button1.TabIndex = 4;
            this.button1.Text = "Просмотр задачи";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FormWatchVariants
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 419);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.comboBoxVar);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listExercises);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormWatchVariants";
            this.Text = "Просмотр вариантов";
            this.Load += new System.EventHandler(this.FormWatchVariants_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxVar;
        private System.Windows.Forms.ListView listExercises;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Button button1;
    }
}