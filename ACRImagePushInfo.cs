using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace azFunctions
{
    // Reference document for the ACR webhook payload: https://docs.microsoft.com/en-us/azure/container-registry/container-registry-webhook-reference
    // https://github.com/MicrosoftDocs/azure-docs/blob/main/articles/container-registry/container-registry-webhook-reference.md
    public class ACRImagePushInfo
    {        
        public string plID { get; set; }
        public string plTimestamp { get; set; }
        public string plAction { get; set; }
        public string pltgMediaType { get; set; }
        public int pltgSize { get; set; }
        public string pltgDigest { get; set; }
        public int pltgLength { get; set; }
        public string pltgRepository { get; set; }
        public string pltgTag { get; set; }


        public string reqID { get; set; }
        public string reqHost { get; set; }
        public string reqMethod { get; set; }
        public string reqUserAgent { get; set; }

    }
}
