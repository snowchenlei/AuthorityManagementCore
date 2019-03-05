using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net.Layout;

namespace Snow.AuthorityManagement.Web.Library
{
    public class CustomerPatternLayout : PatternLayout
    {
        public CustomerPatternLayout()
        {
            this.AddConverter("Property", typeof(CustomerPatternConvert));
        }
    }
}