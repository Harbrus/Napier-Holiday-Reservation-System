using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    /*
     * Author name: Andrea Silvestro Ortino
     * Business.Guest is a class which helds guest properties/attributes
     * Date last modified: 22/11/2017 16:43
     */ 

    [Serializable]
    public class Guest
    {
        private string _name;
        private string _passportNo;
        private int _age;

        // Guest constructor which takes name, passportNo and age as a parameters in order to instantiate an object.
        public Guest(string name, string passportNo, int age)
        {
            this._name = name;
            this._passportNo = passportNo;
            this._age = age;
        }

        // Name property with get/set method (with proper validation check) to return and/or set the corrispective private attribute.
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value.Length == 0 || value.Length > 10)
                {
                    throw new ArgumentException("Insert a valid passport number.");
                }
                this._name = value;
            }
        }

        // PassportNo property with get/set method (with proper validation check) to return and/or set the corrispective private attribute.
        public String PassportNo
        {
            get
            {
                return _passportNo;
            }
            set
            {
                if (value.Length == 0 || value.Length > 10)
                {
                    throw new ArgumentException("Insert a valid passport number.");
                }
                this._passportNo = value;
            }
        }

        // Age property with get/set method (with proper validation check) to return and/or set the corrispective private attribute.
        public int Age
        {
            get
            {
                return _age;
            }
            set
            {
                if(value < 0 || value > 101)
                {
                    throw new ArgumentException("Insert a valid age between 0 and 101.");
                }
            }
        }

        // This method override the toString method in order to print a guest object directly with the return of this method.
        public override string ToString()
        {
            return "Guest name: " + Name + ", Passport Number: " + PassportNo + ", Age: " + Age.ToString();
        }
    }
}
