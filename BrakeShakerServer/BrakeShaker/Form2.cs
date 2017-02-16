namespace BrakeShaker
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class Form2 : Form
    {
        private IContainer components = null;
        private Label label1;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label14;
        private Label label15;
        private Label label16;
        private Label label17;
        private Label label18;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Panel panel1;
        private Button resetButton;
        private TextBox textBox1;
        private TextBox textBox10;
        private TextBox textBox11;
        private TextBox textBox12;
        private TextBox textBox13;
        private TextBox textBox14;
        private TextBox textBox15;
        private TextBox textBox2;
        private TextBox textBox3;
        private TextBox textBox4;
        private TextBox textBox5;
        private TextBox textBox6;
        private TextBox textBox7;
        private TextBox textBox8;
        private TextBox textBox9;
        private Button closeButton;
        private Panel valuePanel;

        public Form2()
        {
            this.InitializeComponent();
            base.TopMost = true;
            this.ClearInterface();
            this.resetButton.Click += new EventHandler(this.ResetButton_Click);
            this.textBox1.Enabled = false;
            this.textBox2.Enabled = false;
            this.textBox3.Enabled = false;
            this.textBox4.Enabled = false;
            this.textBox5.Enabled = false;
            this.textBox6.Enabled = false;
            this.textBox7.Enabled = false;
            this.textBox8.Enabled = false;
            this.textBox9.Enabled = false;
            this.textBox10.Enabled = false;
            this.textBox11.Enabled = false;
            this.textBox12.Enabled = false;
            this.textBox13.Enabled = false;
            this.textBox14.Enabled = false;
            this.textBox15.Enabled = false;
        }

        public void ClearInterface()
        {
            this.textBox1.Text = "0";
            this.textBox2.Text = "0";
            this.textBox3.Text = "0";
            this.textBox4.Text = "0";
            this.textBox5.Text = "0";
            this.textBox6.Text = "0";
            this.textBox7.Text = "0";
            this.textBox8.Text = "0";
            this.textBox9.Text = "0";
            this.textBox10.Text = "0";
            this.textBox11.Text = "0";
            this.textBox12.Text = "0";
            this.textBox13.Text = "0";
            this.textBox14.Text = "0";
            this.textBox15.Text = "0";
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.valuePanel = new System.Windows.Forms.Panel();
            this.closeButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.resetButton = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.textBox15 = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox14 = new System.Windows.Forms.TextBox();
            this.textBox13 = new System.Windows.Forms.TextBox();
            this.textBox12 = new System.Windows.Forms.TextBox();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.valuePanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // valuePanel
            // 
            this.valuePanel.BackColor = System.Drawing.Color.Transparent;
            this.valuePanel.Controls.Add(this.closeButton);
            this.valuePanel.Controls.Add(this.panel1);
            this.valuePanel.Controls.Add(this.resetButton);
            this.valuePanel.Controls.Add(this.label15);
            this.valuePanel.Controls.Add(this.textBox15);
            this.valuePanel.Controls.Add(this.label14);
            this.valuePanel.Controls.Add(this.label13);
            this.valuePanel.Controls.Add(this.label12);
            this.valuePanel.Controls.Add(this.label11);
            this.valuePanel.Controls.Add(this.label10);
            this.valuePanel.Controls.Add(this.label9);
            this.valuePanel.Controls.Add(this.label8);
            this.valuePanel.Controls.Add(this.label7);
            this.valuePanel.Controls.Add(this.label6);
            this.valuePanel.Controls.Add(this.label5);
            this.valuePanel.Controls.Add(this.label4);
            this.valuePanel.Controls.Add(this.label3);
            this.valuePanel.Controls.Add(this.label2);
            this.valuePanel.Controls.Add(this.label1);
            this.valuePanel.Controls.Add(this.textBox14);
            this.valuePanel.Controls.Add(this.textBox13);
            this.valuePanel.Controls.Add(this.textBox12);
            this.valuePanel.Controls.Add(this.textBox11);
            this.valuePanel.Controls.Add(this.textBox10);
            this.valuePanel.Controls.Add(this.textBox8);
            this.valuePanel.Controls.Add(this.textBox7);
            this.valuePanel.Controls.Add(this.textBox6);
            this.valuePanel.Controls.Add(this.textBox5);
            this.valuePanel.Controls.Add(this.textBox9);
            this.valuePanel.Controls.Add(this.textBox4);
            this.valuePanel.Controls.Add(this.textBox3);
            this.valuePanel.Controls.Add(this.textBox2);
            this.valuePanel.Controls.Add(this.textBox1);
            this.valuePanel.Location = new System.Drawing.Point(12, 10);
            this.valuePanel.Name = "valuePanel";
            this.valuePanel.Size = new System.Drawing.Size(426, 297);
            this.valuePanel.TabIndex = 15;
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(180, 263);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 36;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label18);
            this.panel1.Controls.Add(this.label17);
            this.panel1.Controls.Add(this.label16);
            this.panel1.Location = new System.Drawing.Point(133, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(166, 64);
            this.panel1.TabIndex = 35;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.4F);
            this.label18.Location = new System.Drawing.Point(12, 39);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(133, 15);
            this.label18.TabIndex = 18;
            this.label18.Text = "button in AC RumbleKit";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.4F);
            this.label17.Location = new System.Drawing.Point(12, 22);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(96, 15);
            this.label17.TabIndex = 17;
            this.label17.Text = "click the Monitor";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.4F);
            this.label16.Location = new System.Drawing.Point(12, 6);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(120, 15);
            this.label16.TabIndex = 16;
            this.label16.Text = "To close this window";
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(180, 203);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(75, 23);
            this.resetButton.TabIndex = 34;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.Orange;
            this.label15.Location = new System.Drawing.Point(159, 161);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(118, 13);
            this.label15.TabIndex = 33;
            this.label15.Text = "Reference Load Sliding";
            // 
            // textBox15
            // 
            this.textBox15.Location = new System.Drawing.Point(167, 177);
            this.textBox15.Name = "textBox15";
            this.textBox15.Size = new System.Drawing.Size(100, 20);
            this.textBox15.TabIndex = 32;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.Lime;
            this.label14.Location = new System.Drawing.Point(159, 122);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(119, 13);
            this.label14.TabIndex = 31;
            this.label14.Text = "Reference Load Rolling";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(193, 83);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(51, 13);
            this.label13.TabIndex = 30;
            this.label13.Text = "Car Mass";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Gainsboro;
            this.label12.Location = new System.Drawing.Point(344, 172);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(69, 13);
            this.label12.TabIndex = 29;
            this.label12.Text = "RR Slip Rate";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Gainsboro;
            this.label11.Location = new System.Drawing.Point(13, 172);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(67, 13);
            this.label11.TabIndex = 28;
            this.label11.Text = "RL Slip Rate";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Gainsboro;
            this.label10.Location = new System.Drawing.Point(347, 83);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(67, 13);
            this.label10.TabIndex = 27;
            this.label10.Text = "FR Slip Rate";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Gainsboro;
            this.label9.Location = new System.Drawing.Point(13, 83);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 13);
            this.label9.TabIndex = 26;
            this.label9.Text = "FL Slip Rate";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Orange;
            this.label8.Location = new System.Drawing.Point(307, 211);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(107, 13);
            this.label8.TabIndex = 25;
            this.label8.Text = "RR Max Sliding Load";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Orange;
            this.label7.Location = new System.Drawing.Point(13, 211);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(105, 13);
            this.label7.TabIndex = 24;
            this.label7.Text = "RL Max Sliding Load";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Orange;
            this.label6.Location = new System.Drawing.Point(309, 44);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(105, 13);
            this.label6.TabIndex = 23;
            this.label6.Text = "FR Max Sliding Load";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Orange;
            this.label5.Location = new System.Drawing.Point(13, 44);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 13);
            this.label5.TabIndex = 22;
            this.label5.Text = "FL Max Sliding Load";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Lime;
            this.label4.Location = new System.Drawing.Point(306, 250);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 13);
            this.label4.TabIndex = 21;
            this.label4.Text = "RR Max Rolling Load";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Lime;
            this.label3.Location = new System.Drawing.Point(13, 250);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "RL Max Rolling Load";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Lime;
            this.label2.Location = new System.Drawing.Point(308, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "FR Max Rolling Load";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Lime;
            this.label1.Location = new System.Drawing.Point(13, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "FL Max Rolling Load";
            // 
            // textBox14
            // 
            this.textBox14.Location = new System.Drawing.Point(314, 188);
            this.textBox14.Name = "textBox14";
            this.textBox14.Size = new System.Drawing.Size(100, 20);
            this.textBox14.TabIndex = 17;
            // 
            // textBox13
            // 
            this.textBox13.Location = new System.Drawing.Point(13, 188);
            this.textBox13.Name = "textBox13";
            this.textBox13.Size = new System.Drawing.Size(100, 20);
            this.textBox13.TabIndex = 16;
            // 
            // textBox12
            // 
            this.textBox12.Location = new System.Drawing.Point(314, 99);
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new System.Drawing.Size(100, 20);
            this.textBox12.TabIndex = 15;
            // 
            // textBox11
            // 
            this.textBox11.Location = new System.Drawing.Point(13, 99);
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new System.Drawing.Size(100, 20);
            this.textBox11.TabIndex = 14;
            // 
            // textBox10
            // 
            this.textBox10.Location = new System.Drawing.Point(314, 227);
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new System.Drawing.Size(100, 20);
            this.textBox10.TabIndex = 13;
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point(13, 227);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(100, 20);
            this.textBox8.TabIndex = 12;
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(314, 60);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(100, 20);
            this.textBox7.TabIndex = 11;
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(13, 60);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(100, 20);
            this.textBox6.TabIndex = 10;
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(167, 138);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(100, 20);
            this.textBox5.TabIndex = 9;
            // 
            // textBox9
            // 
            this.textBox9.Location = new System.Drawing.Point(167, 99);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(100, 20);
            this.textBox9.TabIndex = 8;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(314, 266);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(100, 20);
            this.textBox4.TabIndex = 3;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(13, 266);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 20);
            this.textBox3.TabIndex = 2;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(314, 21);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 1;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(13, 21);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 0;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 317);
            this.ControlBox = false;
            this.Controls.Add(this.valuePanel);
            this.Name = "Form2";
            this.Text = "Data Monitor";
            this.valuePanel.ResumeLayout(false);
            this.valuePanel.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        public void ResetButton_Click(object sender, EventArgs e)
        {
            this.ClearInterface();
        }

        public void UpdateValues(float[] maxRollingWheelLoad, float[] maxSlidingWheelLoad, float[] currentSlipRate, float carMass, float maxWheelLoad, float slidingCorrectionFactor)
        {
            this.textBox1.Text = maxRollingWheelLoad[0].ToString();
            this.textBox2.Text = maxRollingWheelLoad[1].ToString();
            this.textBox3.Text = maxRollingWheelLoad[2].ToString();
            this.textBox4.Text = maxRollingWheelLoad[3].ToString();
            this.textBox6.Text = maxSlidingWheelLoad[0].ToString();
            this.textBox7.Text = maxSlidingWheelLoad[1].ToString();
            this.textBox8.Text = maxSlidingWheelLoad[2].ToString();
            this.textBox10.Text = maxSlidingWheelLoad[3].ToString();
            this.textBox11.Text = currentSlipRate[0].ToString();
            this.textBox12.Text = currentSlipRate[1].ToString();
            this.textBox13.Text = currentSlipRate[2].ToString();
            this.textBox14.Text = currentSlipRate[3].ToString();
            this.textBox9.Text = carMass.ToString();
            this.textBox5.Text = maxWheelLoad.ToString();
            this.textBox15.Text = (maxWheelLoad / slidingCorrectionFactor).ToString();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            //Form1.sessionFlags.monitorFormShown = false;
            Close();
        }
    }
}

