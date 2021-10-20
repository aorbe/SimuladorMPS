using Simulador_MPS;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Simulador_MPS
{
    public partial class SimuladorForm : Form
    {
        FormSimulador1 frm1;
        FormSimulador2 frm2;

        public SimuladorForm()
        {
            InitializeComponent();

            btnInfo.Image = (Image)(new Bitmap(Simulador_MPS.Properties.Resources.Info_Image, btnInfo.Size));
            Simulador.instance().ConnectionsChanged += new Simulador.ConnectionsChangedHandler(connectionsChanged);
            Simulador.instance().ProcessChanged1 += new Simulador.ProcessChangedHandler(processChanged);
        }

        bool LockProcessChanged = false;
        delegate void processChangedCallback(int inputs, int outputs, int altura);

        private void processChanged(int inputs, int outputs, int altura)
        {
            if (this.lblPressao.InvokeRequired && !LockProcessChanged)
            {
                {
                    lock (this)
                    {
                        LockProcessChanged = true;
                        processChangedCallback d = new processChangedCallback(processChanged);
                        try
                        {
                            this.Invoke(d, inputs, outputs, altura);
                        }
                        catch (Exception) { }
                        finally
                        {
                            LockProcessChanged = false;
                        }
                    }
                }
            }
            else
            {
                try
                {
                    if ((inputs & 0x1) != 0)
                    {
                        btnDownPressure.BackColor = Color.Green;
                        btnUpPressure.BackColor = Color.Green;
                    }
                    else
                    {
                        btnDownPressure.BackColor = Color.White;
                        btnUpPressure.BackColor = Color.White;
                    }

                }
                catch (Exception)
                { }
            }
        }

        bool LockConnectionsChanged = false;
        delegate void connectionsChangedCallback(int numberConnections);

        private void connectionsChanged(int numberConnections)
        {
            if (this.lblConnections.InvokeRequired && !LockConnectionsChanged)
            {
                {
                    lock (this)
                    {
                        LockConnectionsChanged = true;
                        connectionsChangedCallback d = new connectionsChangedCallback(connectionsChanged);
                        try
                        {
                            this.Invoke(d, numberConnections);
                        }
                        catch (Exception) { }
                        finally
                        {
                            LockConnectionsChanged = false;
                        }
                    }
                }
            }
            else
            {
                try
                {
                    lblConnections.Text = numberConnections.ToString();
                }
                catch (Exception)
                { }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int pressao = int.Parse(lblPressao.Text);
            if (pressao < 10)
            {
                pressao++;
                Simulador.instance().pressaoChanged(pressao);
                lblPressao.Text = pressao.ToString();
            }
        }

        private void btnDownPressure_Click(object sender, EventArgs e)
        {
            int pressao = int.Parse(lblPressao.Text);
            if (pressao > 0)
            {
                pressao--;
                Simulador.instance().pressaoChanged(pressao);
                lblPressao.Text = pressao.ToString();
            }
        }

        private void btnPartida_MouseDown(object sender, MouseEventArgs e)
        {
            Simulador.instance().partidaChanged(true);
            btnPartida.BackColor = Color.Green;
        }

        private void btnPartida_MouseUp(object sender, MouseEventArgs e)
        {
            Simulador.instance().partidaChanged(false);
            btnPartida.BackColor = SystemColors.Control;
        }

        private void btnPeca_Click(object sender, EventArgs e)
        {
            Simulador.instance().inserir();

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Simulador.instance().reset();
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            AboutBox1 about = new AboutBox1();
            about.ShowDialog();
        }

        private void btnEst12_Click(object sender, EventArgs e)
        {
            if (frm1 == null)
                frm1 = new FormSimulador1();
            frm1.Show();
        }

        private void btnEst34_Click(object sender, EventArgs e)
        {
            if (frm2 == null)
                frm2 = new FormSimulador2();
            frm2.Show();
        }

    }

}