
namespace AdminDesctopApplication
{
    partial class BlockUserForm
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.reasonBox = new System.Windows.Forms.TextBox();
            this.usernameBox = new System.Windows.Forms.ComboBox();
            this.planetwarsDataSet = new AdminDesctopApplication.planetwarsDataSet();
            this.usernameListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.planetwarsDataSetBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.reasonLabel = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.usernameListTableAdapter = new AdminDesctopApplication.planetwarsDataSetTableAdapters.usernameTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.planetwarsDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.usernameListBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.planetwarsDataSetBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Username:";
            // 
            // reasonBox
            // 
            this.reasonBox.Location = new System.Drawing.Point(104, 39);
            this.reasonBox.Multiline = true;
            this.reasonBox.Name = "reasonBox";
            this.reasonBox.Size = new System.Drawing.Size(392, 55);
            this.reasonBox.TabIndex = 2;
            this.reasonBox.TextChanged += new System.EventHandler(this.reasonBox_TextChanged);
            // 
            // usernameBox
            // 
            this.usernameBox.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.planetwarsDataSet, "user_list.username", true));
            this.usernameBox.DataSource = this.usernameListBindingSource;
            this.usernameBox.DisplayMember = "username";
            this.usernameBox.FormattingEnabled = true;
            this.usernameBox.Location = new System.Drawing.Point(104, 12);
            this.usernameBox.Name = "usernameBox";
            this.usernameBox.Size = new System.Drawing.Size(392, 21);
            this.usernameBox.TabIndex = 3;
            this.usernameBox.ValueMember = "user_id";
            this.usernameBox.TextChanged += new System.EventHandler(this.usernameBox_TextChanged);
            // 
            // planetwarsDataSet
            // 
            this.planetwarsDataSet.DataSetName = "planetwarsDataSet";
            this.planetwarsDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // usernameListBindingSource
            // 
            this.usernameListBindingSource.DataMember = "username_list";
            this.usernameListBindingSource.DataSource = this.planetwarsDataSetBindingSource;
            // 
            // planetwarsDataSetBindingSource
            // 
            this.planetwarsDataSetBindingSource.DataSource = this.planetwarsDataSet;
            this.planetwarsDataSetBindingSource.Position = 0;
            // 
            // reasonLabel
            // 
            this.reasonLabel.AutoSize = true;
            this.reasonLabel.Location = new System.Drawing.Point(12, 42);
            this.reasonLabel.Name = "reasonLabel";
            this.reasonLabel.Size = new System.Drawing.Size(72, 13);
            this.reasonLabel.TabIndex = 4;
            this.reasonLabel.Text = "Block reason:";
            // 
            // okButton
            // 
            this.okButton.Enabled = false;
            this.okButton.Location = new System.Drawing.Point(421, 100);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 5;
            this.okButton.Text = "Ok";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(340, 100);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // usernameListTableAdapter
            // 
            this.usernameListTableAdapter.ClearBeforeFill = true;
            // 
            // BlockUserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(508, 135);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.reasonLabel);
            this.Controls.Add(this.usernameBox);
            this.Controls.Add(this.reasonBox);
            this.Controls.Add(this.label1);
            this.Name = "BlockUserForm";
            this.Text = "BlockUserForm";
            this.Load += new System.EventHandler(this.BlockUserForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.planetwarsDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.usernameListBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.planetwarsDataSetBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox reasonBox;
        private System.Windows.Forms.ComboBox usernameBox;
        private planetwarsDataSet planetwarsDataSet;
        private System.Windows.Forms.BindingSource planetwarsDataSetBindingSource;
        private System.Windows.Forms.Label reasonLabel;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.BindingSource usernameListBindingSource;
        private planetwarsDataSetTableAdapters.usernameTableAdapter usernameListTableAdapter;
    }
}