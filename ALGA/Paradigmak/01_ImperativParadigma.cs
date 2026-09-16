using OE.ALGA.Paradigmak;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace OE.ALGA.Paradigmak
{
    public interface IVegrehajthato
    {
        public void Vegrehajtas();
    }

    public class TaroloMegteltKivetel : Exception
    {
        public string Message { get; set; } = "A tároló megtelt!";
    }

    public class FeladatTarolo<T> : IEnumerable<T> where T : IVegrehajthato
    {
        protected T[] tároló;
        protected int n = 0;

        public FeladatTarolo(int méret)
        {
            tároló = new T[méret];
        }

        public void Felvesz(T elem)
        {
            if (n < tároló.Length)
            {
                tároló[n] = elem;
                n++;
            }
            else
            {
                throw new TaroloMegteltKivetel();
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new FeladatTaroloBejaro<T>(tároló, n);
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public virtual void MindentVegrehajt()
        {
            for (int i=0; i < n; i++)
            {
                tároló[i].Vegrehajtas();
            }
        }

    }

    public interface IFuggo
    {
        bool FuggosegTeljesul { get; }
    }


    public class FuggoFeladatTarolo<T> : FeladatTarolo<T> where T : IVegrehajthato, IFuggo
    {

        public bool FuggosegTeljesul { get; set; }

        public FuggoFeladatTarolo(int méret) : base(méret)
        {
        }


            override public void MindentVegrehajt()
            {
                for (int i=0; i < n;i++)
                {
                    if (tároló[i].FuggosegTeljesul)
                    {
                        tároló[i].Vegrehajtas();
                    }
                }
            }
    }


    public class FeladatTaroloBejaro<T> : IEnumerator<T> where T : IVegrehajthato
    {
        private T[] tarolo;
        private int n;
        private int aktualisIndex=-1;
        public T Aktualis => tarolo[aktualisIndex];

        public T Current => Aktualis;

        object IEnumerator.Current => Current;

        public FeladatTaroloBejaro(T[] tarolo, int n)
        {
            this.tarolo = tarolo;
            this.n = n;
        }

        public void Alaphelyzet()
        {
            aktualisIndex = -1;
        }

        public bool Kovetkezo()
        {
            aktualisIndex++;
            if (aktualisIndex<n)
            {
                return true;
            }
            return false;
        }

        public bool MoveNext() => Kovetkezo();
        

        public void Reset() => Alaphelyzet();

        public void Dispose() { }
    }


}



