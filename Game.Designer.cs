
namespace Praktika11
{
    partial class Game
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Game));
            this.Timer = new System.Windows.Forms.Timer(this.components);
            this.ExitButton = new System.Windows.Forms.Button();
            this.CleanButton = new System.Windows.Forms.Button();
            this.PauseButton = new System.Windows.Forms.Button();
            this.StartButton = new System.Windows.Forms.Button();
            this.GamePlace = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.GamePlace)).BeginInit();
            this.SuspendLayout();
            // 
            // Timer
            // 
            this.Timer.Tick += new System.EventHandler(this.TimerTick);
            // 
            // ExitButton
            // 
            this.ExitButton.BackColor = System.Drawing.Color.IndianRed;
            this.ExitButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ExitButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ExitButton.Location = new System.Drawing.Point(12, 147);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(81, 23);
            this.ExitButton.TabIndex = 1;
            this.ExitButton.Text = "Выход";
            this.ExitButton.UseVisualStyleBackColor = false;
            this.ExitButton.Click += new System.EventHandler(this.Exit);
            // 
            // CleanButton
            // 
            this.CleanButton.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.CleanButton.Enabled = false;
            this.CleanButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CleanButton.Location = new System.Drawing.Point(12, 106);
            this.CleanButton.Name = "CleanButton";
            this.CleanButton.Size = new System.Drawing.Size(81, 23);
            this.CleanButton.TabIndex = 3;
            this.CleanButton.Text = "Очистка";
            this.CleanButton.UseVisualStyleBackColor = false;
            this.CleanButton.Click += new System.EventHandler(this.ClearColors);
            // 
            // PauseButton
            // 
            this.PauseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.PauseButton.Enabled = false;
            this.PauseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PauseButton.Location = new System.Drawing.Point(12, 65);
            this.PauseButton.Name = "PauseButton";
            this.PauseButton.Size = new System.Drawing.Size(81, 23);
            this.PauseButton.TabIndex = 2;
            this.PauseButton.Text = "Остановить";
            this.PauseButton.UseVisualStyleBackColor = false;
            this.PauseButton.Click += new System.EventHandler(this.PauseClick);
            // 
            // StartButton
            // 
            this.StartButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.StartButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StartButton.Location = new System.Drawing.Point(12, 24);
            this.StartButton.Margin = new System.Windows.Forms.Padding(1);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(81, 23);
            this.StartButton.TabIndex = 0;
            this.StartButton.Text = "Начать";
            this.StartButton.UseVisualStyleBackColor = false;
            this.StartButton.Click += new System.EventHandler(this.Start);
            // 
            // GamePlace
            // 
            this.GamePlace.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.GamePlace.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GamePlace.Location = new System.Drawing.Point(111, 0);
            this.GamePlace.Name = "GamePlace";
            this.GamePlace.Size = new System.Drawing.Size(700, 554);
            this.GamePlace.TabIndex = 0;
            this.GamePlace.TabStop = false;
            this.GamePlace.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PrintColor);
            // 
            // Game
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(812, 555);
            this.Controls.Add(this.GamePlace);
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.CleanButton);
            this.Controls.Add(this.PauseButton);
            this.Controls.Add(this.StartButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(400, 400);
            this.Name = "Game";
            this.Text = "Игра \"Жизнь\"";
            this.Deactivate += new System.EventHandler(this.PauseClick);
            this.Resize += new System.EventHandler(this.ResizeComponents);
            ((System.ComponentModel.ISupportInitialize)(this.GamePlace)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer Timer;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Button CleanButton;
        private System.Windows.Forms.Button PauseButton;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.PictureBox GamePlace;
    }
}

