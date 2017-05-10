﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elara.Fisher
{
    public partial class UserControlFisher : MetroFramework.Controls.MetroUserControl
    {
        private readonly Fisher m_Fisher;

        public UserControlFisher(Fisher p_Fisher)
        {
            m_Fisher = p_Fisher;

            InitializeComponent();
        }

        private void timerRefreshUI_Tick(object sender, EventArgs e)
        {
            if (m_Fisher.Running)
            {
                metroLabelState.Text = m_Fisher.Engine?.Root?.LastStatus?.ToString() ?? "Idle";
                metroLabelState.Text = m_Fisher.Engine?.Root?.LastStatus?.ToString() ?? "Idle";
                metroButtonToggleFisher.Text = "Stop fisher";
                metroButtonToggleFisher.ForeColor = Color.Red;
            }
            else
            {
                metroButtonToggleFisher.Text = "Start fisher";
                metroButtonToggleFisher.ForeColor = Color.Green;
                metroLabelState.Text = "Stopped";
                metroLabelLastState.Text = "Stopped";
            }
        }

        private void metroButtonToggleFisher_Click(object sender, EventArgs e)
        {
            if (!m_Fisher.Running)
                m_Fisher.Start();
            else
                m_Fisher.Stop();

            metroLabelLastState.Focus(); // Dirty !
        }
    }
}
