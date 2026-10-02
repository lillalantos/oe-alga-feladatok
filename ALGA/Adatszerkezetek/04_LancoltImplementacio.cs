//using System;
//using System.Collections;
//using System.Collections.Generic;

//namespace OE.ALGA.Adatszerkezetek
//{
//    // 4. heti labor feladat - Tesztek: 04_LancoltImplementacioTesztek.cs

//    public class LancElem<T>
//    {
//        public T tart;
//        public LancElem<T> kov;

//        public LancElem(T tartalom, LancElem<T> kovetkezo = null)
//        {
//            tart = tartalom;
//            kov = kovetkezo;
//        }
//    }

//    public class LancoltVerem<T> : Verem<T>
//    {

//            private LancElem<T> fej;
//            public bool Ures { get; private set; }

//            public LancoltVerem()
//            {
//                LancElem<T> fej = null;
//            }

//            public void Verembe(T ertek)
//            {
//                fej = new LancElem<T>(ertek, fej);
//            }

//            public T Verembol()
//            {
//                if (Ures)
//                {
//                    throw new NincsElemKivetel();
//                }
//                else
//                {
//                    fej.tart = fej.kov.tart;
//                    fej = fej.kov;
//                    return fej.tart;
//                }

//            }

//            public T Felso()
//            {
//                if (Ures)
//                {
//                    throw new NincsElemKivetel();
//                }
//                else
//                {
//                    return fej.tart;
//                }
//            }


//    }
   

//        public class LancoltSor<T> : Sor<T>
//        {

//            private LancElem<T> fej;
//            private LancElem<T> vege;
//            public bool Ures { get; private set; }

//            public LancoltSor()
//            {
//                fej = null;
//                vege = null;
//            }



//            public T Elso()
//            {
//                if (fej!=null)
//                {
//                    return fej.tart;
//                }
//                throw new NincsElemKivetel();
//            }

//            public void Sorba(T ertek)
//            {
//                LancElem<T> ujElem = new LancElem<T>(ertek);
//                if (vege != null)
//                {
//                    vege.kov = ujElem;
//                }
//                else 
//                    fej = ujElem;

//                vege = ujElem;
//                Ures = false;
//            }

//            public T Sorbol()
//            {
//                if (fej != null)
//                {
//                    fej.tart = fej.kov.tart;
//                    fej= fej.kov;
//                    if(fej == null)
//                    {
//                        vege = null;
//                        Ures = true;
//                    }
//                    return fej.tart;
//                }
//                else
//                    throw new NincsElemKivetel();

//            }
//        }


//        public class LancoltLista<T> : Lista<T>, IEnumerable<T>
//        {
//            public LancElem<T> fej;
//            public int Elemszam { get; private set; }

//            public LancoltLista()
//            {
//                fej = null;
//            }
//            public void Hozzafuz(T ertek)
//            {
//                LancElem<T> ujElem = new LancElem<T>(ertek);
//                if (fej == null)
//                {
//                    fej = ujElem;
//                }
//                else
//                {
//                    LancElem<T> aktualis = fej;
//                    while (aktualis.kov != null)
//                    {
//                        aktualis = aktualis.kov;
//                    }
//                    aktualis.kov = ujElem;
//                }
//            }
            

//            public T Kiolvas(int index)
//            {
//                throw new NotImplementedException();
//            }

//            public void Modosit(int index, T ertek)
//            {
//                throw new NotImplementedException();
//            }

            

//            public void Beszur(int index, T ertek)
//            {
//                throw new NotImplementedException();
//            }

//            public void Torol(T ertek)
//            {
//                LancElem<T> aktualis = fej;

//                while (aktualis != null && !aktualis.tart.Equals(ertek))
//                {
//                    aktualis = aktualis.kov;
//                }

//                if (aktualis.tart.Equals(ertek))
//                {
//                    fej = aktualis.kov;
//                }

//        }

//            public void Bejar(Action<T> muvelet)
//            {
//                throw new NotImplementedException();
//            }


//            IEnumerator IEnumerable.GetEnumerator()
//            {
//                return GetEnumerator();
//            }
//            public IEnumerator<T> GetEnumerator()
//            {
//                return new LancoltListaBejaro(fej);
//            }
//        }

//        public class LancoltListaBejaro<T> : IEnumerator<T>
//        {

//            private LancElem<T> fej;
//            private LancElem<T> aktualisElem;
//            public T Aktualis => aktualisElem.tart;

//            public T Current => Aktualis;
//            object IEnumerator.Current => Current;

//            public LancoltListaBejaro(LancElem<T> fej)
//            {
//                this.fej = fej;
//                this.aktualisElem = fej;
//            }


//            public bool MoveNext()
//            {
//                fej = fej.kov;
//                if (fej != null)
//                {
                    
//                    return true;
//                }
//                return false;
//            }

//            public void Reset()
//            {
//                fej = null;

//            }

//            public void Dispose()
//            { }
//        }

    

//}