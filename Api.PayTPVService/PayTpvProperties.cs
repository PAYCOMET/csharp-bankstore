using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiPayTPV
{
    public class PayTpvProperties
    {
        #region Authentication Properties
        public string MERCHANTCODE { get; set; }
        public string TERMINAL { get; set; }
        public string PASSWORD { get; set; }
        #endregion

        public string PAN { get; set; }
        public string EXPDATE { get; set; }
        public string CVV { get; set; }

    }

}
