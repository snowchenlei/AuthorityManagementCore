using log4net.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Common.Logger
{
    public class CustomerPatternLayout : PatternLayout
    {
        public CustomerPatternLayout()
        {
            this.AddConverter("Property", typeof(CustomerPatternConvert));
        }
    }
}
