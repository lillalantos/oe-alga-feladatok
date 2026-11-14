using OE.ALGA.Paradigmak;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace OE.ALGA.Paradigmak
{
    public interface IVegrehajthato
    {
        public void Vegrehajtas();
    }

    public class TaroloMegteltKivetel : Exception
    {
        public string Message { get; set; } = "A tároló megtelt!";
    }

    public class FeladatTarolo<T> : IEnumerable<T> where T : IVegrehajthato             // a T típusra biztosan meghívható lesz a Vegrehajtas() metódus és bejárható
    {
        protected T[] tároló;
        protected int n = 0;            // a tárolóban lévő elemek száma

        public FeladatTarolo(int méret)
        {
            tároló = new T[méret];
        }


        
        public void Felvesz(T elem)
        {
            if (n < tároló.Length)              // ellenőrzés, hogy van e még hely a tárolóban
            {
                tároló[n] = elem;
                n++;
            }
            else
            {
                throw new TaroloMegteltKivetel();
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new FeladatTaroloBejaro<T>(tároló, n);       // tudnia kell mit járunk be és meddig lehet benne menni
        }


        IEnumerator IEnumerable.GetEnumerator()             // régi nyelvből megmaradt, újat hívja meg (fentebbi), ezért nem is public
        {
            return GetEnumerator();                         // foreach indításkor kér egy Enumeratort
        }

        public virtual void MindentVegrehajt()              // virtual !!! - megendegi származó osztályoknak hogy ezt felülírják
        {
            for (int i=0; i < n; i++)                       // minden jelenlegi feladatot végrehajt a tárolóban
            {
                tároló[i].Vegrehajtas();
            }
        }

    }

    public interface IFuggo
    {
        bool FuggosegTeljesul { get; }
    }


    public class FuggoFeladatTarolo<T> : FeladatTarolo<T> where T : IVegrehajthato, IFuggo          // T-nek a függőséget is meg kell tudnia vizsgálni mostmár
    {       

        public FuggoFeladatTarolo(int méret) : base(méret)
        {
        }

         override public void MindentVegrehajt()            // felülírás - csak akkor hajt végre egy taskot ha annak függőségei teljesültek, amúgy átugorja
         {
             for (int i=0; i < n;i++)
             {
                 if (tároló[i].FuggosegTeljesul)
                 {
                     tároló[i].Vegrehajtas();
                 }
             }
         }

    }


    public class FeladatTaroloBejaro<T> : IEnumerator<T> where T : IVegrehajthato
    {
        private T[] tarolo;
        private int n;
        private int aktualisIndex = -1;                     // mindig első index elöttről indul 

        public T Aktualis => tarolo[aktualisIndex];
        public T Current => Aktualis;
        object IEnumerator.Current => Current;             // régi verzió miatt

        public FeladatTaroloBejaro(T[] tarolo, int n)
        {
            this.tarolo = tarolo;
            this.n = n;
        }

        public void Alaphelyzet()
        {
            aktualisIndex = -1;
        }

        public bool Kovetkezo()
        {
            aktualisIndex++;
            if (aktualisIndex < n)          // ellenőrzés még aktuális elemeket járunk e be
            {
                return true;
            }
            return false;
        }

        public bool MoveNext() => Kovetkezo();

        public void Reset() => Alaphelyzet();

        public void Dispose() { }
    }


}



