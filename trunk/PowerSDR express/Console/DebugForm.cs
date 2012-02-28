﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.IO;
using SlimDX;
using SlimDX.Direct3D9;
using System.Diagnostics;

namespace PowerSDR
{
    public partial class DebugForm : Form
    {
        Console console;
        string file = "debug report.txt";

        public DebugForm(Console c)
        {
            InitializeComponent();
            console = c;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            rtbDebugMsg.Clear();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = "Save debug report";
            saveFileDialog1.ShowDialog();
        }

        #region enable/disable debugging

        private void chkAudio_CheckedChanged(object sender, EventArgs e)
        {
            Audio.debug = chkAudio.Checked;
        }

        private void chkDirectX_CheckedChanged(object sender, EventArgs e)
        {
#if(DirectX)
            Display_DirectX.debug = chkDirectX.Checked;
#endif
        }

        private void chkCAT_CheckedChanged(object sender, EventArgs e)
        {
            console.Siolisten.debug = chkCAT.Checked;
        }

        private void chkConsole_CheckedChanged(object sender, EventArgs e)
        {
            console.debug_enabled = chkConsole.Checked;
        }

        private void chkUSB_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkUSB.Checked)
                {
                    console.g59.SetCallback(console.DebugInvokeCallback);
                    console.g11.SetCallback(console.DebugInvokeCallback);
                }
                else
                {
                    console.g59.callback_enabled = false;
                    console.g11.callback_enabled = false;
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }
        }

        #endregion

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            if (saveFileDialog1.FileName != "")
                file = saveFileDialog1.FileName.ToString();

            BinaryWriter writer = new BinaryWriter(File.Open(file, FileMode.Create));
            writer.Seek(0, SeekOrigin.Begin);
            writer.Write(rtbDebugMsg.Text.ToCharArray(),0, rtbDebugMsg.Text.Length);
            writer.Flush();
            writer.Close();
            writer = null;
        }

        private void buttonTS1_Click(object sender, EventArgs e)
        {
            Direct3D w = new Direct3D();
            Capabilities q = w.GetDeviceCaps(0, DeviceType.Hardware);
            rtbDebugMsg.AppendText("Vertex Shader ver: \n" +  q.VertexShaderVersion.ToString() + "\n");
            rtbDebugMsg.AppendText("Device Type: \n" + q.DeviceType.ToString() + "\n");
            rtbDebugMsg.AppendText("Device Caps: \n" + q.DeviceCaps.ToString() + "\n");
            rtbDebugMsg.AppendText("Device Caps2: \n" + q.DeviceCaps2.ToString() + "\n");
            rtbDebugMsg.AppendText("Pixel Shader: \n" + q.PixelShaderVersion.ToString() + "\n");
            rtbDebugMsg.AppendText("Texture Caps: \n" + q.TextureCaps.ToString() + "\n");
        }
    }
}