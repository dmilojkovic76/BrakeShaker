using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using BrakeShaker.Properties;

namespace BrakeShaker
{
	public class Form1 : Form
	{
		public struct flags
		{
			public bool sharedMemAvailable;
			public bool comPortAvailable;
			public bool monitorFormShown;
            public bool testMode;
		}

		private struct tyreVariables
		{
			public string tyreType;
			public float upperLimitPerRollingWheel;
			public float upperLimitPerSlidingWheel;
			public double slidingExponent;
			public double rollingExponent;
			public double cornerRollingReductionFactor;
			public double cornerRollingExponent;
			public double brakeScalingExponent;
			public float wheelLoadFactor;
			public float[] maxWheelLoad;
			public float[] wheelLoadCoefficient;
			public double slipExponent;
			public float slidingCorrectionFactor;
		}

		private tyreVariables[] curTyreVariables = new tyreVariables[4];
        private string acStaticsSharedMemFileLocation = "Local\\acpmf_static";
        private string acPhysicsSharedMemFileLocation = "Local\\acpmf_physics";
        private string acGraphicsSharedMemFileLocation = "Local\\acpmf_graphics";
        private MemoryMappedFile acPhysicsSharedMem;
		private SerialPort port;

		private float[] wheelLoadFactorList = new float[]
		{
			0.58f,
			0.58f,
			0.63f,
			0.65f
		};

		private Form2 MonitorForm = new Form2();
        private float previousMaxRumble = 0f;
		private float rumble = 0f;
		private float carMass;
		private float[] maxRollingWheelLoad = new float[4];
		private float[] maxSlidingWheelLoad = new float[4];
		private float[] currentRollingWheelLoad = new float[4];
		private float[] currentSlidingWheelLoad = new float[4];
		private float[] currentSlipRate = new float[4];
		private int tyreChosen;
		private float gammaExponent;
		private float maxRumblePerRollingWheel = 0.16f;
		private float maxRumblePerSlidingWheel = 0.28f;
		private int master = 100;
		private int gamma = 0;
		private float upperLimitPerRollingWheel = 0.6f;
		private float upperLimitPerSlidingWheel = 0.8f;

        private string[] tyreType = new string[]
		{
			"Vintage",
			"Road",
			"Semislick",
			"Slick"
		};

		private int counter = 300;
		public flags sessionFlags;
		private ACPhysic acPhysics;
		private IContainer components = null;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel StatusTelemetryLabel1;
        private ToolStripStatusLabel StatusTelemetryLabel2;
        private ToolStripStatusLabel StatusCOMLabel1;
        private ToolStripStatusLabel StatusCOMLabel2;
        private ToolStripStatusLabel StatusTCPLabel1;
        private ToolStripStatusLabel StatusTCPLabel2;
        private Panel bottomPanel;
        private Panel topPanel;
        private TextBox tcpPortTextBox;
        private Label tcpPortEditLabel;
        private Label comPortComboLabel;
        private Panel panel3;
        private Panel panel2;
        private Button button4;
        private TextBox textBox3;
        private TextBox textBox4;
        private Label label5;
        private Label label6;
        private HScrollBar hScrollBar2;
        private HScrollBar hScrollBar3;
        private ComboBox comboBox1;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label label10;
        private NumericUpDown numericUpDown1;
        private NumericUpDown numericUpDown2;
        private Panel panel1;
        private Label label1;
        private HScrollBar hScrollBar1;
        private Button button1;
        private Button button2;
        private Button button3;
        private TextBox textBox1;
        private Label label2;
        private Label label3;
        private TextBox textBox2;
        private Label label4;
        private Panel brakeSettingsPanel;
        private Button defaultButton;
        private TextBox gammaTextBox;
        private TextBox masterTextBox;
        private Label Gammalabel;
        private Label Masterlabel;
        private HScrollBar masterScrollBar;
        private HScrollBar gammaScrollBar;
        private ComboBox tyreCB;
        private Label tyreLabel;
        private Label Slidinglabel;
        private Label Settingslabel;
        private Label Rolinglabel;
        private NumericUpDown state2Spinner;
        private NumericUpDown state1Spinner;
        private Panel brakeStatusPanel;
        private Label testLabel;
        private HScrollBar testScrollBar;
        private Button testButton;
        private Button monitorButton;
        private Button resetButton;
        private TextBox maxRumbleTextBox;
        private Label maxRumbleLabel;
        private Label rumbleLabel;
        private TextBox rumbleSentTextBox;
        private Label Statuslabel;
        private ComboBox comPortComboBox;

		public Form1()
		{
            InitializeComponent();
            InitializeTyreTypes();
            this.Load += Form1_Load;
            maxRumblePerRollingWheel = Settings.Default.RollingRumble;
            maxRumblePerSlidingWheel = Settings.Default.SlidingRumble;
            tyreChosen = Settings.Default.tyreChosen;
            master = Settings.Default.master;
            gamma = Settings.Default.gamma;
            masterScrollBar.Minimum = 0;
            masterScrollBar.Maximum = 100;
            masterScrollBar.Value = master;
            masterScrollBar.ValueChanged += new EventHandler(masterScrollBar_ValueChanged);
            masterTextBox.Text = master.ToString();
            gammaScrollBar.Minimum = 0;
            gammaScrollBar.Maximum = 100;
            gammaScrollBar.Value = gamma;
            gammaExponent = 1f - (float)gamma * 0.008f; // range 0 <-> 0.2
            gammaScrollBar.ValueChanged += new EventHandler(gammaScrollBar_ValueChanged);
            gammaTextBox.Text = gamma.ToString();
            sessionFlags.comPortAvailable = false;
            sessionFlags.sharedMemAvailable = false;
            sessionFlags.monitorFormShown = false;
            sessionFlags.testMode = false;
            ClearInterface();
            defaultButton.Click += new EventHandler(DefaultButton_Click);
            tyreCB.DataSource = tyreType;
            tyreCB.UseWaitCursor = false;
            tyreCB.DropDownStyle = ComboBoxStyle.DropDownList;
            tyreCB.SelectedItem = tyreType[tyreChosen];
            //TopMost = true;
			Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;
            state1Spinner.Value = (decimal)(maxRumblePerRollingWheel * 100f);
            state1Spinner.Maximum = (decimal)(upperLimitPerRollingWheel * 100f);
            state1Spinner.ValueChanged += new EventHandler(state1Spinner_ValueChanged);
            state1Spinner.ReadOnly = true;
            state1Spinner.UseWaitCursor = false;
            state2Spinner.Value = (decimal)(maxRumblePerSlidingWheel * 100f);
            state2Spinner.Maximum = (decimal)(upperLimitPerSlidingWheel * 100f);
            state2Spinner.ValueChanged += new EventHandler(state2Spinner_ValueChanged);
            state2Spinner.ReadOnly = true;
            state2Spinner.UseWaitCursor = false;
            tyreCB.TextChanged += new EventHandler(tyreCB_TextChanged);
			System.Timers.Timer timer = new System.Timers.Timer();
			timer.Interval = 10.0;
			timer.SynchronizingObject = this;
			timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
			timer.AutoReset = true;
			timer.Start();
            resetButton.Click += new EventHandler(resetButton_Click);
            monitorButton.Click += new EventHandler(monitorButton_Click);
		}

        void Form1_Load(object sender, EventArgs e)
        {
            var ports = SerialPort.GetPortNames();
            comPortComboBox.DataSource = ports;
        }
        
		private void InitializeComponent()
		{
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusTelemetryLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusTelemetryLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusCOMLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusCOMLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusTCPLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusTCPLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.topPanel = new System.Windows.Forms.Panel();
            this.tcpPortTextBox = new System.Windows.Forms.TextBox();
            this.tcpPortEditLabel = new System.Windows.Forms.Label();
            this.comPortComboLabel = new System.Windows.Forms.Label();
            this.comPortComboBox = new System.Windows.Forms.ComboBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button4 = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.hScrollBar2 = new System.Windows.Forms.HScrollBar();
            this.hScrollBar3 = new System.Windows.Forms.HScrollBar();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.brakeSettingsPanel = new System.Windows.Forms.Panel();
            this.defaultButton = new System.Windows.Forms.Button();
            this.gammaTextBox = new System.Windows.Forms.TextBox();
            this.masterTextBox = new System.Windows.Forms.TextBox();
            this.Gammalabel = new System.Windows.Forms.Label();
            this.Masterlabel = new System.Windows.Forms.Label();
            this.masterScrollBar = new System.Windows.Forms.HScrollBar();
            this.gammaScrollBar = new System.Windows.Forms.HScrollBar();
            this.tyreCB = new System.Windows.Forms.ComboBox();
            this.tyreLabel = new System.Windows.Forms.Label();
            this.Slidinglabel = new System.Windows.Forms.Label();
            this.Settingslabel = new System.Windows.Forms.Label();
            this.Rolinglabel = new System.Windows.Forms.Label();
            this.state2Spinner = new System.Windows.Forms.NumericUpDown();
            this.state1Spinner = new System.Windows.Forms.NumericUpDown();
            this.brakeStatusPanel = new System.Windows.Forms.Panel();
            this.testLabel = new System.Windows.Forms.Label();
            this.testScrollBar = new System.Windows.Forms.HScrollBar();
            this.testButton = new System.Windows.Forms.Button();
            this.monitorButton = new System.Windows.Forms.Button();
            this.resetButton = new System.Windows.Forms.Button();
            this.maxRumbleTextBox = new System.Windows.Forms.TextBox();
            this.maxRumbleLabel = new System.Windows.Forms.Label();
            this.rumbleLabel = new System.Windows.Forms.Label();
            this.rumbleSentTextBox = new System.Windows.Forms.TextBox();
            this.Statuslabel = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.topPanel.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            this.panel1.SuspendLayout();
            this.brakeSettingsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.state2Spinner)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.state1Spinner)).BeginInit();
            this.brakeStatusPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusTelemetryLabel1,
            this.StatusTelemetryLabel2,
            this.StatusCOMLabel1,
            this.StatusCOMLabel2,
            this.StatusTCPLabel1,
            this.StatusTCPLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 470);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(535, 22);
            this.statusStrip1.TabIndex = 15;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusTelemetryLabel1
            // 
            this.StatusTelemetryLabel1.Name = "StatusTelemetryLabel1";
            this.StatusTelemetryLabel1.Size = new System.Drawing.Size(62, 17);
            this.StatusTelemetryLabel1.Text = "Telemetry:";
            this.StatusTelemetryLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // StatusTelemetryLabel2
            // 
            this.StatusTelemetryLabel2.AutoSize = false;
            this.StatusTelemetryLabel2.Name = "StatusTelemetryLabel2";
            this.StatusTelemetryLabel2.Size = new System.Drawing.Size(30, 17);
            this.StatusTelemetryLabel2.Text = "NO";
            this.StatusTelemetryLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // StatusCOMLabel1
            // 
            this.StatusCOMLabel1.Name = "StatusCOMLabel1";
            this.StatusCOMLabel1.Size = new System.Drawing.Size(63, 17);
            this.StatusCOMLabel1.Text = "COM port:";
            this.StatusCOMLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // StatusCOMLabel2
            // 
            this.StatusCOMLabel2.AutoSize = false;
            this.StatusCOMLabel2.Name = "StatusCOMLabel2";
            this.StatusCOMLabel2.Size = new System.Drawing.Size(30, 17);
            this.StatusCOMLabel2.Text = "NO";
            this.StatusCOMLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // StatusTCPLabel1
            // 
            this.StatusTCPLabel1.Name = "StatusTCPLabel1";
            this.StatusTCPLabel1.Size = new System.Drawing.Size(94, 17);
            this.StatusTCPLabel1.Text = "TCP connection:";
            this.StatusTCPLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // StatusTCPLabel2
            // 
            this.StatusTCPLabel2.AutoSize = false;
            this.StatusTCPLabel2.Name = "StatusTCPLabel2";
            this.StatusTCPLabel2.Size = new System.Drawing.Size(30, 17);
            this.StatusTCPLabel2.Text = "NO";
            this.StatusTCPLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bottomPanel
            // 
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.Location = new System.Drawing.Point(0, 448);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(535, 22);
            this.bottomPanel.TabIndex = 20;
            // 
            // topPanel
            // 
            this.topPanel.Controls.Add(this.tcpPortTextBox);
            this.topPanel.Controls.Add(this.tcpPortEditLabel);
            this.topPanel.Controls.Add(this.comPortComboLabel);
            this.topPanel.Controls.Add(this.comPortComboBox);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Padding = new System.Windows.Forms.Padding(12, 9, 0, 0);
            this.topPanel.Size = new System.Drawing.Size(535, 32);
            this.topPanel.TabIndex = 21;
            // 
            // tcpPortTextBox
            // 
            this.tcpPortTextBox.Location = new System.Drawing.Point(189, 6);
            this.tcpPortTextBox.Name = "tcpPortTextBox";
            this.tcpPortTextBox.Size = new System.Drawing.Size(60, 20);
            this.tcpPortTextBox.TabIndex = 21;
            // 
            // tcpPortEditLabel
            // 
            this.tcpPortEditLabel.AutoSize = true;
            this.tcpPortEditLabel.Location = new System.Drawing.Point(138, 9);
            this.tcpPortEditLabel.Name = "tcpPortEditLabel";
            this.tcpPortEditLabel.Size = new System.Drawing.Size(53, 13);
            this.tcpPortEditLabel.TabIndex = 20;
            this.tcpPortEditLabel.Text = "TCP Port:";
            // 
            // comPortComboLabel
            // 
            this.comPortComboLabel.AutoSize = true;
            this.comPortComboLabel.Location = new System.Drawing.Point(12, 9);
            this.comPortComboLabel.Name = "comPortComboLabel";
            this.comPortComboLabel.Size = new System.Drawing.Size(53, 13);
            this.comPortComboLabel.TabIndex = 18;
            this.comPortComboLabel.Text = "Com Port:";
            // 
            // comPortComboBox
            // 
            this.comPortComboBox.FormattingEnabled = true;
            this.comPortComboBox.Location = new System.Drawing.Point(65, 6);
            this.comPortComboBox.Name = "comPortComboBox";
            this.comPortComboBox.Size = new System.Drawing.Size(65, 21);
            this.comPortComboBox.TabIndex = 19;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel2);
            this.panel3.Controls.Add(this.panel1);
            this.panel3.Controls.Add(this.brakeSettingsPanel);
            this.panel3.Controls.Add(this.brakeStatusPanel);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 32);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(535, 416);
            this.panel3.TabIndex = 22;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.button4);
            this.panel2.Controls.Add(this.textBox3);
            this.panel2.Controls.Add(this.textBox4);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.hScrollBar2);
            this.panel2.Controls.Add(this.hScrollBar3);
            this.panel2.Controls.Add(this.comboBox1);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.numericUpDown1);
            this.panel2.Controls.Add(this.numericUpDown2);
            this.panel2.Location = new System.Drawing.Point(261, 180);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(234, 221);
            this.panel2.TabIndex = 23;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(175, 4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(54, 26);
            this.button4.TabIndex = 18;
            this.button4.Text = "Defaults";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // textBox3
            // 
            this.textBox3.Enabled = false;
            this.textBox3.Location = new System.Drawing.Point(175, 165);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(41, 20);
            this.textBox3.TabIndex = 17;
            // 
            // textBox4
            // 
            this.textBox4.Enabled = false;
            this.textBox4.Location = new System.Drawing.Point(175, 109);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(41, 20);
            this.textBox4.TabIndex = 16;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 168);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Gamma";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 116);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Master";
            // 
            // hScrollBar2
            // 
            this.hScrollBar2.Location = new System.Drawing.Point(8, 135);
            this.hScrollBar2.Name = "hScrollBar2";
            this.hScrollBar2.Size = new System.Drawing.Size(208, 19);
            this.hScrollBar2.TabIndex = 13;
            // 
            // hScrollBar3
            // 
            this.hScrollBar3.Location = new System.Drawing.Point(8, 189);
            this.hScrollBar3.Name = "hScrollBar3";
            this.hScrollBar3.Size = new System.Drawing.Size(208, 19);
            this.hScrollBar3.TabIndex = 12;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(46, 34);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(105, 21);
            this.comboBox1.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 37);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Tyre:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 93);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(72, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "Sliding Shake";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(4, 4);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(141, 16);
            this.label9.TabIndex = 0;
            this.label9.Text = "Brake FFB Settings";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(7, 63);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(73, 13);
            this.label10.TabIndex = 8;
            this.label10.Text = "Rolling Shake";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(86, 92);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(38, 20);
            this.numericUpDown1.TabIndex = 7;
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.numericUpDown2.Location = new System.Drawing.Point(85, 61);
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(38, 20);
            this.numericUpDown2.TabIndex = 6;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.hScrollBar1);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.textBox2);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Location = new System.Drawing.Point(261, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(234, 157);
            this.panel1.TabIndex = 22;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(105, 114);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 0);
            this.label1.TabIndex = 11;
            this.label1.Text = "0";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.Visible = false;
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Location = new System.Drawing.Point(8, 127);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(217, 17);
            this.hScrollBar1.TabIndex = 10;
            this.hScrollBar1.Visible = false;
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(155, 84);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(65, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Motor test";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(82, 83);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(65, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "Live data";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(7, 83);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(65, 23);
            this.button3.TabIndex = 6;
            this.button3.Text = "Reset";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(175, 54);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(50, 20);
            this.textBox1.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoEllipsis = true;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Maximum shake allowed";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(169, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Shake percentage sent to Arduino";
            // 
            // textBox2
            // 
            this.textBox2.Enabled = false;
            this.textBox2.Location = new System.Drawing.Point(175, 28);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(50, 20);
            this.textBox2.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(4, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(128, 16);
            this.label4.TabIndex = 0;
            this.label4.Text = "Brake FFB Status";
            // 
            // brakeSettingsPanel
            // 
            this.brakeSettingsPanel.BackColor = System.Drawing.Color.Transparent;
            this.brakeSettingsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.brakeSettingsPanel.Controls.Add(this.defaultButton);
            this.brakeSettingsPanel.Controls.Add(this.gammaTextBox);
            this.brakeSettingsPanel.Controls.Add(this.masterTextBox);
            this.brakeSettingsPanel.Controls.Add(this.Gammalabel);
            this.brakeSettingsPanel.Controls.Add(this.Masterlabel);
            this.brakeSettingsPanel.Controls.Add(this.masterScrollBar);
            this.brakeSettingsPanel.Controls.Add(this.gammaScrollBar);
            this.brakeSettingsPanel.Controls.Add(this.tyreCB);
            this.brakeSettingsPanel.Controls.Add(this.tyreLabel);
            this.brakeSettingsPanel.Controls.Add(this.Slidinglabel);
            this.brakeSettingsPanel.Controls.Add(this.Settingslabel);
            this.brakeSettingsPanel.Controls.Add(this.Rolinglabel);
            this.brakeSettingsPanel.Controls.Add(this.state2Spinner);
            this.brakeSettingsPanel.Controls.Add(this.state1Spinner);
            this.brakeSettingsPanel.Location = new System.Drawing.Point(12, 180);
            this.brakeSettingsPanel.Name = "brakeSettingsPanel";
            this.brakeSettingsPanel.Size = new System.Drawing.Size(234, 221);
            this.brakeSettingsPanel.TabIndex = 21;
            // 
            // defaultButton
            // 
            this.defaultButton.Location = new System.Drawing.Point(175, 4);
            this.defaultButton.Name = "defaultButton";
            this.defaultButton.Size = new System.Drawing.Size(54, 26);
            this.defaultButton.TabIndex = 18;
            this.defaultButton.Text = "Defaults";
            this.defaultButton.UseVisualStyleBackColor = true;
            // 
            // gammaTextBox
            // 
            this.gammaTextBox.Enabled = false;
            this.gammaTextBox.Location = new System.Drawing.Point(175, 165);
            this.gammaTextBox.Name = "gammaTextBox";
            this.gammaTextBox.Size = new System.Drawing.Size(41, 20);
            this.gammaTextBox.TabIndex = 17;
            // 
            // masterTextBox
            // 
            this.masterTextBox.Enabled = false;
            this.masterTextBox.Location = new System.Drawing.Point(175, 109);
            this.masterTextBox.Name = "masterTextBox";
            this.masterTextBox.Size = new System.Drawing.Size(41, 20);
            this.masterTextBox.TabIndex = 16;
            // 
            // Gammalabel
            // 
            this.Gammalabel.AutoSize = true;
            this.Gammalabel.Location = new System.Drawing.Point(5, 168);
            this.Gammalabel.Name = "Gammalabel";
            this.Gammalabel.Size = new System.Drawing.Size(43, 13);
            this.Gammalabel.TabIndex = 15;
            this.Gammalabel.Text = "Gamma";
            // 
            // Masterlabel
            // 
            this.Masterlabel.AutoSize = true;
            this.Masterlabel.Location = new System.Drawing.Point(5, 116);
            this.Masterlabel.Name = "Masterlabel";
            this.Masterlabel.Size = new System.Drawing.Size(39, 13);
            this.Masterlabel.TabIndex = 14;
            this.Masterlabel.Text = "Master";
            // 
            // masterScrollBar
            // 
            this.masterScrollBar.Location = new System.Drawing.Point(8, 135);
            this.masterScrollBar.Name = "masterScrollBar";
            this.masterScrollBar.Size = new System.Drawing.Size(208, 19);
            this.masterScrollBar.TabIndex = 13;
            // 
            // gammaScrollBar
            // 
            this.gammaScrollBar.Location = new System.Drawing.Point(8, 189);
            this.gammaScrollBar.Name = "gammaScrollBar";
            this.gammaScrollBar.Size = new System.Drawing.Size(208, 19);
            this.gammaScrollBar.TabIndex = 12;
            // 
            // tyreCB
            // 
            this.tyreCB.FormattingEnabled = true;
            this.tyreCB.Location = new System.Drawing.Point(46, 34);
            this.tyreCB.Name = "tyreCB";
            this.tyreCB.Size = new System.Drawing.Size(105, 21);
            this.tyreCB.TabIndex = 11;
            // 
            // tyreLabel
            // 
            this.tyreLabel.AutoSize = true;
            this.tyreLabel.Location = new System.Drawing.Point(9, 37);
            this.tyreLabel.Name = "tyreLabel";
            this.tyreLabel.Size = new System.Drawing.Size(31, 13);
            this.tyreLabel.TabIndex = 10;
            this.tyreLabel.Text = "Tyre:";
            // 
            // Slidinglabel
            // 
            this.Slidinglabel.AutoSize = true;
            this.Slidinglabel.Location = new System.Drawing.Point(8, 93);
            this.Slidinglabel.Name = "Slidinglabel";
            this.Slidinglabel.Size = new System.Drawing.Size(72, 13);
            this.Slidinglabel.TabIndex = 9;
            this.Slidinglabel.Text = "Sliding Shake";
            // 
            // Settingslabel
            // 
            this.Settingslabel.AutoSize = true;
            this.Settingslabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Settingslabel.Location = new System.Drawing.Point(4, 4);
            this.Settingslabel.Name = "Settingslabel";
            this.Settingslabel.Size = new System.Drawing.Size(141, 16);
            this.Settingslabel.TabIndex = 0;
            this.Settingslabel.Text = "Brake FFB Settings";
            // 
            // Rolinglabel
            // 
            this.Rolinglabel.AutoSize = true;
            this.Rolinglabel.Location = new System.Drawing.Point(7, 63);
            this.Rolinglabel.Name = "Rolinglabel";
            this.Rolinglabel.Size = new System.Drawing.Size(73, 13);
            this.Rolinglabel.TabIndex = 8;
            this.Rolinglabel.Text = "Rolling Shake";
            // 
            // state2Spinner
            // 
            this.state2Spinner.Location = new System.Drawing.Point(86, 92);
            this.state2Spinner.Name = "state2Spinner";
            this.state2Spinner.Size = new System.Drawing.Size(38, 20);
            this.state2Spinner.TabIndex = 7;
            // 
            // state1Spinner
            // 
            this.state1Spinner.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.state1Spinner.Location = new System.Drawing.Point(85, 61);
            this.state1Spinner.Name = "state1Spinner";
            this.state1Spinner.Size = new System.Drawing.Size(38, 20);
            this.state1Spinner.TabIndex = 6;
            // 
            // brakeStatusPanel
            // 
            this.brakeStatusPanel.BackColor = System.Drawing.Color.Transparent;
            this.brakeStatusPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.brakeStatusPanel.Controls.Add(this.testLabel);
            this.brakeStatusPanel.Controls.Add(this.testScrollBar);
            this.brakeStatusPanel.Controls.Add(this.testButton);
            this.brakeStatusPanel.Controls.Add(this.monitorButton);
            this.brakeStatusPanel.Controls.Add(this.resetButton);
            this.brakeStatusPanel.Controls.Add(this.maxRumbleTextBox);
            this.brakeStatusPanel.Controls.Add(this.maxRumbleLabel);
            this.brakeStatusPanel.Controls.Add(this.rumbleLabel);
            this.brakeStatusPanel.Controls.Add(this.rumbleSentTextBox);
            this.brakeStatusPanel.Controls.Add(this.Statuslabel);
            this.brakeStatusPanel.Location = new System.Drawing.Point(12, 12);
            this.brakeStatusPanel.Name = "brakeStatusPanel";
            this.brakeStatusPanel.Size = new System.Drawing.Size(234, 157);
            this.brakeStatusPanel.TabIndex = 20;
            // 
            // testLabel
            // 
            this.testLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.testLabel.Location = new System.Drawing.Point(105, 114);
            this.testLabel.Name = "testLabel";
            this.testLabel.Size = new System.Drawing.Size(26, 0);
            this.testLabel.TabIndex = 11;
            this.testLabel.Text = "0";
            this.testLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.testLabel.Visible = false;
            // 
            // testScrollBar
            // 
            this.testScrollBar.Location = new System.Drawing.Point(8, 127);
            this.testScrollBar.Name = "testScrollBar";
            this.testScrollBar.Size = new System.Drawing.Size(217, 17);
            this.testScrollBar.TabIndex = 10;
            this.testScrollBar.Visible = false;
            // 
            // testButton
            // 
            this.testButton.Enabled = false;
            this.testButton.Location = new System.Drawing.Point(155, 84);
            this.testButton.Name = "testButton";
            this.testButton.Size = new System.Drawing.Size(65, 23);
            this.testButton.TabIndex = 8;
            this.testButton.Text = "Motor test";
            this.testButton.UseVisualStyleBackColor = true;
            // 
            // monitorButton
            // 
            this.monitorButton.Location = new System.Drawing.Point(82, 83);
            this.monitorButton.Name = "monitorButton";
            this.monitorButton.Size = new System.Drawing.Size(65, 23);
            this.monitorButton.TabIndex = 7;
            this.monitorButton.Text = "Live data";
            this.monitorButton.UseVisualStyleBackColor = true;
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(7, 83);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(65, 23);
            this.resetButton.TabIndex = 6;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            // 
            // maxRumbleTextBox
            // 
            this.maxRumbleTextBox.Enabled = false;
            this.maxRumbleTextBox.Location = new System.Drawing.Point(175, 54);
            this.maxRumbleTextBox.Name = "maxRumbleTextBox";
            this.maxRumbleTextBox.Size = new System.Drawing.Size(50, 20);
            this.maxRumbleTextBox.TabIndex = 3;
            // 
            // maxRumbleLabel
            // 
            this.maxRumbleLabel.AutoEllipsis = true;
            this.maxRumbleLabel.AutoSize = true;
            this.maxRumbleLabel.Location = new System.Drawing.Point(4, 57);
            this.maxRumbleLabel.Name = "maxRumbleLabel";
            this.maxRumbleLabel.Size = new System.Drawing.Size(122, 13);
            this.maxRumbleLabel.TabIndex = 2;
            this.maxRumbleLabel.Text = "Maximum shake allowed";
            // 
            // rumbleLabel
            // 
            this.rumbleLabel.AutoSize = true;
            this.rumbleLabel.Location = new System.Drawing.Point(4, 32);
            this.rumbleLabel.Name = "rumbleLabel";
            this.rumbleLabel.Size = new System.Drawing.Size(169, 13);
            this.rumbleLabel.TabIndex = 1;
            this.rumbleLabel.Text = "Shake percentage sent to Arduino";
            // 
            // rumbleSentTextBox
            // 
            this.rumbleSentTextBox.Enabled = false;
            this.rumbleSentTextBox.Location = new System.Drawing.Point(175, 28);
            this.rumbleSentTextBox.Name = "rumbleSentTextBox";
            this.rumbleSentTextBox.Size = new System.Drawing.Size(50, 20);
            this.rumbleSentTextBox.TabIndex = 0;
            // 
            // Statuslabel
            // 
            this.Statuslabel.AutoSize = true;
            this.Statuslabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Statuslabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Statuslabel.Location = new System.Drawing.Point(4, 4);
            this.Statuslabel.Name = "Statuslabel";
            this.Statuslabel.Size = new System.Drawing.Size(128, 16);
            this.Statuslabel.TabIndex = 0;
            this.Statuslabel.Text = "Brake FFB Status";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(535, 492);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.bottomPanel);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.topPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 530);
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Brake Shaker";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.brakeSettingsPanel.ResumeLayout(false);
            this.brakeSettingsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.state2Spinner)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.state1Spinner)).EndInit();
            this.brakeStatusPanel.ResumeLayout(false);
            this.brakeStatusPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        public ACPhysic ReadSharedMem(MemoryMappedFile curMappedFile)
		{
			ACPhysic result;
			using (MemoryMappedViewStream memoryMappedViewStream = curMappedFile.CreateViewStream())
			{
				using (BinaryReader binaryReader = new BinaryReader(memoryMappedViewStream))
				{
					int count = Marshal.SizeOf(typeof(ACPhysic));
					byte[] value = binaryReader.ReadBytes(count);
					GCHandle gCHandle = GCHandle.Alloc(value, GCHandleType.Pinned);
					ACPhysic physics = (ACPhysic)Marshal.PtrToStructure(gCHandle.AddrOfPinnedObject(), typeof(ACPhysic));
					gCHandle.Free();
					result = physics;
				}
			}
			return result;
		}

		private float ProcessBrakeRumble(ACPhysic curPhysics)
		{
			float num = 0;
			float[] array = new float[4];
			float[] curWheelLoadRatio = array;
			if ((double)Math.Abs(curPhysics.speedKmh) > 0.05)
			{
				if (curPhysics.brake != 0)
				{
					for (short tireCounter = 0; tireCounter <= 3; tireCounter += 1)
					{
                        currentSlipRate[(int)tireCounter] = curPhysics.wheelSlip[(int)tireCounter];
						curWheelLoadRatio[(int)tireCounter] = curPhysics.wheelLoad[(int)tireCounter] / curTyreVariables[tyreChosen].maxWheelLoad[(int)tireCounter];
						if (curWheelLoadRatio[(int)tireCounter] >= 1)   // If Current Wheel Load Ratio is greater than 1 it means that
                        {                                               // the Current Wheel Load is greater than Max Wheel Load for the chosen Tire
							curWheelLoadRatio[(int)tireCounter] = 1;    // therefore clamp the value to 1
						}
						float num3;
						if (curPhysics.wheelSlip[(int)tireCounter] <= 1)
						{
							num3 = (float)(Math.Pow((double)curPhysics.wheelSlip[(int)tireCounter], curTyreVariables[tyreChosen].slipExponent) * Math.Pow((double)curWheelLoadRatio[(int)tireCounter], curTyreVariables[tyreChosen].rollingExponent) * (double)maxRumblePerRollingWheel);
                            // num3 = ((0 <> 1) ^ 0.8) * (0 <> 1)^ 0.4* 0<>1
                            currentRollingWheelLoad[(int)tireCounter] = curPhysics.wheelLoad[(int)tireCounter];
							num *= (float)(1.0 - Math.Pow((double)Math.Abs(curPhysics.steerAngle), curTyreVariables[tyreChosen].cornerRollingExponent) * curTyreVariables[tyreChosen].cornerRollingReductionFactor);
						}
						else
						{
							num3 = (float)Math.Pow((double)(curWheelLoadRatio[(int)tireCounter] * curTyreVariables[tyreChosen].slidingCorrectionFactor), curTyreVariables[tyreChosen].slidingExponent) * maxRumblePerSlidingWheel;
                            // num3 = (((0 <> 1)*1.12)^0.8) * (0 <> 1)        0 < 0.314 > 1.095
                            currentSlidingWheelLoad[(int)tireCounter] = curPhysics.wheelLoad[(int)tireCounter];
						}
						if (num3 < 0)
						{
							num3 = 0;
						}
						num += num3;
					}
					num = (float)Math.Pow((double)num, (double)gammaExponent);  // num^(0 <> 0.2)
					num *= 100;
					num *= (float)Math.Pow((double)curPhysics.brake, curTyreVariables[tyreChosen].brakeScalingExponent); // 0 <> num/2
                    num *= (float)master / 100;
                    if (num > 100)
					{
						num = 100;
					}
					if (sessionFlags.monitorFormShown)
					{
                        updateMonitorForm();
					}
				}   // end if brake != 0
            }   //end if SpeedKmh > 0.05
            else
			{
                processCarMass(curPhysics);
			}
			return num;
		}

		private void processCarMass(ACPhysic curPhysics)
		{
            carMass = 0;
			for (short num = 0; num <= 3; num += 1)
			{
                carMass += curPhysics.wheelLoad[(int)num];
			}
			for (short num = 0; num <= 3; num += 1)
			{
                curTyreVariables[tyreChosen].maxWheelLoad[(int)num] = carMass * curTyreVariables[tyreChosen].wheelLoadFactor * curTyreVariables[tyreChosen].wheelLoadCoefficient[(int)num];
			}
		}

		private void updateMonitorForm()
		{
			for (short num = 0; num <= 3; num += 1)
			{
				if (currentRollingWheelLoad[(int)num] > maxRollingWheelLoad[(int)num])
				{
                    maxRollingWheelLoad[(int)num] = currentRollingWheelLoad[(int)num];
				}
				if (currentSlidingWheelLoad[(int)num] > maxSlidingWheelLoad[(int)num])
				{
                    maxSlidingWheelLoad[(int)num] = currentSlidingWheelLoad[(int)num];
				}
                MonitorForm.UpdateValues(maxRollingWheelLoad, maxSlidingWheelLoad, currentSlipRate, carMass, curTyreVariables[tyreChosen].maxWheelLoad[0], curTyreVariables[tyreChosen].slidingCorrectionFactor);
			}
		}

		private void CheckForPhysics()
		{
			try
			{
                acPhysicsSharedMem = MemoryMappedFile.OpenExisting(acPhysicsSharedMemFileLocation);
                sessionFlags.sharedMemAvailable = true;
                StatusTelemetryLabel2.Text = "YES";
			}
			catch
			{
                sessionFlags.sharedMemAvailable = false;
                StatusTelemetryLabel2.Text = "NO";
                ClearInterface();
			}
		}

		private void CheckForComPort()
		{
			try
			{
                port = new SerialPort(comPortComboBox.SelectedItem.ToString(), 115200, Parity.None, 8, StopBits.One);
                port.ReadTimeout = 1000;
                port.Open();
                port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
                sessionFlags.comPortAvailable = true;
                StatusCOMLabel2.Text = "YES";
                this.testButton.Enabled = true;
            }
			catch
			{
                sessionFlags.comPortAvailable = false;
                StatusCOMLabel2.Text = "NO";
                this.testButton.Enabled = false;
                sessionFlags.testMode = false;
            }
		}
		
		private void comPortComboBox_DropDown(object sender, EventArgs e)
        {
            var ports = SerialPort.GetPortNames();
            comPortComboBox.DataSource = ports;
        }

		public bool CheckForACSProcess()
		{
			bool result = false;
			try
			{
				Process.GetProcessesByName("acs.exe");
				result = true;
			}
			catch
			{
			}
			return result;
		}

		private void ClearInterface()
		{
			for (int i = 0; i <= 3; i++)
			{
                maxRollingWheelLoad[i] = 0;
                maxSlidingWheelLoad[i] = 0;
			}
            previousMaxRumble = 0;
            maxRumbleTextBox.Text = "0";
            rumbleSentTextBox.Text = "0";
		}
		
        private void testButton_Click(object sender, EventArgs e)
        {
            if (!sessionFlags.testMode)
            {
                sessionFlags.testMode = true;
                this.testButton.Text = "Stop testing";
            }
            else
            {
                sessionFlags.testMode = false;
                this.testButton.Text = "Motor test";
            }
            this.testScrollBar.Visible = sessionFlags.testMode;
            this.testLabel.Visible = sessionFlags.testMode;
        }

        private void testScrollBar_ValueChanged(object sender, EventArgs e)
        {
            this.testLabel.Text = testScrollBar.Value.ToString();
        }

		private void state1Spinner_ValueChanged(object sender, EventArgs e)
		{
			float num = (float)state1Spinner.Value;
			num /= 100;
            maxRumblePerRollingWheel = num;
			Settings.Default.RollingRumble = maxRumblePerRollingWheel;
			Settings.Default.Save();
		}

		private void state2Spinner_ValueChanged(object sender, EventArgs e)
		{
			float num = (float)state2Spinner.Value;
			num /= 100;
            maxRumblePerSlidingWheel = num;
			Settings.Default.SlidingRumble = maxRumblePerSlidingWheel;
			Settings.Default.Save();
		}

		private void tyreCB_TextChanged(object sender, EventArgs e)
		{
			string text = tyreCB.Text;
			if (text != null)
			{
				if (text == "Vintage")
				{
                    tyreChosen = 0;
					goto whenFinished;
				}
				if (text == "Road")
				{
                    tyreChosen = 1;
					goto whenFinished;
				}
				if (text == "Semislick")
				{
                    tyreChosen = 2;
					goto whenFinished;
				}
				if (text == "Slick")
				{
                    tyreChosen = 3;
					goto whenFinished;
				}
			}
            tyreChosen = 3;
			whenFinished:
			Settings.Default.tyreChosen = tyreChosen;
			Settings.Default.Save();
		}

		private void timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			/*if (counter >= 300)
			{
                previousMaxRumble = 0;
				if (!sessionFlags.sharedMemAvailable)
				{*/
                    CheckForPhysics();
				/*}
				if (!sessionFlags.comPortAvailable)
				{*/
                    CheckForComPort();
				/*}
                counter = 0;
			}*/
			if (sessionFlags.comPortAvailable && sessionFlags.sharedMemAvailable)
			{
				try
				{
                    acPhysics = ReadSharedMem(acPhysicsSharedMem);
                    rumble = ProcessBrakeRumble(acPhysics);
				}
				catch
				{
                    sessionFlags.sharedMemAvailable = false;
                    StatusTelemetryLabel2.Text = "NO";
                    rumble = 0;
                    ClearInterface();
				}
				try
				{
                    port.Write("<" + rumble.ToString() + ">");
				}
				catch
				{
                    sessionFlags.comPortAvailable = false;
                    StatusCOMLabel2.Text = "NO";
                    showCOMerror();
				}
			}
			else if (sessionFlags.comPortAvailable && !sessionFlags.sharedMemAvailable)
			{
				try
				{
                    if (!sessionFlags.testMode)
                    {
                        port.Write("<0>");
                    }
                    else if (sessionFlags.testMode)
                    {
                        port.Write("<" + this.testScrollBar.Value.ToString() + ">");
                    }
				}
				catch
				{
                    sessionFlags.comPortAvailable = false;
                    StatusCOMLabel2.Text = "NO";
                    sessionFlags.testMode = false;
                    this.testButton.Enabled = false;
                    this.testButton.Text = "Motor test";
                    showCOMerror();
				}
			}
			else if (!sessionFlags.comPortAvailable && sessionFlags.sharedMemAvailable)
			{
				try
				{
                    acPhysics = ReadSharedMem(acPhysicsSharedMem);
                    rumble = ProcessBrakeRumble(acPhysics);
				}
				catch
				{
                    sessionFlags.sharedMemAvailable = false;
                    StatusTelemetryLabel2.Text = "NO";
                    ClearInterface();
                    rumble = 0;
				}
			}
			if (rumble > previousMaxRumble)
			{
                previousMaxRumble = rumble;
                maxRumbleTextBox.Text = previousMaxRumble.ToString();
			}
            rumbleSentTextBox.Text = rumble.ToString();
            counter++;
		}

        private void showCOMerror()
        {
            MessageBox.Show("An Error has occured!\nCannot comunicate with hardware!\nPlease check connections!");
            base.Close();
            Application.Exit();
        }

		private void resetButton_Click(object sender, EventArgs e)
		{
            ClearInterface();
		}

		private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			Debug.WriteLine("Message received: " + port.ReadLine());
		}

		private void monitorButton_Click(object sender, EventArgs e)
		{
			if (!sessionFlags.monitorFormShown)
			{
                MonitorForm.Show();
//                sessionFlags.monitorFormShown = true;
			}
			else
			{
                MonitorForm.Hide();
//                sessionFlags.monitorFormShown = false;
			}
		}

		private void DefaultButton_Click(object sender, EventArgs e)
		{
            maxRumblePerRollingWheel = 0.45f;
            maxRumblePerSlidingWheel = 0.55f;
            master = 100;
            gamma = 0;
            state1Spinner.Value = 45;
            state2Spinner.Value = 55;
            masterScrollBar.Value = 100;
            gammaScrollBar.Value = 0;
            masterTextBox.Text = master.ToString();
            gammaTextBox.Text = gamma.ToString();
			Settings.Default.RollingRumble = maxRumblePerRollingWheel;
			Settings.Default.SlidingRumble = maxRumblePerSlidingWheel;
			Settings.Default.master = master;
			Settings.Default.gamma = gamma;
			Settings.Default.Save();
		}

		private void masterScrollBar_ValueChanged(object sender, EventArgs e)
		{
            master = masterScrollBar.Value;
			Settings.Default.master = master;
			Settings.Default.Save();
            masterTextBox.Text = master.ToString();
		}

		private void gammaScrollBar_ValueChanged(object sender, EventArgs e)
		{
            gamma = gammaScrollBar.Value;
			Settings.Default.gamma = gamma;
			Settings.Default.Save();
            gammaExponent = 1f - (float)gamma * 0.008f; // 0 <-> 0.2
            gammaTextBox.Text = gamma.ToString();
		}

		private void InitializeTyreTypes()
		{
            tyreVariables tyreVariables;
			tyreVariables.tyreType = tyreType[0];
			tyreVariables.upperLimitPerRollingWheel = 0.6f;
			tyreVariables.upperLimitPerSlidingWheel = 0.8f;
			tyreVariables.slidingExponent = 0.8;
			tyreVariables.rollingExponent = 0.4;
			tyreVariables.cornerRollingReductionFactor = 0.33;
			tyreVariables.cornerRollingExponent = 0.5;
			tyreVariables.brakeScalingExponent = 0.5;
			tyreVariables.wheelLoadFactor = wheelLoadFactorList[0];
			tyreVariables.maxWheelLoad = new float[]
			{
				8500,
				8500,
				8500,
				8500
			};
			tyreVariables.wheelLoadCoefficient = new float[]
			{
				1,
				1,
				1,
				1
			};
			tyreVariables.slipExponent = 0.8;
			tyreVariables.slidingCorrectionFactor = 1.12f;
            curTyreVariables[0] = tyreVariables;
			tyreVariables.tyreType = tyreType[1];
			tyreVariables.upperLimitPerRollingWheel = 0.6f;
			tyreVariables.upperLimitPerSlidingWheel = 0.8f;
			tyreVariables.slidingExponent = 0.8;
			tyreVariables.rollingExponent = 0.4;
			tyreVariables.cornerRollingReductionFactor = 0.33;
			tyreVariables.cornerRollingExponent = 0.5;
			tyreVariables.brakeScalingExponent = 0.5;
			tyreVariables.wheelLoadFactor = wheelLoadFactorList[1];
			tyreVariables.maxWheelLoad = new float[]
			{
				8500,
				8500,
				8500,
				8500
			};
			tyreVariables.wheelLoadCoefficient = new float[]
			{
				1,
				1,
				1,
				1
			};
			tyreVariables.slipExponent = 0.8;
			tyreVariables.slidingCorrectionFactor = 1.12f;
            curTyreVariables[1] = tyreVariables;
			tyreVariables.tyreType = tyreType[2];
			tyreVariables.upperLimitPerRollingWheel = 0.6f;
			tyreVariables.upperLimitPerSlidingWheel = 0.8f;
			tyreVariables.slidingExponent = 0.8;
			tyreVariables.rollingExponent = 0.4;
			tyreVariables.cornerRollingReductionFactor = 0.33;
			tyreVariables.cornerRollingExponent = 0.5;
			tyreVariables.brakeScalingExponent = 0.5;
			tyreVariables.wheelLoadFactor = wheelLoadFactorList[2];
			tyreVariables.maxWheelLoad = new float[]
			{
				8500,
				8500,
				8500,
				8500
			};
			tyreVariables.wheelLoadCoefficient = new float[]
			{
				1,
				1,
				1,
				1
			};
			tyreVariables.slipExponent = 0.8;
			tyreVariables.slidingCorrectionFactor = 1.12f;
            curTyreVariables[2] = tyreVariables;
			tyreVariables.tyreType = tyreType[3];
			tyreVariables.upperLimitPerRollingWheel = 0.6f;
			tyreVariables.upperLimitPerSlidingWheel = 0.8f;
			tyreVariables.slidingExponent = 0.8;
			tyreVariables.rollingExponent = 0.4;
			tyreVariables.cornerRollingReductionFactor = 0.33;
			tyreVariables.cornerRollingExponent = 0.5;
			tyreVariables.brakeScalingExponent = 0.5;
			tyreVariables.wheelLoadFactor = wheelLoadFactorList[3];
			tyreVariables.maxWheelLoad = new float[]
			{
				8500,
				8500,
				8500,
				8500
			};
			tyreVariables.wheelLoadCoefficient = new float[]
			{
				1,
				1,
				1,
				1
			};
			tyreVariables.slipExponent = 0.8;
			tyreVariables.slidingCorrectionFactor = 1.12f;
            curTyreVariables[3] = tyreVariables;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
                components.Dispose();
			}
			base.Dispose(disposing);
		}




    }



}
