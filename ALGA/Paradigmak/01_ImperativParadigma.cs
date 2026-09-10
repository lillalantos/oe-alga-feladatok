using OE.ALGA.Paradigmak;
using System;
using System.Collections;
using System.Collections.Generic;

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
            throw new NotImplementedException();
        }

        public void MindentVegrehajt()
        {
            foreach (var elem in tároló)
            {
                if (elem != null)
                {
                    elem.Vegrehajtas();
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public interface IFuggo
    {
        public bool FuggosegTeljesul { get; }
    }


    public class FuggoFeladatTarolo<T> : FeladatTarolo<T> where T : IVegrehajthato, IFuggo
    {
        public FuggoFeladatTarolo(int méret) : base(méret)
        {
        }

        public void MindentVégrehajt()
        {
            foreach (var elem in tároló)
            {
                if (elem.FuggosegTeljesul)
                {
                    elem.Vegrehajtas();
                }
            }
        }

    }
}



public class FeladatTaroloBejaro<T> : IEnumerator<T> where T : IVegrehajthato
{
    private T[] tarolo;
    private int n;
    private int aktualisIndex;
    public T Aktualis => tarolo[aktualisIndex];

    public T Current => throw new NotImplementedException();

    object IEnumerator.Current => Current;

    public FeladatTaroloBejaro(T[] tarolo, int n)
    {
        this.tarolo = tarolo;
        this.n = n;
    }

    public bool MoveNext()
    {
        throw new NotImplementedException();
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}

