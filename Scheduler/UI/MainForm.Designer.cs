using System.ComponentModel;

namespace Scheduler.UI
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            tblAppointments = new DataGridView();
            lblViewingDate = new Label();
            tblCustomers = new DataGridView();
            label1 = new Label();
            btnAddCustomer = new Button();
            btnEditCustomer = new Button();
            btnDeleteCustomer = new Button();
            btnDeleteAppt = new Button();
            btnEditAppt = new Button();
            btnAddAppt = new Button();
            datCalendarPicker = new MonthCalendar();
            btnGenerateReport = new Button();
            ((ISupportInitialize)tblAppointments).BeginInit();
            ((ISupportInitialize)tblCustomers).BeginInit();
            SuspendLayout();
            // 
            // tblAppointments
            // 
            tblAppointments.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tblAppointments.Location = new Point(14, 332);
            tblAppointments.Margin = new Padding(4, 3, 4, 3);
            tblAppointments.Name = "tblAppointments";
            tblAppointments.Size = new Size(905, 173);
            tblAppointments.TabIndex = 1;
            // 
            // lblViewingDate
            // 
            lblViewingDate.Location = new Point(14, 309);
            lblViewingDate.Margin = new Padding(4, 0, 4, 0);
            lblViewingDate.Name = "lblViewingDate";
            lblViewingDate.Size = new Size(355, 20);
            lblViewingDate.TabIndex = 2;
            lblViewingDate.Text = "Today's Appointments:";
            // 
            // tblCustomers
            // 
            tblCustomers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tblCustomers.Location = new Point(14, 29);
            tblCustomers.Margin = new Padding(4, 3, 4, 3);
            tblCustomers.Name = "tblCustomers";
            tblCustomers.Size = new Size(280, 277);
            tblCustomers.TabIndex = 3;
            // 
            // label1
            // 
            label1.Location = new Point(14, 10);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(117, 15);
            label1.TabIndex = 4;
            label1.Text = "Customers:";
            // 
            // btnAddCustomer
            // 
            btnAddCustomer.Location = new Point(301, 30);
            btnAddCustomer.Margin = new Padding(4, 3, 4, 3);
            btnAddCustomer.Name = "btnAddCustomer";
            btnAddCustomer.Size = new Size(124, 27);
            btnAddCustomer.TabIndex = 5;
            btnAddCustomer.Text = "Add Customer";
            btnAddCustomer.UseVisualStyleBackColor = true;
            btnAddCustomer.Click += btnAddCustomer_Click;
            // 
            // btnEditCustomer
            // 
            btnEditCustomer.Location = new Point(301, 63);
            btnEditCustomer.Margin = new Padding(4, 3, 4, 3);
            btnEditCustomer.Name = "btnEditCustomer";
            btnEditCustomer.Size = new Size(124, 27);
            btnEditCustomer.TabIndex = 6;
            btnEditCustomer.Text = "Edit Customer";
            btnEditCustomer.UseVisualStyleBackColor = true;
            btnEditCustomer.Click += btnEditCustomer_Click;
            // 
            // btnDeleteCustomer
            // 
            btnDeleteCustomer.Location = new Point(301, 97);
            btnDeleteCustomer.Margin = new Padding(4, 3, 4, 3);
            btnDeleteCustomer.Name = "btnDeleteCustomer";
            btnDeleteCustomer.Size = new Size(124, 27);
            btnDeleteCustomer.TabIndex = 7;
            btnDeleteCustomer.Text = "Delete Customer";
            btnDeleteCustomer.UseVisualStyleBackColor = true;
            btnDeleteCustomer.Click += btnDeleteCustomer_Click;
            // 
            // btnDeleteAppt
            // 
            btnDeleteAppt.Location = new Point(639, 299);
            btnDeleteAppt.Margin = new Padding(4, 3, 4, 3);
            btnDeleteAppt.Name = "btnDeleteAppt";
            btnDeleteAppt.Size = new Size(135, 27);
            btnDeleteAppt.TabIndex = 10;
            btnDeleteAppt.Text = "Delete Appointment";
            btnDeleteAppt.UseVisualStyleBackColor = true;
            btnDeleteAppt.Click += btnDeleteAppt_Click;
            // 
            // btnEditAppt
            // 
            btnEditAppt.Location = new Point(509, 299);
            btnEditAppt.Margin = new Padding(4, 3, 4, 3);
            btnEditAppt.Name = "btnEditAppt";
            btnEditAppt.Size = new Size(124, 27);
            btnEditAppt.TabIndex = 9;
            btnEditAppt.Text = "Edit Appointment";
            btnEditAppt.UseVisualStyleBackColor = true;
            btnEditAppt.Click += btnEditAppt_Click;
            // 
            // btnAddAppt
            // 
            btnAddAppt.Location = new Point(378, 299);
            btnAddAppt.Margin = new Padding(4, 3, 4, 3);
            btnAddAppt.Name = "btnAddAppt";
            btnAddAppt.Size = new Size(124, 27);
            btnAddAppt.TabIndex = 8;
            btnAddAppt.Text = "Add Appointment";
            btnAddAppt.UseVisualStyleBackColor = true;
            btnAddAppt.Click += btnAddAppt_Click;
            // 
            // datCalendarPicker
            // 
            datCalendarPicker.CalendarDimensions = new Size(2, 1);
            datCalendarPicker.Location = new Point(457, 30);
            datCalendarPicker.Name = "datCalendarPicker";
            datCalendarPicker.TabIndex = 11;
            datCalendarPicker.DateChanged += datCalendarPicker_DateChanged;
            datCalendarPicker.DateSelected += datCalendarPicker_DateSelected;
            // 
            // btnGenerateReport
            // 
            btnGenerateReport.Location = new Point(802, 204);
            btnGenerateReport.Name = "btnGenerateReport";
            btnGenerateReport.Size = new Size(113, 23);
            btnGenerateReport.TabIndex = 12;
            btnGenerateReport.Text = "Generate Report";
            btnGenerateReport.UseVisualStyleBackColor = true;
            btnGenerateReport.Click += btnGenerateReport_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(933, 519);
            Controls.Add(btnGenerateReport);
            Controls.Add(datCalendarPicker);
            Controls.Add(btnDeleteAppt);
            Controls.Add(btnEditAppt);
            Controls.Add(btnAddAppt);
            Controls.Add(btnDeleteCustomer);
            Controls.Add(btnEditCustomer);
            Controls.Add(btnAddCustomer);
            Controls.Add(label1);
            Controls.Add(tblCustomers);
            Controls.Add(lblViewingDate);
            Controls.Add(tblAppointments);
            Margin = new Padding(4, 3, 4, 3);
            Name = "MainForm";
            Text = "Main";
            ((ISupportInitialize)tblAppointments).EndInit();
            ((ISupportInitialize)tblCustomers).EndInit();
            ResumeLayout(false);
        }

        private System.Windows.Forms.Button btnDeleteAppt;
        private System.Windows.Forms.Button btnEditAppt;
        private System.Windows.Forms.Button btnAddAppt;

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAddCustomer;
        private System.Windows.Forms.Button btnEditCustomer;
        private System.Windows.Forms.Button btnDeleteCustomer;

        private System.Windows.Forms.Label lblViewingDate;
        private System.Windows.Forms.DataGridView tblCustomers;
        private System.Windows.Forms.DataGridView tblAppointments;

        #endregion

        private MonthCalendar datCalendarPicker;
        private Button btnGenerateReport;
    }
}