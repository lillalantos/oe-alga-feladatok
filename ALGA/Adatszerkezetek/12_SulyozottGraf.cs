using System;
using System.Diagnostics.Tracing;
using System.Reflection.Metadata.Ecma335;

namespace OE.ALGA.Adatszerkezetek
{
    public class SulyozottEgeszGrafEl : EgeszGrafEl, SulyozottGrafEl<int>, IComparable
    {

        public float Suly { get; private set; }
        public SulyozottEgeszGrafEl(int honnan, int hova, float suly) : base(honnan, hova)
        {
            this.Suly = suly;
        }


        // súly nem számít, megint csak honnan, hova alapján hasonlít
        public int CompareTo(object? obj)
        {
            if (obj != null && obj is SulyozottEgeszGrafEl b)
            {
                return base.CompareTo(b);
            }
            else
                throw new NincsElKivetel();
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
                    M[i, j] = float.NaN;                // nem lehet 0 mert az egy érvényes súlynak számítana -> a NaN jelzi, hogy nincs él
                }
            }
        }

        public int CsucsokSzama => n;

        // minden nem NaN érték
        public int ElekSzama
        {
            get
            {
                int elek = 0;
                for (int i = 0; i < n; ++i)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (!float.IsNaN(M[i, j]))
                        {
                            elek++;
                        }
                    }
                }
                return elek;
            }
        }

        // 0...n-1 csúcsok
        public Halmaz<int> Csucsok
        {
            get
            {
                FaHalmaz<int> csucsok = new FaHalmaz<int>();
                for (int i = 0; i < n; i++)
                {
                    csucsok.Beszur(i);
                }
                return csucsok;
            }
        }


        // Mátrix minden nem NaN eleméből egyedülálló SulyozottEgeszGrafEl objektumot készít, és egy halmazba teszi őket
        public Halmaz<SulyozottEgeszGrafEl> Elek
        {
            get
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
        }


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
                if (!float.IsNaN(M[csucs, j]))                  // csucs sorához tartozó minden nem NaN oszlopindexe
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
        public static Szotar<V, float> Dijkstra<V, E>(SulyozottGraf<V, E> g, V start) where V : IComparable, IComparable<V>
        {
            Szotar<V, float> L = new HasitoSzotarTulcsordulasiTerulettel<V, float>(g.CsucsokSzama);                 // starttól, adott csúcsig eddig ismert legrövidebb út hossza, ha nincs elérhető út, akkor +∞
            Szotar<V, V> P = new HasitoSzotarTulcsordulasiTerulettel<V, V>(g.CsucsokSzama);                         // P[v] = u, ha a v csúcsot az u csúcson keresztül érjük el a legrövidebb úton -> legutóbbi előd
            PrioritasosSor<V> S = new KupacPrioritasosSor<V>(g.CsucsokSzama, (a, b) => L.Kiolvas(a) < L.Kiolvas(b));        // akinek kisebb a becsült távolsága -> nagyobb prioritású - L-ből olvas, ha az változik kell a Frissít

            g.Csucsok.Bejar((x) =>
            {
                L.Beir(x, float.PositiveInfinity);          // kezdetben minden csúcs elérhetetlen - végtelen
                P.Beir(x, default);                         // kezdetben nincs előd
                S.Sorba(x);                                 // minden csúcs bekerül a prioritási sorba
            });

            L.Beir(start, 0);           // start 0 távolságra van önmagától
            S.Frissit(start);           // mivel csökkent a végtelenhez képest a távolság, ezért frissíteni kell a prioritási sorban -> sor elejére kerül 
            while (!S.Ures)
            {
                V u = S.Sorbol();                                               // sorból kövi elem kiolvasása
                g.Szomszedai(u).Bejar((v) =>
                {
                    if ( L.Kiolvas(u) + g.Suly(u, v) < L.Kiolvas(v))            // ha a v csúcsot az u csúcson keresztül érjük el rövidebb úton, mint eddig ismert legrövidebb út
                    {
                        L.Beir(v, L.Kiolvas(u) + g.Suly(u, v));                 // frissítjük a legrövidebb út hosszát
                        P.Beir(v, u);                                           // frissítjük a v csúcs elődjét
                        S.Frissit(v);                                           // frissítjük a prioritási sorban a v csúcsot, mivel csökkent a becsült távolsága
                    }
                });
            }
            return L;
        }
    }


    public class FeszitofaKereses
    {

        // minden lépésnél hozzáadjuk a feszítőfához a legkisebb súlyú élt, ami egy új csúcsot köt be a feszítőfába
        public static Szotar<V, V> Prim<V, E>(SulyozottGraf<V, E> g, V start) where V : IComparable, IComparable<V>
        {

            Szotar<V, float> K = new HasitoSzotarTulcsordulasiTerulettel<V, float>(g.CsucsokSzama);                     // adott csúcs milyen súllyal érhető el (K[v] : milyen súlyú éllel érem el elgolcsóbban a v-t)
            Szotar<V, V> P = new HasitoSzotarTulcsordulasiTerulettel<V, V>(g.CsucsokSzama);                             // melyik csúcson át köthető be a feszítőfába
            PrioritasosSor<V> S = new KupacPrioritasosSor<V>(g.CsucsokSzama, (a, b) => K.Kiolvas(a) < K.Kiolvas(b));    // minél kisebb a súly, annál előrébb kerül a prioritási sorban, ha változik a súly -> Frissít
            Halmaz<V> pillanatnyiElemek = new FaHalmaz<V>();                                                            // még nem a fában lévő csúcsok halmaza

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
                V u = S.Sorbol();                           // a prioritási sorból kiolvasott csúcsot a feszítőfához adjuk
                pillanatnyiElemek.Torol(u);                 // kivesszük a halmazból a feszítőfához hozzáadott csúcsot, hogy ne legyen újra feldolgozva
                g.Szomszedai(u).Bejar((x) =>
                {
                    if (pillanatnyiElemek.Eleme(x) && g.Suly(u,x) < K.Kiolvas(x))       // még nincs a feszítőfában és kisebb súlyú éllel érhető el, mint eddig ismert legkisebb súlyú él
                    {
                        K.Beir(x, g.Suly(u, x));                    // frissítjük a csúcs súlyát
                        P.Beir(x, u);                               // frissítjük a csúcs elődjét
                        S.Frissit(x);                               // frissítjük a prioritási sorban a csúcsot, mivel csökkent a becsült súlya
                    }
                });
            }

            return P;       // maga a feszítőfa nem kerül visszaadásra, csak a csúcsok elődjei, amiből a feszítőfa rekonstruálható

        }



        // élek súly szerint növekvő sorrendbe állítása, majd a legkisebb súlyú élt hozzáadjuk a feszítőfához, ha nem keletkezik kör
        public static Halmaz<E> Kruskal<V, E>(SulyozottGraf<V, E> g) where E : SulyozottGrafEl<V>, IComparable<E>, IComparable
        {
            Halmaz<E> A = new FaHalmaz<E>();                                                            // feszítőfa élei -> végeredmény
            Szotar<V, V> halmazok = new HasitoSzotarTulcsordulasiTerulettel<V, V>(g.CsucsokSzama);      // minden csúcs egy halmazhoz tartozik, ha két csúcs ugyanahhoz a halmazhoz tartozik, akkor a feszítőfában összekötötték őket
            PrioritasosSor<E> S = new KupacPrioritasosSor<E>(g.ElekSzama, (u, v) => u.Suly < v.Suly);   // élek súly szerint növekvő sorrendbe állítása

            void HalmazLetrehozas(V x)
            {
                halmazok.Beir(x, x);            // minden csúcs kezdetben a saját halmazához tartozik
            }

            V TartalmazoHalmaz(V u)
            {
                V szulo = halmazok.Kiolvas(u);                      
                if (szulo.Equals(u))                        // alapeset -> egyezik a csúcs a saját halmazával, tehát ő a halmaz gyökere
                {
                    return u;
                }

                V gyoker = TartalmazoHalmaz(szulo);         // addig hívja magát, amíg meg nem találja az egyezést önmagával
                halmazok.Beir(u, gyoker);                   // útvonal tömörítés -> "aa  bc  ca" esetén ha a b-t keressük "ba" kerül beírásra
                return gyoker;
            }

            void HalmazOsszevonas(V a,V b)
            {
                var halmazAzonositoA = TartalmazoHalmaz(a);
                var halmazAzonositoB = TartalmazoHalmaz(b);

                if (!halmazAzonositoA.Equals(halmazAzonositoB))             // egyik gyökerét másik gyökeréhez rendeljük, így a két halmaz egyesül
                {
                    halmazok.Beir(halmazAzonositoA, halmazAzonositoB);
                }
            }


            g.Csucsok.Bejar( x =>
            {
                HalmazLetrehozas(x);    // minden csúcs kezdetben a saját halmazához tartozik
            });

            g.Elek.Bejar(e =>
            {
                S.Sorba(e);             // minden él a sorba -> rendezve
            });

            while (!S.Ures)
            {
                E e = S.Sorbol();                                                               // legkisebb él kiolvasása
                if ( !TartalmazoHalmaz(e.Honnan).Equals( TartalmazoHalmaz(e.Hova) ))           // ha az adott él eleje és végpontja nem egy halmazban szerepel 
                {
                    A.Beszur(e);                                                                // él hozzáadása a feszítőfához
                    HalmazOsszevonas(TartalmazoHalmaz(e.Honnan), TartalmazoHalmaz(e.Hova));     // a két csúcs halmazának egyesítése -> ne kerüljön újra feldolgozásra
                }

            }
            return A;
        }
    }

}
