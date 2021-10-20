using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulador_MPS
{
    class Piece
    {
        public Rectangle r;
        public Color cor;
        public short tamanho;
        public int tipo;
        public enum LOCAL { MAGAZINE, EMPURRADOR, ROTATIVO, ELEVADOR, EXPULSAO, ESTEIRA, REJEICAO, REMOVER,
        MESA, MESA_ROTATIVO, RAMPA, SEP_ENTRADA, SEP_PRESA, SER_ARMAZENADA};
        public LOCAL local;
        public string message;
        private static Random rand = new Random();
        private int mean = 5000;
        private int stdDev = 500;
        public double tmpValue;

        public Piece(int pos = 0)
        {
            double u1 = 1.0 - rand.NextDouble(); //uniform(0,1] random doubles
            double u2 = 1.0 - rand.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
            tamanho = (short) (mean + stdDev * randStdNormal);
            tipo = (rand.Next(999) % 3) + 1;
            switch (tipo)
            {
                case 1:
                    cor = Color.Red;
                    break;
                case 2:
                    cor = Color.Black;
                    break;
                case 3:
                    cor = Color.Silver;
                    break;
            }
            r = new Rectangle(175, pos, 85, 50);
            local = LOCAL.MAGAZINE;
#if DEBUG
            Console.Write("Adicionada Peça Tipo {0} com tamanho {1}", tipo, tamanho);
#endif
        }
    }
}
