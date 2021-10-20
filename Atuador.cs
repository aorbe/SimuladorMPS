using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulador_MPS
{
    class Atuador
    {
        public double pos, angle;
        public System.Drawing.Point p;
        double MAX;
        const double MIN = 0;

        public Atuador(double max)
        {
            pos = 0;
            MAX = max;
            p = new System.Drawing.Point(0, 0);
        }

        protected virtual void calcPoint() { }
        public bool avanca()
        {
            if (pos < MAX)
            {
                pos++;
                if (pos > MAX)
                    pos = MAX;
                calcPoint();
                return true;
            }
            return false;

        }
        public bool recua()
        {
            if (pos > MIN)
            {
                pos--;
                if (pos < MIN)
                    pos = MIN;
                calcPoint();
                return true;
            }
            return false;

        }
        public bool control(bool on, bool off)
        {
            bool ret = false;
            if (on)
                ret |= avanca();
            if (off)
                ret |= recua();
            return ret;
        }
        public bool max() { return (MAX == pos); }
        public bool min() { return (MIN == pos); }

    }

    class AtuadorRotativo : Atuador
    {
        public double angle_A, angle_B, radius, X_offset, Y_offset;

        public AtuadorRotativo(double deviation, double factor_angle, double offset_angle, 
            double radius, double offset_x, double offset_y) : base(deviation) {
            angle_A = factor_angle;
            angle_B = offset_angle;
            this.radius = radius;
            
            X_offset = offset_x;
            Y_offset = offset_y;
            calcPoint();
        }

        protected override void calcPoint()
        {
            angle = pos * angle_A + angle_B;
            p.X = (int) (X_offset + radius *  Math.Cos(angle / 180.0 * Math.PI));
            p.Y = (int) (Y_offset + radius * Math.Sin(angle / 180.0 * Math.PI));
        }
    }

    class AtuadorLinear : Atuador
    {
        public double X_a, X_b, Y_a, Y_b;

        public AtuadorLinear(double deviation, double fator_x, double offset_x, double fator_y, double offset_y) : base(deviation) {
            X_a = fator_x;
            X_b = offset_x;
            Y_a = fator_y;
            Y_b = offset_y;
            calcPoint();
        }

        protected override void calcPoint()
        {
            p.X = (int)(X_a * pos + X_b);
            p.Y = (int)(Y_a * pos + Y_b);
        }
    }
}
