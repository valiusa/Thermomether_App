using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Thermometer
{
    public partial class Form1 : Form
    {
        SolidBrush sbUpperFill = new SolidBrush(Color.White);
        SolidBrush sbBottomFill = new SolidBrush(Color.Red);
        Pen pMiddleLine = new Pen(Color.Black, 2);
        Pen pRect = new Pen(Color.Black, 5);
        Pen pDegreeLine = new Pen(Color.Black, 2);
        SerialPort myPort;
        String[] ports;
        string data;
        float dataToFloat = 0f;

        public Form1()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {           
            Graphics thermShape = panel1.CreateGraphics();
            Graphics line = e.Graphics;

            // Draws the shape for the thermometer
            thermShape.DrawRectangle(pRect, 70, 50, 100, 400);

            // Drawing 0,25,50,75,100 *C lines
            for (int i = 50; i <= 450; i += 100)
            {
                line.DrawLine(pDegreeLine, 50, i, 60, i);
            }            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Getting aveilable ports
            ports = SerialPort.GetPortNames();

            foreach (var port in ports)
            {
                comboBox1.Items.Add(port);
            }

            // Selects the first element of combobox to be the default
            comboBox1.SelectedItem = ports[0];
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            string selectedPort = comboBox1.GetItemText(comboBox1.SelectedItem);
            myPort = new SerialPort(selectedPort, 115200);

            myPort.Open();
            timer1.Enabled = true;
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            myPort.Close();
            timer1.Enabled = false;
            textBox1.Clear();
            panel1.Refresh();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (timer1.Enabled == true)
            {
                data = myPort.ReadLine();

                textBox1.Text = $"{data} *C";

                dataToFloat = float.Parse(data);

                panel1.CreateGraphics().FillRectangle(sbUpperFill, 71, 51, 99, (450 - (dataToFloat * 4))/* - dataToFloat*/ );
                panel1.CreateGraphics().DrawLine(pMiddleLine, 70, 450 - (dataToFloat * 4), 170, 450 - (dataToFloat * 4));
                panel1.CreateGraphics().FillRectangle(sbBottomFill, 71, 450 - (dataToFloat * 4), 99, dataToFloat * 4);
            }
        }
    }
}
