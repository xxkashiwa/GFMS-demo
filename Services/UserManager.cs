﻿using GFMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Pwm;

namespace GFMS.Services
{
    public sealed class UserManager
    {
        private static UserManager? _instance;
        public static UserManager Instance => _instance ??= new UserManager();
        private UserManager()
        {
            OnAuthedUserChanged = delegate { }; // Initialize the event with an empty delegate  
        }

        private User? _authedUser;
        public bool IsAuthed => _authedUser != null;
        public User? AuthedUser
        {
            get => _authedUser;
            private set
            {
                _authedUser = value;
                OnAuthedUserChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler OnAuthedUserChanged;
        public void AuthenticateUser(User user)
        {
            AuthedUser = user;
        }

        public void Logout()
        {
            AuthedUser = null;
            ClearAuthedState();
        }

        public void LoadLoginState()
        {
            if (Windows.Storage.ApplicationData.Current.LocalSettings.Values.TryGetValue("AuthedUserId", out var userIdObj) &&
                userIdObj is string userId)
            {
                AuthedUser = new User
                {
                    UserId = userId,
                    GrantedType = "Admin"
                };
            }
            else
            {
                AuthedUser = null;
            }
        }
        public void SetAuthedState(User user)
        {
            Windows.Storage.ApplicationData.Current.LocalSettings.Values["AuthedUserId"] = user.UserId;
        }
        public void ClearAuthedState()
        {
            Windows.Storage.ApplicationData.Current.LocalSettings.Values.Remove("AuthedUserId");
        }
    }
}
