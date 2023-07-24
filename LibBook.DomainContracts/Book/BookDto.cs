﻿namespace LibBook.DomainContracts.Book;

public class BookDto
{
    public int PublisherId { get; set; }
    public string Publisher { get; set; }
    public int AuthorId { get; set; }
    public string Author { get; set; }
    public int TranslatorId { get; set; }
    public string Translator { get; set; }
    public int CategoryId { get; set; }
    public string Category { get; set; }

    public string Title { get; set; }
    public string ISBN { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
}