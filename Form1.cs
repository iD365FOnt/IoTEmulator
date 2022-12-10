using F12020Telemetry;
using Kusto.Cloud.Platform.Utils;
using Kusto.Data;
using Kusto.Data.Common;
using Kusto.Data.Net.Client;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Message = Microsoft.Azure.Devices.Client.Message;
using System.Configuration;

namespace IoTEmulator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public DeviceClient deviceClient;
        public DateTime lastSent;
        public IStreamingIngestProvider kustoClient;
        private void IOT1_Scroll(object sender, EventArgs e)
        {
            this.value1.Text = System.Convert.ToString(this.IOT1.Value);
        }

        private void IOT2_Scroll(object sender, EventArgs e)
        {
            this.value2.Text = System.Convert.ToString(this.IOT2.Value);
        }

        private void IOT3_Scroll(object sender, EventArgs e)
        {
            this.value3.Text = System.Convert.ToString(this.IOT3.Value);
        }

        private void IOT4_Scroll(object sender, EventArgs e)
        {
            this.value4.Text = System.Convert.ToString(this.IOT4.Value);
        }

        private void IOT5_Scroll(object sender, EventArgs e)
        {
            this.value5.Text = System.Convert.ToString(this.IOT5.Value);
        }

        private void IOT6_Scroll(object sender, EventArgs e)
        {
            this.value6.Text = System.Convert.ToString(this.IOT6.Value);
        }

        private void IOT7_Scroll(object sender, EventArgs e)
        {
            this.value7.Text = System.Convert.ToString(this.IOT7.Value);
        }

        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            // Do not access the form's BackgroundWorker reference directly.
            // Instead, use the reference provided by the sender parameter.
            BackgroundWorker bw = sender as BackgroundWorker;

            // Start the time-consuming operation.
            e.Result = TimeConsumingOperation(bw);

            // If the operation was canceled by the user,
            // set the DoWorkEventArgs.Cancel property to true.
            if (bw.CancellationPending)
            {
                e.Cancel = true;
            }
        }

        // This event handler demonstrates how to interpret
        // the outcome of the asynchronous operation implemented
        // in the DoWork event handler.
        private void BackgroundWorker1_RunWorkerCompleted(
            object sender,
            RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                // The user canceled the operation.
                MessageBox.Show("Operation was canceled");
            }
            else if (e.Error != null)
            {
                // There was an error during the operation.
                string msg = String.Format("An error occurred: {0}", e.Error.Message);
                MessageBox.Show(msg);
            }
            else
            {
                // The operation completed normally.
                string msg = String.Format("Result = {0}", e.Result);
                MessageBox.Show(msg);
            }
        }

        // This method models an operation that may take a long time
        // to run. It can be cancelled, it can raise an exception,
        // or it can exit normally and return a result. These outcomes
        // are chosen randomly.
        private int TimeConsumingOperation(
            BackgroundWorker bw)
        {

            while (!bw.CancellationPending)
            {
                this.SendInfo(this.IOTLabel1.Text, this.IOT1.Value, this.checkBox1.Checked);
                this.SendInfo(this.IOTLabel2.Text, this.IOT2.Value, this.checkBox2.Checked);
                this.SendInfo(this.IOTLabel3.Text, this.IOT3.Value, this.checkBox3.Checked);
                this.SendInfo(this.IOTLabel4.Text, this.IOT4.Value, this.checkBox4.Checked);
                this.SendInfo(this.IOTLabel5.Text, this.IOT5.Value, this.checkBox5.Checked);
                this.SendInfo(this.IOTLabel6.Text, this.IOT6.Value, this.checkBox6.Checked);
                this.SendInfo(this.IOTLabel7.Text, this.IOT7.Value, this.checkBox7.Checked);

                System.Threading.Thread.Sleep(System.Convert.ToInt32(this.textBox1.Text));
            }

            return 0;
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            string deviceKey = ConfigurationManager.AppSettings["deviceKey"];
            string deviceId = ConfigurationManager.AppSettings["deviceId"];
            string iotHubHostName = ConfigurationManager.AppSettings["iotHubHostName"];
            var deviceAuthentication = new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey);

            deviceClient = DeviceClient.Create(iotHubHostName, deviceAuthentication, TransportType.Mqtt);

            this.backgroundWorker1.RunWorkerAsync(2000);
        }

        private void StopBtn_Click(object sender, EventArgs e)
        {
            this.backgroundWorker1.CancelAsync();
        }

        private void SendInfo(string sensorId, double value, bool active)
        {
            Random rnd = new Random();
            var settings = new JsonSerializerSettings { DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ" };

            var telemetryDataPoint = new
            {
                value = value + Math.Round((rnd.NextDouble() * 2 - 1) * value * 0.2, 0),
                sensorId,
            };
            string messageString = JsonConvert.SerializeObject(telemetryDataPoint, settings);
            Message message = new Message(Encoding.ASCII.GetBytes(messageString));

            if (active)
            {
                deviceClient.SendEventAsync(message);
            }
        }
    }
}
