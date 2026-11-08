using System;

namespace OE.ALGA.Adatszerkezetek
{
    public class Kupac<T>
    {
        protected T[] E;            // kupac elemei
        protected int n;            // kupac aktuális elemei

        Func<T, T, bool> nagyobbPrioritas;      // igaz ha az első prio nagyobb mint a másodiké

        public Kupac(T[] e, int n, Func<T, T, bool> nagyobbPrioritas)
        {
            E = e;
            this.n = n;
            this.nagyobbPrioritas = nagyobbPrioritas;
            KupacotEpit(e);
        }

        public int Bal(int i)
        {
            return 2 * i;
        }

        public int Jobb(int i)
        {
            return 2 * i + 1;
        }

        public static int Szulo(int i)
        {
            return i / 2;
        }

        public void Kupacol(int i)
        {
            int t = 0;
            while (nagyobbPrioritas(E[t], E[i]))
            {
                t++;
            }
            E[t] = E[i];
        }

        public void KupacotEpit(T[] values)
        {   
            this.E = values;
            this.n = values.Length-1;

            for (int i = n/2; i>0; i--)
            {
                Kupacol(i);
            }
        }

    }
}