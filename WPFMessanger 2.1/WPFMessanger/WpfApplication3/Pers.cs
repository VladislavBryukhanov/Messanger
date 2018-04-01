using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication3
{
    class Pers: IComparable
    {
        public int id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullNameToStr { get; set; }
        public Pers(int id, string FirstName, string LastName)
        {
            this.id = id;
            this.FirstName = FirstName;
            this.LastName = LastName;
            FullNameToStr = this.FirstName + " " + this.LastName;
        }
        public int CompareTo(object o)//сортироовка объектов класса по полю fullName
        {
            Pers p = o as Pers;
            if (p != null)
                return this.FullNameToStr.CompareTo(p.FullNameToStr);
            else
                throw new Exception("Невозможно сравнить два объекта");
        }
        //public string ToStr()
        //{
        //    return FirstName + " " + LastName;
        //}
    }
}
