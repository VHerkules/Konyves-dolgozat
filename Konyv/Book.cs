using System;
using System.Collections.Generic;
using System.Linq;

public class Book
{
    public long ISBN { get; private set; }
    public List<Author> Authors { get; private set; }
    public string Title { get; private set; }
    public int PublicationYear { get; private set; }
    public string Language { get; private set; }
    public int Stock { get; private set; }
    public int Price { get; private set; }

    private static HashSet<long> existingISBNs = new HashSet<long>();


    public Book(long isbn, List<Author> authors, string title, int publicationYear, string language, int stock, int price)
    {
        if (isbn.ToString().Length != 10 || existingISBNs.Contains(isbn))
            throw new ArgumentException("Az ISBN 10 számjegyű, egyedi azonosító legyen.");

        if (authors.Count < 1 || authors.Count > 3)
            throw new ArgumentException("A szerzők listájának 1-3 elemet kell tartalmaznia.");

        if (title.Length < 3 || title.Length > 64)
            throw new ArgumentException("A cím hossza 3 és 64 karakter között kell legyen.");

        if (publicationYear < 2007 || publicationYear > DateTime.Now.Year)
            throw new ArgumentException("A kiadás éve 2007 és a jelen év közötti legyen.");

        if (language != "angol" && language != "német" && language != "magyar")
            throw new ArgumentException("A nyelv csak angol, német vagy magyar lehet.");

        if (stock < 0)
            throw new ArgumentException("A készlet nem lehet negatív.");

        if (price < 1000 || price > 10000 || price % 100 != 0)
            throw new ArgumentException("Az ár 1000 és 10000 között kell legyen, kerek 100-as szám.");

        ISBN = isbn;
        existingISBNs.Add(isbn);
        Authors = authors;
        Title = title;
        PublicationYear = publicationYear;
        Language = language;
        Stock = stock;
        Price = price;
    }

    public Book(string title, string authorName)
        : this(GenerateRandomISBN(),
            new List<Author> { new Author(authorName) },
            title,
            2024,
            "magyar",
            0,
            4500)
    { }

    private static long GenerateRandomISBN()
    {
        Random rnd = new Random();
        long isbn;
        do
        {
            isbn = rnd.Next(100000000, 1000000000);
            isbn = isbn * 10 + rnd.Next(0, 10);
        } while (existingISBNs.Contains(isbn));

        return isbn;
    }

    public override string ToString()
    {
        string authorLabel = Authors.Count == 1 ? "szerző" : "szerzők:";
        string stockInfo = Stock > 0 ? $"{Stock} db" : "beszerzés alatt";
        return $"Cím: {Title}, {authorLabel} {string.Join(", ", Authors.Select(a => a.FirstName + " " + a.LastName))}, " +
               $"Kiadás éve: {PublicationYear}, Nyelv: {Language}, Készlet: {stockInfo}, Ár: {Price} Ft";
    }
}