using System;
using System.Collections;
using System.Collections.Generic;

namespace OE.ALGA.Paradigmak
{


    public class FeltetelesFeladatTarolo<T> : FeladatTarolo<T>, IEnumerable where T : IVegrehajthato
    {

        public Func<T, bool> BejaroFeltetel { get; set; } = x => true;

        public FeltetelesFeladatTarolo(int méret) : base(méret)
        {
        }

        public void FeltetelesVegrehajtas(Func<T, bool> feltetel)           // csak azokat a feladatokat hajtja végre, amik megfelelnek a feltételnek
        {
            for (int i = 0; i < n; i++)
            {
                if (feltetel(tároló[i]))
                {
                    tároló[i].Vegrehajtas();
                }
            }
        }

        public IEnumerator GetEnumerator()
        {
            return new FeltetelesFeladatTaroloBejaro<T>(BejaroFeltetel, tároló, n);
        }

    }

    public class FeltetelesFeladatTaroloBejaro<T> : IEnumerator where T : IVegrehajthato
        {

            private Func<T, bool> feltetel;
            private int aktualis = -1;
            private T[] tároló;
            private int n;


            public FeltetelesFeladatTaroloBejaro(Func<T, bool> feltetel, T[] tároló, int n)
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
                while (aktualis<n && !feltetel(tároló[aktualis]))           // while mert ha egy nem felel meg a feltételnek, attól még van több elemünk amit vizsgálunk nem állhat le false-l
                {
                    aktualis++;
                }
                return aktualis < n;
            }
        }
    }

