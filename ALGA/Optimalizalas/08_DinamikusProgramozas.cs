using System;
using System.Diagnostics.Contracts;

namespace OE.ALGA.Optimalizalas
{

    // részproblémákra bontás -> semmit nem számol ki kétszer -> 2^n-en hihetetlen sok kombináció, ezt gyorsítja

    public class DinamikusHatizsakPakolas
    {
        private HatizsakProblema problema;      // n, Wmax, w (súly), p(érték)

        public DinamikusHatizsakPakolas(HatizsakProblema problema)
        {
            this.problema = problema;
        }

        public int LepesSzam { get; private set; }


        // t - hányadik tárgyig jutottunk 
        // h - mekkora az addig vizsgált részhátizsák mérete
        // ezek metszete egy érték (pl. Ft), ami meg azt adja meg hogy pl ha belerakjuk akkor az eddigi 4kg-os zsákba már csak 3 kg fér DE az értéke mostmár 10 helyett 05 Ft
        public float[,] TablazatFeltoltes()
        {
            LepesSzam = 0;          // OptimalisErtek + OptimalisMegoldas egymás utáni hívásakor duplázódna enélkül

            float[,] F = new float[(problema.n + 1), problema.Wmax + 1];            // 0...n tárgy, 0...Wmax kapacitás
            for (int t = 0; t < problema.n + 1; t++)
            {
                for (int h = 0; h < problema.Wmax + 1; h++)
                {
                    if (t == 0 || h == 0)
                    {
                        F[t, h] = 0;                                // alapeset -> 0 tárgy vagy 0 kapacitással mindig 0 az összérték
                    }
                    else if (problema.w[t - 1] > h)                 // jelenlegi tárgy nehezebb mint a max kapacitás -> előző tárgy súlya kerül bele
                    {
                        F[t, h] = F[t - 1, h];
                        LepesSzam++;
                    }
                    else                                                // melyik éri meg jobban? -> nem teszem bele, beleteszem -> bal és jobb oldala a Max-nak
                    {
                        F[t, h] = Math.Max(F[t - 1, h], F[t - 1, h - problema.w[t - 1]] + problema.p[t - 1]);       // adott tárgy súlyával csökkentett max súlyú oszlop és adott érték sorába lévő eddigi értékhez hozzáadjuk mostanit 
                        LepesSzam++;
                    }
                    
                }
                
            }
            return F;
        }


        public float OptimalisErtek()
        {
            float[,] feltoltes = TablazatFeltoltes();
            return feltoltes[problema.n, problema.Wmax];            // végigmenve a táblázaton a legalsó érték lesz a legjobb érték
        }


        public bool[] OptimalisMegoldas()
        {
            float[,] F = TablazatFeltoltes();
            bool[] megoldas = new bool[problema.n];             // feltöltjük az optimális lépésekkel
            int t = problema.n;                                 // adott probléma elemszáma
            int h = problema.Wmax;                              // adott probléma max súly kapacitása

            while (t > 0 && h > 0)                              // jobb lentről haladunk vissza
            {
                if (F[t, h] != F[t - 1, h])                     // ha nem egyezik a felette lévő elemmel, akkor pakolás történt, tehát feljegyezhetjük mint lépés és csökkenthetjük az oszlopszámot (kapacitásból levonjuk a már betett súlyt)
                {
                    megoldas[t - 1] = true;
                    h -= problema.w[t - 1];
                }
                t--;                                            // sorszám csökkentés
            }

            return megoldas;
        }

    }

}
