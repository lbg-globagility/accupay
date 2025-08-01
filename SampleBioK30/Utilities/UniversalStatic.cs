﻿using System;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using SampleBioK30.Info;

namespace SampleBioK30.Utilities
{
    internal class UniversalStatic
    {

        public const string acx_Disconnect = "Disconnected";
        public const string acx_Connect = "Conncected";

        public static bool ValidateIP(string addrString)
        {
            IPAddress address;
            if (IPAddress.TryParse(addrString, out address))
                return true;
            else
                return false;
        }

        public static void DrawLineInFooter(Control control, Color color, int thickness)
        {
            int y = control.Height;
            DrawLine(control, color, 0, y, control.Width, y, thickness);
        }
        public static void DrawLine(Control control, Color color, int x, int y, int x1, int y1, int thickness)
        {
            Graphics graphicsObj = control.CreateGraphics();
            graphicsObj.DrawLine(new Pen(color, thickness), x, y, x1, y1);
        }

        public static bool PingTheDevice(string ipAdd)
        {
            try
            {
                IPAddress ipAddress = IPAddress.Parse(ipAdd);

                Ping pingSender = new Ping();
                PingOptions options = new PingOptions();
                options.DontFragment = true;

                // Create a buffer of 32 bytes of data to be transmitted. 
                string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                int timeout = 120;
                PingReply reply = pingSender.Send(ipAddress, timeout, buffer, options);

                if (reply.Status == IPStatus.Success)
                    return true;
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string SplitPascal(string text)
        {
            Regex r = new Regex("([A-Z]+[a-z]+)");
            string result = r.Replace(text, m => (m.Value.Length > 3 ? m.Value : m.Value.ToLower()) + " ");
            return result;
        }


        public static Image StringToImage(string imageString)
        {
            byte[] bArray1 = Convert.FromBase64String(imageString);
            MemoryStream ms = new MemoryStream(bArray1);
            Image logo = Image.FromStream(ms, true, true);
            return logo;
        }
        public static string IntegerValidation(char[] enteredString, string actualString)
        {
            foreach (char c in enteredString.AsEnumerable())
            {

                if (Char.IsDigit(c))
                { actualString = actualString + c; }
                else
                {
                    actualString.Replace(c, ' ');
                    actualString.Trim();
                }
            }
            return actualString;
        }

        public static void ValidateInteger(TextBox tbx)
        {
            string actualString = string.Empty;
            char[] enteredString = tbx.Text.ToCharArray();
            tbx.Text = IntegerValidation(enteredString, actualString);
            tbx.Select(tbx.Text.Length, 0);
        }



        public static void ChangeGridProperties(DataGridView dgvMaster)
        {
            dgvMaster.DefaultCellStyle.Font = new Font("Segoe UI", 8F);
            dgvMaster.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMaster.RowHeadersWidth = 10;
            dgvMaster.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgvMaster.RowHeadersVisible = false;
            dgvMaster.RowTemplate.DefaultCellStyle.Padding = new Padding(5, 0, 10, 0);
            foreach (DataGridViewColumn c in dgvMaster.Columns)
            {
                c.Resizable = DataGridViewTriState.False;
                c.ReadOnly = true;
            }
            dgvMaster.AllowUserToOrderColumns = true;
            dgvMaster.BackgroundColor = SystemColors.Control;
            dgvMaster.BorderStyle = BorderStyle.Fixed3D;
        }


        public static void SaveConfigurations(ConfigurationInfo config)
        {
            string fileName = "C:\\AccuPayFPConfigurations\\Configuration.txt";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            using (StreamWriter writeConfig = new StreamWriter(fileName, true))
            {
                writeConfig.WriteLine(config.IpAddress);
                writeConfig.WriteLine(config.Port);
                writeConfig.WriteLine(config.MachineNumber);
                writeConfig.WriteLine(config.RunOnStartUp);
            }
        }

        public static ConfigurationInfo GetConfigurationInfo()
        {
            ConfigurationInfo config = null;
            string fileName = "C:\\AccuPayFPConfigurations\\Configuration.txt";
            if (!File.Exists(fileName))
            {
                return null;
            }
            using (StreamReader readConfig = new StreamReader(fileName))
            {
                string line = String.Empty;
                string[] lineArray = new string[4];
                int i = 0;
                while ((line = readConfig.ReadLine()) != null)
                {
                    lineArray[i] = line;
                    i++;
                }
                if (lineArray.Length >= 4)
                {
                    config = new ConfigurationInfo()
                    {
                        IpAddress = lineArray[0],
                        Port = lineArray[1],
                        MachineNumber = lineArray[2],
                        RunOnStartUp = lineArray[3]
                    };
                }
            }
            return config;
        }

        
        
    }
}
