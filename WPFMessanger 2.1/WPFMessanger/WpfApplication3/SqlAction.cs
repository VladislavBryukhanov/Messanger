using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
namespace WpfApplication3
{
    class SqlAction
    {
        private SqlConnection con;
        private SqlCommand com;
        private SqlConnectionStringBuilder conBuilder;
        private int isBothFieldWereChanged = -1;
        private bool isConOpen = false;
        public SqlAction()
        {
            con = new SqlConnection();
            conBuilder = new SqlConnectionStringBuilder();
            conBuilder.InitialCatalog = "ChatNetwork";
            conBuilder.UserID = "admin";
            conBuilder.Password = "Admin";
            //conBuilder.DataSource = "134.249.132.121,49172";
            conBuilder.DataSource = "localhost";
            conBuilder.IntegratedSecurity = false;
            con.ConnectionString = conBuilder.ConnectionString;
            con.Open();
            con.Close();
        }
        /*
        public List<String> Auentific(string login, string password)//List<String>
        {
            List<String> lst = new List<string>();
            com = new SqlCommand("Auentific", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter p = com.Parameters.AddWithValue("@Login", login);
            // SqlParameter p1 = new SqlParameter("@MyParam", SqlDbType.Int);
            p.Direction = System.Data.ParameterDirection.Input;
            SqlParameter p1 = com.Parameters.AddWithValue("@Password", password);
            p1.Direction = System.Data.ParameterDirection.Input;

            //  p.Value = login;
            //  p1.Value = password;
            // com.Parameters.Add(p);
            // com.Parameters.Add(p1);
            // List<String> ret = new List<String>();
            con.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                lst.Add(reader.ToString());
            }
            //  while (reader.Read())
            //   {
            //      ret.Add(reader[0].ToString());
            //  }
            //  while (reader.Read())
            //string ret = reader[0].ToString();//.Add(reader[0].ToString());
            //  com.ExecuteNonQuery();
            con.Close();
            //   return ret;
            //  com = new SqlCommand();
            //  com.Connection = con;
            //  com.CommandText = @"SELECT Login,Password FROM City";
            //  SqlDataReader reader = com.ExecuteReader();
            // while (reader.Read())
            //  {
            //      lst.Add(reader.GetString(0));
            //  }
            //  con.Close();
            return lst;
        }
               */
        public List<String> A(string login, string password)
        {
            List<String> lst = new List<string>();
            try
            {
                com = new SqlCommand();
                com.Connection = con;
                com.CommandText = @"Select LastName from Account";
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(reader.GetString(0));
                }
                com.Dispose();
                con.Close();
            }
            catch (SqlException ex)
            {

            }

            return lst;
        }
        public int Auentific(string login, string password)//List<String>
        {
            int id = 0;
            com = new SqlCommand("Select id From Account where Login=@Login and Password=@Password", con);//"Sergeyus", "Serg"
            com.Connection = con;
            com.Parameters.Add("@Login", SqlDbType.VarChar);
            com.Parameters["@Login"].Value = login;
            com.Parameters.AddWithValue("@Password", password);
            // com.CommandType = System.Data.CommandType.StoredProcedure;
            // SqlParameter p = com.Parameters.AddWithValue("@Login", login);
            // p.Direction = System.Data.ParameterDirection.Input;
            // SqlParameter p1 = com.Parameters.AddWithValue("@Password", password);
            // p1.Direction = System.Data.ParameterDirection.Input;
            con.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                id = reader.GetInt32(0);
            }
            con.Close();
            return id;

        }

        public void AddPersson(string FirstName, string LastName, int age, string Country, string Description, string Login, string Password)
        {

            com = new SqlCommand("AddPers", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;

            //SqlParameter param = com.Parameters.AddWithValue("@Photo", "NULL");//!!
            //param.Direction = System.Data.ParameterDirection.Input;

            SqlParameter param = com.Parameters.AddWithValue("@FirstName", FirstName);
            param.Direction = System.Data.ParameterDirection.Input;

            param = com.Parameters.AddWithValue("@LastName", LastName);
            param.Direction = System.Data.ParameterDirection.Input;

            param = com.Parameters.AddWithValue("@Age", age);
            param.Direction = System.Data.ParameterDirection.Input;

            param = com.Parameters.AddWithValue("@Country", Country);
            param.Direction = System.Data.ParameterDirection.Input;

            param = com.Parameters.AddWithValue("@Description", Description);
            param.Direction = System.Data.ParameterDirection.Input;

            param = com.Parameters.AddWithValue("@Login", Login);
            param.Direction = System.Data.ParameterDirection.Input;

            param = com.Parameters.AddWithValue("@Password", Password);
            param.Direction = System.Data.ParameterDirection.Input;

            try
            {
                con.Open();
                com.ExecuteNonQuery();

            }
            catch (SqlException ex)
            {

            }
            finally
            {
                con.Close();
            }


        }
        public string[] GetData(int id)
        {
            string LastName = null, FirstName = null, Description = null, Photo = null, Country = null;
            int age = 0;
            Photo = "NULL";
            //   string[] data = { "FirsName", "LastName", "Description", "Photo", "Country", "Age" };
            for (int i = 0; i < 6; i++)
            {
                switch (i)
                {
                    case 0:
                        com = new SqlCommand("Select FirsName From Account where id=@id", con);//"Sergeyus", "Serg"
                        break;
                    case 1:
                        com = new SqlCommand("Select LastName From Account where id=@id", con);//"Sergeyus", "Serg"
                        break;
                    case 2:
                        com = new SqlCommand("Select Description From Account where id=@id", con);//"Sergeyus", "Serg"
                        break;
                    case 3:
                        com = new SqlCommand("Select PathOfPhoto From Account where id=@id", con);//"Sergeyus", "Serg"
                        break;
                    case 4:
                        com = new SqlCommand("Select Country From Account where id=@id", con);//"Sergeyus", "Serg"
                        break;
                    case 5:
                        com = new SqlCommand("Select Age From Account where id=@id", con);//"Sergeyus", "Serg"
                        break;
                }
                com.Connection = con;
                com.Parameters.AddWithValue("@id", id);
                //      com.Parameters.AddWithValue("@id", Key);//Key global tmp
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    switch (i)
                    {
                        case 0:
                            FirstName = reader.GetString(0);
                            break;
                        case 1:
                            LastName = reader.GetString(0);
                            break;
                        case 2:
                            Description = reader.GetString(0);
                            break;
                        case 3:
                            if (!reader.IsDBNull(0))
                                Photo = reader.GetString(0);
                            break;
                        case 4:
                            Country = reader.GetString(0);
                            break;
                        case 5:
                            age = reader.GetInt32(0);
                            break;
                    }

                }
                con.Close();
            }
            /*
        com = new SqlCommand("Select @type From Account where id=@id", con);//"Sergeyus", "Serg"
            com.Connection = con;
            com.Parameters.AddWithValue("@id", 1);
            com.Parameters.AddWithValue("@type",@"FirsName");
      //      com.Parameters.AddWithValue("@id", Key);//Key global tmp
            con.Open();
            SqlDataReader reader1 = com.ExecuteReader();
            while (reader1.Read())
            {

                FirstName = reader1.GetString(0);
            }
            con.Close();
           */
            string[] Data = { FirstName, LastName, Convert.ToString(age), Country, Description, Photo };
            return Data;
        }


        public void LoadPicture(string Photo, int id)
        {
            com = new SqlCommand("LoadImage", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;

            SqlParameter param = com.Parameters.AddWithValue("@Photo", Photo);//!!
            param.Direction = System.Data.ParameterDirection.Input;
            param = com.Parameters.AddWithValue("@id", id);
            param.Direction = System.Data.ParameterDirection.Input;
            try
            {
                con.Open();
                com.ExecuteNonQuery();

            }
            catch (SqlException ex)
            {
            }
            finally
            {
                con.Close();
            }

        }
        /*
        public List<String> Test()
        {
            List<String> lst = new List<string>();
            com = new SqlCommand("Auentification", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;

            //SqlParameter param = com.Parameters.AddWithValue("@Photo", "NULL");//!!
            //param.Direction = System.Data.ParameterDirection.Input;
            SqlParameter p = com.Parameters.AddWithValue("@Login", "Sergeyus");
            // SqlParameter p1 = new SqlParameter("@MyParam", SqlDbType.Int);
            p.Direction = System.Data.ParameterDirection.Input;
            SqlParameter p1 = com.Parameters.AddWithValue("@Password", "Serg");
            p1.Direction = System.Data.ParameterDirection.Input;
            try
            {
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(reader[0].ToString());
                }

            }
            catch (SqlException ex)
            {

            }
            finally
            {
                con.Close();
            }
            return lst;

        }*/

        //--------------------------------------------------------------------------
        public string DownloadMessages(int id, int To)
        {
            string TextOfMess = "";
            com = new SqlCommand("Select content From Messag where (idAccount=@id and ToAcc=@To) or(idAccount=@To and ToAcc=@id)", con);//"Sergeyus", "Serg"

            com.Connection = con;
            com.Parameters.AddWithValue("@id", id);
            com.Connection = con;
            com.Parameters.AddWithValue("@To", To);
            con.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                TextOfMess = reader.GetString(0);
            }
            con.Close();
            return TextOfMess;
        }
        public void SendMess(string mess, int id, int to)
        {
            com = new SqlCommand("SendMessage", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;

            SqlParameter param = com.Parameters.AddWithValue("@Mess", mess);//!!
            param.Direction = System.Data.ParameterDirection.Input;
            param = com.Parameters.AddWithValue("@id", id);
            param.Direction = System.Data.ParameterDirection.Input;
            param = com.Parameters.AddWithValue("@to", to);
            param.Direction = System.Data.ParameterDirection.Input;
            try
            {
                con.Open();
                com.ExecuteNonQuery();

            }
            catch (SqlException ex)
            {
            }
            finally
            {
                con.Close();
            }
        }

        public void SendChatMess(string mess, int ChatId)
        {
            com = new SqlCommand("SendChatMessage", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;

            SqlParameter param = com.Parameters.AddWithValue("@Mess", mess);//!!
            param.Direction = System.Data.ParameterDirection.Input;
            param = com.Parameters.AddWithValue("@id", ChatId);
            param.Direction = System.Data.ParameterDirection.Input;
            try
            {
                con.Open();
                com.ExecuteNonQuery();

            }
            catch (SqlException ex)
            {
            }
            finally
            {
                con.Close();
            }
        }
        public string DownloadChat(int id)
        {
            string TextOfMess = "";
            com = new SqlCommand("Select content From Chat where id=@id ", con);//"Sergeyus", "Serg"

            com.Connection = con;
            com.Parameters.AddWithValue("@id", id);
            con.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                TextOfMess = reader.GetString(0);
            }
            con.Close();
            return TextOfMess;
        }
   

        int getCount()
        {
            int count = 0;
            com = new SqlCommand();
            com.Connection = con;
            com.CommandText = @"SELECT COUNT(id) FROM Account";
            con.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                count = reader.GetInt32(0);
            }
            com.Dispose();
            con.Close();
            return count;
        }
        int getCountOfChats()
        {
            int count = 0;
            com = new SqlCommand();
            com.Connection = con;
            com.CommandText = @"SELECT COUNT(id) FROM Chat";
            con.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                count = reader.GetInt32(0);
            }
            com.Dispose();
            con.Close();
            return count;
        }
        public Pers[] GetPeople()
        {
            Pers[] persons = new Pers[getCount()];
            com = new SqlCommand();

            string[] FullName;
            FullName = new string[getCount()];



            for (int i = 0; i < getCount(); i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    switch (j)
                    {
                        case 0:
                            com = new SqlCommand("Select FirsName From Account where id=@id", con);//"Sergeyus", "Serg"
                            break;
                        case 1:
                            com = new SqlCommand("Select LastName From Account where id=@id", con);//"Sergeyus", "Serg"
                            break;
                    }
                    com.Connection = con;
                    com.Parameters.AddWithValue("@id", i + 1);
                    //      com.Parameters.AddWithValue("@id", Key);//Key global tmp
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        switch (j)
                        {
                            case 0:
                                FullName[i] = reader.GetString(0);
                                FullName[i] += " ";
                                break;
                            case 1:
                                FullName[i] += reader.GetString(0);
                                // FullName[i] += "\n";
                                persons[i] = new Pers(i + 1, FullName[i].Split(' ')[0], FullName[i].Split(' ')[1]);//

                                break;
                        }

                    }
                    con.Close();
                }

            }
            return persons;
        }




        public List<int> Search(string SrchString)// сомнительный алгоритм в плане производительности(нагрузки на сеть), возможно будет эффективнее искать id в listbox или disconnected режим
        {
            List<int> id = new List<int>();
            //id.Add(999);//недостижимое число по которому можно определить, что поиск ничего не нашел
            String SQLtmp1 = "@FirsName";
            String SQLtmp2 = "@LastName";
            string SQLquery = "Select id From Account where FirsName=@FirsName";
            for (int j = 0; j < 2 && id.Count == 0; j++)
            {



                if (SrchString.Contains(' '))
                {
                    string[] FLName = SrchString.Split(' ');

                    // List<int> id;

                    com = new SqlCommand("Select id From Account where FirsName=@FirsName and LastName=@LastName", con);// and Password=@Password
                    com.Connection = con;
                    com.Parameters.AddWithValue(SQLtmp1, FLName[0]);
                    com.Parameters.AddWithValue(SQLtmp2, FLName[1]);
                    //com.Parameters.AddWithValue("@FirsName", FLName[1]);
                    //com.Parameters.AddWithValue("@LastName", FLName[0]);
                }
                else
                {
                    com = new SqlCommand(SQLquery, con);// and Password=@Password
                    com.Connection = con;
                    com.Parameters.AddWithValue(SQLtmp1, SrchString);
                    //com.Parameters.AddWithValue("@LastName", SrchString);
                }
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    id.Add(reader.GetInt32(0));
                }
                con.Close();
                if (id.Count == 0)
                {
                    SQLtmp1 = "@LastName";
                    SQLtmp2 = "@FirsName";
                    SQLquery = "Select id From Account where LastName=@LastName";
                }
            }
            return id;
        }


        public Pers[] GetPeople(int[] id)
        {
            Pers[] persons = new Pers[id.Length];//

            com = new SqlCommand();

            string[] FullName;
            FullName = new string[id.Length];

            for (int i = 0; i < id.Length; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    switch (j)
                    {
                        case 0:
                            com = new SqlCommand("Select FirsName From Account where id=@id", con);//"Sergeyus", "Serg"
                            break;
                        case 1:
                            com = new SqlCommand("Select LastName From Account where id=@id", con);//"Sergeyus", "Serg"
                            break;
                    }
                    com.Connection = con;
                    com.Parameters.AddWithValue("@id", id[i]);
                    //      com.Parameters.AddWithValue("@id", Key);//Key global tmp
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        switch (j)
                        {
                            case 0:
                                FullName[i] = reader.GetString(0);
                                FullName[i] += " ";
                                break;
                            case 1:
                                FullName[i] += reader.GetString(0);
                                //FullName[i] += "\n";
                                persons[i] = new Pers(id[i], FullName[i].Split(' ')[0], FullName[i].Split(' ')[1]);//
                                break;
                        }

                    }
                    con.Close();
                }

            }
            return persons;
        }



        public bool isFriend(int ID, int to)// 1-isWrite, 2-isFriend,3-in BlackList
        {
            string isFriendField = "";
            string IdField = "";
            SqlDataReader reader;


            com = new SqlCommand(@"Select idOfPers from Relations where idOfOwner=@id", con);
            con.Open();
            com.Parameters.AddWithValue("@id", ID);
            reader = com.ExecuteReader();
            while (reader.Read())
            {
                IdField = reader.GetString(0);
            }
            con.Close();
            com = new SqlCommand(@"Select isfriend from Relations where idOfOwner=@id", con);
            con.Open();
            com.Parameters.AddWithValue("@id", ID);
            reader = com.ExecuteReader();
            while (reader.Read())
            {
                isFriendField = reader.GetString(0);
            }
            con.Close();
            int IF = new int();
            IF = Convert.ToInt32(IdField.IndexOf(Convert.ToString(to), StringComparison.CurrentCulture));
            if (isFriendField[IF] == '1')
                return true;
            else return false;
        }

        public bool isBlackList(int ID, int to)// 1-isWrite, 2-isFriend,3-in BlackList
        {
            string isFriendField = "";
            string IdField = "";
            SqlDataReader reader;


            com = new SqlCommand(@"Select idOfPers from Relations where idOfOwner=@id", con);
            con.Open();
            com.Parameters.AddWithValue("@id", ID);
            reader = com.ExecuteReader();
            while (reader.Read())
            {
                IdField = reader.GetString(0);
            }
            con.Close();
            com = new SqlCommand(@"Select isblackList from Relations where idOfOwner=@id", con);
            con.Open();
            com.Parameters.AddWithValue("@id", ID);
            reader = com.ExecuteReader();
            while (reader.Read())
            {
                isFriendField = reader.GetString(0);
            }
            con.Close();
            int IF = new int();
            IF = Convert.ToInt32(IdField.IndexOf(Convert.ToString(to), StringComparison.CurrentCulture));
            if (isFriendField[IF] == '1')
                return true;
            else return false;
        }


        public Pers[] StateOf(int ID, int Category)// 1-isWrite, 2-isFriend,3-in BlackList
        {
            string isWriteField = "";
            string IdField = "";
            SqlDataReader reader;
            string QueryString = "";
            int size = getCount();
            Pers[] AllPeople = new Pers[size];
            List<Pers> Companion = new List<Pers>();
            com = new SqlCommand();

            string[] FullName;
            FullName = new string[size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    switch (j)
                    {
                        case 0:
                            com = new SqlCommand("Select FirsName From Account where id=@id", con);//"Sergeyus", "Serg"
                            break;
                        case 1:
                            com = new SqlCommand("Select LastName From Account where id=@id", con);//"Sergeyus", "Serg"
                            break;
                    }
                    com.Connection = con;
                    com.Parameters.AddWithValue("@id", i + 1);

                    //      com.Parameters.AddWithValue("@id", Key);//Key global tmp
                    con.Open();
                    reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        switch (j)
                        {
                            case 0:
                                FullName[i] = reader.GetString(0);
                                FullName[i] += " ";
                                break;
                            case 1:
                                FullName[i] += reader.GetString(0);
                                // FullName[i] += "\n";
                                AllPeople[i] = new Pers(i + 1, FullName[i].Split(' ')[0], FullName[i].Split(' ')[1]);//

                                break;

                        }

                    }
                    con.Close();
                }

            }

            com = new SqlCommand(@"Select idOfPers from Relations where idOfOwner=@id", con);
            con.Open();
            com.Parameters.AddWithValue("@id", ID);
            reader = com.ExecuteReader();
            while (reader.Read())
            {
                IdField = reader.GetString(0);
            }
            con.Close();
            switch (Category)
            {
                case 1:
                    QueryString = @"Select isWrite from Relations where idOfOwner=@id";
                    break;
                case 2:
                    QueryString = @"Select isfriend from Relations where idOfOwner=@id";
                    break;
                case 3:
                    QueryString = @"Select isblackList from Relations where idOfOwner=@id";
                    break;
            }




            //
            com = new SqlCommand(QueryString, con);
            con.Open();
            com.Parameters.AddWithValue("@id", ID);
            reader = com.ExecuteReader();
            while (reader.Read())
            {
                isWriteField = reader.GetString(0);
            }
            con.Close();




            int[] IF = new int[size];
            int[] CF = new int[size];
            List<int> FriendsId = new List<int>();
            for (int i = 0; i < size - 1; i++)
            {
                IF[i] = Convert.ToInt32(IdField.Split(' ')[i]);
                CF[i] = Convert.ToInt32(isWriteField.Split(' ')[i]);
                if (CF[i] == 1)
                    FriendsId.Add(IF[i]);
            }

            for (int i = 0; i < FriendsId.Count; i++)
            {
                Companion.Add(AllPeople[FriendsId[i] - 1]);
            }
            //FriendsData
            return Companion.ToArray();
        }

        public Chat[] GetChats(int ID)
        {
            SqlDataReader reader;
            int size = getCountOfChats();
            List<Chat> AllChats = new List<Chat>();
            com = new SqlCommand();
            List<string> Name, IdOfMembers, Photo;
            List < int> idOfCreator, id;
            Name = new List<string>();
            Photo = new List<string>();
            IdOfMembers = new List<string>();
            idOfCreator = new List<int>();
            id = new List<int>();
                for (int j = 0; j < 5; j++)
                {
                    switch (j)
                    {
                        case 0:
                            com = new SqlCommand("Select Name From Chat where idOfCreator=@id or CHARINDEX(cast(@id as varchar(max)),Chat.IdOfMembers)>0", con);//надо сделать функцию в sql которая будет искать в строке входящий символ
                            break;
                        case 1:
                            com = new SqlCommand("Select ID From Chat where idOfCreator=@id or CHARINDEX(cast(@id as varchar(max)),Chat.IdOfMembers)>0", con);
                            break;
                        case 2:
                            com = new SqlCommand("Select idOfCreator From Chat where idOfCreator=@id or CHARINDEX(cast(@id as varchar(max)),Chat.IdOfMembers)>0", con);
                            break;
                        case 3:
                            com = new SqlCommand("Select IdOfMembers From Chat where idOfCreator=@id or CHARINDEX(cast(@id as varchar(max)),Chat.IdOfMembers)>0", con);
                            break;
                        case 4:
                            com = new SqlCommand("Select PathOfPhoto From Chat where idOfCreator=@id or CHARINDEX(cast(@id as varchar(max)),Chat.IdOfMembers)>0", con);
                            break;
                    }
                    com.Connection = con;
                    com.Parameters.AddWithValue("@id", ID);

                    //      com.Parameters.AddWithValue("@id", Key);//Key global tmp
                    con.Open();
                    reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        switch (j)
                        {
                            case 0:
                                Name.Add(reader.GetString(0));
                                break;
                            case 1:
                                id.Add(reader.GetInt32(0));
                                break;
                            case 2:
                                idOfCreator.Add(reader.GetInt32(0));
                                break;
                            case 3:
                                IdOfMembers.Add(reader.GetString(0));
                                break;
                            case 4:
                                Photo.Add(reader.GetString(0));
                                break;
                        }

                    }
                    con.Close();

            }
            for(int i=0;i< Name.Count; i++)
            AllChats.Add (new Chat(id[i], Name[i], idOfCreator[i], IdOfMembers[i], Photo[i]));
            return AllChats.ToArray();
        }


        public void EditChats(int ID,string IDMemb,string name,string photo)
        {

                com = new SqlCommand("EditChat", con);
                com.Connection = con;
                com.Parameters.AddWithValue("@IdOfMembers", IDMemb);
                com.Parameters.AddWithValue("@photo", photo);
                com.Parameters.AddWithValue("@Name", name);
                com.Parameters.AddWithValue("@id", ID);
                com.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                com.ExecuteNonQuery();

                con.Close();
        }
        public void newChat(string name, string photo,int ID, string IDMemb )
        {

            com = new SqlCommand("NewChat", con);
            com.Connection = con;
            com.Parameters.AddWithValue("@IdOfMembers", IDMemb);
            com.Parameters.AddWithValue("@photo", photo);
            com.Parameters.AddWithValue("@Name", name);
            com.Parameters.AddWithValue("@id", ID);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            con.Open();
            com.ExecuteNonQuery();

            con.Close();
        }
        public void Action(int id, int to, int TypeOfAction, bool AddOrRemove)//int Category- 1 isWrite, 2-  Friend, 3- in BlackList
        {
            SqlDataReader reader;

            string Field = "";
            string IdField = "";

            string QueryString = "";
            string QueryTmp = "";
            string QueryFunc = "";

            com = new SqlCommand(@"Select idOfPers from Relations where idOfOwner=@id", con);
            con.Open();
            com.Parameters.AddWithValue("@id", id);
            reader = com.ExecuteReader();
            while (reader.Read())
            {
                IdField = reader.GetString(0);
            }
            con.Close();
            //
            switch (TypeOfAction)
            {
                case 1:
                    QueryString = @"Select isWrite from Relations where idOfOwner=@id";
                    QueryTmp = "@isWriteField";
                    QueryFunc = "AddToCompanion";
                    break;
                case 2:
                    QueryString = @"Select isfriend from Relations where idOfOwner=@id";
                    QueryTmp = "@isfriendField";
                    QueryFunc = "AddToFriends";
                    break;
                case 3:
                    QueryString = @"Select isblackList from Relations where idOfOwner=@id";
                    QueryTmp = "@isblackListField";
                    QueryFunc = "AddToBlackList";
                    break;
            }

            com = new SqlCommand(QueryString, con);
            con.Open();
            com.Parameters.AddWithValue("@id", id);
            reader = com.ExecuteReader();
            while (reader.Read())
            {
                Field = reader.GetString(0);
            }
            con.Close();



            StringBuilder someString = new StringBuilder(Field);
            if (AddOrRemove)
                someString[IdField.IndexOf(Convert.ToString(to), StringComparison.CurrentCulture)] = '1';
            else
                someString[IdField.IndexOf(Convert.ToString(to), StringComparison.CurrentCulture)] = '0';
            Field = someString.ToString();

            com = new SqlCommand(QueryFunc, con);
            com.CommandType = System.Data.CommandType.StoredProcedure;

            SqlParameter param = com.Parameters.AddWithValue(QueryTmp, Field);
            param.Direction = System.Data.ParameterDirection.Input;

            param = com.Parameters.AddWithValue("@id", id);
            param.Direction = System.Data.ParameterDirection.Input;

            con.Open();
            com.ExecuteNonQuery();

            con.Close();
            isBothFieldWereChanged *= -1;
            if (isBothFieldWereChanged == 1)
                Action(to, id, TypeOfAction, AddOrRemove);
            else
            {
                isBothFieldWereChanged = -1;
            }
        }
        /*
        public void SaveFileToDatabase(string Path)//обычная таблица с полем варбинари
        {

            con.Open();
            com = new SqlCommand(@"INSERT INTO SendFile VALUES (@FileName,  @Content)");//(NewId(),@FileName,  @Content)
            com.Connection = con;
            //string Path = @"D:\Desktop\Null.png";
            string FileName = Path.Substring(Path.LastIndexOf('\\') + 1);
            byte[] imageData;// массив для хранения бинарных данных файла
            using (System.IO.FileStream fs = new System.IO.FileStream(Path, System.IO.FileMode.Open))
            {
                imageData = new byte[fs.Length];
                fs.Read(imageData, 0, imageData.Length);
            }

            com.Parameters.AddWithValue("@FileName", FileName);
            com.Parameters.AddWithValue("@Content", imageData);

            com.ExecuteNonQuery();
            con.Close();

        }



        public  void ReadFileFromDatabase(string path)
        {
            List<File> files = new List<File>();

            con.Open();
            com = new SqlCommand(@"SELECT * FROM SendFile");
            //com.CommandText = @"SELECT * FROM SendFile";
            com.Connection = con;

            SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string filename = reader.GetString(1);
                    byte[] data = (byte[])reader.GetValue(2);

                    File fl = new File(id, filename,  data);
                    files.Add(fl);
                }
            // сохраним первый файл из списка
            if (files.Count > 0)
            {
                using (System.IO.FileStream fs = new System.IO.FileStream(path +"/"+ files[0].Name, System.IO.FileMode.OpenOrCreate))//@"D:\Desktop\as\"
                {
                    fs.Write(files[0].Content, 0, files[0].Content.Length);
                    //Console.WriteLine("Изображение '{0}' сохранено", files[0].Name);
                }
            }
            con.Close();
        }

*/



        public void SaveFileToDatabase(string Path)//таблица файлТейбл
        {
            //You need to use the DynamicParameters class and specify the parameter's data type. For the example I created a table TEST with a column called Stream VARBINARY(MAX)
            /*
            using (var connection = new SqlConnection(Properties.Settings.Default.connectionString))
            {
                connection.Open();
                using (var fs = File.Open(Properties.Settings.Default.filePath, FileMode.Open))
                {
                    var sql = "INSERT INTO TEST (Stream) VALUES (@fs)";
                    var dParams = new DynamicParameters();
                    dParams.Add("@fs", fs, DbType.Binary);
                    connection.Execute(sql, dParams);
                }
            }
            */
            con.Open();
            com = new SqlCommand(@"INSERT INTO SendFile (file_stream, name) VALUES (@fs,@Nm)");//(NewId(),@FileName,  @Content)
            com.Connection = con;
            string FileName = Path.Substring(Path.LastIndexOf('\\') + 1);
            byte[] Content;// массив для хранения бинарных данных файла
            System.IO.FileStream fs = new System.IO.FileStream(Path, System.IO.FileMode.Open);
                Content = new byte[fs.Length];
                fs.Read(Content, 0, Content.Length);

            com.Parameters.AddWithValue("@Nm", FileName);
            com.Parameters.AddWithValue("@fs", Content);
            int n = 0;
            bool isSuccess=false;//Решает конфликт имен(если имена совпадают добавляет (n))
            while(!isSuccess)
            try
            {
            com.ExecuteNonQuery();
                isSuccess = true;
            }
            catch(System.Data.SqlClient.SqlException)
            {
                com.Parameters.Clear();
                com.Parameters.AddWithValue("@fs", Content);
                com.Parameters.AddWithValue("@Nm", ("(" + n + ")").ToString()+ FileName);//если имена файлов совпадают
                    n++;
            }
            con.Close();
        }


        /*
                public async override Task ProcessRequestAsync(HttpContext context)
        {
            using (StreamWriter sw = new StreamWriter(context.Response.OutputStream))
            {
                await sw.WriteLineAsync("<!DOCTYPE html><html><head></head><body><table>");

                using (var connection = new SqlConnection(@"Data Source =""(LocalDB)\TestInstance"""))
                {
                    await connection.OpenAsync().ConfigureAwait(false);

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "Select id, FirstName, LastName From [dbo].[Table];";

                        var reader = await command.ExecuteReaderAsync().ConfigureAwait(false);

                        while (await reader.ReadAsync().ConfigureAwait(false))
                        {
                            var id = reader.GetInt32(0);
                            var firstName = reader.GetString(1);
                            var lastName = reader.GetString(2);
                            var line = string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", id, firstName, lastName);
                            await sw.WriteLineAsync(line);
                        }
                    }
                }

                await sw.WriteLineAsync("</table></body></html>");
            }
        }

    */




        public  void DownloadFileFromDB(Guid StreamId, string path)
        {
            System.Diagnostics.Stopwatch stopwatch=new System.Diagnostics.Stopwatch();
            stopwatch.Start();
 
            List<Files> files = new List<Files>();//
            //await con.OpenAsync().ConfigureAwait(false);
            con.Open();
            com = new SqlCommand(@"SELECT * FROM SendFile where Stream_id=@SID");
            com.Connection = con;
            com.Parameters.AddWithValue(@"@SID", StreamId);

             //var reader = await com.ExecuteReaderAsync().ConfigureAwait(false);
            //while (await reader.ReadAsync().ConfigureAwait(false))
            //{

               SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                byte[] data = (byte[])reader.GetValue(1);
                string filename = reader.GetString(2);

                Files fl = new Files(StreamId, filename, data);
                files.Add(fl);//
            }
            try
            {
                System.IO.FileStream fs = new System.IO.FileStream(path + "/" + files[0].Name, System.IO.FileMode.OpenOrCreate);//@"D:\Desktop\as\"
                fs.Write(files[0].Content, 0, files[0].Content.Length);
            }
            catch(System.IO.IOException)
            {
             //   System.Windows.MessageBox.Show("This file aready downloaded!", "Warning", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
            catch (System.ArgumentOutOfRangeException)
            {
                System.Windows.MessageBox.Show("This file deleted from server!", "Warning", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
            con.Close();
            stopwatch.Stop();
            System.Windows.MessageBox.Show("The file successfully downloaded!", "Success");//\nIt took a"+ stopwatch.Elapsed.ToString()
            //System.Windows.MessageBox.Show(stopwatch.Elapsed.ToString(), "");
        }

        public void EditData(string Name,string Country,string About,int age,string photo,int ID)
        {
            com = new SqlCommand("EditData", con);
            com.Connection = con;
            com.Parameters.AddWithValue("@ID", ID);
            com.Parameters.AddWithValue("@FName", Name.Split(' ')[0]);
            com.Parameters.AddWithValue("@LName", Name.Split(' ')[1]);
            com.Parameters.AddWithValue("@Country", Country);
            com.Parameters.AddWithValue("@Description", About);
            com.Parameters.AddWithValue("@age", age);
            com.Parameters.AddWithValue("@photo", photo);

            com.CommandType = System.Data.CommandType.StoredProcedure;
            con.Open();
            com.ExecuteNonQuery();
            con.Close();


        }


        //_____
        public Guid GetStreamID(string path)
        {
            Guid StreamId = new Guid();
            con.Open();
            com = new SqlCommand(@"SELECT stream_id FROM SendFile where name=@Pt ");
            com.Connection = con;
            com.Parameters.AddWithValue("@Pt", path);

            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                StreamId = reader.GetGuid(0);
            }
            con.Close();
            return StreamId;
        }
        //______

        public byte[] GetPicture(Guid StreamId)
        {
            List<Files> files = new List<Files>();//
            //await con.OpenAsync().ConfigureAwait(false);
            con.Open();
            com = new SqlCommand(@"SELECT * FROM SendFile where Stream_id=@SID");
            com.Connection = con;
            com.Parameters.AddWithValue(@"@SID", StreamId);

            //var reader = await com.ExecuteReaderAsync().ConfigureAwait(false);
            //while (await reader.ReadAsync().ConfigureAwait(false))
            //{
            byte[] data=null;
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                 data = (byte[])reader.GetValue(1);
            }
            con.Close();
            return data;
        }






    }
}
