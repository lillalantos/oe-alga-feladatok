using System;
using System.Collections;
using System.Collections.Generic;

namespace OE.ALGA.Paradigmak
{
    // 2. heti labor feladat - Tesztek: 02_FunkcionálisParadigmaTesztek.cs

    // FeltételesFeladatTároló T típusú (-- IVégrehajtható i. megvalósító) generikus osztály -- FeladatTároló leszármazottja, ami képes végrehajtható feladatokat tárolni és feltételesen végrehajtani
    public class FeltetelesFeladatTarolo<T> : FeladatTarolo<T>, IEnumerable where T : IVegrehajthato
    {
        //BejáróFeltétel -- T paraméterrel, logikai visszatéréssel
        public Func<T, bool> BejaroFeltetel { get; set; } = x => true;

        //FeltetesFeladatTárolóBejáró példányt visszaadó bejárhatóság interfész implementálás
        public IEnumerator GetEnumerator()
        {
            return new FeltetelesFeladatTaroloBejaro<T>(BejaroFeltetel, tároló, n);
        }


        // Konstruktor csak méret értéket kérje, továbbad ős konstruktorának
        public FeltetelesFeladatTarolo(int méret) : base(méret)
        {
        }

        // FeltételesVégrehajtás metódus  -- feltétel paraméterként T paraméterű függvényt vár -- visszatérés logikai
        public void FeltetelesVegrehajtas(Func<T, bool> feltetel)
        {

            // Függvény: MindentVégrehajt-hoz hasonlóan elemek vizsgálata
            for (int i = 0; i < n; i++)
            {
                // Minden elemnél hívja meg a feltétel paraméterként kapott függvényt -- átadva az aktuális elemet paraméterként
                if (feltetel(tároló[i]))
                {
                    // IGAZ - adott elem Végrehajtás
                    tároló[i].Vegrehajtas();
                }
                // HAMIS - kihagyás
            }
        }

        public class FeltetelesFeladatTaroloBejaro<T> : FeltetelesFeladatTarolo<T>, IEnumerator where T : IVegrehajthato
        {

            private Func<T, bool> feltetel;
            private int aktualis = -1;
            private T[] tároló;
            private int n;


            public FeltetelesFeladatTaroloBejaro(Func<T, bool> feltetel, T[] tároló, int n) : base(n)
            {
                this.feltetel = feltetel;
                this.tároló = tároló;
                this.n = n;
            }

            public void Reset()
            {
                aktualis = -1;
            }

            public T Current => tároló[aktualis];
            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }

            // Csak azok az értékek, amik megfelelnek a feltételnek
            public bool MoveNext()
            {
                aktualis++;
                while (aktualis<n && !feltetel(tároló[aktualis]))
                {
                    aktualis++;
                }
                return aktualis < n;
            }
        }
    }
}
