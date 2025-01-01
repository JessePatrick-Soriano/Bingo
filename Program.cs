using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Soriano_JessePatrick_3H_31_12_2024_FinalBingo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Tabellone PrimoTabellone = new Tabellone();

            Fogliettino PrimoFogliettino = new Fogliettino(PrimoTabellone.Lettere);

            for (int i = 0; i < PrimoTabellone.TheTabellone.Length; i++)
            {
                Tabellone.OutputLettere(PrimoTabellone.Lettere);
                Tabellone.OutputOggetto(PrimoTabellone.TheTabellone, PrimoTabellone.Lettere, 15, true);
                Tabellone.OutputLettere(PrimoTabellone.Lettere);
                Fogliettino.OutputOggetto(PrimoFogliettino.TheFogliettino, PrimoTabellone.Lettere, 5, false);

                Tabellone.Estrattore(PrimoTabellone.TheTabellone, PrimoFogliettino.TheFogliettino);
                string bruh = Console.ReadLine();
                Console.Clear();
            }

            Console.ReadKey();
        }
    }

    abstract public class BingoGenerale
    {
        public char[] Lettere { get; set; }

        public char[] RichiestaLettere()
        {
            Console.Write($"Inserire il nome che si vuole mettere sul fogliettino del bingo: ");
            string InputUtente = Console.ReadLine();

            int TotLettere = 0;
            var Lettere = new char[TotLettere];

            foreach (char Lettera in InputUtente) //Ogni lettera verrà inserità dentro l'array di char Lettere
            {
                TotLettere++;
                Array.Resize(ref Lettere, TotLettere);
                Lettere[TotLettere - 1] = Lettera;
            }

            return Lettere;
        }
        public static void OutputLettere(char[] Lettere)
        {
            foreach (char Lettera in Lettere)
            {
                Console.Write($"{Lettera,-5}");
            }

            Console.WriteLine($" "); // Divisore
        }

        public static int[,] InizializzatoreGenerale(char[] Lettere, int QuantitaRow, bool EUnTabellone) //QuantitaRow per il tabellone sarà 15 e per il fogliettino 5
        {
            int QuantitaNOggetto = Lettere.Length * QuantitaRow;
            var Oggetto = new int[QuantitaNOggetto, 2];

            int ColN = Lettere.Length, RowN = QuantitaRow;

            for (int RowIterator = 0; RowIterator < RowN; RowIterator++)
            {
                int N = 0;
                int NFogliettino = 0;

                for (int ColIterator = 0; ColIterator < ColN; ColIterator++)
                {
                    if (EUnTabellone == true)
                    {
                        Oggetto[RowIterator + N, 0] = RowIterator + N;
                        Oggetto[RowIterator + N, 1] = 0;
                    }
                    else
                    {
                        int NCasuale;
                        bool Uscita;
                        do // Verifica se il numero casuale è già presente nell'array
                        {
                            NCasuale = NumeroCasuale(NFogliettino + 1, NFogliettino + 15);
                            Uscita = !EGiaPresente(Oggetto, NCasuale);
                        } while (!Uscita);

                        Oggetto[RowIterator + N, 0] = NCasuale;
                        Oggetto[RowIterator + N, 1] = 0;

                        NFogliettino += 15;
                    }

                    N += QuantitaRow;
                }
            }

            return Oggetto;
        }

        public static int NumeroCasuale(int MinNCasuale, int MaxNCasuale)
        {
            var rnd = new Random();
            return rnd.Next(MinNCasuale, MaxNCasuale + 1);
        }

        public static void OutputOggetto(int[,] Oggetto, char[] Lettere, int QuantitaRow, bool EUnTabellone)
        {
            int ColN = Lettere.Length, RowN = QuantitaRow;
            string output;

            for (int RowI = 0; RowI < RowN; RowI++)
            {
                int N = 0;

                for (int ColI = 0; ColI < ColN; ColI++)
                {

                    if (EUnTabellone == true)
                    {
                        if (Oggetto[RowI + N, 1] == 1)
                        {
                            output = $"{Oggetto[RowI + N, 0] + 1,-5}";
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(output);
                            Console.ResetColor();  // Ripristina il colore predefinito
                        }
                        else
                        {
                            output = $"{Oggetto[RowI + N, 0] + 1,-5}";
                            Console.Write(output);
                        }
                    }
                    else
                    {
                        if (Oggetto[RowI + N, 1] == 1)
                        {
                            output = $"{Oggetto[RowI + N, 0],-5}";
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(output);
                            Console.ResetColor();  // Ripristina il colore predefinito
                        }
                        else
                        {
                            output = $"{Oggetto[RowI + N, 0],-5}";
                            Console.Write(output);
                        }
                    }

                    N += QuantitaRow;
                }

                Console.WriteLine($" "); // Divisore
            }

            Console.WriteLine($" "); // Divisore
        }

        public static bool EGiaPresente(int[,] Oggetto, int target)
        {
            int[,] TemporaryObject = (int[,])Oggetto.Clone();

            // Ordina l'array utilizzando SelectionSort
            SelectionSort(TemporaryObject);

            // Cerca il target utilizzando BinarySearch
            return BinarySearch(TemporaryObject, target);
        }

        public static void SelectionSort(int[,] array)
        {
            int Temporary;

            for (int i = 0; i < array.GetLength(0) - 1; i++)
            {
                int MinIndex = i;

                for (int j = i + 1; j < array.GetLength(0); j++)
                {
                    if (array[j, 0] < array[MinIndex, 0])
                    {
                        MinIndex = j;
                    }
                }

                // Scambia i valori
                Temporary = array[MinIndex, 0];
                array[MinIndex, 0] = array[i, 0];
                array[i, 0] = Temporary;
            }
        }

        public static bool BinarySearch(int[,] array, int target)
        {
            int left = 0;
            int right = array.GetLength(0) - 1;

            while (left <= right)
            {
                int mid = left + (right - left) / 2;

                if (array[mid, 0] == target)
                {
                    return true; // Elemento trovato
                }

                if (array[mid, 0] < target)
                {
                    left = mid + 1;
                }
                else
                {
                    right = mid - 1;
                }
            }

            return false; // Elemento non trovato
        }

    }

    public class Tabellone : BingoGenerale
    {
        public int[,] TheTabellone { get; set; }
        public Tabellone()
        {
            this.Lettere = RichiestaLettere();
            this.TheTabellone = InizializzatoreGenerale(Lettere, 15, true);
        }

        public static void Estrattore(int[,] Tabellone, int[,] Fogliettino)
        {
            int NCasuale, PosizioneNCasuale, NPosition=0;
            int Lunghezza1DFogliettino = Fogliettino.GetLength(0);
            int LunghezzaTotTabellone = Tabellone.Length - 1;
            bool Uscita = false;

            do
            {
                PosizioneNCasuale = NumeroCasuale(0, Tabellone.GetLength(0) - 1);
                NCasuale = Tabellone[PosizioneNCasuale, 0];  // Ottieni il numero estratto

                if (Tabellone[PosizioneNCasuale, 1] == 1) //Il numero è gia stato estratto, ripetere il procedimento
                {
                    Uscita = false;
                }
                else //Il numero non è stato ancora estratto
                {
                    Tabellone[PosizioneNCasuale, 1] = 1; //Adesso il numero adesso è considerato estratto

                    for (int i = 0; i < Lunghezza1DFogliettino; i++) // Cerca NCasuale in fogliettino
                    {
                        if (Fogliettino[i, 0] == NCasuale) //Numero trovato nel fogliettino
                        {
                            NPosition = i;
                            Fogliettino[NPosition, 1] = 1;
                            break;
                        }
                    }
                    Uscita = true;
                }
            } while (!Uscita);

            Console.WriteLine($"Numero estratto: {NCasuale}");
        }
    }

    public class Fogliettino : BingoGenerale
    {
        public int[,] TheFogliettino { get; set; }
        public Fogliettino(char[] Lettere)
        {
            this.TheFogliettino = InizializzatoreGenerale(Lettere, 5, false);
        }
    }

}