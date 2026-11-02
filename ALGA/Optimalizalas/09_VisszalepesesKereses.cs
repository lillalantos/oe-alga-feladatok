using System;

namespace OE.ALGA.Optimalizalas
{
    public class VisszaLepesesOptimalizalo<T>
    {
        protected int n;                        //szintek száma
        protected int[] M;                      //szintek részmegoldásainak száma
        protected T[,] R;                       //lehetséges részmegoldások     -> R[szint, i] = szint i. részmegoldása
        protected Func<int, T, bool> ft;        //ft[szint, r] = igaz ha a szinten az r választható megoldás
        protected Func<int, T, T[], bool> fk;   //ft[szint, r, E] = igaz ha a szinten az r választható megoldás, ha előtte E-ben lévő részmegoldásokat választottuk
        protected Func<T[], int> josag;         //teljes megoldások értékelése

        public VisszaLepesesOptimalizalo(int n, int[] m, T[,] r, Func<int, T, bool> ft, Func<int, T, T[], bool> fk, Func<T[], int> josag)
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
            while (!van && i < M[szint])
            {    
                if ( ft(szint, R[szint, i]) )
                {
                    E[szint] = R[szint, i];
                    if (szint == n-1)
                    {
                        O = E;
                        van = true;
                    }
                    else
                    {
                        Backtrack(szint + 1, E, van, O);
                    }
                }
                LepesSzam++;
                i++;
            }

            O = E;
            
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
}
