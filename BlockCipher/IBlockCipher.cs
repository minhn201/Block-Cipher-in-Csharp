using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockCipher
{
    internal interface IBlockCipher
    {
        void Encryption(bool print);
        void Decryption(bool print);
        long getResult();
        void printRound();

    }
}
