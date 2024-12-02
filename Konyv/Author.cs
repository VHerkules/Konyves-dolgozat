using System;

public class Author
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public Guid Id { get; private set; }

    public Author(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("A név nem lehet üres.");

        var names = fullName.Split(' ');
        if (names.Length != 2 || names[0].Length < 3 || names[1].Length < 3)
            throw new ArgumentException("A névnek kettő szóból kell állnia, mindkét szó minimum 3 és maximum 64 karakter hosszú!");

        FirstName = names[0];
        LastName = names[1];
        Id = Guid.NewGuid();
    }
}