using System;
using System.Diagnostics.Contracts;

namespace OE.ALGA.Adatszerkezetek
{
    public class SzotarElem<K, T>
    {
        public K kulcs;
        public T tart;

        public SzotarElem(K kulcs, T tartalom)
        {
            this.kulcs = kulcs;
            this.tart = tartalom;
        }
    }

    public class HasitoSzotarTulcsordulasiTerulettel<K, T> : Szotar<K, T> where K : IComparable<K>
    {
        SzotarElem<K, T>[] E;
        private int h(K kulcs);
        LancoltLista<SzotarElem<K, T>>[] U;     // tulcsordulasi terulet, SzotarElemek típusokkal benne


        // Konstruktorok

        // 2 paraméter: E mérete + hasítófüggvény
        public HasitoSzotarTulcsordulasiTerulettel(int meret, Func<K, int> hasitoFuggveny)
        {
            E = new SzotarElem<K, T>[meret];
            h = x => hasitoFuggveny(x) % E.Length;
        }

        // 1 paraméter: E mérete, hasítófüggvény GetHashCode() lesz
        public HasitoSzotarTulcsordulasiTerulettel(int meret) : this(meret, x => x.GetHashCode())
        { }

        private SzotarElem<K, T> KulcsKeres(K kulcs)
        {
            if (E[h(kulcs)] != null && E[h(kulcs)] == kulcs)
            {
                return E[h(kulcs)];
            }
            else
            {
                SzotarElem<K, T> temp = null;
                if (U != null && U[h(kulcs)] != null)
                {
                    U.[U[h(kulcs)].Bejar(x => if(x.kulcs.Equals(kulcs))){temp=x};]
                }
                return temp;
            }


        } 
    }
