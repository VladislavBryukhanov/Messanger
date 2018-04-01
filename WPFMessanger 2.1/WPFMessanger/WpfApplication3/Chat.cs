using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication3
{

 public   class Chat
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string PathToPhoto { get; set; }
        public int IdOfCreator { get; set; }
        public List<int> Members { get; set; }
        public Chat(int ID, string Name,int IdOfCreator,string Members,string PathToPhoto)
        {
            this.ID = ID;
            this.Name = Name;
            this.PathToPhoto = PathToPhoto;
            this.IdOfCreator = IdOfCreator;
            this.Members = new List<int>();
            string[] ArrOfMembers = Members.Split(' ');
            for(int i=0;i< ArrOfMembers.Length;i++)
            this.Members.Add(Convert.ToInt32(ArrOfMembers[i]));
        }
    }
}
