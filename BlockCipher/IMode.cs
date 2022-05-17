using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockCipher
{
    internal interface IMode
    {
        void ECB();
        void OFB();
        void CFB();
        void CBC();
    }
}
