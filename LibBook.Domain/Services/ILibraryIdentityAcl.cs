﻿namespace LibBook.Domain.Services;

public interface ILibraryIdentityAcl
{
    (string name, string email) GetAccountBy(int id);
}