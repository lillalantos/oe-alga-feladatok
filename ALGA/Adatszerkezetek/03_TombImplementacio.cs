using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace OE.ALGA.Adatszerkezetek
{
    // 3. heti labor feladat - Tesztek: 03_TombImplementacioTesztek.cs

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
            if (!Ures)
            {
                return E[n];
                n--;
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
                return E[n];
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
        private int e; // első elem index 
        private int u;  // utoljára betett elem 
        private int n; // a sor aktuális elemszáma
        public bool Ures { get; private set; }

        public TombSor(int meret)
        {
            E = new T[meret];
            e = 0;
            u = -1;
            n = 0;
            Ures = true;
        }

        public void Sorba(T ertek)
        {
            if (n < E.Length)
            {
                u = (u + 1) % E.Length;
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
                e = (e + 1) % E.Length;
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
        private int n; // a lista aktuális elemszáma
        public bool Ures { get; private set; }

        public int Elemszam => throw new NotImplementedException();

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


        public void Hozzafuz(T ertek)
        {
            if (n < E.Length)
            {
                E[n] = ertek;
                n++;
                Ures = false;
            }
            else
            {
                T[] e = new T[E.Length * 2];
                foreach (T t in E)
                {
                    e[n] = t;
                    n++;
                }
                e[n] = ertek;
                E = e;
            }
        }


        public void Beszur(int index, T ertek)
        {
            if (index >= 0 && index <= n)
            {
                if (n == (E.Length - 1))
                {
                    T[] e = new T[E.Length * 2];
                    foreach (T t in E)
                    {
                        e[n] = t;
                        n++;
                    }
                    e[n] = ertek;
                    E = e;
                }
                n++;
                for (int i = n - 1; i > index; i--)
                {
                    E[i] = E[i - 1];
                }
                E[index] = ertek;
            }
            else
            {
                throw new HibasIndexKivetel();
            }
        }

        public void Torol(T ertek)
        {
            int db = 0;
            for (int i = 0; i < n; i++)
            {
                if (E[i].Equals(ertek))
                {
                    db++;
                }
                else
                {
                    E[i - db] = E[i];
                }
            }
            n -= db;
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
            throw new NotImplementedException();
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
        int aktualisIndex;
        public T Aktualis => E[aktualisIndex];

        public TombListaBejaro(T[] e, int n)
        {
            E = new T[n];
            this.n = n;
            aktualisIndex = -1;
        }


        public T Current => Aktualis;

        object IEnumerator.Current => Current;

        public bool Kovetkezo()
        {
            if (aktualisIndex < n)
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
        }

        public void Reset() => Alaphelyzet();

        public void Dispose()
        { }
    }

}
