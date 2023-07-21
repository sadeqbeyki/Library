﻿namespace LMS.Contracts.Lend;

public class LendItemDto
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public string Book { get; set; }
    public int Count { get; set; } = 1;
    public long LendId { get; set; }
}