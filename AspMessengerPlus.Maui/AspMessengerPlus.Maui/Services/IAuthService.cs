using System;
using System.Collections.Generic;
using System.Text;
using AspMessengerPlus.Maui.Models;

namespace AspMessengerPlus.Maui.Services;

public interface IAuthService
{
    Task<UserDto?> LoginAsync(string email, string password);
}
