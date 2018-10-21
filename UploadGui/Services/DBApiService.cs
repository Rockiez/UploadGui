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
    class UsersDBApiService : LinqToDB.Data.DataConnection
    {


        public UsersDBApiService() : base("LocalDB") { }
        public ITable<User> Users => GetTable<User>();

        public static List<User> All()
        {
            using (var db = new UsersDBApiService())
            {
                var query = from u in db.Users
                    orderby u.ID descending
                    select u;
                return query.ToList();
            }
        }

        public static bool InsertNewUser(User newUser)
        {
            if (QueryUserExistence(newUser.Email))
            {
                return true;
            }

            using (var db = new UsersDBApiService())
            {
                db.Insert(newUser);
                return true;
            }
        }

        public static bool QueryUserExistence(string userEmail)
        {
            using (var db = new UsersDBApiService())
            {
                var query = from u in db.Users
                    where u.Email == userEmail
                    select u;
                return query.Any();
            }
        }
    }
}
