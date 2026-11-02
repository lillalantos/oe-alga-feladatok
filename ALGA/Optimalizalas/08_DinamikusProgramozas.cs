using System;
using System.Diagnostics.Contracts;

namespace OE.ALGA.Optimalizalas
{

    public class DinamikusHatizsakPakolas
    {
        private HatizsakProblema problema;

        public DinamikusHatizsakPakolas(HatizsakProblema problema)
        {
            this.problema = problema;
        }

        public int LepesSzam { get; private set; }


        public float[,] TablazatFeltoltes()
        {
            float[,] F = new float[(problema.n + 1), problema.Wmax + 1];
            for (int t = 0; t < problema.n + 1; t++)
            {
                for (int h = 0; h < problema.Wmax + 1; h++)
                {
                    if (t == 0 || h == 0)
                    {
                        F[t, h] = 0;
                    }
                    else if (problema.w[t - 1] > h)
                    {
                        F[t, h] = F[t - 1, h];
                        LepesSzam++;
                    }
                    else
                    {
                        F[t, h] = Math.Max(F[t - 1, h], F[t - 1, h - problema.w[t - 1]] + problema.p[t - 1]);
                        LepesSzam++;
                    }
                    
                }
                
            }
            return F;
        }


        public float OptimalisErtek()
        {
            float[,] feltoltes = TablazatFeltoltes();
            return feltoltes[problema.n, problema.Wmax];
        }

        public bool[] OptimalisMegoldas()
        {
            float[,] F = TablazatFeltoltes();
            bool[] megoldas = new bool[problema.n];
            int t = problema.n;
            int h = problema.Wmax;

            while (t > 0 && h > 0)
            {
                if (F[t, h] != F[t - 1, h])
                {
                    megoldas[t - 1] = true;
                    h -= problema.w[t - 1];
                }
                t--;
            }

            return megoldas;
        }

    }

}
