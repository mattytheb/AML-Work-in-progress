using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AML
{
    public class Subject
    {
        public string description;
        public Boolean isChecked;

        public Subject(string description)
        {
            this.description = description;
            this.isChecked = false;
        }

        public override bool Equals(object obj)
        {
            //return base.Equals(obj);
            return ((Subject)obj).description.Equals(this.description);
        }
    }

}
