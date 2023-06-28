using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<BenchmarkObjects>();

[MemoryDiagnoser]
public class BenchmarkObjects
{
    [Params(1000, 10000)]
    public int N;

    [Benchmark]
    public string TestNullableObject()
    {
        Person p1 = Person.Create("Sanjay", "Sahani");
        Person p2 = Person.Create("Aarav");

        Book b1 = Book.Create("My First Book", p1);
        Book b2 = Book.Create("Books About Kids", p2);
        Book b3 = Book.Create("Unknown Book");

        GetBookLabel(b1);
        GetBookLabel(b2);
        return GetBookLabel(b3);
    }

    [Benchmark]
    public string TestOptionalObject()
    {
        PersonV2 pV1 = PersonV2.Create("Sanjay", "Sahani");
        PersonV2 pV2 = PersonV2.Create("Aarav");

        BookV2 bV1 = BookV2.Create("My First Book", pV1);
        BookV2 bV2 = BookV2.Create("Books About Kids", pV2);
        BookV2 bV3 = BookV2.Create("Unknown Book");


        GetBookLabelV2(bV1);
        GetBookLabelV2(bV2);
        return GetBookLabelV2(bV3);
    }

    string GetBookLabel(Book book) =>
        GetLabel(book.Author) is string author ? $"{book.Title} by { author }" : book.Title;

    string? GetLabel(Person? person) =>
        person is null ? null
        : person.LastName is null ? person.FirstName
        : $"{ person.FirstName } { person.LastName }";

    string GetBookLabelV2(BookV2 book) => book
        .Author
        .Map(GetLabelV2)
        .Map(author => $"{book.Title} by {author}")
        .Reduce(book.Title);

    string GetLabelV2(PersonV2 person) => person
        .LastName
        .Map(lastName => $"{person.FirstName} {lastName}")
        .Reduce(person.FirstName);
}


class Option<T> where T : class
{
    private T? _object = null;

    public static Option<T> Some(T obj) => new() { _object = obj };
    public static Option<T> None() => new();
    public Option<TResult> Map<TResult>(Func<T, TResult> map) where TResult : class =>
        _object is null ? Option<TResult>.None() : Option<TResult>.Some(map(_object));
    public T Reduce(T @default) => _object ?? @default;
}

class Person
{
    public string FirstName { get; }
    public string? LastName { get; }

    private Person(string firstName, string? lastName) =>
        (FirstName, LastName) = (firstName, lastName);

    public static Person Create(string firstName, string lastName) => new(firstName, lastName);
    public static Person Create(string name) => new(name, null);
}

class Book
{
    public string Title { get; }
    public Person? Author { get; }

    private Book(string title, Person? author) =>
        (Title, Author) = (title, author);

    public static Book Create(string title, Person author) => new(title, author);
    public static Book Create(string title) => new(title, null);
}


class PersonV2
{
    public string FirstName { get; }
    public Option<string> LastName { get; }

    private PersonV2(string firstName, Option<string> lastName) =>
        (FirstName, LastName) = (firstName, lastName);

    public static PersonV2 Create(string firstName, string lastName) => new(firstName, Option<string>.Some(lastName));
    public static PersonV2 Create(string name) => new(name, Option<string>.None());
}

class BookV2
{
    public string Title { get; }
    public Option<PersonV2> Author { get; }

    private BookV2(string title, Option<PersonV2> author) =>
        (Title, Author) = (title, author);

    public static BookV2 Create(string title, PersonV2 author) => new(title,  Option<PersonV2>.Some(author));
    public static BookV2 Create(string title) => new(title,  Option<PersonV2>.None());
}
