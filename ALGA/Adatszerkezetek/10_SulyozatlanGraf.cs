using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace OE.ALGA.Adatszerkezetek
{

    public class EgeszGrafEl : GrafEl<int>, IComparable<EgeszGrafEl>
    {
        public EgeszGrafEl(int honnan, int hova)
        {
            Honnan = honnan;
            Hova = hova;
        }

        public int Honnan { get; }
        public int Hova { get; }


        // előbb Honnan, egyezés esetén Hova alapján döntés 
        public virtual int CompareTo(EgeszGrafEl? obj)
        {
            if (obj != null && obj is EgeszGrafEl b)
            {
                if (Honnan != b.Honnan)
                {
                    return Honnan.CompareTo(b.Honnan);
                }
                return Hova.CompareTo(b.Hova);
            }
            else
                throw new InvalidOperationException();
        }
    }

    public class CsucsmatrixSulyozatlanEgeszGraf : SulyozatlanGraf<int, EgeszGrafEl>
    {
        private int n;                                      // csúcsok száma
        private bool[,] M;                                  // M[honnan, hova] = true, ha vezet köztük él

        public CsucsmatrixSulyozatlanEgeszGraf(int n)
        {
            this.n = n;
            M = new bool[n,n];

            for (int i = 0; i < n; i++)                     // explicit nullázása minden élnek alapértelmezetten 
            {
                for (int j = 0; j < n; j++)
                {
                    M[i,j] = false;
                }
            }
        }

        


        public int CsucsokSzama => n;
        public Halmaz<int> Csucsok 
        { 
            get
            {
                FaHalmaz<int> azonositok = new FaHalmaz<int>();
                for (int i = 0; i < n; i++)
                {
                    azonositok.Beszur(i);
                }
                return azonositok;
            } 
        }



        public int ElekSzama()
        {
            int elekSzama = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (M[i,j])                                 // ha igaz a M[honnan, hova]
                    {
                        elekSzama++;
                    }
                }
            }
            return elekSzama;
        }

        public Halmaz<EgeszGrafEl> Elek 
        {
            get
            {
                FaHalmaz<EgeszGrafEl> faHalmaz = new FaHalmaz<EgeszGrafEl>();
                for (int i = 0; i < n; i++)                                         // i sor -> Honnan
                {
                    for (int j = 0; j < n; j++)                                     // j oszlop -> Hova
                    {
                        if (M[i, j])
                        {
                            faHalmaz.Beszur( new EgeszGrafEl(i, j));
                        }
                    }
                }
                return faHalmaz;
            }
        }

        int Graf<int, EgeszGrafEl>.ElekSzama => ElekSzama();
        //{
        //    get
        //    {
        //        int elekSzama = 0;
        //        for (int i = 0; i < n; i++)
        //        {
        //            for (int j = 0; j < n; j++)
        //            {
        //                if (M[i, j])
        //                {
        //                    elekSzama++;
        //                }
        //            }
        //        }
        //        return elekSzama;
        //    }
        //}


        public Halmaz<int> Szomszedai(int csucs)
        {
            FaHalmaz<int> szomszedok = new FaHalmaz<int>();
            for (int i=0; i<n; i++)
            {
                if (M[csucs, i])
                {
                    szomszedok.Beszur(i);
                }
            }
            return szomszedok;
        }

        public void UjEl(int honnan, int hova)
        {
            M[honnan, hova] = true;
        }

        public bool VezetEl(int honnan, int hova)
        {
            return M[honnan, hova];
        }
    }

    public class GrafBejarasok
    {
        // először szomszédok, aztán azok szomszédjai stb.
        public static Halmaz<V> SzelessegiBejaras<V, E>(Graf<V, E> g, V start, Action<V> muvelet ) where V : IComparable<V>
        {
            LancoltSor<V> S = new LancoltSor<V>();
            S.Sorba(start);
            Halmaz<V> F = new FaHalmaz<V>();
            F.Beszur(start);


            while (!S.Ures)
            {
                V k = S.Sorbol();
                muvelet(k);

                g.Szomszedai(k).Bejar(i =>
                {
                    if (!F.Eleme(i))
                    {
                        S.Sorba(i);
                        F.Beszur(i);
                    }
                });
            }
            return F;
        }


        // előrször egy szomszéd összes szomszédja lefele és aztán többi oldal
        public static Halmaz<V> MelysegiBejaras<V, E>(Graf<V, E> g, V start, Action<V> muvelet) where V : IComparable<V>
        {
            Halmaz<V> F = new FaHalmaz<V>();
            MelysegBejarasRekurzio(g, start, F, muvelet);
            return F;
        }


        public static void MelysegBejarasRekurzio<V, E>(Graf<V, E> g, V k, Halmaz<V> F, Action<V> muvelet)
        {
            F.Beszur(k);
            muvelet(k);
            g.Szomszedai(k).Bejar(i =>
             {
                 if (!F.Eleme(i))
                 {
                     MelysegBejarasRekurzio(g, i, F, muvelet);
                 }
             });
        }
    }

}
