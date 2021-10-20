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
    public partial class FormSimulador2 : Form
    {

        public FormSimulador2()
        {
            InitializeComponent();
            Simulador.instance().ProcessChanged2 += new Simulador.ProcessChangedHandler(processChanged);
            Simulador.instance().StoreChanged += new Simulador.ProcessChangedHandler(storedChanged);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Simulador.instance().updateImage2(e.Graphics);
        }

        bool LockStoredChanged = false;
        delegate void processStoredCallback(int pos1, int pos2, int pos3);

        private void storedChanged(int pos1, int pos2, int pos3)
        {
            if (this.lblY1.InvokeRequired && !LockProcessChanged)
            {
                {
                    lock (this)
                    {
                        LockStoredChanged = true;
                        processStoredCallback d = new processStoredCallback(storedChanged);
                        try
                        {
                            this.Invoke(d, pos1, pos2, pos3);
                        }
                        catch (Exception) { }
                        finally
                        {
                            LockStoredChanged = false;
                        }
                    }
                }
            }
            else
            {
                try
                {
                    lblPos1.Text = pos1.ToString();
                    lblPos2.Text = pos2.ToString();
                    lblPos3.Text = pos3.ToString();

                }
                catch (Exception)
                { }
            }
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
                    if ((outputs & 0x0001) != 0)
                        lblM0.BackColor = Color.Green;
                    else
                        lblM0.BackColor = Color.White;
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
                        lblM5.BackColor = Color.Green;
                    else
                        lblM5.BackColor = Color.White;
                    if ((outputs & 0x0040) != 0)
                        lblY6.BackColor = Color.Green;
                    else
                        lblY6.BackColor = Color.White;
                    if ((outputs & 0x0080) != 0)
                        lblY7.BackColor = Color.Green;
                    else
                        lblY7.BackColor = Color.White;
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

                    if ((inputs & 0x0001) != 0)
                        lblB0.BackColor = Color.Green;
                    else
                        lblB0.BackColor = Color.White;
                    if ((inputs & 0x0002) != 0)
                        lblB1.BackColor = Color.Green;
                    else
                        lblB1.BackColor = Color.White;
                    if ((inputs & 0x0004) != 0)
                        lblB2.BackColor = Color.Green;
                    else
                        lblB2.BackColor = Color.White;
                    if ((inputs & 0x0008) != 0)
                        lblB3.BackColor = Color.Green;
                    else
                        lblB3.BackColor = Color.White;
                    if ((inputs & 0x0010) != 0)
                        lblB4.BackColor = Color.Green;
                    else
                        lblB4.BackColor = Color.White;
                    if ((inputs & 0x0020) != 0)
                        lblB5.BackColor = Color.Green;
                    else
                        lblB5.BackColor = Color.White;
                    if ((inputs & 0x0040) != 0)
                        lblB6.BackColor = Color.Green;
                    else
                        lblB6.BackColor = Color.White;
                    if ((inputs & 0x0080) != 0)
                        lblB7.BackColor = Color.Green;
                    else
                        lblB7.BackColor = Color.White;

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
                        lblS35.BackColor = Color.Green;
                    else
                        lblS35.BackColor = Color.White;
                    if ((inputs & 0x4000) != 0)
                        lblS36.BackColor = Color.Green;
                    else
                        lblS36.BackColor = Color.White;

                    pictureBox1.Invalidate();


                }
                catch (Exception)
                { }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }

        private void lbl_Manual(object sender, EventArgs e)
        {
#if DEBUG
            Label obj = (Label)sender;
            int output_register = 17;
            switch (obj.Text)
            {
                case "M0":
                    Simulador.instance().toggleHoldingRegister(output_register, 0x0001);
                    break;
                case "Y1":
                    Simulador.instance().toggleHoldingRegister(output_register, 0x0002);
                    break;
                case "Y2":
                    Simulador.instance().toggleHoldingRegister(output_register, 0x0004);
                    break;
                case "Y3":
                    Simulador.instance().toggleHoldingRegister(output_register, 0x0008);
                    break;
                case "Y4":
                    Simulador.instance().toggleHoldingRegister(output_register, 0x0010);
                    break;
                case "M5":
                    Simulador.instance().toggleHoldingRegister(output_register, 0x0020);
                    break;
                case "Y6":
                    Simulador.instance().toggleHoldingRegister(output_register, 0x0040);
                    break;
                case "Y7":
                    Simulador.instance().toggleHoldingRegister(output_register, 0x0080);
                    break;
                case "Y31":
                    Simulador.instance().toggleHoldingRegister(output_register, 0x0200);
                    break;
                case "Y32":
                    Simulador.instance().toggleHoldingRegister(output_register, 0x0400);
                    break;
                case "Y33":
                    Simulador.instance().toggleHoldingRegister(output_register, 0x0800);
                    break;
                case "Y34":
                    Simulador.instance().toggleHoldingRegister(output_register, 0x1000);
                    break;

            }
#endif
        }

        private void FormSimulador2_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
