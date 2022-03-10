using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace otszaz
{
    class Program
    {
        public static List<Vasarlas> vasarlasok = new List<Vasarlas>();
        static void Main(string[] args)
        {
            //1. Feladat
            fajlBeolvasas();

            //2. Feladat
            Console.WriteLine("2. feladat");
            Console.WriteLine("A fizetések száma: {0}\n",vasarlasok.Count);

            //3. feladat
            Console.WriteLine("3. feladat");
            Console.WriteLine("Az első vásárló {0} darab árucikket vásárolt.\n",vasarlasok.First().termekek.Count);

            //4. feladat
            Console.WriteLine("4. feladat");
            int sorszam = int.Parse(adatBeker("Adja meg egy vásárlás sorszámát! "));
            string arucikk = adatBeker("Adja meg egy árucikk nevét! ");
            int darabszam = int.Parse(adatBeker("Adja meg a vásárolt darabszámot! "));

            //5. feladat
            Console.WriteLine("\n5. feladat");
            Console.WriteLine("Az első vásárlás sorszáma: {0}", vasarlasok.Where(v => v.termekek.ContainsKey(arucikk)).Min(v => v.sorszam));
            Console.WriteLine("Az utolsó vásárlás sorszáma: {0}", vasarlasok.Where(v => v.termekek.ContainsKey(arucikk)).Max(v => v.sorszam));
            Console.WriteLine("{0} vásárlás során vettek belőle.\n", vasarlasok.Where(v => v.termekek.ContainsKey(arucikk)).Count());

            //6. feladat
            Console.WriteLine("6. feladat");
            Console.WriteLine("{0} darab vételekor fizetendő: {1}\n",darabszam,ertek(darabszam));

            //7. feladat
            Console.WriteLine("7. feladat");
            foreach (var t in vasarlasok.Where(v=>v.sorszam==sorszam).First().termekek)
            {
                Console.WriteLine("{0} {1}",t.Value,t.Key);
            }

            //8. feladat
            fajlIras();

            Console.ReadKey();
        }
        static void fajlBeolvasas()
        {
            int i = 1;
            Vasarlas tmp = new Vasarlas(i);
            foreach (var sor in File.ReadAllLines("penztar.txt"))
            {
                if (sor == "F")
                {
                    vasarlasok.Add(tmp);
                    tmp = new Vasarlas(++i);
                }
                else
                {
                    tmp.termekHozzadas(sor);
                }
            }
        }
        static void fajlIras()
        {
            using (FileStream f = File.Open("osszeg.txt",FileMode.Create))
            using(StreamWriter writer = new StreamWriter(f))
            {
                foreach (var item in vasarlasok)
                {
                    writer.WriteLine($"{item.sorszam}: {item.osszeg()}");
                }
            }
        }
        static string adatBeker(string uzenet)
        {
            Console.Write(uzenet);
            return Console.ReadLine();
        }
        public static int ertek(int darabszam)
        {
            if (darabszam >= 3)
            {
                return 500 + 450 + (darabszam - 2) * 400;
            }
            if (darabszam > 1)
            {
                return 500 + 450;
            }
            return 500;
        }
    }
    class Vasarlas
    {
        public int sorszam;
        public Dictionary<string,int> termekek = new Dictionary<string, int>();
        public Vasarlas(int ssz)
        {
            this.sorszam = ssz;
        }
        public int osszeg()
        {
            int osszeg = 0;
            foreach (var item in this.termekek)
            {
                osszeg = osszeg+Program.ertek(item.Value);
            }
            return osszeg;
        }
        public void termekHozzadas(string nev)
        {
            if (termekek.ContainsKey(nev))
            {
                termekek[nev]++;
            } else
            {
                termekek.Add(nev, 1);
            }
        }
    }
}
