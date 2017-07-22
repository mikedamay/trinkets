using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drive
{
    internal interface Platform
    {    
       string GetHomeDirectoryName();
    }
    internal class WindowsPlatform : Platform
    {
        public string GetHomeDirectoryName()
        {
            return "";
        }
    }
}
