using System;

namespace OE.ALGA.Adatszerkezetek
{

    public class SzotarElem<K, T>
    {
        public K kulcs;
        public T tart;

        public SzotarElem(K kulcs, T tart)
        {
            this.kulcs = kulcs;
            this.tart = tart;
        }
    }

    public class HasitoSzotarTulcsordulasiTerulettel<K, T> : Szotar<K, T>
    {
        SzotarElem<K, T>[] E;
        Func<K, int> h;                                                     // hasítófüggvény -> kulcsból indexet csinál kereséshez

        Lista<SzotarElem<K, T>> U = new LancoltLista<SzotarElem<K, T>>();   // tulcsordulasi terulet, SzotarElemek típusokkal benne


        // Konstruktorok
        // 2 paraméter: E mérete + hasítófüggvény
        public HasitoSzotarTulcsordulasiTerulettel(int meret, Func<K, int> hasitoFuggveny)
        {
            E = new SzotarElem<K, T>[meret];
            h = (x => Math.Abs(hasitoFuggveny(x)) % E.Length);              // GetHashCode() negatívat is visszaadhat, ami nem jó -> Math.Abs, % E.Length -> biztosítja hogy 0 én (n-1)en belül marad
        }

        // 1 paraméter: E mérete, hasítófüggvény GetHashCode() lesz
        public HasitoSzotarTulcsordulasiTerulettel(int meret) : this(meret, x => x.GetHashCode())
        {
        }


        // melyik fiókban van? fő (E) vagy túlcs. (U)
        private SzotarElem<K, T> KulcsKeres(K kulcs)
        {
            if (E[h(kulcs)] != null && E[h(kulcs)].kulcs.Equals(kulcs))
            {
                return E[h(kulcs)];                                             // megvan a fő listában, visszaadjuk az értéket
            }
            else
            {
                SzotarElem<K, T>? e = null;
                U.Bejar(x => { if (x.kulcs.Equals(kulcs))
                    {
                        e = x; }                                                // megvan a túlcsordulásiban, visszaadjuk az értéket (nem fontosak az indexek)
                    }
                );
                return e;                                                       // ha benne van, visszaadja, ha nem null marad
            }
        }
        public void Beir(K kulcs, T ertek)
        {
            SzotarElem<K, T> meglevo = KulcsKeres(kulcs);                       // létezik-e már a kulcs bárhol
            if (meglevo != null)
            {
                meglevo.tart = ertek;                                           // ha létezik csak értéket frissít, nem szúrja be megint
            }
            else
            {
                SzotarElem<K, T> uj = new SzotarElem<K, T>(kulcs, ertek);       // ha még nem, létrehozza az új dobozt neki
                if (E[h(kulcs)] == null)
                {
                    E[h(kulcs)] = uj;                                           // ha van szabad fiók -> eredetibe megy
                }
                else
                {
                    U.Hozzafuz(uj);                                             // foglalt -> túlcsordulásiba
                }       
            }
        }

        public T Kiolvas(K kulcs)
        {
            SzotarElem<K, T> meglevo = KulcsKeres(kulcs);
            if (meglevo != null)
            {
                return meglevo.tart;
            }
            else
            {
                throw new HibasKulcsKivetel();
            }
        }

        public void Torol(K kulcs)
        {
            if (E[h(kulcs)] != null && E[h(kulcs)].kulcs.Equals(kulcs))
            {
                E[h(kulcs)] = null;                                              // fiók kiürítés
            }
            else
            {
                SzotarElem<K, T> e = null;
                U.Bejar(x => 
                {
                    if (x.kulcs.Equals(kulcs)) 
                    {
                        e = x; 
                    } 
                }
                );

                if (e != null)
                {
                    U.Torol(e);                             // láncolt lista törlési metódus
                }
                else
                {
                    throw new HibasKulcsKivetel();
                }
            }
        }

        private void Felszabadit(SzotarElem<K, T> szotarElem)
        {

        }


    }
}
