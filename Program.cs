using System.Collections;
using System.Diagnostics;
using System.Reflection;

namespace Facsemete
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
             * Adatszerkezet létrehozása
             * Lista, amiben egy rendelés adatait tároljuk
             * (Sorrend: "kozTerulet","hazSzam","rendelesiNap","faFajta","mennyisegDarab")
             * Majd ezt eltároljuk egy másik listában, aminek mindegyik eleme egy lista!
             */

            List<List<string>> rendelesLista = new List<List<string>>();
            Console.WriteLine("1. lépés: a rendelesek beolvasása. Kérem, üssön enter-t a folytatáshoz...");
            Console.ReadLine();

            // rendelések beolvasása
            string[] rendelesekBeolvasas = System.IO.File.ReadAllLines(@"C:\TEMP\rendeles.txt");

            foreach (string sor in rendelesekBeolvasas)
            {
                // az aktuális sor splittelése a ";"-nél
                string[] rendelesAdatok = sor.Split(";");
                // lista létrehozása új rendeléshez
                List<string> rendeles = new List<string>();

                foreach (string adat in rendelesAdatok)
                {
                    // a tömb elemeinek átrakása listába
                    rendeles.Add(adat);
                }

                // a feltöltött lista hozzáfűzése a rendelés-listához
                rendelesLista.Add(rendeles);
            }

            Console.WriteLine("2. lépés: a fajták beolvasása. Kérem, üssön enter-t a folytatáshoz...");
            Console.ReadLine();

            // fafajták beolvasása
            // Itt csak két sor van, és egy sorban van az adat, nem pedig soronként
            string[] faFajtakBeolvasas = System.IO.File.ReadAllLines(@"C:\TEMP\fajtak.txt");
            string[] faFajtaKodok = faFajtakBeolvasas[0].Split(";");
            string[] faFajtaNevek = faFajtakBeolvasas[1].Split(";");
            Console.WriteLine("A fajták beolvasva.");

            // rendelések kiírása
            foreach (List<string> aktualisRendeles in rendelesLista)
            {
                foreach (string rendelesAdat in aktualisRendeles)
                {
                    Console.Write(rendelesAdat + " ");
                }
            }


            // fafajták kiírása
            for (int i = 0; i < faFajtaKodok.Length; i++)
            {
                Console.WriteLine("Fa kód: " + faFajtaKodok[i]);
                Console.WriteLine("Fa neve: " + faFajtaNevek[i]);
            }
































            /*
           
            */

        }
    }
}