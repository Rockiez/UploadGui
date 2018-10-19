using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToDB.Mapping;

namespace UploadGui.Models
{
    

    public class LoginRequest
    {

        public string Email;
        public string Password;
        public string TwoFactorAuth;
        public string DeveloperToolProductName;
        public string DeveloperToolProductVersion;
    }

    public class LoginResult
    {
        public string DeveloperClientToken;
    }

    public class GetStudiosRequest
    {
        public string DeveloperClientToken;
    }

    public class GetStudiosResult
    {
        public Studio[] Studios;
    }

    public class Title
    {

        public string Id;
        public string Name;
        public string SecretKey;
        public string GameManagerUrl;
        public override string ToString()
        {
            return Name;
        }
    }

    public class Studio
    {
        public static Studio OVERRIDE = new Studio { Id = "", Name = "_OVERRIDE_", Titles = null };

        public string Id;
        public string Name;

        public Title[] Titles;

        public Title GetTitle(string titleId)
        {
            if (Titles == null)
                return null;
            for (var i = 0; i < Titles.Length; i++)
                if (Titles[i].Id == titleId)
                    return Titles[i];
            return null;
        }

        public string GetTitleSecretKey(string titleId)
        {
            var title = GetTitle(titleId);
            return title == null ? "" : title.SecretKey;
        }
        public override string ToString()
        {
            return Name;
        }
    }


    public class HttpResponseObject
    {
        public int code;
        public string status;
        public object data;
    }

    [Table(Name = "Products")]
    public class User
    {
        [PrimaryKey, Identity]
        public int ID;

        [Column(Name = "Email"), NotNull]
        public string Email;

        [Column(Name = "Password")]
        public string Password;
        public override string ToString()
        {
            return Email;
        }
    }

}
