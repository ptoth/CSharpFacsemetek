using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography;

namespace Facsemete
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /****************************************************************************
             * 1. 
             * Olvassa be a szóközökkel tagolt, UTF-8 kódolású rendeles.txt és a fajtak.txt állományokban lévő adatokat, 
             * és tárolja el egy olyan adatszerkezetben, ami a további feladatok megoldására alkalmas!
             */

            List<string[]> rendelesLista = new List<string[]>();
            Console.WriteLine("1. lépés: a rendelesek beolvasása. Kérem, üssön enter-t a folytatáshoz...");
            Console.ReadLine();

            // rendelések beolvasása
            string[] rendelesekBeolvasas = System.IO.File.ReadAllLines(@"C:\TEMP\rendeles.txt");
            foreach (string sor in rendelesekBeolvasas)
            {
                /* az aktuális sor splittelése a ";"-nél, tárolás []-ben
                 * rendelesAdatok[0] = Közterület;
                 * rendelesAdatok[1] = Házszám;
                 * rendelesAdatok[2] = Rendelési nap;
                 * rendelesAdatok[3] = Fafajta;
                 * rendelesAdatok[4] = Mennyiség (db)
                 */

                string[] rendelesAdatok = sor.Split(";");
                // a feltöltött lista hozzáfűzése a rendelés-listához
                rendelesLista.Add(rendelesAdatok);
            }
            // a fejléc sor eltávolítása
            rendelesLista.RemoveAt(0);

            Console.WriteLine("A rendelések beolvasva.");
            Console.WriteLine("2. lépés: a fajták beolvasása. Kérem, üssön enter-t a folytatáshoz...");
            Console.ReadLine();

            // fafajták beolvasása
            // Itt csak két sor van, és egy sorban van az adat, nem pedig soronként
            string[] faFajtakBeolvasas = System.IO.File.ReadAllLines(@"C:\TEMP\fajtak.txt");
            string[] faFajtaKodok = faFajtakBeolvasas[0].Split(";");
            string[] faFajtaNevek = faFajtakBeolvasas[1].Split(";");

            Console.WriteLine("A fajták beolvasva.");

            /*rendelések kiírása
            foreach (string[] aktualisRendeles in rendelesLista)
            {
                foreach (string adat in aktualisRendeles)
                {
                    Console.Write(adat);
                }
                Console.Write("\n");
            }

            // fafajták kiírása
            for (int i = 0; i < faFajtaKodok.Length; i++)
            {
                Console.WriteLine("Fa kód: " + faFajtaKodok[i]);
                Console.WriteLine("Fa neve: " + faFajtaNevek[i]);
            }
            */

            /****************************************************************************
             * 2.
             * Cserélje le a rendelés állományból beolvasott adatok fafajta oszlopában 
             * az „580”-at „mezei juhar”-ra és az „581”-et „korai juhar”- ra! 
             */
            foreach (string[] aktualisRendeles in rendelesLista)
            {
                // rendelesAdatok[3] = Fafajta;
                if (aktualisRendeles[3] == "580") { aktualisRendeles[3] = "mezei juhar"; }
                if (aktualisRendeles[3] == "581") { aktualisRendeles[3] = "korai juhar"; }    
            }

            /****************************************************************************
             * 3.
             * Rendezze a rendelési adatokat a közterület neve, házszáma és azon belül az igényelt fafajta neve szerint növekvően! 
             */

            /****************************************************************************
             * 4.
             * Kérje be a felhasználótól a facsemeték kiszállítási napját.Pl.: 2019.március 12 - i dátum! 
             * Határozza meg, és írja ki, hogy a rendelési időponthoz képest hány napot kellett várakozni a kiszállításra!
             */

            Console.WriteLine("Kérem adjon meg egy dátumot: ");
            DateTime datum = DateTime.Parse(Console.ReadLine());

            foreach (string[] aktualisRendeles in rendelesLista)
            {
                //rendelesAdatok[2] = Rendelési nap;
                int kulonbseg = (datum - DateTime.Parse(aktualisRendeles[2])).Days;
                Console.WriteLine("A rendelés óta eltelt napok: " + kulonbseg + " nap");
            }


            /****************************************************************************
             * 5.
             * Határozza meg, és írja ki a várakozási idők átlagát.Az eredmény két tizedesjegyre kerekítve jelenjen meg!
             */


            // Puskázzunk az előzőből!
            int osszesVarakozasiIdoNapban = 0;
            
            foreach (string[] aktualisRendeles in rendelesLista)
            {
                osszesVarakozasiIdoNapban += (datum - DateTime.Parse(aktualisRendeles[2])).Days;
            }

            double atlagosVarakozasiIdoNapban = osszesVarakozasiIdoNapban / rendelesLista.Count;
            Console.WriteLine("Az átlagos várakozási idő napban: " + atlagosVarakozasiIdoNapban.ToString("F2") + " nap");

            /****************************************************************************
             * 6.Írja ki a várakozási idők legnagyobb és legkisebb értékét!
             */

            // Használjuk megint a 4-est!

            string[] elsoElem = rendelesLista.ElementAt(0);            
            int maxVarakozas = 0;
            int minVarakozas = (datum - DateTime.Parse(elsoElem[2])).Days; // nem kezdhetsz nullával, mert akkor az lesz a legkisebb!
            
            foreach (string[] aktualisRendeles in rendelesLista)
            {
                if ((datum - DateTime.Parse(aktualisRendeles[2])).Days > maxVarakozas) { maxVarakozas = (datum - DateTime.Parse(aktualisRendeles[2])).Days; }
                if ((datum - DateTime.Parse(aktualisRendeles[2])).Days < minVarakozas) { minVarakozas = (datum - DateTime.Parse(aktualisRendeles[2])).Days; }
            }

            Console.WriteLine("A legnagyobb várakozási idő: " + maxVarakozas + "nap.");
            Console.WriteLine("A legkisebb várakozási idő: " + minVarakozas + "nap.");


            /****************************************************************************
             * 7. Írja ki, hogy az egyes facsemetefajtákat hány címről rendelték! 
             * 
             * 
             */



            /****************************************************************************
             * 8. Számolja meg, és írja ki, hogy az egyes facsemetefajtákból hány darabot rendeltek összesen! 
             */

            // létrehozunk egy, a fa fajtákkal egyező hosszúságú tömböt
            // ha egy fafajta egyezik, azon a helyiértéken 1-gyel növelünk
            int[] rendelesDarab = new int[faFajtaNevek.Length];

            foreach (string[] aktualisRendeles in rendelesLista)
            {
                for (int i = 0; i < faFajtaNevek.Length; i++)
                {
                    if (aktualisRendeles[3] == faFajtaNevek[i])
                    {
                        rendelesDarab[i] = rendelesDarab[i] +1;
                    }
                }
            }

            for (int i = 0; i < faFajtaNevek.Length; i++)
            {
                Console.WriteLine("A fa fajtája: " + faFajtaNevek[i]);
                Console.WriteLine("Rendelések darabszáma: " + rendelesDarab[i]);
            }
        }
    }
}