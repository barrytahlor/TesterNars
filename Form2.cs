using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;

namespace Termie
{
    public partial class Form2 : Form
    {

        public Settings _settings;
        public string _inPort;
        public CommPort _com;

        public Form2(string inPort, Settings settings, CommPort com)
        {
            InitializeComponent();
            this._com    = com;
            this._inPort = inPort;

            this._settings = settings;
            this.Text = "Settings for " + this._inPort;

            int found = 0;
            string[] portList = com.GetAvailablePorts();
            for (int i=0; i<portList.Length; ++i)
                {
                string name = portList[i];
                comboBox1.Items.Add(name);
                if (name == settings.port.PortName)
                    found = i;
                }
            if (portList.Length > 0)
                comboBox1.SelectedIndex = found;

            Int32[] baudRates = {
                9600,14400,19200,
                38400,56000,57600,115200,128000,256000,0
                };
            found = 0;
            for (int i=0; baudRates[i] != 0; ++i)
                {
                comboBox2.Items.Add(baudRates[i].ToString());
                if (baudRates[i] == settings.port.BaudRate)
                    found = i;
                }
            comboBox2.SelectedIndex = found;

            comboBox3.Items.Add("5");
            comboBox3.Items.Add("6");
            comboBox3.Items.Add("7");
            comboBox3.Items.Add("8");
            comboBox3.SelectedIndex = settings.port.DataBits - 5;

            foreach (string s in Enum.GetNames(typeof(Parity)))
                {
                comboBox4.Items.Add(s);
                }
            comboBox4.SelectedIndex = (int)settings.port.Parity;

            foreach (string s in Enum.GetNames(typeof(StopBits)))
                {
                comboBox5.Items.Add(s);
                }
            comboBox5.SelectedIndex = (int)settings.port.StopBits;

            foreach (string s in Enum.GetNames(typeof(Handshake)))
                {
                comboBox6.Items.Add(s);
                }
            comboBox6.SelectedIndex = (int)settings.port.Handshake;

            //switch (_settings.option.AppendToSend)
            //    {
            //    case Settings.Option.AppendType.AppendNothing:
            //        radioButton1.Checked = true;
            //        break;
            //    case Settings.Option.AppendType.AppendCR:
            //        radioButton2.Checked = true;
            //        break;
            //    case Settings.Option.AppendType.AppendLF:
            //        radioButton3.Checked = true;
            //        break;
            //    case Settings.Option.AppendType.AppendCRLF:
            //        radioButton4.Checked = true;
            //        break;
            //    }

            //checkBox1.Checked = this._settings.option.HexOutput;
            //checkBox2.Checked = this._settings.option.MonoFont;
            //checkBox3.Checked = this._settings.option.LocalEcho;
            //checkBox4.Checked = this._settings.option.StayOnTop;
            //checkBox5.Checked = this._settings.option.FilterUseCase;

            textBox2.Text = this._settings.option.ReadTimeOut;


            textBox1.Text     = this._settings.option.LogFileName;
            checkBox1.Checked = this._settings.option.Logging;
        }

		// Open Setting Form
		private void button1_Click(object sender, EventArgs e)
		{
            this._settings.port.PortName        = comboBox1.Text;
            this._settings.port.BaudRate        = Int32.Parse(comboBox2.Text);
            this._settings.port.DataBits        = comboBox3.SelectedIndex + 5;
            this._settings.port.Parity          = (Parity)comboBox4.SelectedIndex;
            this._settings.port.StopBits        = (StopBits)comboBox5.SelectedIndex;
            this._settings.port.Handshake       = (Handshake)comboBox6.SelectedIndex;
            this._settings.option.LogFileName   = textBox1.Text;
            this._settings.option.ReadTimeOut   = textBox2.Text;
            this._settings.option.Logging       = checkBox1.Checked;

            //string stat = this._com.Open(this._settings);
            this._settings.Write(this._inPort);

			Close();
		}

		// Cancel
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

		private void button3_Click(object sender, EventArgs e)
        {
            this._settings.option.LogFileName = "";

            SaveFileDialog fileDialog1 = new SaveFileDialog();

            fileDialog1.Title = "Save Log As";
            fileDialog1.Filter = "Log files (*.log)|*.log|All files (*.*)|*.*";
            fileDialog1.FilterIndex = 2;
            fileDialog1.RestoreDirectory = true;
			fileDialog1.FileName = this._settings.option.LogFileName;

            if (fileDialog1.ShowDialog() == DialogResult.OK)
            {
				textBox1.Text = fileDialog1.FileName;
				if (File.Exists(textBox1.Text))
					File.Delete(textBox1.Text);
			}
            else
            {
				textBox1.Text = "";
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}