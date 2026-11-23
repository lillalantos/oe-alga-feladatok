using System;
using System.Runtime.InteropServices.JavaScript;

namespace OE.ALGA.Optimalizalas
{
    public class VisszaLepesesOptimalizalo<T>
    {
        protected int n;                        //szintek száma
        protected int[] M;                      //szintek részmegoldásainak száma
        protected T[,] R;                       //lehetséges részmegoldások     -> R[szint, i] = szint i. részmegoldása
        protected Func<int, T, bool> ft;        //ft[szint, r] = igaz ha a szinten az r választható megoldás
        protected Func<int, T, T[], bool> fk;   //ft[szint, r, E] = igaz ha a szinten az r választható megoldás, ha előtte E-ben lévő részmegoldásokat választottuk
        protected Func<T[], float> josag;       //teljes megoldások értékelése

        public VisszaLepesesOptimalizalo(int n, int[] m, T[,] r, Func<int, T, bool> ft, Func<int, T, T[], bool> fk, Func<T[], float> josag)
        {
            this.n = n;
            M = m;
            R = r;
            this.ft = ft;
            this.fk = fk;
            this.josag = josag;
        }

        public int LepesSzam { get; protected set; }


        protected virtual void Backtrack(int szint, T[] E, ref bool van, T[] O)         
        {
            int i = 0;
            while (i < M[szint])                        // adott szinten a részmegoldások bejárása
            {
                LepesSzam++;
                if ( ft(szint, R[szint, i]) && fk(szint, R[szint, i], E) )          // ha adott szinten a részmegoldás lehetséges és előtte választottal sem zárják ki egymást
                {
                    E[szint] = R[szint, i];             // rögzítjük a részmegoldást
                    if (szint == n - 1)                 // legmélyebb szint -> teljes megoldás elérése
                    {
                        if (josag(E) > josag(O))        // ha ez jobb megoldás, mint az eddigi, állítsa át arra
                        {
                            for (int j=0; j<=szint; j++)
                            {
                                //Array.Copy(E, O, n);
                                O[j] = E[j];
                            }
                            van = true;
                        }  
                    }
                    else
                    {
                        Backtrack(szint + 1, E, ref van, O);
                    }
                }
                i++;
            }

            
            
        }

        public T[] OptimalisMegoldas()
        {
            T[] E = new T[n];       // épp épülő megoldás
            T[] O = new T[n];       // eddigi legjobb megoldás
            bool van = false;

            Backtrack(0, E, ref van, O);

            return O;
        }

    }



    public class VisszalepesesHatizsakPakolas
    {
        protected HatizsakProblema? problema;

        public VisszalepesesHatizsakPakolas(HatizsakProblema? problema)
        {
            this.problema = problema;
        }

        public int LepesSzam { get; protected set; }

        public virtual bool[] OptimalisMegoldas()
        {
            int[] M = new int[problema.n];
            bool[,] R = new bool[problema.n, 2];

            for (int i=0; i<problema.n; i++)
            {
                M[i] = 2;                                   // vagy bekerül vagy nem 

                R[i, 0] = true;
                R[i, 1] = false;
            }

            Func<int, bool, bool> ft = (szint, r) =>                    // ha a tárgy súlya nagyobb mint a kapacitás false
            {
                if (r && problema.w[szint] > problema.Wmax)
                {
                    return false;
                }
                return true;
            };

            Func<int, bool, bool[], bool> fk = (szint, r, E) =>         // érvényes, ha eddigi szinteken betett súly + mosatni nem haladja meg a kapacitást
            {
                int eddigiSuly = 0;
                for (int i=0; i < szint; i++)
                {
                    if (E[i])
                        eddigiSuly += problema.w[i];
                }

                if (r)
                    eddigiSuly += problema.w[szint];

                return eddigiSuly <= problema.Wmax;

            };

            Func<bool[], float> josag = (E) => problema.OsszErtek(E);               // akkor jobb egyik megoldás másiknál, ha az összértéke nagyobb

            VisszaLepesesOptimalizalo<bool> visszaLepesesOptimalizalo = new VisszaLepesesOptimalizalo<bool>(problema.n, M, R, ft, fk, josag);

            bool[] optimalis = visszaLepesesOptimalizalo.OptimalisMegoldas();

            LepesSzam = visszaLepesesOptimalizalo.LepesSzam;
            return optimalis;
            
        }


        public float OptimalisErtek()
        {
            return problema.OsszErtek(OptimalisMegoldas());
        }

    }



    public class SzetvalasztasEsKorlatozasOptimalizacio<T> : VisszaLepesesOptimalizalo<T>
    {
        protected Func<int, T[], float> fb;     //felső becslés a hátralevő feladatok optimális megoldására
        
        public SzetvalasztasEsKorlatozasOptimalizacio(int n, int[] m, T[,] r, Func<int, T, bool> ft, Func<int, T, T[], bool> fk, Func<T[], float> josag, Func<int, T[], float> fb) : base(n, m, r, ft, fk, josag)
        {
            this.fb = fb;
        }


        protected override void Backtrack(int szint, T[] E, ref bool van, T[] O)
        {
            int i = 0;
            while (i < M[szint] )
            {
                LepesSzam++;
                if (ft(szint, R[szint, i]) && fk(szint, R[szint, i], E) )           // adott szinten lehetséges részmegoldás és nem ütközik előzővel sem
                {
                    E[szint] = R[szint, i];
                    if (szint == n-1)
                    {
                        if (!van || josag(E) > josag(O))                        // ha még nincs teljes megoldásunk vagy az eddigi legjobb megoldásnál jobb a mostani
                        {
                            Array.Copy(E, O, n);
                            van = true;
                        }
                    }
                    else if((josag(E) + fb(szint, E) > josag(O)))              // ha még nincs teljes megoldás VAGY eddigi értékek és a hátralevő szintek becsült maximuma meghaladja eddigi legjobbat -> különben nincs értelme vizsgálni 
                    {
                        Backtrack(szint + 1, E, ref van, O);
                    }
                }
                i++;
            }
            
        }


    }

    public class SzetvalasztasEsKorlatozasHatizsakPakolas : VisszalepesesHatizsakPakolas
    {
        
        public SzetvalasztasEsKorlatozasHatizsakPakolas(HatizsakProblema? problema) : base(problema)
        {
        }

        public override bool[] OptimalisMegoldas()
        {
            int[] M = new int[problema.n];
            bool[,] R = new bool[problema.n, 2];

            for (int i = 0; i < problema.n; i++)
            {
                M[i] = 2;

                R[i, 0] = true;
                R[i, 1] = false;
            }

            Func<int, bool, bool> ft = (szint, r) =>
            {
                if (r && problema.w[szint] > problema.Wmax)
                {
                    return false;
                }
                return true;
            };

            Func<int, bool, bool[], bool> fk = (szint, r, E) =>
            {
                int eddigiSuly = 0;
                for (int i = 0; i < szint; i++)
                {
                    if (E[i])
                        eddigiSuly += problema.w[i];
                }

                if (r)
                    eddigiSuly += problema.w[szint];

                return eddigiSuly <= problema.Wmax;

            };

            Func<bool[], float> josag = (E) =>
            {
                return problema.OsszErtek(E);
            };


            // felső becslés
            Func<int, bool[], float> fb = (szint, E) =>
            {
                float becsultErtek = 0;
                for (int i = szint; i < problema.n; i++)
                {
                    if (problema.OsszSuly(E) + problema.w[i] <= problema.Wmax)
                    becsultErtek += problema.p[i];
                }
                return becsultErtek ;
            };

            SzetvalasztasEsKorlatozasOptimalizacio<bool> szetvalasztasOptimalizalo = new SzetvalasztasEsKorlatozasOptimalizacio<bool>(problema.n, M, R, ft, fk, josag, fb);
            
            bool[] optimalis = szetvalasztasOptimalizalo.OptimalisMegoldas();

            LepesSzam = szetvalasztasOptimalizalo.LepesSzam;
            return optimalis;

        }

    }
}
