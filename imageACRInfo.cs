using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace azFunctions
{
    
    public class imageACRInfo
    {
        public Guid Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Action { get; set; }
        public string LoginServer { get; set; }
        public string Repository { get; set; }
        public string Image { get; set; }
        public string Tag { get; set; }        

    }
}
