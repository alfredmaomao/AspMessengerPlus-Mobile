using System;
using System.Collections.Generic;
using System.Text;

namespace AspMessengerPlus.Maui.Services;

public class FakeAuthService : IAuthService
{
    public Task<bool> LoginAsync(string username, string password)
    {
        return Task.FromResult(true);
    }
}
