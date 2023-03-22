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
using ZedGraph;

namespace ShockDynamometer
{
    public partial class Form1 : Form
    {
        private SerialPort _serialPort;
        private RollingPointPairList _forceVelocityData;
        private LineItem _forceVelocityCurve;
        private double _lastForce = 0;

        public Form1()
        {
            InitializeComponent();

            // Setup the graph
            
            GraphPane graphPane = zedGraphControl1.GraphPane;

            graphPane.Title.Text = "Force vs Velocity";
            graphPane.XAxis.Title.Text = "Velocity";
            graphPane.YAxis.Title.Text = "Force";

            _forceVelocityData = new RollingPointPairList(1000);
            _forceVelocityCurve = graphPane.AddCurve("", _forceVelocityData, Color.Blue, SymbolType.None);

            // Setup the serial port
            _serialPort = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);
            _serialPort.DataReceived += SerialPortOnDataReceived;
            _serialPort.Open();
        }

        private void SerialPortOnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string line = _serialPort.ReadLine();
            string[] values = line.Split(',');

            if (values.Length == 2 && double.TryParse(values[0], out double velocity) && double.TryParse(values[1], out double force))
            {
                _lastForce = force;

                // Add the data point to the graph
                _forceVelocityData.Add(velocity, force);

                // Update the graph
                zedGraphControl1.AxisChange();
                zedGraphControl1.Invalidate();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Handle the click event for picture box 1
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            // Handle the click event for picture box 2
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _serialPort.Close();
        }
    }
}
