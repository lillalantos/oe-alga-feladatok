using System;
using System.Reflection.Metadata.Ecma335;

namespace OE.ALGA.Optimalizalas
{
    
    public class HatizsakProblema
    {
        
        public int n { get; }       //hátizsákba pakolható tárgyak száma
        public int Wmax { get; }    //hátizsák mérete   - maximális súlya
        public int[] w { get; }     //tárgyak súlyai
        public float[] p { get;}    //tárgyak értékei


        public HatizsakProblema(int n, int wmax, int[] w, float[] p)
        {
            this.n = n;
            Wmax = wmax;
            this.w = w;
            this.p = p;
        }


        public int OsszSuly(bool[] pakolas)
        {
            int ossz = 0;
            for (int i = 0; i < n; i++)
            {
                if (pakolas[i])
                {
                    ossz += w[i];
                }
            }
            return ossz;
        }


        public float OsszErtek(bool[] pakolas)
        {
            float ertekek = 0;
            for (int i = 0; i < n; i++)
            {
                if (pakolas[i])
                {
                    ertekek += p[i];
                }
            }
            return ertekek;
        }


        public bool Ervenyes(bool[] pakolas)
        {
            return OsszSuly(pakolas) <= Wmax;
        }

    }


    // nem tud a hátizsákról, csak legenerálja az összes lehetséges megoldást és jóság alapján megmondja melyik optimális
    public class NyersEro<T>
    {
        public int m;                       // megoldások száma
        public Func<int, T> generator;      // lehetséges megoldások generálása
        public Func<T, float> josag;        // lehetséges megoldások értéke
        public int LepesSzam { get; private set; }

        public NyersEro(int m, Func<int, T> generator, Func<T, float> josag)
        {
            this.m = m;
            this.generator = generator;
            this.josag = josag;
        }

        public T OptimalisMegoldas()
        {
            var o = generator(1);
            for (int i = 2; i <= m; i++)
            {
                LepesSzam++;
                var x = generator(i);
                if( josag(x) > josag(o) )
                {
                    o = x;                      // ha van jobb, lecseréljük
                }
            }
            return o;
        }
    }


    public class NyersEroHatizsakPakolas
    {
        public int LepesSzam { get; private set; }        //végrehajtott összehasonlítások száma
        private HatizsakProblema problema;


        public NyersEroHatizsakPakolas(HatizsakProblema problema)
        {
            this.problema = problema;
        }


        public bool[] Generator(int i)
        {   
            int szam = i - 1;                       // mivel 1-m-ig vizsgáltuk a generátort kell a -1
            bool[] K = new bool[problema.n];        // hátizsákba pakolható tárgyak száma
            
            for (int j = 0; j < problema.n; j++)
            {
                int binaris = 1;                    // 2^j -n kiszámolása (ennyi lehetséges megoldás van - vagy belekerül vagy nem)
                for (int k=0; k < j; k++)
                {
                    binaris *= 2;
                }

                K[j] = (szam / binaris) % 2 == 1;           // adott elem kettes számrendszerben 1-es e (true)
            }

            return K;            
        }

        public float Josag(bool[] pakolas)
        {
            if (problema.Ervenyes(pakolas))                 // ha érvényes az adott pakolásunk, annak összértéke adja meg mennyire jó az adott megoldás 
            {
                return problema.OsszErtek(pakolas);
            }
            else
                return -1;                                  // ha érvénytelen -1 -> biztosan rosszabb lesz, mint az üres hátizsáké
        }

        public bool[] OptimalisMegoldas()
        {
            int lehetsegesMegolasokSzama = 1;               // 2 azon a hatványon, ahány elemet el akarunk rakni
            for (int k = 0; k < problema.n; k++)
            {
                lehetsegesMegolasokSzama *= 2;               
            }

            NyersEro<bool[]> lehetsegesPakolasok = new NyersEro<bool[]>(lehetsegesMegolasokSzama, Generator, Josag);        // saját Generator és Josag metófusok átadása

            bool[] legjobbMegoldas = lehetsegesPakolasok.OptimalisMegoldas();
            this.LepesSzam = lehetsegesPakolasok.LepesSzam;

            return legjobbMegoldas;
        }
        
        public float OptimalisErtek()
        {
            var ertek = OptimalisMegoldas();
            return problema.OsszErtek(ertek);
        }


    }



}
