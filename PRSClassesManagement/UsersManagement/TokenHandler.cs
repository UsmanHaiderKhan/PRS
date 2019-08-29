using PRSClassesManagement.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PRSClassesManagement.UsersManagement
{
    public class TokenHandler
    {
        private PRSContext db = PRSContext.GetInstance();

        public Token GenerateToken(User user, bool rememberMe = true)
        {
            var token = new Token()
            {
                User = user,
                Key = HelperMethods.Sha256((user.Id + DateTime.Now.Ticks).ToString()),
                ExpiryDT = rememberMe ? DateTime.Now.AddDays(7) : DateTime.Now.AddHours(1)
            };

            db.Tokens.Add(token);
            db.SaveChanges();
            return token;
        }

        public bool IsTokenAnyAdmin(int id)
        {
            var t = GetAnyToken(id);
            return t != null && GetAnyToken(id).User.Role.Id == 1;
        }

        public bool IsTokenAnyAdmin(string key)
        {
            var t = GetAnyToken(key);
            return t != null && t.User.Role.Id == 1;
        }

        public bool IsTokenValidAdmin(int id)
        {
            var t = GetValidToken(id);
            return t != null && t.User.Role.Id == 1;
        }

        public bool IsTokenValidAdmin(string key)
        {
            var t = GetValidToken(key);
            return t != null && t.User.Role.Id == 1;
        }

        public List<Token> GetAllTokens()
        {
            return db.Tokens.ToList();
        }

        public Token GetAnyToken(int id)
        {
            return db.Tokens.Find(id);
        }

        public Token GetValidToken(string key)
        {
            var token = GetAnyToken(key);
            if (token == null || token.ExpiryDT < DateTime.Now)
                return null;
            return token;
        }

        public Token GetValidToken(int id)
        {
            var token = GetAnyToken(id);
            if (token.ExpiryDT < DateTime.Now)
                return null;
            return token;
        }

        public Token GetAnyToken(string key)
        {
            if (string.IsNullOrEmpty(key)) return null;
            return db.Tokens.FirstOrDefault(x => x.Key == key);
        }

        public void DeleteToken(int id)
        {
            var token = db.Tokens.Find(id);
            db.Tokens.Remove(token);
            db.SaveChanges();
        }
    }
}
