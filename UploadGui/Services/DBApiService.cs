using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UploadGui.Models;
using LinqToDB;
using LinqToDB.Mapping;

namespace UploadGui.Services
{
    class DBApiService : LinqToDB.Data.DataConnection
    {
        public DBApiService() : base("LocalDB") { }
        public ITable<User> Users => GetTable<User>();

        public static List<User> All()
        {
            using (var db = new DBApiService())
            {
                var query = from u in db.Users
                    orderby u.ID descending
                    select u;
                return query.ToList();
            }
        }

        public static bool InsertNewUser(User newUser)
        {
            if (QueryUserExist(newUser.Email))
            {
                return true;
            }

            using (var db = new DBApiService())
            {
                db.Insert(newUser);
                return true;
            }
            
        }

        public static bool QueryUserExist(string userEmail)
        {
            using (var db = new DBApiService())
            {
                var query = from u in db.Users
                    where u.Email == userEmail
                    select u;
                return query.First() != null;
            }
        }
    }
}
