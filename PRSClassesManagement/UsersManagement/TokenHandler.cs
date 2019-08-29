using PRSClassesManagement.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PRSClassesManagement.UsersManagement
{
    class TokenHandler
    {
        private PRSContext db = PRSContext.GetInstance();

        public Token GenerateToken(User user)
        {
            var token = new Token()
            {
                User = user,
                Key = HelperMethods.Sha256((user.Id + DateTime.Now.Ticks).ToString()),
                ExpiryDT = DateTime.Now
            };
            db.Tokens.Add(token);
            db.SaveChanges();

            return token;
        }

        public bool IsTokenAdmin(int id)
        {
            return GetToken(id).User.Role.Id == 1;
        }

        public List<Token> GetAllTokens()
        {
            using (db)
            {
                return db.Tokens.ToList();
            }
        }

        public Token GetToken(int id)
        {
            using (db)
            {
                return db.Tokens.Find(id);
            }
        }

        public Token GetValidToken(string key)
        {
            var token = GetToken(key);
            if (token.ExpiryDT < DateTime.Now)
                return null;
            return token;
        }

        public Token GetValidToken(int id)
        {
            var token = GetToken(id);
            if (token.ExpiryDT < DateTime.Now)
                return null;
            return token;
        }

        public Token GetToken(string key)
        {
            using (db)
            {
                return db.Tokens.FirstOrDefault(x => x.Key == key);
            }
        }

        public void DeleteToken(int id)
        {
            using (db)
            {
                var token = db.Tokens.Find(id);
                db.Tokens.Remove(token);
                db.SaveChanges();
            }
        }
    }
}
