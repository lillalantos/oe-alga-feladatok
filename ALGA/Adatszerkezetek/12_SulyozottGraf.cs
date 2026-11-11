using System;
using System.Diagnostics.Tracing;
using System.Reflection.Metadata.Ecma335;

namespace OE.ALGA.Adatszerkezetek
{
    public class SulyozottEgeszGrafEl : EgeszGrafEl, SulyozottGrafEl<int>
    {

        public float Suly { get; private set; }
        public SulyozottEgeszGrafEl(int honnan, int hova, float suly) : base(honnan, hova)
        {
            this.Suly = suly;
        }
    }


    public class CsucsmatrixSulyozottEgeszGraf : SulyozottGraf<int, SulyozottEgeszGrafEl>
    {

        private int n;
        private float[,] M;

        public CsucsmatrixSulyozottEgeszGraf(int n)
        {
            this.n = n;
            M = new float[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    M[i, j] = float.NaN;
                }
            }
        }

        public int CsucsokSzama => n;

        public int ElekSzama()
        {
            int elek = 0;
            foreach (int i in M)
            {
                foreach (int j in M)
                {
                    if (!float.IsNaN(M[i, j]))
                    {
                        elek++;
                    }
                }
            }
            return elek;
        }
        int Graf<int, SulyozottEgeszGrafEl>.ElekSzama => ElekSzama();

        public Halmaz<int> Csucsok()
        {
            FaHalmaz<int> csucsok = new FaHalmaz<int>();
            for (int i = 0; i < n; i++)
            {
                csucsok.Beszur(i);
            }
            return csucsok;
        }
        Halmaz<int> Graf<int, SulyozottEgeszGrafEl>.Csucsok => Csucsok();

        public Halmaz<SulyozottEgeszGrafEl> Elek()
        {
            FaHalmaz<SulyozottEgeszGrafEl> elek = new FaHalmaz<SulyozottEgeszGrafEl>();
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (!float.IsNaN(M[i, j]))
                    {
                        elek.Beszur(new SulyozottEgeszGrafEl(i, j, M[i, j]));
                    }
                }
            }
            return elek;
        }
        Halmaz<SulyozottEgeszGrafEl> Graf<int, SulyozottEgeszGrafEl>.Elek => Elek();


        public float Suly(int honnan, int hova)
        {
            if (!float.IsNaN(M[honnan, hova]))
            {
                return M[honnan, hova];
            }
            else
                throw new NincsElKivetel();
        }

        public Halmaz<int> Szomszedai(int csucs)
        {
            FaHalmaz<int> szomszedok = new FaHalmaz<int>();
            for (int j = 0; j < n; j++)
            {
                if (!float.IsNaN(M[csucs, j]))
                {
                    szomszedok.Beszur(j);
                }
            }
            return szomszedok;
        }

        public void UjEl(int honnan, int hova, float suly)
        {
            M[honnan, hova] = suly;
        }

        
        public bool VezetEl(int honnan, int hova)
        {
            return !float.IsNaN(M[honnan, hova]);
        }
    }

    public class Utkereses
    {
        public static Szotar<V, float> Dijkstra<V, E>(SulyozottGraf<V, E> g, V start) where V : IComparable
        {
            Szotar<V, float> L = new HasitoSzotarTulcsordulasiTerulettel<V, float>(g.CsucsokSzama);
            Szotar<V, V> P = new HasitoSzotarTulcsordulasiTerulettel<V, V>(g.CsucsokSzama);
            PrioritasosSor<V> S = new KupacPrioritasosSor<V>(g.CsucsokSzama, (a, b) => L.Kiolvas(a) < L.Kiolvas(b));

            g.Csucsok.Bejar((x) =>
            {
                L.Beir(x, float.PositiveInfinity);
                P.Beir(x, default);
                S.Sorba(x);
            });

            L.Beir(start, 0);
            while (!S.Ures)
            {
                V u = S.Sorbol();
                g.Szomszedai(u).Bejar((v) =>
                {
                    if (L.Kiolvas(u) + g.Suly(u, v) < L.Kiolvas(v))
                    {
                        L.Beir(v, L.Kiolvas(u) + g.Suly(u, v));
                        P.Beir(v, u);
                        S.Frissit(v);
                    }
                });
            }
            return L;
        }
    }

    public class FeszitofaKereses
    {
        public static Szotar<V, V> Prim<V, E>(SulyozottGraf<V, E> g, V start) where V : IComparable, IComparable<V>
        {

            Szotar<V, float> K = new HasitoSzotarTulcsordulasiTerulettel<V, float>(g.CsucsokSzama);                     // adott csúcs milyen súllyal érhető el
            Szotar<V, V> P = new HasitoSzotarTulcsordulasiTerulettel<V, V>(g.CsucsokSzama);                             // melyik csúcson át köthető be a feszítőfába
            PrioritasosSor<V> S = new KupacPrioritasosSor<V>(g.CsucsokSzama, (a, b) => K.Kiolvas(a) < K.Kiolvas(b));
            Halmaz<V> pillanatnyiElemek = new FaHalmaz<V>();

            g.Csucsok.Bejar((x) =>
            {
                K.Beir(x, float.PositiveInfinity);      
                P.Beir(x, default);                     
                S.Sorba(x);
                pillanatnyiElemek.Beszur(x);
            });


            K.Beir(start, 0);
            S.Frissit(start);
            while (!S.Ures)
            {
                V u = S.Sorbol();
                pillanatnyiElemek.Torol(u);
                g.Szomszedai(u).Bejar((x) =>
                {
                    if (pillanatnyiElemek.Eleme(x) && g.Suly(u,x) < K.Kiolvas(x))
                    {
                        K.Beir(x, g.Suly(u, x));
                        P.Beir(x, u);
                        S.Frissit(x);
                    }
                });
            }

            return P;

        }


        public static Halmaz<E> Kruskal<V, E>(SulyozottGraf<V, E> g, V start) where E : SulyozottGrafEl<V>, IComparable<E>, IComparable
        {
            Halmaz<E> A = new FaHalmaz<E>();
            Szotar<V, V> halmazok = new HasitoSzotarTulcsordulasiTerulettel<V, V>(g.CsucsokSzama);
            PrioritasosSor<E> S = new KupacPrioritasosSor<E>(g.ElekSzama, (u, v) => u.Suly < v.Suly);

            void HalmazLetrehozas(V x)
            {
                halmazok.Beir(x, x);
            }

            V TartalmazoHalmaz(V u)
            {
                V szulo = halmazok.Kiolvas(u);
                if (szulo.Equals(u))
                {
                    return u;
                }

                V gyoker = TartalmazoHalmaz(szulo);
                halmazok.Beir(u, gyoker);
                return gyoker;
            }

            void HalmazOsszevonas(V a,V b)
            {
                var halmazAzonositoA = TartalmazoHalmaz(a);
                var halmazAzonositoB = TartalmazoHalmaz(b);

                if (!halmazAzonositoA.Equals(halmazAzonositoB))
                {
                    halmazok.Beir(halmazAzonositoA, halmazAzonositoB);
                }
            }


            g.Csucsok.Bejar( x =>
            {
                HalmazLetrehozas(x);
            });

            g.Elek.Bejar(e =>
            {
                S.Sorba(e);
            });

            while (!S.Ures)
            {
                E e = S.Sorbol();
                if ( !TartalmazoHalmaz(e.Honnan).Equals( TartalmazoHalmaz(e.Hova) ) )
                {
                    A.Beszur(e);
                    HalmazOsszevonas(TartalmazoHalmaz(e.Honnan), TartalmazoHalmaz(e.Hova));
                }

            }
            return A;
        }
    }

}
