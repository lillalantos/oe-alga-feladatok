using System;

namespace OE.ALGA.Adatszerkezetek
{
    public class Kupac<T>
    {
        // minden szülőnek nagyobb prioritásúnak kell lennie , mint a gyerekeinek -> E[0] legnagyobb mindig


        protected T[] E;            // kupac elemei
        protected int n;            // kupac aktuális elemei

        protected Func<T, T, bool> nagyobbPrioritas;            // igaz ha első paraméter nagyobb piroritású, mint a második

        public Kupac(T[] e, int n, Func<T, T, bool> nagyobbPrioritas)
        {
            this.E = e;
            this.n = n;
            this.nagyobbPrioritas = nagyobbPrioritas;
            KupacotEpit();
        }

        public static int Bal(int i)
        {
            return 2 * i + 1;               // 0 alapú indexelés miatt
        }

        public static int Jobb(int i)
        {
            return 2 * i + 2;
        }

        public static int Szulo(int i)
        {
            return (i - 1) / 2;
        }


        // ha az i-edik kisebb prioritású, mint valamelyik gyereke, akkor cserél és rekurzívan folytat
        // feltétel: i alatti részfa már kupac, csak az i-edik csomópontnál lehet probléma
        public void Kupacol(int i)
        {
            int bal = Bal(i);
            int jobb = Jobb(i);
            int legnagyobb = i;

            if (bal < n && nagyobbPrioritas(E[bal], E[legnagyobb]))
                legnagyobb = bal;

            if (jobb < n && nagyobbPrioritas(E[jobb], E[legnagyobb]))
                legnagyobb = jobb;

            if (legnagyobb != i)                                            // ha talált olyat, aminél nem stimmel a prioritás, akkor tömbön belül helycsere
            {
                T seged = E[i];
                E[i] = E[legnagyobb];
                E[legnagyobb] = seged;

                Kupacol(legnagyobb);                    // rekurzív folytatás lefele
            }
        }

        // alulról felfele építi fel a kupacot ->  minden nem levél csomópontot kupacol
        public void KupacotEpit()
        {
            // n/2 -1 az utolsó nem levél csomópont indexe
            for (int i = (n / 2) - 1; i >= 0; i--)
            {
                Kupacol(i);
            }
        }
    }

    public class KupacRendezes<T> : Kupac<T> where T : IComparable<T>
    {
        public KupacRendezes(T[] A) : base(A, A.Length, (a, b) => a.CompareTo(b) > 0)           // összehasonlíthatóság a prioritás meghatározására
        {
        }

        public void Rendezes()
        {
            this.n = E.Length;
            KupacotEpit();

            // hátulról haladva a legnagyobb elemet a tömb végére rakja, majd újra kupacol 
            for (int i = E.Length - 1; i > 0; i--)
            {
                T seged = E[0];
                E[0] = E[i];
                E[i] = seged;
                n--;

                Kupacol(0); 
            }
        }
    }

    public class KupacPrioritasosSor<T> : Kupac<T>, PrioritasosSor<T> where T : IComparable
    {
        public KupacPrioritasosSor(int meret, Func<T, T, bool> nagyobbPrioritas)
            : base(new T[meret], 0, nagyobbPrioritas)
        { }

        public bool Ures => (n == 0);

        public T Elso()
        {
            if (n == 0) throw new NincsElemKivetel();
            return E[0]; 
        }


        // egy elem prioritása változott, frissíteni kell
        public void Frissit(T elem)
        {
            int i = 0; 
            while (i < n && E[i].CompareTo(elem) != 0)
            {
                i++;
            }
            if (i < n)                          // nem tudni nőtt vagy csökkent
            {
                KulcsotFelvisz(i);              // felfelé mozgatja, ha nőtt a prioritása
                Kupacol(i);                     // lefelé mozgatja, ha csökkent a prioritása
            }
            else
                throw new NincsElemKivetel();
        }


        // kupac végére tesszük, majd felfelé mozgatjuk, amíg a szülőnél nagyobb prioritású
        public void Sorba(T ertek)
        {
            if (n < E.Length) 
            {
                E[n] = ertek; 
                KulcsotFelvisz(n); 
                n++; 
            }
            else
                throw new NincsHelyKivetel();
        }

        public T Sorbol()
        {
            if (n == 0) throw new NincsElemKivetel();

            var seged = E[0];           // legnagyobb elem kimentése
            E[0] = E[n - 1];            // utolsó elem gyökérbe -> megmarad a fa váza
            n--;
            Kupacol(0);                 // újra kupacolás a gyökérnél, hogy a fa ismét kupac legyen
            return seged;
        }


        // elemet felfelé mozgatja, amíg a szülőnél nagyobb prioritású          - Kupacol párja, szülővel hasonlít, nem gyerekkel 
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