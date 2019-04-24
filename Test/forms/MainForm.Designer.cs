namespace Test
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.listExercises = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuStrip2 = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.новыйToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.открытьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.сохранитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьКакToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.скачатьРезультатыТестированияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.сменитьПользователяToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выходToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.онтологияToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.управлениеТипамиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.управлениеМетодамиToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.привязкаПутиКЗадачеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.задачиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.добавитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.удалитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.вариантыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.открытьВариантыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.генерацияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.просмотрВариантовToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.сохранитьВариантыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьВариантыКакToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.распечататьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // listExercises
            // 
            this.listExercises.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.listExercises.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listExercises.FullRowSelect = true;
            this.listExercises.Location = new System.Drawing.Point(0, 28);
            this.listExercises.Margin = new System.Windows.Forms.Padding(4);
            this.listExercises.Name = "listExercises";
            this.listExercises.Size = new System.Drawing.Size(1075, 472);
            this.listExercises.TabIndex = 0;
            this.listExercises.UseCompatibleStateImageBehavior = false;
            this.listExercises.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "№";
            this.columnHeader1.Width = 25;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Задание";
            this.columnHeader2.Width = 406;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Параметры";
            this.columnHeader3.Width = 223;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Количество";
            this.columnHeader4.Width = 103;
            // 
            // menuStrip2
            // 
            this.menuStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.онтологияToolStripMenuItem1,
            this.задачиToolStripMenuItem,
            this.вариантыToolStripMenuItem});
            this.menuStrip2.Location = new System.Drawing.Point(0, 0);
            this.menuStrip2.Name = "menuStrip2";
            this.menuStrip2.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip2.Size = new System.Drawing.Size(1075, 28);
            this.menuStrip2.TabIndex = 1;
            this.menuStrip2.Text = "menuStrip2";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.новыйToolStripMenuItem,
            this.открытьToolStripMenuItem,
            this.toolStripMenuItem1,
            this.сохранитьToolStripMenuItem,
            this.сохранитьКакToolStripMenuItem,
            this.toolStripMenuItem2,
            this.скачатьРезультатыТестированияToolStripMenuItem,
            this.toolStripSeparator1,
            this.сменитьПользователяToolStripMenuItem,
            this.выходToolStripMenuItem});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(57, 24);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // новыйToolStripMenuItem
            // 
            this.новыйToolStripMenuItem.Name = "новыйToolStripMenuItem";
            this.новыйToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.новыйToolStripMenuItem.Size = new System.Drawing.Size(331, 26);
            this.новыйToolStripMenuItem.Text = "Новый шаблон";
            this.новыйToolStripMenuItem.Click += new System.EventHandler(this.новыйToolStripMenuItem_Click);
            // 
            // открытьToolStripMenuItem
            // 
            this.открытьToolStripMenuItem.Name = "открытьToolStripMenuItem";
            this.открытьToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.открытьToolStripMenuItem.Size = new System.Drawing.Size(331, 26);
            this.открытьToolStripMenuItem.Text = "Открыть шаблон";
            this.открытьToolStripMenuItem.Click += new System.EventHandler(this.открытьToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(328, 6);
            // 
            // сохранитьToolStripMenuItem
            // 
            this.сохранитьToolStripMenuItem.Name = "сохранитьToolStripMenuItem";
            this.сохранитьToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.сохранитьToolStripMenuItem.Size = new System.Drawing.Size(331, 26);
            this.сохранитьToolStripMenuItem.Text = "Сохранить шаблон";
            this.сохранитьToolStripMenuItem.Click += new System.EventHandler(this.сохранитьToolStripMenuItem_Click);
            // 
            // сохранитьКакToolStripMenuItem
            // 
            this.сохранитьКакToolStripMenuItem.Name = "сохранитьКакToolStripMenuItem";
            this.сохранитьКакToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.S)));
            this.сохранитьКакToolStripMenuItem.Size = new System.Drawing.Size(331, 26);
            this.сохранитьКакToolStripMenuItem.Text = "Сохранить шаблон как...";
            this.сохранитьКакToolStripMenuItem.Click += new System.EventHandler(this.сохранитьКакToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(328, 6);
            // 
            // скачатьРезультатыТестированияToolStripMenuItem
            // 
            this.скачатьРезультатыТестированияToolStripMenuItem.Enabled = false;
            this.скачатьРезультатыТестированияToolStripMenuItem.Name = "скачатьРезультатыТестированияToolStripMenuItem";
            this.скачатьРезультатыТестированияToolStripMenuItem.Size = new System.Drawing.Size(331, 26);
            this.скачатьРезультатыТестированияToolStripMenuItem.Text = "Скачать результаты";
            this.скачатьРезультатыТестированияToolStripMenuItem.Click += new System.EventHandler(this.скачатьРезультатыТестированияToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(328, 6);
            // 
            // сменитьПользователяToolStripMenuItem
            // 
            this.сменитьПользователяToolStripMenuItem.Name = "сменитьПользователяToolStripMenuItem";
            this.сменитьПользователяToolStripMenuItem.Size = new System.Drawing.Size(331, 26);
            this.сменитьПользователяToolStripMenuItem.Text = "Сменить пользователя";
            this.сменитьПользователяToolStripMenuItem.Click += new System.EventHandler(this.сменитьПользователяToolStripMenuItem_Click);
            // 
            // выходToolStripMenuItem
            // 
            this.выходToolStripMenuItem.Name = "выходToolStripMenuItem";
            this.выходToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.выходToolStripMenuItem.Size = new System.Drawing.Size(331, 26);
            this.выходToolStripMenuItem.Text = "Выход";
            this.выходToolStripMenuItem.Click += new System.EventHandler(this.выходToolStripMenuItem_Click);
            // 
            // онтологияToolStripMenuItem1
            // 
            this.онтологияToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.управлениеТипамиToolStripMenuItem,
            this.управлениеМетодамиToolStripMenuItem1,
            this.привязкаПутиКЗадачеToolStripMenuItem});
            this.онтологияToolStripMenuItem1.Name = "онтологияToolStripMenuItem1";
            this.онтологияToolStripMenuItem1.Size = new System.Drawing.Size(96, 24);
            this.онтологияToolStripMenuItem1.Text = "Онтология";
            // 
            // управлениеТипамиToolStripMenuItem
            // 
            this.управлениеТипамиToolStripMenuItem.Name = "управлениеТипамиToolStripMenuItem";
            this.управлениеТипамиToolStripMenuItem.Size = new System.Drawing.Size(248, 26);
            this.управлениеТипамиToolStripMenuItem.Text = "Управление типами";
            this.управлениеТипамиToolStripMenuItem.Click += new System.EventHandler(this.управлениеТипамиToolStripMenuItem_Click);
            // 
            // управлениеМетодамиToolStripMenuItem1
            // 
            this.управлениеМетодамиToolStripMenuItem1.Name = "управлениеМетодамиToolStripMenuItem1";
            this.управлениеМетодамиToolStripMenuItem1.Size = new System.Drawing.Size(248, 26);
            this.управлениеМетодамиToolStripMenuItem1.Text = "Управление методами";
            this.управлениеМетодамиToolStripMenuItem1.Click += new System.EventHandler(this.управлениеМетодамиToolStripMenuItem1_Click);
            // 
            // привязкаПутиКЗадачеToolStripMenuItem
            // 
            this.привязкаПутиКЗадачеToolStripMenuItem.Name = "привязкаПутиКЗадачеToolStripMenuItem";
            this.привязкаПутиКЗадачеToolStripMenuItem.Size = new System.Drawing.Size(248, 26);
            this.привязкаПутиКЗадачеToolStripMenuItem.Text = "Привязка пути к задаче";
            this.привязкаПутиКЗадачеToolStripMenuItem.Click += new System.EventHandler(this.привязкаПутиКЗадачеToolStripMenuItem_Click);
            // 
            // задачиToolStripMenuItem
            // 
            this.задачиToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.добавитьToolStripMenuItem,
            this.удалитьToolStripMenuItem});
            this.задачиToolStripMenuItem.Name = "задачиToolStripMenuItem";
            this.задачиToolStripMenuItem.Size = new System.Drawing.Size(79, 24);
            this.задачиToolStripMenuItem.Text = "Задания";
            // 
            // добавитьToolStripMenuItem
            // 
            this.добавитьToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem3});
            this.добавитьToolStripMenuItem.Name = "добавитьToolStripMenuItem";
            this.добавитьToolStripMenuItem.Size = new System.Drawing.Size(172, 26);
            this.добавитьToolStripMenuItem.Text = "Добавить";
            this.добавитьToolStripMenuItem.Click += new System.EventHandler(this.добавитьToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(63, 6);
            // 
            // удалитьToolStripMenuItem
            // 
            this.удалитьToolStripMenuItem.Name = "удалитьToolStripMenuItem";
            this.удалитьToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.удалитьToolStripMenuItem.Size = new System.Drawing.Size(172, 26);
            this.удалитьToolStripMenuItem.Text = "Удалить";
            this.удалитьToolStripMenuItem.Click += new System.EventHandler(this.удалитьToolStripMenuItem_Click);
            // 
            // вариантыToolStripMenuItem
            // 
            this.вариантыToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.открытьВариантыToolStripMenuItem,
            this.генерацияToolStripMenuItem,
            this.toolStripSeparator2,
            this.просмотрВариантовToolStripMenuItem,
            this.toolStripSeparator3,
            this.сохранитьВариантыToolStripMenuItem,
            this.сохранитьВариантыКакToolStripMenuItem,
            this.toolStripSeparator4,
            this.распечататьToolStripMenuItem});
            this.вариантыToolStripMenuItem.Name = "вариантыToolStripMenuItem";
            this.вариантыToolStripMenuItem.Size = new System.Drawing.Size(90, 24);
            this.вариантыToolStripMenuItem.Text = "Варианты";
            // 
            // открытьВариантыToolStripMenuItem
            // 
            this.открытьВариантыToolStripMenuItem.Name = "открытьВариантыToolStripMenuItem";
            this.открытьВариантыToolStripMenuItem.Size = new System.Drawing.Size(268, 26);
            this.открытьВариантыToolStripMenuItem.Text = "Открыть варианты";
            this.открытьВариантыToolStripMenuItem.Click += new System.EventHandler(this.открытьВариантыToolStripMenuItem_Click);
            // 
            // генерацияToolStripMenuItem
            // 
            this.генерацияToolStripMenuItem.Name = "генерацияToolStripMenuItem";
            this.генерацияToolStripMenuItem.Size = new System.Drawing.Size(268, 26);
            this.генерацияToolStripMenuItem.Text = "Генерация";
            this.генерацияToolStripMenuItem.Click += new System.EventHandler(this.генерацияToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(265, 6);
            // 
            // просмотрВариантовToolStripMenuItem
            // 
            this.просмотрВариантовToolStripMenuItem.Name = "просмотрВариантовToolStripMenuItem";
            this.просмотрВариантовToolStripMenuItem.Size = new System.Drawing.Size(268, 26);
            this.просмотрВариантовToolStripMenuItem.Text = "Просмотр вариантов";
            this.просмотрВариантовToolStripMenuItem.Click += new System.EventHandler(this.просмотрВариантовToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(265, 6);
            // 
            // сохранитьВариантыToolStripMenuItem
            // 
            this.сохранитьВариантыToolStripMenuItem.Name = "сохранитьВариантыToolStripMenuItem";
            this.сохранитьВариантыToolStripMenuItem.Size = new System.Drawing.Size(268, 26);
            this.сохранитьВариантыToolStripMenuItem.Text = "Сохранить варианты";
            this.сохранитьВариантыToolStripMenuItem.Click += new System.EventHandler(this.сохранитьВариантыToolStripMenuItem_Click);
            // 
            // сохранитьВариантыКакToolStripMenuItem
            // 
            this.сохранитьВариантыКакToolStripMenuItem.Name = "сохранитьВариантыКакToolStripMenuItem";
            this.сохранитьВариантыКакToolStripMenuItem.Size = new System.Drawing.Size(268, 26);
            this.сохранитьВариантыКакToolStripMenuItem.Text = "Сохранить варианты как....";
            this.сохранитьВариантыКакToolStripMenuItem.Click += new System.EventHandler(this.сохранитьВариантыКакToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(265, 6);
            // 
            // распечататьToolStripMenuItem
            // 
            this.распечататьToolStripMenuItem.Name = "распечататьToolStripMenuItem";
            this.распечататьToolStripMenuItem.Size = new System.Drawing.Size(268, 26);
            this.распечататьToolStripMenuItem.Text = "Печать";
            this.распечататьToolStripMenuItem.Click += new System.EventHandler(this.распечататьToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1075, 500);
            this.Controls.Add(this.listExercises);
            this.Controls.Add(this.menuStrip2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "ALIS";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.menuStrip2.ResumeLayout(false);
            this.menuStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listExercises;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.MenuStrip menuStrip2;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem онтологияToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem управлениеМетодамиToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem задачиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem добавитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem удалитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem новыйToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem открытьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьКакToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem выходToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem управлениеТипамиToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem сменитьПользователяToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem скачатьРезультатыТестированияToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem вариантыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem генерацияToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ToolStripMenuItem просмотрВариантовToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem открытьВариантыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem привязкаПутиКЗадачеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem распечататьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьВариантыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьВариантыКакToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
    }
}