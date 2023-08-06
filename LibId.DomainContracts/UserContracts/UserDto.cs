﻿using AppFramework.Application;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibIdentity.DomainContracts.UserContracts;

public class UserDto
{
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }
    [Required]
    [MaxLength(50)]
    public string LastName { get; set; }
    [MaxLength(50)]
    public string BirthDate { get; set; }
    [Required]
    [MaxLength(50)]
    public string UserName { get; set; }
    [Required]
    [MaxLength(100)]
    public string Email { get; set; }
    [Required]
    [MaxLength(50)]
    public string Password { get; set; }
    public string ReturnUrl { get; set; } = "/";
}
public class UserViewModel : UserDto
{
    public int Id { get; set; }
}