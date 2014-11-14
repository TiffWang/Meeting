using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Entities;
using System.Net;
using System.IO;

namespace BusinessTier
{
    public class UsersManager
    {
        //public static string Login(string username,string password)
        //{
        //   //// WebRequest request = WebRequest.Create(Config.LoginUrl + "?action=LoginVal&username=" + username + "&pwd=" + password + "&code=hnanjwang");

        //   // request.Method = "POST";
        //   // Stream dataStream = request.GetRequestStream();
        //   // dataStream.Close();

        //   // WebResponse response = request.GetResponse();
        //   // dataStream = response.GetResponseStream();
        //   // StreamReader reader = new StreamReader(dataStream);

        //   // string responseFromServer = reader.ReadToEnd();

        //   // reader.Close();
        //   // dataStream.Close();
        //   // response.Close();

        //   // if (responseFromServer.Contains("true"))
        //   //     responseFromServer = username;

        //   // return responseFromServer;
        //}

        //public static int GetUsersNum()
        //{
        //    HishopDataBase db = new HishopDataBase();

        //    var query = from a in db.aspnet_Users select a;

        //    return query.Count();
        //}
    }
}
