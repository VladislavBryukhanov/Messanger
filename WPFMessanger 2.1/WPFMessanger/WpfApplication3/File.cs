using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication3
{
    class Files
    {
        public Guid streamID { get; set; }
        public string Name { get; set;  }
        public byte[]  Content{ get; set;}
        public Files(Guid streamID, string Name,byte[] Content)
        {
            this.streamID = streamID;
            this.Name = Name;
            this.Content = Content;
        }


    }
}
