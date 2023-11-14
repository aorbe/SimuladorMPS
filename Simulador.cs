using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Simulador_MPS;

namespace Simulador_MPS
{
    
    class Simulador
    {
        // Estações 1 e 2
        Atuador magazine;
        Atuador rotativo;
        Atuador ventosa;
        Atuador elevador;
        Atuador expulsor;
        bool recalc1;

        bool partida;
        bool peca_posicionada;
        int pressao;
        Timer timer;
        List<Piece> pecas1;
        List<Piece> pecas2;
        Point posVentosa;
        Random rand = new Random();
        int numberConnections = 0;
        short[] memOutputs = { 0, 0 };


        // Estações 3 e 4
        Atuador carrossel;
        Atuador fura_desce;
        Atuador fura_prende;
        Atuador fura_motor;
        Atuador furo_teste;
        Atuador mesa_rotativo;
        Atuador mesa_ventosa;
        Atuador sepa_ventosa;
        Atuador sepa_cilindro;
        Atuador sepa_motor;
        bool recalc2;
        int pecasPos1, pecasPos2, pecasPos3;

        private static Simulador _simulador = null;
        private static Simulador_MPS.ModbusServer easyModbusTCPServer;

        public static Simulador instance()
        {
            if (_simulador == null)
            {
                easyModbusTCPServer = new Simulador_MPS.ModbusServer();
                easyModbusTCPServer.Listen();
                _simulador = new Simulador();
                easyModbusTCPServer.HoldingRegistersChanged += 
                    new ModbusServer.HoldingRegistersChangedHandler(_simulador.HoldingRegistersChanged);
            }
            return _simulador;
        }

        delegate void numberOfConnectionsCallback();
        private void NumberOfConnectionsChanged()
        {
            numberConnections =  easyModbusTCPServer.NumberOfConnections;
            ConnectionsChanged(numberConnections);
        }


        private Simulador()
        {
            easyModbusTCPServer.NumberOfConnectedClientsChanged += 
                new ModbusServer.NumberOfConnectedClientsChangedHandler(NumberOfConnectionsChanged);

            magazine = new AtuadorLinear(10, 14, 60.0, 0.0, 250.0);

            // posMagazine.X = 60 + (int)(magazine.pos * 1.4);
            // posMagazine.Y = 250;
            rotativo = new AtuadorRotativo(10, 13.996, 20, -110, 315+110, 150);
            // double fator = 110;
            // angleRotativo = rotativo.pos * 1.3996 + 20;
            // posRotativo.X = 315 + fator - fator * Math.Cos(angleRotativo / 180.0 * Math.PI)));
            // posRotativo.Y = 150 - (int)(fator * Math.Sin(angleRotativo / 180.0 * Math.PI));

            elevador = new AtuadorLinear(10, 0, 600, -22, 260);


            expulsor = new AtuadorLinear(10, 8, 608, 0, 16);
            // posExpulsor.X = 608 + (int)(expulsor.pos * 0.8);
            // posExpulsor.Y = posElevador.Y + 16;
            
            ventosa = new Atuador(50);



            pecas1 = new List<Piece>();
            pecas1.Add(new Piece(250));
            pecas1.Add(new Piece(200));
            pecas1.Add(new Piece(150));

            carrossel = new AtuadorRotativo(360, 1, 0, 82, 0, 137);

            fura_desce = new AtuadorLinear(10, 0, 32, 2.2, 10);
            fura_prende = new AtuadorLinear(12, 0, 196, 0, 136);
            //fura_motor = new AtuadorLinear(10, 0, 41, 0, 78);
            // This position are relative to drill base position
            fura_motor = new AtuadorLinear(1, 0, 9, 0, 68);

            furo_teste = new AtuadorLinear(10, 0, 319, 0, 101);
            mesa_rotativo = new AtuadorLinear(10, -13.6, 318, 0, 375);
            mesa_ventosa = new AtuadorLinear(10, 0, 240, 0, 310);
            // ventosa is related to sepa_motor
            sepa_ventosa = new AtuadorLinear(2, 0, 4, 0, 54);
            sepa_cilindro = new AtuadorLinear(10, 0, 81, 2, 30);
            sepa_motor = new AtuadorLinear(193, 1, 519, 0, 45);

            //Simulador.instance().updateImage2(e.Graphics);
            pecas2 = new List<Piece>();
            pecas2.Add(new Piece());
            pecas2.Add(new Piece());
            pecas2[0].r.Width = 48;
            pecas2[0].r.Height = 48;
            pecas2[0].r.X = 100;
            pecas2[0].r.Y = 228;

            pecas2[1].r.Width = 48;
            pecas2[1].r.Height = 48;
            pecas2[1].r.X = 100;
            pecas2[1].r.Y = 228;
            pecas2[1].tmpValue = 90;
            pecas2[1].local = Piece.LOCAL.MESA;

            pecasPos1 = 0;
            pecasPos2 = 0;
            pecasPos3 = 0;

            recalc1 = true;
            recalc2 = true;

            partida = false;
            peca_posicionada = true;
            int period = 200;
            pressao = 0;
            timer = new Timer(this.atualiza, null, period, period);
        }

        public void Stop()
        {
            timer.Dispose();

        }

        private void erro(string message, int removerPeca)
        {
            var result = System.Windows.Forms.MessageBox.Show(message, "ERRO", System.Windows.Forms.MessageBoxButtons.OK);
            if (removerPeca >= 0)
                pecas1.RemoveAt(removerPeca);
        }

        // Empurra peças que estão depois da peça na posição pos considerando que essa peça irá para posição x
        private bool empurrapecas1(int pos, int x, int y)
        {
            bool changes = false;
            // Podem existir peças na frente
            if (pos > 0)
            {
                int posColisao = x + 85;
                for (int count = pos - 1; count >= 0; count--)
                {
                    Piece p = pecas1[count];
                    if (p.r.Y <= y && p.r.Y + 100 > y && p.r.X < posColisao)
                    {
                        p.r.X = posColisao;
                        posColisao += 85;
                        pecas1[count] = p;
                        changes = true;
                    }

                }
            }
            return changes;

        }

        private short CalcInputs()
        {
            short inputs = 0;
            if (pressao > 4) inputs += 0x0001;
            if (magazine.min()) inputs += 0x0002;
            if (magazine.max()) inputs += 0x0004;
            if (rotativo.min()) inputs += 0x0008;
            if (rotativo.max()) inputs += 0x0010;
            if (ventosa.max()) inputs += 0x0020;
            //if (peca) inputs += 0x0040;            
            if (partida) inputs += 0x0080;
            if (elevador.min()) inputs += 0x0200;
            if (elevador.max()) inputs += 0x0400;
            if (expulsor.min()) inputs += 0x0800;
            if (expulsor.max()) inputs += 0x1000;
            return inputs;
        }

        private void atualizaProcesso1()
        {
            recalc1 = false;
            short outputs = memOutputs[0];
            bool Y1 = (outputs & 0x0002) != 0;
            bool Y2 = (outputs & 0x0004) != 0;
            bool Y3 = (outputs & 0x0008) != 0;
            bool Y4 = (outputs & 0x0010) != 0;
            bool Y5 = (outputs & 0x0020) != 0;

            bool Y31 = (outputs & 0x0200) != 0;
            bool Y32 = (outputs & 0x0400) != 0;
            bool Y33 = (outputs & 0x0800) != 0;
            bool Y34 = (outputs & 0x1000) != 0;


            recalc1 |= magazine.control(Y1, !Y1);
            recalc1 |= ventosa.control(Y2 && !Y3, Y3);
            recalc1 |= rotativo.control(Y4, Y5);
            recalc1 |= elevador.control(Y31, Y32);
            recalc1 |= expulsor.control(Y33, !Y33);

            short inputs = CalcInputs();
            int tipo = 0;
            short altura = 0;

            // Controle das Peças
            for (int count = 0; count < pecas1.Count; count++)
            {
                Piece p = pecas1[count];
                int x = p.r.X;
                int y = p.r.Y;

                if (p.local == Piece.LOCAL.MAGAZINE)
                {
                    if (y == 250)
                    {
                        p.local = Piece.LOCAL.EMPURRADOR;
                        peca_posicionada = true;
                    }
                }

                if (p.local == Piece.LOCAL.EMPURRADOR)
                {
                    // Se a peça for colidir com ventosa
                    if ((magazine.p.X + 110 + 85 >= posVentosa.X) && (x < magazine.p.X + 110))
                    {
                        if (magazine.recua()) recalc1 = true;
                    }

                    // Se a peça for colidir com a próxima


                    // Empurrando peça
                    if (x < magazine.p.X + 110)
                    {
                        x = magazine.p.X + 110;
                        peca_posicionada = false;
                    }

                    // Peça em posição para ser capturada pelo rotativo
                    if (x >= 310 && ventosa.max() && rotativo.min() && !magazine.max())
                    {
                        y = 249;
                        p.local = Piece.LOCAL.ROTATIVO;
                    }
                }

                if (p.local == Piece.LOCAL.ROTATIVO)
                {
                    // Se o vacuo soltar a peça fora de lugar provoca queda rápida
                    if (x != 310 && x != 692 && !Y2)
                    {
                        p.message = "Peça caiu pois foi solta no meio do percurso";
                        p.local = Piece.LOCAL.REMOVER;
                        y += 50;
                        recalc1 = true;
                    }
                    else if (!ventosa.max())
                    {
                        if (x == 310)
                        {
                            p.local = Piece.LOCAL.MAGAZINE;
                            y = 250;
                        }
                        else if (x == 692 && elevador.min() && expulsor.min())
                        {
                            p.local = Piece.LOCAL.ELEVADOR;
                            y = 250;
                        }
                    }
                    else
                    {
                        x = 310 + 191 - (int)(204 * Math.Cos(rotativo.angle / 180.0 * Math.PI));
                        y = 250 + 68 - (int)(204 * Math.Sin(rotativo.angle / 180.0 * Math.PI));
                        if (x > 600 && !elevador.min())
                        {
                            p.message = "Peça removida pois elevador não está em posição";
                            p.local = Piece.LOCAL.REMOVER;
                            recalc1 = true;
                        }
                        if (x > 650 && !expulsor.min())
                        {
                            p.message = "Peça removida. Expulsador está avançado";
                            p.local = Piece.LOCAL.REMOVER;
                            recalc1 = true;
                        }
                    }
                }

                if (p.local == Piece.LOCAL.ELEVADOR)
                {
                    y = elevador.p.Y - 10;

                    // Quando vácuo é ligado e peça e rotativo estão no elevado, a peça é capturada
                    if (x == 692 && y == 250 && ventosa.max() && rotativo.max())
                    {
                        p.local = Piece.LOCAL.ROTATIVO;
                        y = 249;
                    }

                    if (expulsor.p.X + 84 > x)
                    {
                        x = expulsor.p.X + 84;
                        empurrapecas1(count, x, y);
                        if (x >= 772)
                        {
                            if (y == 250)
                                p.local = Piece.LOCAL.REJEICAO;
                            else if (y == 30)
                                p.local = Piece.LOCAL.ESTEIRA;
                            else
                            {
                                p.message = "Peça empurrada do elevador fora da posição";
                                p.local = Piece.LOCAL.REMOVER;
                            }
                        }
                    }
                }

                if (p.local == Piece.LOCAL.ESTEIRA)
                {
                    if (x < 781)
                    {
                        x++;
                        recalc1 = true;
                        empurrapecas1(count, x, y);
                    }
                    if (y < 35)
                    {
                        y++;
                        recalc1 = true;
                        empurrapecas1(count, x, y);
                    }
                    else
                    {
                        y = 35 + (x - 781) / 18;
                    }
                    if (Y34 && x < 1000)
                    {
                        x += 10;
                        recalc1 = true;
                        empurrapecas1(count, x, y);
                    }
                }

                if (p.local == Piece.LOCAL.REJEICAO)
                {
                    x += 5;
                    y = 245 + (x / 36);
                    recalc1 = true;
                    empurrapecas1(count, x, y);
                }

                // Peça na posição de detecção de tipo
                if (x == 692)
                    tipo = p.tipo;
                if (y == 30 && x == 692)
                {
#if DEBUG
                    Console.Write("Altura {0}", altura);
#endif
                    double u1 = 1.0 - rand.NextDouble(); //uniform(0,1] random doubles
                    double u2 = 1.0 - rand.NextDouble();
                    double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
                    altura = (short)(p.tamanho + 20 * randStdNormal);
                }

                // Não tem mais peças
                if (count > 0)
                {
                    Piece next_p = pecas1[count - 1];

                    if (p.local == Piece.LOCAL.MAGAZINE && magazine.min())
                    {
                        // Rearrange pieces
                        if (next_p.r.X > x + p.r.Width)
                        {
                            y += 5;
                            recalc1 = true;
                        }
                        else if (next_p.r.Y - 55 >= y)
                        {
                            y += 5;
                            recalc1 = true;
                        }
                    }
                    else
                    {
                        // Colisão frontal
                        if ((x + p.r.Width > next_p.r.X) && ((y == next_p.r.Y) || (y + 1 == next_p.r.Y)))
                        {
#if DEBUG
                            Console.WriteLine("Colisão X {0} Y{1} L{2}", x, y, p.local.ToString());
                            Console.WriteLine("Colisão X {0} Y{1} L{2}", next_p.r.X, next_p.r.Y, next_p.local.ToString());
#endif
                            p.message = "Colisão lateral entre peças";
                            p.local = Piece.LOCAL.REMOVER;
                        }
                        // Colisão superior
                        if ((y + p.r.Height > next_p.r.Y) && (x == next_p.r.X))
                        {
#if DEBUG
                            Console.WriteLine("Colisão X {0} Y{1} L{2}", x, y, p.local.ToString());
                            Console.WriteLine("Colisão X {0} Y{1} L{2}", next_p.r.X, next_p.r.Y, next_p.local.ToString());
#endif
                            p.message = "Colisão entre peças";
                            p.local = Piece.LOCAL.REMOVER;
                        }
                    }
                }

                p.r.X = x;
                p.r.Y = y;

                pecas1[count] = p;

            }


            lock (pecas1)
            {

                for (int count = pecas1.Count - 1; count >= 0; count--)
                {
                    // Peças que chegaram ao fim do processo também somem
                    if (pecas1[count].r.X >= 1000)
                    {
                        if (pecas1[count].r.Y < 200) {
                            pecas1[count].r.Width = 48;
                            pecas1[count].r.Height = 48;
                            pecas1[count].r.X = 100;
                            pecas1[count].r.Y = 228;
                            pecas2[count].local = Piece.LOCAL.MESA;
                            pecas2[count].tmpValue = carrossel.angle;
                            pecas2.Add(pecas1[count]);
                            
                        }

                        pecas1.RemoveAt(count);
                    }
                    else if (pecas1[count].local == Piece.LOCAL.REMOVER)
                    {
                        erro(pecas1[count].message, count);
                    }


                }
            }

            if (!peca_posicionada)
                inputs += 0x0040;
            if (tipo == 1) // Orange
                inputs += unchecked((short)0xC000);
            else if (tipo == 2) // Brown
                inputs += (short)0x4000;
            else if (tipo == 3) // Silver
                inputs += unchecked((short)0xE000);



            double ventF = 204;
            posVentosa.X = 516 - (int)(ventF * Math.Cos(rotativo.angle / 180.0 * Math.PI));
            posVentosa.Y = 268 - (int)(ventF * Math.Sin(rotativo.angle / 180.0 * Math.PI));



            // Atualizando sinais do processo no mapa Modbus
            easyModbusTCPServer.changeHoldingRegisters(0, inputs);
            easyModbusTCPServer.changeHoldingRegisters(1, altura);

            if (ProcessChanged1 != null)
                ProcessChanged1(inputs, outputs, altura);

#if DEBUG
            Console.WriteLine("Quantidade de Peças {0}", pecas1.Count);
#endif


        }

        private void atualizaProcesso2()
        {
            recalc2 = false;

            short outputs = memOutputs[1];
            bool BM0 = (outputs & 0x0001) != 0;
            bool BY1 = (outputs & 0x0002) != 0;
            bool BY2 = (outputs & 0x0004) != 0;
            bool BY3 = (outputs & 0x0008) != 0;
            bool BY4 = (outputs & 0x0010) != 0;
            bool BM5 = (outputs & 0x0020) != 0;
            bool BY6 = (outputs & 0x0040) != 0;
            bool BY7 = (outputs & 0x0080) != 0;

            bool BY31 = (outputs & 0x0200) != 0;
            bool BY32 = (outputs & 0x0400) != 0;
            bool BY33 = (outputs & 0x0800) != 0;
            bool BY34 = (outputs & 0x1000) != 0;

            if (BM5)
            {
                if (!fura_prende.min())
                {
                    var result = System.Windows.Forms.MessageBox.Show("Cilindro que prende peça preso", "ERRO", System.Windows.Forms.MessageBoxButtons.OK);
                }
                else if (!fura_desce.min())
                {
                    var result = System.Windows.Forms.MessageBox.Show("Broca quebrada", "ERRO", System.Windows.Forms.MessageBoxButtons.OK);
                }
                else if (!furo_teste.min())
                {
                    var result = System.Windows.Forms.MessageBox.Show("Cilindro que verifica furao preso", "ERRO", System.Windows.Forms.MessageBoxButtons.OK);
                }
                else
                    carrossel.angle = (carrossel.angle + 1) % 360;
                recalc2 = true;
            }

            recalc2 |= fura_motor.control(BM0, !BM0);
            recalc2 |= fura_desce.control(BY1, BY2);
            recalc2 |= fura_prende.control(BY4, !BY4);
            recalc2 |= furo_teste.control(BY3, !BY3);
            recalc2 |= mesa_rotativo.control(BY7, !BY7);
            recalc2 |= mesa_ventosa.control(BY6, !BY6);

            recalc2 |= sepa_cilindro.control(BY31, !BY31);
            recalc2 |= sepa_ventosa.control(BY32, !BY32);
            recalc2 |= sepa_motor.control(BY33 && !BY34, BY33 && BY34);

            bool peca_entrada = false;
            bool pos2 = ((sepa_motor.pos >= 118) && (sepa_motor.pos <= 123));
            bool pos3 = ((sepa_motor.pos >= 58) && (sepa_motor.pos <= 64));

            // Controle das Peças
            for (int count = 0; count < pecas2.Count; count++)
            {
                Piece p = pecas2[count];
                int x = p.r.X;
                int y = p.r.Y;

                if (p.local == Piece.LOCAL.MAGAZINE)
                {
                    p.local = Piece.LOCAL.MESA;
                    p.tmpValue = carrossel.angle;
                }
                
                if (p.local == Piece.LOCAL.MESA)
                {
                    // 1, 0, 82, 0, 137
                    x = (int)(175.5 - 75.5 * Math.Cos((carrossel.angle - p.tmpValue) / 180.0 * Math.PI));
                    y = (int)(228 - 75.5 * Math.Sin((carrossel.angle - p.tmpValue) / 180.0 * Math.PI));
                    // Start piece position 100, 228
                    // Pos 174, 303
                    if ((x<=175) && (y>=303))
                    {
                        if (mesa_ventosa.max() && mesa_rotativo.max())
                        {
                            p.local = Piece.LOCAL.MESA_ROTATIVO;
                            p.tmpValue = x - mesa_rotativo.p.X;
                        }
                    }

                }

                if (p.local == Piece.LOCAL.MESA_ROTATIVO)
                {
                    if (!mesa_ventosa.max())
                    {
                        if (mesa_rotativo.min())
                        {
                            p.local = Piece.LOCAL.RAMPA;
                        }
                        else if (mesa_rotativo.max())
                        {
                            p.local = Piece.LOCAL.MESA;

                        }
                        else
                        {
                            // ERRO
                        }
                    }
                    x = (int)(p.tmpValue + mesa_rotativo.p.X);



                }

                if (p.local == Piece.LOCAL.RAMPA)
                {
                    if (x < 420)
                        x = x + 1;
                    if (x == 420)
                    {
                        p.local = Piece.LOCAL.SEP_ENTRADA;
                    }
                    recalc2 = true;
                }

                if (p.local == Piece.LOCAL.SEP_ENTRADA)
                {
                    x = 709;
                    y = 152;
                    p.r.Width = 40;
                    p.r.Height = 15;

                    if (sepa_cilindro.max() && sepa_ventosa.max() && sepa_motor.max())
                    {
                        p.local = Piece.LOCAL.SEP_PRESA;
                        p.tmpValue = (int) (x - sepa_motor.p.X - sepa_ventosa.p.X);
                        y = sepa_ventosa.p.Y + sepa_cilindro.p.Y;
                    }

                }

                if (p.local == Piece.LOCAL.SEP_PRESA)
                {
                    x = (int) (p.tmpValue + sepa_motor.p.X + sepa_ventosa.p.X);
                    y = 100 + sepa_cilindro.p.Y;

                    if (!sepa_ventosa.max())
                    {
                        if (sepa_motor.min() && sepa_cilindro.max())
                        {
                            p.local = Piece.LOCAL.SER_ARMAZENADA;
                            pecasPos1++;
                        }
                        else if (pos2 && sepa_cilindro.max())
                        {
                            p.local = Piece.LOCAL.SER_ARMAZENADA;
                            pecasPos2++;
                        }
                        else if (pos3 && sepa_cilindro.max())
                        {
                            p.local = Piece.LOCAL.SER_ARMAZENADA;
                            pecasPos3++;
                            
                        }
                        else if (sepa_motor.max() && sepa_cilindro.max())
                        {
                            p.local = Piece.LOCAL.SEP_ENTRADA;
                        }
                        else
                        {
                            p.local = Piece.LOCAL.REMOVER;
                            p.message = "Peça solta fora de posição";
                        }
                    }

                }
                if (p.local == Piece.LOCAL.SER_ARMAZENADA)
                {
                    if (y < 250)
                    {
                        y++;
                        recalc2 = true;
                    }
                }

                if (x == 100)
                    peca_entrada = true;
                p.r.X = x;
                p.r.Y = y;
            }

            for (int count = pecas2.Count - 1; count >= 0; count--)
            {
                // Peças que chegaram ao fim do processo também somem
                if (pecas2[count].r.Y <= 250 && pecas2[count].local == Piece.LOCAL.SER_ARMAZENADA) {
                    pecas2.RemoveAt(count);
                    StoreChanged(pecasPos1, pecasPos2, pecasPos3);
                }
                else if (pecas2[count].local == Piece.LOCAL.REMOVER)
                {
                    erro(pecas2[count].message, count);
                }


            }

            // Estações 3 e 4
            short inputs = 0;

            if (peca_entrada) inputs += 0x0001;
            if (fura_desce.min()) inputs += 0x0002;
            if (fura_desce.max()) inputs += 0x0004;
            if (furo_teste.min()) inputs += 0x0008;
            if (furo_teste.max()) inputs += 0x0010;
            if (fura_prende.min()) inputs += 0x0020;
            if (fura_prende.max()) inputs += 0x0040;
            if (((carrossel.angle + 362) % 90) <= 4) inputs += 0x0080;

            if (pos2) inputs += 0x0200;
            if (pos3) inputs += 0x0400;
            if (sepa_cilindro.min()) inputs += 0x0800;
            if (sepa_cilindro.max()) inputs += 0x1000;
            if (sepa_motor.pos > 191) inputs += 0x2000;
            if (sepa_motor.pos < 3) inputs += 0x4000;
            if (pressao > 4) inputs += unchecked((short)0x8000);

            // Atualizando sinais do processo no mapa Modbus
            easyModbusTCPServer.changeHoldingRegisters(2, inputs);

            if (ProcessChanged2 != null)
                ProcessChanged2(inputs, outputs, 0);

        }

        bool run1 = false;
        bool run2 = false;

        private void atualiza(Object stateInfo)
        {
            if (recalc1 & !run1)
            {
                run1 = true;
                atualizaProcesso1();
                run1 = false;
            }

            if (recalc2 & !run2)
            {
                run2 = true;
                atualizaProcesso2();
                run2 = false;
            }
        }

        delegate void registersChangedCallback(int register, int numberOfRegisters);

        public void HoldingRegistersChanged(int register, int numberOfRegisters)
        {
            if (register <= 16 && (register + numberOfRegisters) > 16)
            {
                if (memOutputs[0] != easyModbusTCPServer.holdingRegisters[16])
                {
                    memOutputs[0] = easyModbusTCPServer.holdingRegisters[16];
                    recalc1 = true;
                }
            }
            if (register <= 17 && (register + numberOfRegisters) > 17)
            {
                if (memOutputs[1] != easyModbusTCPServer.holdingRegisters[17])
                {
                    memOutputs[1] = easyModbusTCPServer.holdingRegisters[17];
                    recalc2 = true;
                }
            }
        }

        public void pressaoChanged(int newvalue)
        {
            pressao = newvalue;
            recalc1 = true;
            recalc2 = true;
        }

        public void reset()
        {
            pecas1.Clear();
            recalc1 = true;
            recalc2 = true;
            atualiza(null);
        }

        public void partidaChanged(bool newvalue)
        {
            partida = newvalue;
            recalc1 = true;
            recalc2 = true;
        }

        public void updateImage1(System.Drawing.Graphics graph)
        {
            Image img = Properties.Resources.Magazine;
            graph.DrawImage(img, magazine.p);

            Image img2 = RotateImage(Properties.Resources.RotativoBraco, (float)rotativo.angle);
            graph.DrawImage(img2, rotativo.p);

            //e.Graphics.DrawImage(img2, posX, posY, img2.Width, img2.Height);

            Image img3 = Properties.Resources.Expulsador;
            graph.DrawImage(img3, expulsor.p.X, expulsor.p.Y + elevador.p.Y);

            Image img4 = Properties.Resources.Ventosa;
            graph.DrawImage(img4, posVentosa);

            Image img5 = Properties.Resources.Elevador;
            graph.DrawImage(img5, elevador.p);


            lock (pecas1)
            {
                foreach (Piece p in pecas1.ToList())
                {
#if DEBUG
                    Console.WriteLine("X {0} Y{1} L{2}", p.r.X, p.r.Y, p.local.ToString());
#endif
                    if (p.r.X <= 773)
                    {
                        graph.FillRectangle(new SolidBrush(p.cor), p.r);
                        graph.DrawRectangle(Pens.Black, p.r);
                    }
                    else
                    {
                        RotateRectangle(graph, p.r, (float)3.5, p.cor);
                    }

                }
            }

        }

        public void updateImage2(System.Drawing.Graphics graph)
        {
            //Simulador.instance().updateImage2(e.Graphics);
            Image img1 = Properties.Resources.Carrossel;
            Image img1r = RotateImage(img1, (float)carrossel.angle);
            graph.DrawImage(img1r, carrossel.p.X, carrossel.p.Y, img1r.Width, img1r.Height);

            Image imgMesaVentosa = Properties.Resources.RotativoVentosa;
            graph.DrawImage(imgMesaVentosa, mesa_rotativo.p.X, mesa_ventosa.p.Y, imgMesaVentosa.Width, imgMesaVentosa.Height);

            Image imgMesaRotativo = Properties.Resources.Rotativo2;
            graph.DrawImage(imgMesaRotativo, mesa_ventosa.p.X - (int)mesa_rotativo.pos*5, mesa_rotativo.p.Y, imgMesaRotativo.Width, imgMesaRotativo.Height);

            Image imgFuradeira = Properties.Resources.Furadeira;
            graph.DrawImage(imgFuradeira, fura_desce.p.X, fura_desce.p.Y, imgFuradeira.Width, imgFuradeira.Height);

            Image imgBroca = Properties.Resources.Broca;
            if (fura_motor.max())
                imgBroca = Properties.Resources.BrocaAzul;
            graph.DrawImage(imgBroca, fura_desce.p.X + fura_motor.p.X, fura_desce.p.Y + fura_motor.p.Y, imgBroca.Width, imgBroca.Height);

            Image imgPrende = Properties.Resources.PrendePeca;
            graph.DrawImage(imgPrende, fura_prende.p.X, fura_prende.p.Y, imgPrende.Width, (int)(imgPrende.Height / (2.2 - fura_prende.pos / 10)));

            Image imgTeste = Properties.Resources.TestePeca;
            graph.DrawImage(imgTeste, furo_teste.p.X, furo_teste.p.Y, imgTeste.Width, (int)(imgTeste.Height / (2 - furo_teste.pos / 10)));

            Image imgVentosa = Properties.Resources.SeparadorVentosa;
            graph.DrawImage(imgVentosa, sepa_motor.p.X + sepa_ventosa.p.X, sepa_ventosa.p.Y + sepa_cilindro.p.Y, imgVentosa.Width, imgVentosa.Height);

            lock (pecas2)
            {
                foreach (Piece p in pecas2.ToList())
                {
#if DEBUG
                    Console.WriteLine("P2 X {0} Y{1} L{2}", p.r.X, p.r.Y, p.local.ToString());
#endif
                    if (p.local == Piece.LOCAL.SEP_ENTRADA || p.local == Piece.LOCAL.SEP_PRESA || p.local == Piece.LOCAL.SER_ARMAZENADA)
                    {
                        graph.FillRectangle(new SolidBrush(p.cor), p.r) ;
                        graph.DrawRectangle(Pens.Black, p.r);
                    }
                    else
                    {
                        graph.FillEllipse(new SolidBrush(p.cor), p.r);
                        graph.DrawEllipse(Pens.Black, p.r);

                        // Piece on position 2
                        if ((p.r.X > 174) && (p.r.Y <= 152))
                        {
                            graph.FillRectangle(new SolidBrush(p.cor), new Rectangle(20, 132, 50, 15));
                            graph.DrawRectangle(Pens.Black, new Rectangle(20, 132, 50, 15));
                        }

                        // Piece on position 3
                        if ((p.r.X >= 249) && (p.r.X <= 253) && (p.r.Y >= 220) && (p.r.Y <= 229))
                        {
                            graph.FillRectangle(new SolidBrush(p.cor), new Rectangle(295, 130, 50, 15));
                            graph.DrawRectangle(Pens.Black, new Rectangle(295, 130, 50, 15));
                        }
                    }

                }
            }

            foreach (Piece p in pecas1.ToList())
            {
                if (p.r.X > 900)
                {
                    if (p.r.Y < 250)
                    {
                        graph.FillEllipse(new SolidBrush(p.cor), p.r.X - 900, 228, 48, 48);
                        graph.DrawEllipse(Pens.Black, p.r.X - 900, 228, 48, 48);
                    }
                }
            }


            Image imgSeparador = Properties.Resources.BaseSeparador;
            graph.DrawImage(imgSeparador, sepa_motor.p.X, sepa_motor.p.Y, imgSeparador.Width, imgSeparador.Height);



        }

        public void RotateRectangle(Graphics g, Rectangle r, float angle, Color cor)
        {
            using (Matrix m = new Matrix())
            {
                m.RotateAt(angle, new PointF(r.Left + (r.Width / 2),
                                            r.Top + (r.Height / 2)));
                g.Transform = m;
                g.FillRectangle(new SolidBrush(cor), r);
                g.DrawRectangle(Pens.Black, r);
                g.ResetTransform();
            }
        }

        public Image RotateImage(Image img, float rotationAngle)
        {
            //create an empty Bitmap image
            Bitmap bmp = new Bitmap(img.Width, img.Height);

            //turn the Bitmap into a Graphics object
            Graphics gfx = Graphics.FromImage(bmp);

            //now we set the rotation point to the center of our image
            gfx.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);

            //now rotate the image
            gfx.RotateTransform(rotationAngle);

            gfx.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);

            //set the InterpolationMode to HighQualityBicubic so to ensure a high
            //quality image once it is transformed to the specified size

            gfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            //now draw our new image onto the graphics object
            gfx.DrawImage(img, 0, 0, img.Width, img.Height);

            //dispose of our Graphics object
            gfx.Dispose();

            //return the image
            return bmp;
        }

        public int getPos()
        {
            return elevador.p.Y;
        }

        public void inserir()
        {
            int pos_livre = 300;
            if (!magazine.min())
                pos_livre = 250;
            foreach (Piece p in pecas1)
            {
                if ((p.r.X <= 225) && (pos_livre > p.r.Y))
                    pos_livre = p.r.Y;
            }
            if (pos_livre >= 50)
            {
                if (pos_livre - 50 == 250)
                    peca_posicionada = true;
                pecas1.Add(new Piece(pos_livre - 50));
                recalc1 = true;
                atualiza(null);

            }
        }

        public void toggleHoldingRegister(int register, short value)
        {
            easyModbusTCPServer.holdingRegisters[register] ^= value;
            HoldingRegistersChanged(register, easyModbusTCPServer.holdingRegisters[register]);
        }

        #region events
        public delegate void ProcessChangedHandler(int inputs, int outputs, int altura);
        public event ProcessChangedHandler ProcessChanged1;
        public event ProcessChangedHandler ProcessChanged2;
        public delegate void ConnectionsChangedHandler(int numberConnections);
        public event ConnectionsChangedHandler ConnectionsChanged;
        public event ProcessChangedHandler StoreChanged;
        #endregion
    }


}