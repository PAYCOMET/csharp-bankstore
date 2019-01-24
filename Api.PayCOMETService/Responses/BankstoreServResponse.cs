using System.Collections.Generic;

namespace Api.PayCOMETService.Responses
{
    public class BankstoreServResponse
    {
        public string Result { get; set; }
        public string DsErrorId { get; set; }
        public Dictionary<string, string> Data { get; set; }

        public BankstoreServResponse()
        {
            DsErrorId = "0";
        }
    }
}
