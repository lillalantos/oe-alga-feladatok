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
        private int n;
        private bool[,] M;

        public CsucsmatrixSulyozatlanEgeszGraf(int n)
        {
            this.n = n;
            M = new bool[n,n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    M[i,j] = false;
                }
            }
        }

        public int CsucsokSzama => n;

        public int ElekSzama()
        {
            int elekSzama = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (M[i,j])
                    {
                        elekSzama++;
                    }
                }
            }
            return elekSzama;
        }

        public Halmaz<int> Csucsok { get
            {
                FaHalmaz<int> azonositok = new FaHalmaz<int>();
                for (int i = 0;i < n;i++)
                {
                    azonositok.Beszur(i);
                }
                return azonositok;
            } 
        }

        public Halmaz<EgeszGrafEl> Elek { get
            {
                FaHalmaz<EgeszGrafEl> faHalmaz = new FaHalmaz<EgeszGrafEl>();
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
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

        int Graf<int, EgeszGrafEl>.ElekSzama
        {
            get
            {
                int elekSzama = 0;
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (M[i, j])
                        {
                            elekSzama++;
                        }
                    }
                }
                return elekSzama;
            }
        }

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
        public static Halmaz<V> SzelessegiBejaras<V, E>(Graf<V, E> g, V start, Action<V> muvelet ) where V : IComparable<V>
        {
            V S = start;
            Sor<V> Sor = new LancoltSor<V>();
            Halmaz<V> F = new FaHalmaz<V>();
            Sor.Sorba(start);
            F.Beszur(start);

            while (!Sor.Ures)
            {
                V k = Sor.Sorbol();
                muvelet(k);
                
                for (V i = start; g.Szomszedai(k).Eleme(i); )
                {
                    if (!F.Eleme(i))
                    {
                        S = i;
                        F.Beszur(i);
                    }

                }

            }
            return F;
        }

        public static Halmaz<V> MelysegiBejaras<V, E>(Graf<V, E> g, V start, Action<V> muvelet) where V : IComparable<V>
        {
            Halmaz<V> F = null;
            MelysegBejarasRekurzio(g, start, F, muvelet);
            return F;
        }


        public static void MelysegBejarasRekurzio<V, E>(Graf<V, E> g, V k, Halmaz<V> F, Action<V> muvelet)
        {
            F.Beszur(k);
            muvelet(k);
            for (V i = k; g.Szomszedai(k).Eleme(i);)
            {
                if (!F.Eleme(i))
                {
                    MelysegBejarasRekurzio(g, k, F, muvelet);
                }
            }
        }
    }

}
