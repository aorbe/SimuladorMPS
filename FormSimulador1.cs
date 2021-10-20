using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simulador_MPS
{
    public partial class FormSimulador1 : Form
    {
        public FormSimulador1()
        {
            InitializeComponent();

            Simulador.instance().ProcessChanged1 += new Simulador.ProcessChangedHandler(processChanged);

        }

        bool LockProcessChanged = false;
        delegate void processChangedCallback(int inputs, int outputs, int altura);

        private void processChanged(int inputs, int outputs, int altura)
        {
            if (this.lblY1.InvokeRequired && !LockProcessChanged)
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
                    if ((outputs & 0x0002) != 0)
                        lblY1.BackColor = Color.Green;
                    else
                        lblY1.BackColor = Color.White;
                    if ((outputs & 0x0004) != 0)
                        lblY2.BackColor = Color.Green;
                    else
                        lblY2.BackColor = Color.White;
                    if ((outputs & 0x0008) != 0)
                        lblY3.BackColor = Color.Green;
                    else
                        lblY3.BackColor = Color.White;
                    if ((outputs & 0x0010) != 0)
                        lblY4.BackColor = Color.Green;
                    else
                        lblY4.BackColor = Color.White;
                    if ((outputs & 0x0020) != 0)
                        lblY5.BackColor = Color.Green;
                    else
                        lblY5.BackColor = Color.White;
                    if ((outputs & 0x0040) != 0)
                        lblY6.BackColor = Color.Green;
                    else
                        lblY6.BackColor = Color.White;
                    if ((outputs & 0x0200) != 0)
                        lblY31.BackColor = Color.Green;
                    else
                        lblY31.BackColor = Color.White;
                    if ((outputs & 0x0400) != 0)
                        lblY32.BackColor = Color.Green;
                    else
                        lblY32.BackColor = Color.White;
                    if ((outputs & 0x0800) != 0)
                        lblY33.BackColor = Color.Green;
                    else
                        lblY33.BackColor = Color.White;
                    if ((outputs & 0x1000) != 0)
                        lblY34.BackColor = Color.Green;
                    else
                        lblY34.BackColor = Color.White;

                    if ((inputs & 0x0002) != 0)
                        lblB1.BackColor = Color.Green;
                    else
                        lblB1.BackColor = Color.White;
                    if ((inputs & 0x0004) != 0)
                        lblB2.BackColor = Color.Green;
                    else
                        lblB2.BackColor = Color.White;
                    if ((inputs & 0x0008) != 0)
                        lblS3.BackColor = Color.Green;
                    else
                        lblS3.BackColor = Color.White;
                    if ((inputs & 0x0010) != 0)
                        lblS4.BackColor = Color.Green;
                    else
                        lblS4.BackColor = Color.White;
                    if ((inputs & 0x0020) != 0)
                        lblB5.BackColor = Color.Green;
                    else
                        lblB5.BackColor = Color.White;
                    if ((inputs & 0x0040) != 0)
                        lblB6.BackColor = Color.Green;
                    else
                        lblB6.BackColor = Color.White;

                    if ((inputs & 0x0200) != 0)
                        lblB31.BackColor = Color.Green;
                    else
                        lblB31.BackColor = Color.White;
                    if ((inputs & 0x0400) != 0)
                        lblB32.BackColor = Color.Green;
                    else
                        lblB32.BackColor = Color.White;
                    if ((inputs & 0x0800) != 0)
                        lblB33.BackColor = Color.Green;
                    else
                        lblB33.BackColor = Color.White;
                    if ((inputs & 0x1000) != 0)
                        lblB34.BackColor = Color.Green;
                    else
                        lblB34.BackColor = Color.White;
                    if ((inputs & 0x2000) != 0)
                        lblIND.BackColor = Color.Green;
                    else
                        lblIND.BackColor = Color.White;
                    if ((inputs & 0x4000) != 0)
                        lblCAP.BackColor = Color.Green;
                    else
                        lblCAP.BackColor = Color.White;
                    if ((inputs & 0x8000) != 0)
                        lblFOT.BackColor = Color.Green;
                    else
                        lblFOT.BackColor = Color.White;

                    lblALT.Text = altura.ToString();

                    pictureBox1.Invalidate();


                }
                catch (Exception)
                { }
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Simulador.instance().updateImage1(e.Graphics);
            int pos = Simulador.instance().getPos() + 80;
            lblB33.Location = new Point(lblB33.Location.X, pos);
            lblB34.Location = new Point(lblB34.Location.X, pos);
        }

        private void btnCommand(object sender, EventArgs e)
        {
#if DEBUG
            Label obj = (Label)sender;
            switch (obj.Text)
            {
                case "Y1":
                    Simulador.instance().toggleHoldingRegister(16, 0x0002);
                    break;
                case "Y2":
                    Simulador.instance().toggleHoldingRegister(16, 0x0004);
                    break;
                case "Y3":
                    Simulador.instance().toggleHoldingRegister(16, 0x0008);
                    break;
                case "Y4":
                    Simulador.instance().toggleHoldingRegister(16, 0x0010);
                    break;
                case "Y5":
                    Simulador.instance().toggleHoldingRegister(16, 0x0020);
                    break;
                case "Y6":
                    Simulador.instance().toggleHoldingRegister(16, 0x0040);
                    break;
                case "Y31":
                    Simulador.instance().toggleHoldingRegister(16, 0x0200);
                    break;
                case "Y32":
                    Simulador.instance().toggleHoldingRegister(16, 0x0400);
                    break;
                case "Y33":
                    Simulador.instance().toggleHoldingRegister(16, 0x0800);
                    break;
                case "Y34":
                    Simulador.instance().toggleHoldingRegister(16, 0x1000);
                    break;
            }
#endif
        }

        private void FormSimulador1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
