using System;
using System.Collections.Generic;
using System.Text;

namespace AspMessengerPlus.Maui.Services;

public interface IAuthService
{
    Task<bool> LoginAsync(string username, string password);
}
