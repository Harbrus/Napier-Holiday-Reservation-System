using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    /*
     * Author name: Andrea Silvestro Ortino
     * Business.Extra is the class holding request details from the Chains of Responsibility Pattern. Part of the properties will be modified accordingly to the description set.
     * Date last modified: 22/11/2017 16:32
     */

    [Serializable]
    public class Extra
    {
        private string _description;
        private double _cost = 0;
        private string _driverName;
        private DateTime _startHire;
        private DateTime _endHire;

        // Extra constructor used for create extra object with a specified description. 
        public Extra (string description)
        {
            this._description = description;
        }

        // Descritpion property with get/set method (with proper validation) to return and/or set the corrispective private attribute.
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                if(value.Length == 0)
                {
                    throw new ArgumentException("Extra optionality does not have a description.");
                }

                _description = value;
            }
        }

        // Cost property with get/set method to return and/or set the corrispective private attribute. This property will be modified by the Chain of Responsibility Pattern.
        public double Cost
        {
            get
            {
                return _cost;
            }
            set
            {
                _cost = value;
            }
        }

        // DriverName property with get/set method (with proper validation) to return and/or set the corrispective private attribute. It will be set only if Description = Car Hire.
        public string DriverName
        {
            get
            {
                return _driverName;
            }
            set
            {
                if(Description == "Car Hire" && value.Length == 0)
                {
                    throw new ArgumentException("Please, insert a driver name.");
                }

                _driverName = value;
            }
        }

        // StarHire property with get/set method (with proper validation) to return and/or set the corrispective private attribute. It will be set only if Description = Car Hire.
        public DateTime StartHire
        {
            get
            {
                return _startHire;
            }
            set
            {
                if (Description == "Car Hire" && (value == DateTime.MinValue || StartHire > EndHire))
                {
                    throw new ArgumentException("Insert a valid Start Hire Date, prior than your End Hire Date.");
                }
                _startHire = value;
            }
        }

        // EndHire property with get/set method (with proper validation) to return and/or set the corrispective private attribute. It will be set only if Description = Car Hire.
        public DateTime EndHire
        {
            get
            {
                return _endHire;
            }
            set
            {
                if (Description == "Car Hire" && value == DateTime.MinValue)
                {
                    throw new ArgumentException("Insert a valid End Hire Date, after you Start Hire Date.");
                }
                _endHire = value;
            }
        }
      
    }
}
