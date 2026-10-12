//using System;
//using System.Diagnostics.Contracts;

//namespace OE.ALGA.Adatszerkezetek
//{
//    public class SzotarElem<K, T>
//    {
//        public K kulcs;
//        public T tart;

//        public SzotarElem(K kulcs, T tartalom)
//        {
//            this.kulcs = kulcs;
//            this.tart = tartalom;
//        }
//    }

//    public class HasitoSzotarTulcsordulasiTerulettel<K, T> : Szotar<K, T> where K : IComparable<K>
//    {
//        SzotarElem<K, T>[] E;
//        private Func<K, int> h;
//        LancoltLista<SzotarElem<K, T>>[] U;     // tulcsordulasi terulet, SzotarElemek típusokkal benne


//        // Konstruktorok

//        // 2 paraméter: E mérete + hasítófüggvény
//        public HasitoSzotarTulcsordulasiTerulettel(int meret, Func<K, int> hasitoFuggveny)
//        {
//            E = new SzotarElem<K, T>[meret];
//            h = x => hasitoFuggveny(x) % E.Length;
//        }

//        // 1 paraméter: E mérete, hasítófüggvény GetHashCode() lesz
//        public HasitoSzotarTulcsordulasiTerulettel(int meret) : this(meret, x => x.GetHashCode())
//        { }

//        private SzotarElem<K, T> Muvelet(SzotarElem<K, T> elem)
//        {
//            SzotarElem<K, T> temp = null;
//            if (elem != null)
//            {
//                if (U[h[kulcs]].Equals())
//            }
//        }

//        private SzotarElem<K, T> KulcsKeres(K kulcs)
//        {
//            if (E[h(kulcs)] != null && E[h(kulcs)].Equals(kulcs))
//            {
//                return E[h(kulcs)];
//            }
//            else
//            {
//                SzotarElem<K, T> temp = null;
//                if (U != null && U[h(kulcs)] != null)
//                {
//                    U[h(kulcs)].Bejar(x => x.kulcs.Equals(kulcs));
//                }
//                return temp;
//            }


//        }

//        public void Beir(K kulcs, T ertek)
//        {
//            throw new NotImplementedException();
//        }

//        public T Kiolvas(K kulcs)
//        {
//            throw new NotImplementedException();
//        }

//        public void Torol(K kulcs)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
