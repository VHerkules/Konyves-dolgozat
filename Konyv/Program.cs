using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    private static Random random = new Random();

    static void Main(string[] args)
    {
        List<Book> books = GenerateBooks(15);
        SimulateSales(books, 100);
    }

    static List<Book> GenerateBooks(int count)
    {
        var books = new List<Book>();

        var randomTitles = new List<string>
        {
            "A Magyarság Elűzött", "The Last Stand", "Küzdelem az Életért",
            "Törékeny Egyensúly", "Dreams and Nightmares",
            "Az Éjszaka Hatalma", "A Változás Kora", "Lost in Translation",
            "A Sötétség Tündér", "End of Days"
        };

        var randomAuthors = new List<string>
        {
            "Kovács István", "Németh Anna", "Tóth Péter", "Williams John",
            "Béres Éva", "Johnson Emily", "Szabó Gábor", "Müller Hans",
            "Kiss László", "Tóth Emese"
        };

        for (int i = 0; i < count; i++)
        {
            long isbn = GenerateUniqueISBN(books);
            string title = randomTitles[random.Next(randomTitles.Count)];
            string language = random.NextDouble() < 0.8 ? "magyar" : "angol";

            int stock = random.NextDouble() < 0.3 ? 0 : random.Next(5, 11);
            int price = (random.Next(10, 101) * 100);

            List<Author> authors = new List<Author>();
            int authorCount = random.NextDouble() < 0.7 ? 1 : (random.Next(2) == 0 ? 2 : 3);
            for (int j = 0; j < authorCount; j++)
            {
                string authorName = randomAuthors[random.Next(randomAuthors.Count)];
                authors.Add(new Author(authorName));
            }

            int publicationYear = random.Next(2007, DateTime.Now.Year + 1);

            books.Add(new Book(isbn, authors, title, publicationYear, language, stock, price));
        }

        return books;
    }

    private static long GenerateUniqueISBN(List<Book> books)
    {
        long isbn;
        HashSet<long> existingISBNs = new HashSet<long>(books.Select(b => b.ISBN));

        do
        {
            isbn = random.Next(100000000, 1000000000);
            isbn = isbn * 10 + random.Next(0, 10);
        } while (existingISBNs.Contains(isbn));

        return isbn;
    }

    static void SimulateSales(List<Book> books, int iterations)
    {
        int salesCount = 0;
        decimal totalRevenue = 0;
        int outOfStockCount = 0;
        int initialStockCount = books.Sum(b => b.Stock);

        for (int i = 0; i < iterations; i++)
        {
            var randomBook = books[random.Next(books.Count)];
            if (randomBook.Stock > 0)
            {
                randomBook.GetType().GetProperty("Stock").SetValue(randomBook, randomBook.Stock - 1);
                salesCount++;
                totalRevenue += randomBook.Price;
            }
            else
            {
                if (random.NextDouble() < 0.5)
                {
                    randomBook.GetType().GetProperty("Stock").SetValue(randomBook, randomBook.Stock + random.Next(1, 11));
                }
                else
                {
                    books.Remove(randomBook);
                    outOfStockCount++;
                }
            }
        }

        int currentStockCount = books.Sum(b => b.Stock);
        Console.WriteLine($"Összes bevétel: {totalRevenue} Ft");
        Console.WriteLine($"Elfogyott a nagykerből: {outOfStockCount} db könyv");
        Console.WriteLine($"{initialStockCount} db könyv volt kezdetben, Jelenlegi {currentStockCount} db van, Különbség: {currentStockCount - initialStockCount} db");
    }
}
