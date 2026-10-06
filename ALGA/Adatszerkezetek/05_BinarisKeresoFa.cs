using System;

namespace OE.ALGA.Adatszerkezetek
{
    public class FaElem<T> : IComparable<FaElem<T>>
    {
        public T tart;
        public FaElem<T> bal;
        public FaElem<T> jobb;

        public FaElem(T tartalom, FaElem<T> bal = null, FaElem<T> jobb = null)
        {
            tart = tartalom;
            this.bal = bal;
            this.jobb = jobb;
        }

        public int CompareTo(FaElem<T>? other)
        {
            return ((IComparable<FaElem<T>>)bal).CompareTo(other);
        }
    }

    public class FaHalmaz<T> : Halmaz<T> where T : IComparable<T>
    {
        private FaElem<T> gyoker;

        public FaHalmaz()
        {
            gyoker = null;
        }



        static FaElem<T> ReszfabaBeszur(FaElem<T> p, T ertek)
        {
            if (p == null)
            {
                FaElem<T> uj = new FaElem<T>(ertek);
                return uj;
            }
            else
            {
                if (p.tart.CompareTo(ertek) > 0)
                {
                    p.bal = ReszfabaBeszur(p.bal, ertek);
                }
                else if (p.tart.CompareTo(ertek) < 0)
                {
                    p.jobb = ReszfabaBeszur(p.jobb, ertek);
                }
            }
            return p;
        }

        public void Beszur(T ertek)
        {
            gyoker = ReszfabaBeszur(gyoker, ertek);
        }

        static bool ReszfaEleme(FaElem<T> p, T ertek)
        {
            if (p != null)
            {
                if (p.tart.CompareTo(ertek) > 0)
                {
                    return ReszfaEleme(p.bal, ertek);
                }
                else if (p.tart.CompareTo(ertek) < 0)
                {
                    return ReszfaEleme(p.jobb, ertek);
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        public bool Eleme(T ertek)
        {
            return ReszfaEleme(gyoker, ertek);
        }

        static FaElem<T> KetGyerekesTorles(FaElem<T> p, FaElem<T> q)
        {
            if (q.jobb != null)
            {
                q.jobb = KetGyerekesTorles(p, q.jobb);
                return q;
            }
            else
            {
                p.tart = q.tart;
                FaElem<T> tmp = q;
                q = q.bal;
                return q;
            }
        }



        static FaElem<T> ReszfabolTorol(FaElem<T> p, T ertek)
        {
            if (p != null)
            {
                if (p.tart.CompareTo(ertek) > 0)
                {
                    p.bal = ReszfabolTorol(p.bal, ertek);
                }
                else if (p.tart.CompareTo(ertek) < 0)
                {
                    p.jobb = ReszfabolTorol(p.jobb, ertek);
                }
                else
                {
                    if (p.bal == null)
                    {
                        FaElem<T> tmp = p.jobb;
                        p = tmp;
                    }
                    else if (p.jobb == null)
                    {
                        FaElem<T> tmp = p.bal;
                        p = tmp;
                    }
                    else
                    {
                        p.bal = KetGyerekesTorles(p, p.bal);
                    }
                }

                return p;
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

        public void Bejar(Action<T> muvelet)
        {
            
        }
    }
}

