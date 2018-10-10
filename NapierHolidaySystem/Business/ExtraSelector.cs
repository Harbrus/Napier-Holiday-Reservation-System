using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    /*
     * Author name: Andrea Silvestro Ortino
     * Business.ExtraSelector is a class which let the override of the Extra's Cost property if the child classes' conditions are met.
     * Date last modified: 22/11/2017 16:50
     * This class is the Handler class of the Design Patters Chain of Responsibility defining an interface for handling the requests
     * and implement the correct type of extra.
     */

    public abstract class ExtraSelector
    {
        protected ExtraSelector extraType;

        // This method set the type of selector that will be processed.
        public void SetExtra (ExtraSelector extraType)
        {
            this.extraType = extraType;
        }

        // This abstract method will be override by the child Concrete Handlers to process the Extra object.
        public abstract void ProcessExtra(Extra extra);
    }
}
