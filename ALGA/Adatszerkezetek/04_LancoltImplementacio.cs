using System;
using System.Collections;
using System.Collections.Generic;

namespace OE.ALGA.Adatszerkezetek
{
    // 4. heti labor feladat - Tesztek: 04_LancoltImplementacioTesztek.cs

    public class LancElem<T>
    {
        public T tart;
        public LancElem<T> kov;

        public LancElem(T tartalom, LancElem<T> kovetkezo = null)
        {
            tart = tartalom;
            kov = kovetkezo;
        }

    }

    public class LancoltVerem<T> : Verem<T>
    {

        private LancElem<T> fej;
        public bool Ures { get; private set; }

        public LancoltVerem()
        {
            fej = null;
            Ures = true;
        }

        public void Verembe(T ertek)
        {
            fej = new LancElem<T>(ertek, fej);
            Ures = false;
        }

        public T Verembol()
        {
            if (fej==null)
            {
                throw new NincsElemKivetel();
            }
            else
            {
                T ertek = fej.tart;
                fej = fej.kov;

                if (fej == null)
                    Ures = true;

                return ertek;
            }

        }

        public T Felso()
        {
            if (fej==null)
            {
                throw new NincsElemKivetel();
            }
            else
            {
                return fej.tart;
            }
        }


    }

    public class LancoltSor<T> : Sor<T>
    {

        private LancElem<T> fej;
        private LancElem<T> vege;
        public bool Ures { get; private set; }

        public LancoltSor()
        {
            fej = null;
            vege = null;
            Ures = true;
        }


        public T Elso()
        {
            if (fej != null)
            {
                return fej.tart;
            }
            throw new NincsElemKivetel();
        }

        public void Sorba(T ertek)
        {
            LancElem<T> ujElem = new LancElem<T>(ertek);
            if (vege != null)
            {
                vege.kov = ujElem;
            }
            else
                fej = ujElem;

            vege = ujElem;
            Ures = false;
        }

        public T Sorbol()
        {
            if (fej != null)
            {
                T ertek = fej.tart;
                fej = fej.kov;

                if (fej == null)
                {
                    vege = null;
                    Ures = true;
                }
                return ertek;
            }
            else
                throw new NincsElemKivetel();

        }
    }


    public class LancoltLista<T> : Lista<T>, IEnumerable<T>
    {
        public LancElem<T> fej;
        public int Elemszam { get; private set; }

        public LancoltLista()
        {
            fej = null;
            Elemszam = 0;
        }

        public void Hozzafuz(T ertek)
        {
            LancElem<T> ujElem = new LancElem<T>(ertek);
            Elemszam++;
            if (fej == null)
            {
                fej = ujElem;
            }
            else
            {
                LancElem<T> aktualis = fej;
                while (aktualis.kov != null)
                {
                    aktualis = aktualis.kov;
                }
                aktualis.kov = ujElem;
                aktualis.kov.tart = ertek;
            }
        }


        public T Kiolvas(int index)
        {
            LancElem<T> seged = fej;
            int i = 0;
            while (seged != null && i<index)
            {
                seged=seged.kov;
                i++;
            }
            if (seged != null)
            {
                return seged.tart;
            }
            else
            {
                throw new HibasIndexKivetel();
            }
        }


        public void Modosit(int index, T ertek)
        {
            LancElem<T> seged = fej;
            int i = 0;
            while ((seged != null) && i<index)
            {
                seged=seged.kov;
                i++;
            }
            if (seged != null)
            {
                seged.tart = ertek;
            }
            else
                throw new HibasIndexKivetel();
        }



        public void Beszur(int index, T ertek)
        {
            if (fej == null || index == 0)
            {
                LancElem<T> ujElem = new LancElem<T>(ertek);
                ujElem.kov = fej;
                fej = ujElem;
                Elemszam++;
            }
            else
            {
                LancElem<T> seged = fej;
                int i = 1;
                while (seged.kov != null && i < index)
                {
                    seged = seged.kov;
                    i++;
                }
                if (i == index)
                {
                    LancElem<T> ujElem = new LancElem<T>(ertek);
                    ujElem.kov = seged.kov;
                    seged.kov = ujElem;
                    Elemszam++;
                }
                else
                    throw new HibasIndexKivetel();
            }

        }

        public void Torol(T ertek)
        {
            LancElem<T> aktualis = fej;
            LancElem<T> elozo = null;
            while (aktualis != null)
            {
                if (aktualis.tart.Equals(ertek))
                {
                    Elemszam--;
                    if (elozo == null)
                    {
                        fej = aktualis.kov;
                        aktualis = fej;
                    }
                    else
                    {
                        elozo.kov = aktualis.kov;
                        aktualis = aktualis.kov;
                    }
                    
                }
                else
                {
                    elozo = aktualis;
                    aktualis = aktualis.kov;
                }
            }
        }

        public void Bejar(Action<T> muvelet)
        {
            LancElem<T> seged = fej;
            while (seged!=null)
            {
                muvelet(seged.tart);
                seged = seged.kov;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new LancoltListaBejaro<T>(fej);
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        
    }

    public class LancoltListaBejaro<T> : IEnumerator<T>
    {

        private LancElem<T> fej;
        private LancElem<T> aktualisElem;
        public T Aktualis => aktualisElem.tart;

        public T Current => Aktualis;
        object IEnumerator.Current => Current;

        public LancoltListaBejaro(LancElem<T> fej)
        {
            this.fej = fej;
            this.aktualisElem = null;
        }


        public bool MoveNext()
        {

            if (aktualisElem == null)
            {
                aktualisElem = fej;
            }
            else
            {
                aktualisElem = aktualisElem.kov;
            }

            return aktualisElem != null;
        }

        public void Reset()
        {
            aktualisElem = null;
        }

        public void Dispose()
        { }
    }



}