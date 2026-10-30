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
            for (int h = 0; h < problema.n + 1; h++)
            {
                for (int t = 0; t < problema.Wmax + 1; t++)
                {
                    if (h == 0 || t == 0)
                    {
                        F[h, t] = 0;
                    }
                    else if (problema.w[h - 1] > t)
                    {
                        F[h, t] = F[h - 1, t];
                        LepesSzam++;
                    }
                    else
                    {
                        F[h, t] = Math.Max(F[h - 1, t], F[h - 1, t - problema.w[h - 1]] + problema.p[h - 1]);
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
            int h = problema.n;
            int t = problema.Wmax;

            while (h > 0 && t > 0)
            {
                if (F[h, t] != F[h - 1, t])
                {
                    megoldas[h - 1] = true;
                    t -= problema.w[h - 1];
                }
                h--;
            }

            return megoldas;
        }

    }

}
