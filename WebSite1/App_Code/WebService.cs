using System;
using System.Collections.Generic;
using System.Collections;
//using System.Linq;
using System.Web;
using System.Web.Services;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using System.Web.UI.WebControls;
using Json;
using System.Web.Script.Services;
/// <summary>
/// Summary description for WebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
 [System.Web.Script.Services.ScriptService]
public class WebService : System.Web.Services.WebService {
    private MySqlConnection connection;
    private static List<string> cred = new List<string> {"user","user" };

    public WebService () {
        
    }


    [WebMethod]
    public string InitializeComponent(string user, string password)
    {
        //if (connection.State == ConnectionState.Open) {
        //    connection.Close();

        //}
        cred[0] = user;
        cred[1] = password;
        string connectionString = "SERVER=localhost;DATABASE=sys;UID=" + user + ";PASSWORD=" + password + ";pooling=true;";

        connection = new MySqlConnection(connectionString);

       try
        {
            connection.Open();


        }
        catch (MySqlException e)
        {
            switch (e.Number)
            {
                case 0:
                    return "Cannot connect to server." + e.ToString();

                case 1045:
                    return "Invalid username/password, please try again";
            }
            return e.ToString();
        }

       return "OK";
    }


    [WebMethod]
    public string userRegister(string ID, string uID, string name, string surname, string email, string phone, string amount)
    {
        InitializeComponent(cred[0], cred[1]); 
        //writer
        string sqlWrite = "INSERT INTO `sys`.`" + ID + "` (`ID`, `name`, `surname`, `email`, `phone`, `amount`) VALUES ('"
        + uID + "','" + name + "','" + surname + "','" + email + "','" + phone + "','" + amount + "')";
        MySqlCommand cmd = new MySqlCommand(sqlWrite, connection);
        try
        {
            cmd.ExecuteNonQuery();
        }
        catch (MySqlException e)
        {
            return e.ToString();
        }
        return "OK";
    }

    [WebMethod]
    public string EventCreate(string eventName, string eventAmount, string eventDate, string eventAdmins, string img, string text)
    { //unique event ID
        InitializeComponent(cred[0], cred[1]);
        Random r = new Random();
        string eventID=r.Next(1000, 9999).ToString();
        string sqlWrite = "CREATE TABLE sys.`" + eventID + "` LIKE sys.`templatetable`;" +
            "INSERT INTO `sys`.`" + "events" + "` (`ID`, `name`, `timedate`, `amount`, `admins`,`img`,`text` ) VALUES ('"
            + eventID + "','" + eventName + "','" + eventDate + "','" + eventAmount + "','" + eventAdmins + "','" + img + "','" + text + "')"; 
    MySqlCommand cmd = new MySqlCommand(sqlWrite, connection);
    try
    {
        cmd.ExecuteNonQuery();
    }
    catch (MySqlException e)
    {
         return e.ToString();
    }
    return sqlWrite;
    }

    [WebMethod]
    public string EventDelete(string eventID)
    { //unique event ID
        InitializeComponent(cred[0], cred[1]);
        string sqlWrite = "DELETE FROM sys.`events`WHERE `ID`='" + eventID + "';DROP TABLE sys.`" + eventID + "`;";
        MySqlCommand cmd = new MySqlCommand(sqlWrite, connection);
        try
        {
            cmd.ExecuteNonQuery();
        }
        catch (MySqlException e)
        {
            return e.ToString();
        }
        return sqlWrite;
    }

     [WebMethod]
     [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void AtendantsReader(string ID) {
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
         string what="all";
         if (what.Equals("all") | what.Equals("*"))
         {
             Context.Response.Write(SQLRead("select * from sys.`"+ID+"` where NOT ID = '0'"));
         }
         else
         {
             Context.Response.Write(SQLRead("SELECT * FROM sys.`" + ID + "` WHERE ID='" + what + "'"));
         }

    }

     [WebMethod]
     public string CheckIn(string ID, string usrID, string checkin)
     {

        InitializeComponent(cred[0], cred[1]);
        string sqlWrite= "UPDATE sys.`" + ID + "` SET checkin = '" + checkin + "' WHERE ID = '" + usrID + "';";
                 MySqlCommand cmd = new MySqlCommand(sqlWrite, connection);
        try
        {
            cmd.ExecuteNonQuery();
        }
        catch (MySqlException e)
        {
            return e.ToString();
        }
        return sqlWrite;
    }

     

     [WebMethod]
     [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
     public void EventReader()
     {

         Context.Response.Clear();
         Context.Response.ContentType = "application/json";
         string what = "all";
         if (what.Equals("all") | what.Equals("*"))
         {
             Context.Response.Write(EventSQLRead("select * from sys.`events`"));
         }
         else
         {
             Context.Response.Write(EventSQLRead("SELECT * FROM sys.`events` WHERE ID='" + what + "'"));
         }

     }

    public string SQLRead(string query)
    {

        InitializeComponent(cred[0], cred[1]);
        //Create a list to store the result
        List<Person> people = new List<Person> { };
        //Create Command
        MySqlCommand cmd = new MySqlCommand(query, connection);
        //Create a data reader and Execute the command
        MySqlDataReader dataReader;
        try
        {
            dataReader = cmd.ExecuteReader();
            //Read the data and store them in the list
            while (dataReader.Read())
            {

                people.Add(new Person
                {
                    ID = dataReader["id"].ToString(),
                    name = dataReader["name"].ToString(),
                    surname = dataReader["surname"].ToString(),
                    email = dataReader["email"].ToString(),
                    phone = dataReader["phone"].ToString(),
                    amount = dataReader["amount"].ToString(),
                    time = dataReader["time"].ToString(),
                    checkin = dataReader["checkin"].ToString()

                });

            }

            //close Data Reader
            dataReader.Close();
            //return list to be displayed
            return people.ToJSON();
        }
        catch (MySqlException) { 
                   people.Add(new Person
            {
                ID = "ERROR!",
                name = "This eventID",
                surname = "Does not exist",
                email = "",
                phone = "",
                amount = "",
                time = "",
                checkin = ""

            });
                   return people.ToJSON();
        }

        

    }

    public string EventSQLRead(string query)
    {

        InitializeComponent(cred[0], cred[1]);
        //Create a list to store the result
        List<Event> events = new List<Event> { };
        //Create Command
        MySqlCommand cmd = new MySqlCommand(query, connection);
        //Create a data reader and Execute the command
        MySqlDataReader dataReader = cmd.ExecuteReader();

        //Read the data and store them in the list
        while (dataReader.Read())
        {

            events.Add(new Event
            {
                ID = dataReader["id"].ToString(),
                name = dataReader["name"].ToString(),
                amount = dataReader["amount"].ToString(),
                time = dataReader["eventscol"].ToString(),
                admins = dataReader["admins"].ToString(),
                img = dataReader["img"].ToString(),
                date = dataReader["timedate"].ToString(),
                text = dataReader["text"].ToString()


            });

        }

        //close Data Reader
        dataReader.Close();
        //return list to be displayed
        return events.ToJSON();

    }

    public class Person{
        public string ID { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string amount { get; set; }
        public string time { get; set; }
        public string checkin { get; set; }

    }
    public class Event
    {
        public string ID { get; set; }
        public string name { get; set; }
        public string amount { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public string admins { get; set; }
        public string img { get; set; }
        public string text { get; set; }

    }
}
