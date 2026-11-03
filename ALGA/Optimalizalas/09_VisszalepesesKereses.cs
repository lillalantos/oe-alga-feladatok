using System;
using System.Runtime.InteropServices.JavaScript;

namespace OE.ALGA.Optimalizalas
{
    public class VisszaLepesesOptimalizalo<T>
    {
        protected int n;                        //szintek száma
        protected int[] M;                      //szintek részmegoldásainak száma
        protected T[,] R;                       //lehetséges részmegoldások     -> R[szint, i] = szint i. részmegoldása
        protected Func<int, T, bool> ft;        //ft[szint, r] = igaz ha a szinten az r választható megoldás
        protected Func<int, T, T[], bool> fk;   //ft[szint, r, E] = igaz ha a szinten az r választható megoldás, ha előtte E-ben lévő részmegoldásokat választottuk
        protected Func<T[], float> josag;       //teljes megoldások értékelése

        public VisszaLepesesOptimalizalo(int n, int[] m, T[,] r, Func<int, T, bool> ft, Func<int, T, T[], bool> fk, Func<T[], float> josag)
        {
            this.n = n;
            M = m;
            R = r;
            this.ft = ft;
            this.fk = fk;
            this.josag = josag;
        }

        public int LepesSzam { get; private set; }


        protected void Backtrack(int szint, T[] E, bool van, T[] O)         
        {
            int i = 0;
            while (i < M[szint])
            {
                LepesSzam++;
                if ( ft(szint, R[szint, i]) && fk(szint, R[szint, i], E) )
                {
                    E[szint] = R[szint, i];
                    if (szint == n - 1)
                    {
                        if (josag(E) > josag(O))
                        {
                            for (int j=0; j<=szint; j++)
                            {
                                O[j] = E[j];
                            }
                            van = true;
                        }  
                    }
                    else
                    {
                        Backtrack(szint + 1, E, van, O);
                    }
                }
                i++;
            }

            
            
        }

        public T[] OptimalisMegoldas()
        {
            T[] E = new T[n];
            T[] O = new T[n];
            bool van = false;

            Backtrack(0, E, van, O);

            return O;
        }

    }



    public class VisszalepesesHatizsakPakolas
    {
        protected HatizsakProblema? problema;

        public VisszalepesesHatizsakPakolas(HatizsakProblema? problema)
        {
            this.problema = problema;
        }

        public int LepesSzam { get; private set; }

        public bool[] OptimalisMegoldas()
        {
            int[] M = new int[problema.n];
            bool[,] R = new bool[problema.n, 2];

            for (int i=0; i<problema.n; i++)
            {
                M[i] = 2;

                R[i, 0] = true;
                R[i, 1] = false;
            }

            Func<int, bool, bool> ft = (szint, r) =>
            {
                if (r && problema.w[szint] > problema.Wmax)
                {
                    return false;
                }
                return true;
            };

            Func<int, bool, bool[], bool> fk = (szint, r, E) =>
            {
                int eddigiSuly = 0;
                for (int i=0; i<szint; i++)
                {
                    if (E[i])
                        eddigiSuly += problema.w[i];
                }

                if (r)
                    eddigiSuly += problema.w[szint];

                return eddigiSuly <= problema.Wmax;

            };

            Func<bool[], float> josag = (E) => problema.OsszErtek(E);

            VisszaLepesesOptimalizalo<bool> visszaLepesesOptimalizalo = new VisszaLepesesOptimalizalo<bool>(problema.n, M, R, ft, fk, josag);

            bool[] optimalis = visszaLepesesOptimalizalo.OptimalisMegoldas();

            LepesSzam = visszaLepesesOptimalizalo.LepesSzam;
            return optimalis;
            
        }


        public float OptimalisErtek()
        {
            return problema.OsszErtek(OptimalisMegoldas());
        }

    }




}
