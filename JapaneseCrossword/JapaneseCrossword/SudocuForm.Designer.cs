namespace Sudocu
{
    partial class SudocuForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer m_components = null;
        private CSudocuControl _sudocuControl = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (m_components != null))
            {
                m_components.Dispose();
            }
            if (disposing && (_sudocuControl!= null))
            {
                _sudocuControl.Dispose();
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
            this._sudocuControl = new Sudocu.CSudocuControl();
            this.SuspendLayout();
            // 
            // _sudocuControl
            // 
            this._sudocuControl.BackColor = System.Drawing.SystemColors.Control;
            this._sudocuControl.CellSize = ((byte)(15));
            this._sudocuControl.Location = new System.Drawing.Point(0, 0);
            this._sudocuControl.Name = "_sudocuControl";
            this._sudocuControl.SeparatorSize = ((byte)(7));
            this._sudocuControl.Size = new System.Drawing.Size(184, 346);
            this._sudocuControl.TabIndex = 0;
            // 
            // SudocuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(712, 370);
            this.Controls.Add(this._sudocuControl);
            this.Name = "SudocuForm";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion
    }
}

