using System;

namespace OE.ALGA.Adatszerkezetek
{
    public class Kupac<T>
    {
        protected T[] E;            // kupac elemei
        protected int n;            // kupac aktuális elemei

        protected Func<T, T, bool> nagyobbPrioritas;

        public Kupac(T[] e, int n, Func<T, T, bool> nagyobbPrioritas)
        {
            this.E = e;
            this.n = n;
            this.nagyobbPrioritas = nagyobbPrioritas;
            KupacotEpit();
        }

        // TISZTA 0-S INDEXELÉS KÉPLETEI (STATIKUSAN)
        public static int Bal(int i)
        {
            return 2 * i + 1;
        }

        public static int Jobb(int i)
        {
            return 2 * i + 2;
        }

        public static int Szulo(int i)
        {
            return (i - 1) / 2;
        }

        public void Kupacol(int i)
        {
            int bal = Bal(i);
            int jobb = Jobb(i);
            int legnagyobb = i;

            if (bal < n && nagyobbPrioritas(E[bal], E[legnagyobb]))
                legnagyobb = bal;

            if (jobb < n && nagyobbPrioritas(E[jobb], E[legnagyobb]))
                legnagyobb = jobb;

            if (legnagyobb != i)
            {
                T seged = E[i];
                E[i] = E[legnagyobb];
                E[legnagyobb] = seged;

                Kupacol(legnagyobb);
            }
        }

        public void KupacotEpit()
        {
            // A 0-s indexelésnél innen indulunk visszafelé a 0. indexig
            for (int i = (n / 2) - 1; i >= 0; i--)
            {
                Kupacol(i);
            }
        }
    }

    public class KupacRendezes<T> : Kupac<T> where T : IComparable<T>
    {
        // A teljes tömb hossza az n, mivel minden indexet (0-tól) használunk!
        public KupacRendezes(T[] A) : base(A, A.Length, (a, b) => a.CompareTo(b) > 0)
        {
        }

        public void Rendezes()
        {
            this.n = E.Length;
            KupacotEpit();

            // Az utolsó elemtől az 1. elemig cserélünk a 0-s gyökérrel
            for (int i = E.Length - 1; i > 0; i--)
            {
                T seged = E[0];
                E[0] = E[i];
                E[i] = seged;
                n--;

                Kupacol(0); // Süllyesztés mindig a 0. gyökértől
            }
        }
    }

    public class KupacPrioritasosSor<T> : Kupac<T>, PrioritasosSor<T> where T : IComparable
    {
        // Nincs meret + 1 trükközés, pont annyi hely kell, amennyit kérnek
        public KupacPrioritasosSor(int meret, Func<T, T, bool> nagyobbPrioritas)
            : base(new T[meret], 0, nagyobbPrioritas)
        { }

        public bool Ures => (n == 0);

        public T Elso()
        {
            if (n == 0) throw new NincsElemKivetel();
            return E[0]; // Gyökér a 0. indexen
        }

        public void Frissit(T elem)
        {
            int i = 0; // 0-tól keressük
            while (i < n && E[i].CompareTo(elem) != 0)
            {
                i++;
            }
            if (i < n)
            {
                KulcsotFelvisz(i);
                Kupacol(i);
            }
            else
                throw new NincsElemKivetel();
        }

        public void Sorba(T ertek)
        {
            if (n < E.Length) // Pontosan a méretig mehet
            {
                E[n] = ertek; // Betesszük a legelső üres helyre
                KulcsotFelvisz(n); // Felhúzzuk
                n++; // Növeljük a darabszámot
            }
            else
                throw new NincsHelyKivetel();
        }

        public T Sorbol()
        {
            if (n == 0) throw new NincsElemKivetel();

            var seged = E[0];
            E[0] = E[n - 1]; 
            n--;
            Kupacol(0); 
            return seged;
        }

        public void KulcsotFelvisz(int i)
        {
            int sz = Szulo(i);
            
            if (i > 0 && nagyobbPrioritas(E[i], E[sz]))
            {
                T seged = E[i];
                E[i] = E[sz];
                E[sz] = seged;
                KulcsotFelvisz(sz);
            }
        }
    }
}