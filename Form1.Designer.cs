namespace CaptureHook
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
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (_keyHooking != null)
                {
                    KeyHookingService.Dispose();
                }
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
            key = new Label();
            SuspendLayout();
            // 
            // key
            // 
            key.BackColor = Color.Transparent;
            key.Dock = DockStyle.Fill;
            key.FlatStyle = FlatStyle.Flat;
            key.Font = new Font("Cascadia Mono SemiBold", 21.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            key.ForeColor = Color.White;
            key.Location = new Point(0, 0);
            key.Name = "key";
            key.Size = new Size(334, 161);
            key.TabIndex = 1;
            key.TextAlign = ContentAlignment.MiddleCenter;
            key.DoubleClick += ChangePosition;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(334, 161);
            Controls.Add(key);
            FormBorderStyle = FormBorderStyle.None;
            Name = "Form1";
            Text = "Form1";
            TopMost = true;
            ResumeLayout(false);
        }

        #endregion

        private static Label key;
    }
}
