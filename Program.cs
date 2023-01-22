using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography;
using System.IO;
using System.Collections.Immutable;

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
            string[] rendelesekBeolvasas = File.ReadAllLines("rendeles.txt");
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
            string[] faFajtakBeolvasas = File.ReadAllLines("fajtak.txt");
            string[] faFajtaKodok = faFajtakBeolvasas[0].Split(";");
            string[] faFajtaNevek = faFajtakBeolvasas[1].Split(";");

            Console.WriteLine("A fajták beolvasva.");

            /*rendelések kiírása
            foreach (string[] sor in rendelesLista)
            {
                foreach (string adat in sor)
                {
                    Console.Write(adat + " ");
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
             * A lista elemeit (ami egy string tömb) összefűzöm egyetlen string változóba
             * A string változót pedig hozzáfűm a listához, amit utána rendezek.
             */

            // az adatok átmásolása egy új listába, hogy az eredeti ne legyen módosítva
            
            // a végleges, rendezhető [sort-olható] lista létrehozása
            List<string> rendezettLista = new List<string>();

            // a dátumok átrakása a tömb végére
            foreach (string[] aktualisRendeles in rendelesLista)
            {
                // dátum kimentése ideiglenesen
                string ideiglenesValtozo = aktualisRendeles[2];

                // Fafajta mozgatása a házszám után
                aktualisRendeles[2] = aktualisRendeles[3];

                // darabszám mozgatása a fafajta után
                aktualisRendeles[3] = aktualisRendeles[4];

                // dátum visszarakása a lista végére
                aktualisRendeles[4] = ideiglenesValtozo;
            }

            // a tömbös lista átrakása stringesbe:

            foreach (string[] sor in rendelesLista)
            {
                string temp = "";
                foreach (string adat in sor)
                {
                    temp = temp + adat;
                }
                // hozzáadás a rendezett listához
                rendezettLista.Add(temp);
            }

            rendezettLista.Sort();

            /*
             * kiírás
            foreach (string elem in rendezettLista)
            {
                Console.WriteLine(elem);
            }
            */


            /****************************************************************************
             * 4.
             * Kérje be a felhasználótól a facsemeték kiszállítási napját.Pl.: 2019.március 12 - i dátum! 
             * Határozza meg, és írja ki, hogy a rendelési időponthoz képest hány napot kellett várakozni a kiszállításra!
             */

            Console.WriteLine("Kérem adjon meg egy dátumot: ");
            DateTime datum = DateTime.Parse(Console.ReadLine()); //2019.05.01

            List<int> rendelesiIdok = new List<int>();

            foreach (string[] aktualisRendeles in rendelesLista)
            {
                //rendelesAdatok[4] = Rendelési nap;
                int kulonbseg = (datum - DateTime.Parse(aktualisRendeles[4])).Days;
                Console.WriteLine("A "+ aktualisRendeles[4] +"-i rendelés óta eltelt napok száma: " + kulonbseg);

                // tárolás a későbbi feladatokhoz:
                rendelesiIdok.Add(kulonbseg);
            }


            /****************************************************************************
             * 5.
             * Határozza meg, és írja ki a várakozási idők átlagát.
             * Az eredmény két tizedesjegyre kerekítve jelenjen meg!
             */

            // Puskázzunk az előzőből!
            double atlagosVarakozasiIdoNapban = rendelesiIdok.Sum() / rendelesLista.Count;
            Console.WriteLine("Az átlagos várakozási idő napban: " + atlagosVarakozasiIdoNapban.ToString("F2") + " nap");

            /****************************************************************************
             * 6.Írja ki a várakozási idők legnagyobb és legkisebb értékét!
             */

            // Használjuk megint a 4-est!
            Console.WriteLine("A legnagyobb várakozási idő: " + rendelesiIdok.Max() + " nap.");
            Console.WriteLine("A legkisebb várakozási idő: " + rendelesiIdok.Min() + " nap.");


            /****************************************************************************
             * 7. Írja ki, hogy az egyes facsemetefajtákat hány címről rendelték! 
             * 
             * segítség: a faFajtaNevek tömb = {"gömbjuhar";"mezei juhar";"korai juhar";"keleti ostorfa";"virágos kőris";"csörgőfa"}
             * 
             * utca + házszám?
             */

            int[] rendelesekSzama = new int[faFajtaNevek.Length];

            // for ciklus, ami végigmegy a fa fajtákon
            for (int i = 0; i < faFajtaNevek.Length; i++)
            {
                // foreach, hogy megnézzek minden rendelés
                foreach (string[] aktualisRendeles in rendelesLista)
                {
                    // ha az aktualis rendeles fa tipusa egyezik az éppen vizsgálttal
                    if (aktualisRendeles[2] == faFajtaNevek[i])
                    {
                        // a rendeleskSzama tömbben ugyanazon az indexen az ott lévő értéket növelem 1-gyel
                        rendelesekSzama[i] = rendelesekSzama[i]+1;
                    }
                }
            }

            // kiiratás
            for (int i = 0; i < rendelesekSzama.Length; i++)
            {
                Console.WriteLine("A " + faFajtaNevek[i] +" fajtából " + rendelesekSzama[i] + " helyről rendeltek!");
            }


            /****************************************************************************
             * 8. Számolja meg, és írja ki, hogy az egyes facsemetefajtákból hány darabot rendeltek összesen! 
             */

            // létrehozunk egy, a fa fajtákkal egyező hosszúságú tömböt
            // ha egy fafajta egyezik, azon a helyiértéken 1-gyel növelünk
            int[] rendeltDarabSzam = new int[faFajtaNevek.Length];


            for (int i = 0; i < faFajtaNevek.Length; i++)
            {
                foreach (string[] aktualisRendeles in rendelesLista)
                {
                    if (aktualisRendeles[2] == faFajtaNevek[i])
                    {
                        rendeltDarabSzam[i] = rendeltDarabSzam[i] + Int32.Parse(aktualisRendeles[3]);
                    }
                }
            }

            for (int i = 0; i < faFajtaNevek.Length; i++)
            {
                Console.WriteLine("A fa fajtája: " + faFajtaNevek[i] + " rendelt darabok száma: " + rendeltDarabSzam[i]);
            }
        }
    }
}