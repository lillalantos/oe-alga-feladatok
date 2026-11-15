using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace OE.ALGA.Adatszerkezetek
{

    // T generikus osztály, ami tömbbel implementál verem műveleteket
    // Valósítsa meg a Verem<T> interfészt
    public class TombVerem<T> : Verem<T>
    {
        private T[] E;
        private int n; // a verem aktuális elemszáma
        public bool Ures { get; private set; }

        public TombVerem(int meret)
        {
            E = new T[meret];
            n = 0;
            Ures = true;
        }

        public void Verembe(T ertek)
        {
            if (n < E.Length)
            {
                E[n] = ertek;
                n++;
                Ures = false;
            }
            else
            {
                throw new NincsHelyKivetel();
            }
        }

        public T Verembol()
        {   
            if (n > 0)                  // ellenőrzés - van e még benne elem
            {
                T elem = E[n - 1];      // n mindig a következő szabadot reprezentálja -> n - 1 az aktuális utolsó
                n--;
                if(n==0)
                {
                    Ures = true;
                }
                return elem;
            }
            else
            {
                throw new NincsElemKivetel();
            }
        }

        public T Felso()
        {
            if (!Ures)
            {
                return E[n - 1];
            }
            else
            {
                throw new NincsElemKivetel();
            }
        }

    }


    // TömbSor T generikus osztály, , ami tömbbel sort implementál
    // Valósítsa meg a Sor<T> interfészt
    public class TombSor<T> : Sor<T>
    {
        private T[] E;
        private int e;              // első elem index -> ki vár legrégebben -> first out
        private int u;              // utoljára betett elem -> legkevesebb ideje vár -> last out
        private int n;              // a sor aktuális elemszáma
        public bool Ures { get; private set; }

        public TombSor(int meret)
        {
            E = new T[meret];
            e = 0;      
            u = -1;                 // legelső betételkor is növeléssel kezdünk -> akkor lesz 0
            n = 0;
            Ures = true;
        }

        public void Sorba(T ertek)
        {
            if (n < E.Length)
            {
                u = (u + 1) % E.Length;         // ha eléri a tömb végét visszaugrik az elejére (pl.: 5 hosszú tömbnél 4+1%5 = 0)
                E[u] = ertek;
                n++;
                Ures = false;
            }
            else
            {
                throw new NincsHelyKivetel();
            }
        }

        public T Sorbol()
        {
            if (!Ures)
            {
                T ertek = E[e];
                e = (e + 1) % E.Length;     // első elemnek is körkörösen kell haladnia, elérheti a tömb végét
                n--;
                if (n == 0)
                {
                    Ures = true;
                }
                return ertek;
            }
            else
            {
                throw new NincsElemKivetel();
            }
        }

        public T Elso()
        {
            if (!Ures)
            {
                return E[e];
            }
            else
            {
                throw new NincsElemKivetel();
            }
        }
    }

    // TömbLista T generikus osztály, ami tömbbel listát implementál
    // Valósítsa meg a Lista<T> interfészt
    public class TombLista<T> : Lista<T>, IEnumerable<T>
    {
        private T[] E;
        private int n;                              // a lista aktuális elemszáma
        public bool Ures { get; private set; }

        int Lista<T>.Elemszam => n;

        public TombLista(int meret = 1)
        {
            E = new T[meret];
            n = 0;
            Ures = true;    
        }


        public T Kiolvas(int index)
        {
            if (index >= 0 && index < n)
            {
                return E[index];
            }
            else
            {
                throw new HibasIndexKivetel();
            }
        }

        public void Modosit(int index, T ertek)
        {
            if (index >= 0 && index < n)
            {
                E[index] = ertek;
            }
            else
            {
                throw new HibasIndexKivetel();
            }
        }


        public void Beszur(int index, T ertek)
        {
            if(index < 0 || index > n)
            {
                throw new HibasIndexKivetel();
            }
            else
            {
                if (n == E.Length)
                {
                    T[] e = new T[E.Length * 2];            // ha tele van kétszer akkora méretű
                    for (int i = 0; i < n; i++)
                    {
                        e[i] = E[i];
                    }
                    E = e;                                  // referencia változtatása
                }
                for (int i = n - 1; i > index; i--)
                {
                    E[i] = E[i - 1];                        // előrébb toljuk a már ott lévő adatokat jobbra 
                }
                E[index] = ertek;                           // beszúrjuk a felszabadított helyre az értéket
                n++;
                Ures = false;
            }
            
        }
        
        public void Hozzafuz(T ertek)
        {
            Beszur(n, ertek);
        }


        public void Torol(T ertek)
        {
            int db = 0;                         // törölt darabok száma
            for (int i = 0; i < n; i++)
            {
                if (E[i].Equals(ertek))
                {
                    db++;
                }
                else
                {   
                    E[i - db] = E[i];           // a nem törlendő elem visszamásolása annyival balra ahányat előtte töröltünk
                }
            }
            n -= db;

            if (n == 0)
            {
                Ures = true;
            }
        }

        public void Bejar(Action<T> muvelet)
        {
            for (int i = 0; i < n; i++)
            {
                muvelet(E[i]);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new TombListaBejaro<T>(E, n);
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class TombListaBejaro<T> : IEnumerator<T>
    {
        T[] E;
        int n;
        int aktualisIndex = -1;

        public TombListaBejaro(T[] e, int n)
        {
            this.E = e;
            this.n = n;
        }

        public T Aktualis => E[aktualisIndex];
        public T Current => Aktualis;

        object IEnumerator.Current => Current;

        public bool Kovetkezo()
        {
            if (aktualisIndex < n - 1)
            {
                aktualisIndex++;
                return true;
            }
            return false;

        }

        public bool MoveNext() => Kovetkezo();

        public void Alaphelyzet()
        {
            aktualisIndex = -1;
            n = 0;
        }

        public void Reset() => Alaphelyzet();

        public void Dispose()
        { }
    }

}
