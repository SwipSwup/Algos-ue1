using System;
using System.Security.Policy;

namespace UE1
{
    internal class Program
    {
        public static void Main(string[] args)
        {

            while (1 != 0)
            {

                Console.WriteLine("Enter 'add': Eine Aktie hinzufügen");
                Console.WriteLine("Enter 'del': Eine Aktie löschen");
                Console.WriteLine("Enter 'import': Kurswerte aus CSV importieren");
                Console.WriteLine("Enter 'search': Aktie suchen");
                Console.WriteLine("Enter 'plot': Schlusskurse der letzten 30 Tage als Grafik ausgeben");
                Console.WriteLine("Enter 'save': Hashtabelle in Datei abspeichern");
                Console.WriteLine("Enter 'load': Hashtabelle aus Datei laden");
                Console.WriteLine("Enter 'quit': Programm beenden");
                Console.WriteLine("Bitte wählen Sie eine Aktion aus:");


                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "add":

                        break;
                    case "del":

                        break;
                    case "import":

                        break;
                    case "search":

                        break;
                    case "plot":

                        break;
                    case "save":

                        break;
                    case "load":

                        break;
                    case "quit":
                        return;
                        break;
                    default:
                        Console.WriteLine("Ungültiger Befehl!");
                        break;
                }
            }
        }
    }
}