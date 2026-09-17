using System;
using System.Collections;
using System.Collections.Generic;

namespace OE.ALGA.Paradigmak
{
    // 2. heti labor feladat - Tesztek: 02_FunkcionálisParadigmaTesztek.cs

    // FeltételesFeladatTároló T típusú (-- IVégrehajtható i. megvalósító) generikus osztály -- FeladatTároló leszármazottja, ami képes végrehajtható feladatokat tárolni és feltételesen végrehajtani
    public class FeltetelesFeladatTarolo<T> : FeladatTarolo<T> where T : IVegrehajthato
    {

        public Func<T, bool> BejaroFeltetel { get; set; }

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



    }
}
