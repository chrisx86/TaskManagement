﻿#nullable enable
using TodoApp.Core.Models;
using TodoApp.Core.Services;

namespace TodoApp.Infrastructure.Services;

public class UserContext : IUserContext
{
    public User? CurrentUser { get; private set; }

    public void SetCurrentUser(User user)
    {
        CurrentUser = user;
    }
}