using MapOfDiet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MapOfDiet.Services
{
    internal static class AuthorizationService
    {
        // Авторизация
        public static int? authorization(string login, string enteredPassword)
        {
            UserAuth? userAuth = new UserAuth();
            userAuth = DBWork.getUserModel(login);
            if (userAuth == null) // если такого пользователя нет
            {
                return null;
            }
            byte[] enteredHash = PasswordWork.newHash(enteredPassword, userAuth.Salt);
            if (enteredHash.SequenceEqual(userAuth.Hash)) // если всё хорошо
            {
                return userAuth.UserId;
            }
            return 0; // если хеши не совпали
        }

        // Регистрация
        public static bool registration(string login, string enteredPassword)
        {
            if (enteredPassword.Length < 4) return false;
            var userAuth = new UserAuth
            {
                Login = login,
                Salt = PasswordWork.newSalt(),
                Status = true
            };
            userAuth.Hash = PasswordWork.newHash(enteredPassword, userAuth.Salt);
            return DBWork.pushNewUserAuth(userAuth);
        }
    }
}
