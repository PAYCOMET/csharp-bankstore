using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiPayTPV
{
    public class ApiResponse
    {
        public string RESULT { get; set; }
        public string DS_ERROR_ID { get; set; }
        public Dictionary<string, object> data { get; set; }
    }
}
