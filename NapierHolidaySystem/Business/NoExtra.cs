using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    /*
     * Author name: Andrea Silvestro Ortino
     * Business.NoExtra is a class which override the Extra's Cost property if the condition is met.
     * Date last modified: 22/11/2017 16:28
     * This class is a ConcreteHandler, child of the Handler class "ExtraSelector", part of the Design Patters Chain of Responsibility
     * which set the correct cost if the Extra class object has "No Extra" as a Description.
     */

    public class NoExtra : ExtraSelector
    {
        // This method override the parent's method. It first check if the Extra's Description property is equal to No Extra and then set the appropriate cost.
        public override void ProcessExtra(Extra extra)
        {
            if (extra.Description == "No Extra")
            {
                extra.Cost = 0;
            }
            else if (extraType != null)
            {
                extraType.ProcessExtra(extra);
            }
        }

    }
}
