using System;

namespace OE.ALGA.Adatszerkezetek
{

    // balra nála kisebb, jobbra nála nagyobb elemek csak

    public class FaElem<T> 
    {
        public T tart;                  // adott elem tartalma -  plusz 2 mutató jobbra balra
        public FaElem<T> bal;               
        public FaElem<T> jobb;

        //public int darab = 1;

        public FaElem(T tartalom, FaElem<T> bal = null, FaElem<T> jobb = null)
        {
            tart = tartalom;
            this.bal = bal;
            this.jobb = jobb;
        }
       
    }

    public class FaHalmaz<T> : Halmaz<T> where T : IComparable<T>
    {
        private FaElem<T> gyoker;           // fa legteteje  - van jobb, bal és tartalma

        public FaHalmaz()
        {
            gyoker = null;                  // kezdetben üres fa
        }

        static FaElem<T> ReszfabaBeszur(FaElem<T> p, T ertek)
        {
            if (p == null)                                          // p az adott részfa
            {
                return new FaElem<T>(ertek);                        // ha elérünk egy üres helyre oda kell beszúrni az újat
            }
            else
            {
                if (p.tart.CompareTo(ertek) > 0)                    // ha új érték kisebb -> adott részfa bal oldali ágát vizsgáljuk tovább
                {
                    p.bal = ReszfabaBeszur(p.bal, ertek);
                }
                else if (p.tart.CompareTo(ertek) < 0)
                {
                    p.jobb = ReszfabaBeszur(p.jobb, ertek);
                }
                //else
                //{
                //    p.darab++;                                         
                //}
            }

            // == 0 nem kell mert akkor már szerepel benne az ertek, amit be akarunk szúrni 

            return p;           // módosított részfa
        }

        public void Beszur(T ertek)
        {
            gyoker = ReszfabaBeszur(gyoker, ertek);
        }



        static bool ReszfaEleme(FaElem<T> p, T ertek)
        {
            if (p != null)
            {
                if (p.tart.CompareTo(ertek) > 0)            // keresett érték kisebb -> balra megy
                {
                    return ReszfaEleme(p.bal, ertek);
                }
                else if (p.tart.CompareTo(ertek) < 0)
                {
                    return ReszfaEleme(p.jobb, ertek);
                }
                else
                {
                    return true;                            // megvan az érték 
                }
            }
            else
            {
                return false;                               // elértük a fa alját
            }
        }

        public bool Eleme(T ertek)
        {
            return ReszfaEleme(gyoker, ertek);
        }


        // p - törlendő elem
        // q - ami a helyére kerül
        // ha az elemnek már nincs jobb ága, akkor a bal oldali kerül az eredeti gyökér helyére amit törlünk
        static FaElem<T> KetGyerekesTorles(FaElem<T> p, FaElem<T> q)
        {
            if (q.jobb != null)
            {
                q.jobb = KetGyerekesTorles(p, q.jobb);                  // jobb oldali ág legnagyobb elemét megkeresi
                return q;
            }
            else
            {
                //p.darab--;
                //if (p.darab == 0)
                //{
                    p.tart = q.tart;                                // talált elem felmásolása gyökérbe
                    //p.darab = q.darab;
                    q = q.bal;                                      // talált helyettes elem törlése, úgy hogy annak bal ágával helyettesítjük
                //}                                     
                return q;
            }
        }


        static FaElem<T> ReszfabolTorol(FaElem<T> p, T ertek)
        {
            if (p != null)
            {
                if (p.tart.CompareTo(ertek) > 0)                    // ha törlendő érték kisebb, mint az aktuális fa csúcs -> balra megy
                {
                    p.bal = ReszfabolTorol(p.bal, ertek);
                }
                else if (p.tart.CompareTo(ertek) < 0)       
                {
                    p.jobb = ReszfabolTorol(p.jobb, ertek);
                }
                else                                               // ha megtaláltuk, amit törölni szeretnénk
                {
                    if (p.bal == null)          
                    {
                        //p.darab--;
                        //if (p.darab == 0)
                            p = p.jobb;                                // ha nincs neki bal eleme a törölt helyére csak a jobb kerülhet
                    }
                    else if (p.jobb == null)
                    {
                        //p.darab--;
                        //if (p.darab == 0)
                            p = p.bal;
                    }
                    else
                    {
                        p.bal = KetGyerekesTorles(p, p.bal);       // ha van bal és jobb eleme is a törlendőnek -> kétgyerekes törléssel megkeresi a legnagyobb balt, aminek nincs jobb ága felrakja azt a törlendő helyére és a maradék bal oldalt az alapján tolja fel a lyuk helyére
                    }
                }

                
                return p;               // frissített gyökérelem visszaadás
            }
            else
            {
                throw new NincsElemKivetel();
            }
        }

        public void Torol(T ertek)
        {
            gyoker = ReszfabolTorol(gyoker, ertek);
        }


        // gyökér -> bal -> jobb
        protected void ReszfaBejarasPreOrder(FaElem<T> p, Action<T> muvelet)
        {
            if (p != null)
            {
                muvelet(p.tart);
                ReszfaBejarasPreOrder(p.bal, muvelet);
                ReszfaBejarasPreOrder(p.jobb, muvelet);
            }
        }

        // bal -> gyökér -> jobb
        protected void ReszfaBejarasInOrder(FaElem<T> p, Action<T> muvelet)
        {
            if (p != null)
            {
                ReszfaBejarasInOrder(p.bal, muvelet);
                muvelet(p.tart);
                ReszfaBejarasInOrder(p.jobb, muvelet);
            }
        }

        // bal -> jobb -> gyökér
        protected void ReszfaBejarasPostOrder(FaElem<T> p, Action<T> muvelet)
        {
            if (p != null)
            {
                ReszfaBejarasPostOrder(p.bal, muvelet);
                ReszfaBejarasPostOrder(p.jobb, muvelet);
                muvelet(p.tart);
            }
        }

        public void Bejar(Action<T> muvelet)
        {
            ReszfaBejarasPreOrder(gyoker, muvelet);
        }


        //public int Darab (T ertek)
        //{
        //    if (ertek != null)
        //    {
        //        var p = gyoker;
        //        while (p.tart.CompareTo(ertek) != 0)
        //        {
        //            if (p.tart.CompareTo(ertek) > 0)
        //            {
        //                p = p.bal;
        //            }
        //            else
        //            {
        //                p = p.jobb;
        //            }
        //        }
        //        return p.darab;
        //    }
        //    else
        //    {
        //        throw new NincsElemKivetel();
        //    }
        //}
    }
}

