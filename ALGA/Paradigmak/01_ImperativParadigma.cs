using System;
using System.Collections;
using System.Collections.Generic;

namespace OE.ALGA.Paradigmak
{
    public interface IVégrehajtható
    {
        public void Végrehajtás();
    }

    public class TárolóMegteltKivétel : Exception
    {
        public string Message { get; set; } = "A tároló megtelt!";
    }

    public class FeladatTároló<T> where T : IVégrehajtható
    {
        protected T[] tároló;
        protected int n = 0;

        public FeladatTároló(int méret)
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
                throw new TárolóMegteltKivétel();
            }
        }

        public void MindentVégrehajt()
        {
            foreach (var elem in tároló)
            {
                if (elem != null)
                {
                    elem.Végrehajtás();
                }
            }
        }

    }

    internal interface IFüggő
    {
        public bool FüggőségTeljesül { get; }
    }


}

