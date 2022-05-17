using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockCipher
{
    public class Skipjack : IBlockCipher
    {
        public long w, w1, w2, w3, w4;
        public long[] key;

        //Constructor
        public Skipjack(long w, long[] key)
        {
            this.w = w;
            this.w1 = (w >> (16 * 3)) & 0xFFFF;
            this.w2 = (w >> (16 * 2)) & 0xFFFF;
            this.w3 = (w >> (16 * 1)) & 0xFFFF;
            this.w4 = w & 0xFFFF;
            this.key = key;
        }

        //Encryption function
        public void Encryption(bool print)
        {
            for (int round = 1; round <= 33; round++)
            {
                if ((round >= 1 && round <= 8) || (round >= 17 && round <= 24))
                    this.RuleA(round);
                else if ((round >= 9 && round <= 16) || (round >= 25 && round <= 32))
                    this.RuleB(round);
                if (print == true)
                {
                    printRound();
                }
            }
        }

        //Decryption Function
        public void Decryption(bool print)
        {
            for (int round = 32; round > 0; round--)
            {
                if ((round >= 25 && round <= 32) || (round >= 9 && round <= 16))
                    this.Binv(round);
                else if ((round >= 17 && round <= 24) || (round >= 1 && round <= 8))
                    this.Ainv(round);
                if (print == true)
                {
                    printRound();
                }
            }
        }

        //Rule A Function
        public void RuleA(int round)
        {
            long c1 = w1;
            long c2 = w2;
            long c3 = w3;
            w1 = Gfunc(round, key, c1) ^ w4 ^ round;
            w2 = Gfunc(round, key, c1);
            w3 = c2;
            w4 = c3;
        }

        //Inverse of A for Decryption
        public void Ainv(int round)
        {
            long c1 = w1;
            long c2 = w2;
            w1 = Ginv(round, key, c2);
            w2 = w3;
            w3 = w4;
            w4 = c1 ^ c2 ^ round;
        }

        //Rule B Function
        public void RuleB(int round)
        {
            long c1 = w1;
            long c2 = w2;
            long c3 = w3;
            w1 = w4;
            w2 = Gfunc(round, key, c1);
            w3 = c1 ^ c2 ^ round;
            w4 = c3;
        }

        //Inverse of B for Decryption
        public void Binv(int round)
        {
            long c1 = w1;
            w1 = Ginv(round, key, w2);
            w2 = Ginv(round, key, w2) ^ w3 ^ round;
            w3 = w4;
            w4 = c1;

        }

        //G permutation
        public static long Gfunc(int round, long[] key, long w)
        {
            long[] g = new long[6];
            g[0] = (w >> 8) & 0xFF; //high byte
            g[1] = w & 0xFF;        //low byte
            int j = Mod((4 * (round - 1)), 10);

            for (int i = 2; i < 6; i++)
            {
                long input = g[i - 1] ^ key[j];
                g[i] = Fbox(input) ^ g[i - 2];
                j = Mod((j + 1), 10);
            }

            return (g[4] << 8) | g[5];
        }

        // Inverse of G for Decryption
        public static long Ginv(int round, long[] key, long w)
        {
            long[] g = new long[6];
            g[4] = (w >> 8) & 0xff;
            g[5] = w & 0xff;
            int j = (4 * (round - 1) + 3) % 10;

            for (int i = 3; i >= 0; i--)
            {
                long input = g[i + 1] ^ key[j];
                g[i] = Fbox(input) ^ g[i + 2];
                j = Mod((j - 1), 10);
            }

            return (g[0] << 8) | g[1];
        }

        // Modulo operation
        static int Mod(int a, int b)
        {
            return (Math.Abs(a * b) + a) % b;
        }

        //F Box
        public static long Fbox(long input)
        {
            long i = (input & 0xF0) >> 4;
            long j = input & 0x0F;
            long[,] fBox =
            {
                { 0xa3, 0xd7, 0x09, 0x83, 0xf8, 0x48, 0xf6, 0xf4, 0xb3, 0x21, 0x15, 0x78, 0x99, 0xb1, 0xaf, 0xf9 },
                { 0xe7, 0x2d, 0x4d, 0x8a, 0xce, 0x4c, 0xca, 0x2e, 0x52, 0x95, 0xd9, 0x1e, 0x4e, 0x38, 0x44, 0x28 },
                { 0x0a, 0xdf, 0x02, 0xa0, 0x17, 0xf1, 0x60, 0x68, 0x12, 0xb7, 0x7a, 0xc3, 0xc9, 0xfa, 0x3d, 0x53 },
                { 0x96, 0x84, 0x6b, 0xba, 0xf2, 0x63, 0x9a, 0x19, 0x7c, 0xae, 0xe5, 0xf5, 0xf7, 0x16, 0x6a, 0xa2 },
                { 0x39, 0xb6, 0x7b, 0x0f, 0xc1, 0x93, 0x81, 0x1b, 0xee, 0xb4, 0x1a, 0xea, 0xd0, 0x91, 0x2f, 0xb8 },
                { 0x55, 0xb9, 0xda, 0x85, 0x3f, 0x41, 0xbf, 0xe0, 0x5a, 0x58, 0x80, 0x5f, 0x66, 0x0b, 0xd8, 0x90 },
                { 0x35, 0xd5, 0xc0, 0xa7, 0x33, 0x06, 0x65, 0x69, 0x45, 0x00, 0x94, 0x56, 0x6d, 0x98, 0x9b, 0x76 },
                { 0x97, 0xfc, 0xb2, 0xc2, 0xb0, 0xfe, 0xdb, 0x20, 0xe1, 0xeb, 0xd6, 0xe4, 0xdd, 0x47, 0x4a, 0x1d },
                { 0x42, 0xed, 0x9e, 0x6e, 0x49, 0x3c, 0xcd, 0x43, 0x27, 0xd2, 0x07, 0xd4, 0xde, 0xc7, 0x67, 0x18 },
                { 0x89, 0xcb, 0x30, 0x1f, 0x8d, 0xc6, 0x8f, 0xaa, 0xc8, 0x74, 0xdc, 0xc9, 0x5d, 0x5c, 0x31, 0xa4 },
                { 0x70, 0x88, 0x61, 0x2c, 0x9f, 0x0d, 0x2b, 0x87, 0x50, 0x82, 0x54, 0x64, 0x26, 0x7d, 0x03, 0x40 },
                { 0x34, 0x4b, 0x1c, 0x73, 0xd1, 0xc4, 0xfd, 0x3b, 0xcc, 0xfb, 0x7f, 0xab, 0xe6, 0x3e, 0x5b, 0xa5 },
                { 0xad, 0x04, 0x23, 0x9c, 0x14, 0x51, 0x22, 0xf0, 0x29, 0x79, 0x71, 0x7e, 0xff, 0x8c, 0x0e, 0xe2 },
                { 0x0c, 0xef, 0xbc, 0x72, 0x75, 0x6f, 0x37, 0xa1, 0xec, 0xd3, 0x8e, 0x62, 0x8b, 0x86, 0x10, 0xe8 },
                { 0x08, 0x77, 0x11, 0xbe, 0x92, 0x4f, 0x24, 0xc5, 0x32, 0x36, 0x9d, 0xcf, 0xf3, 0xa6, 0xbb, 0xac },
                { 0x5e, 0x6c, 0xa9, 0x13, 0x57, 0x25, 0xb5, 0xe3, 0xbd, 0xa8, 0x3a, 0x01, 0x05, 0x59, 0x2a, 0x46 }
            };
            return fBox[i, j];
        }


        public long getResult()
        {
            return this.w1 << 3 * 16 | this.w2 << 2 * 16 | this.w3 << 1 * 16 | this.w4;
        }

        public void printRound()
        {
            Console.WriteLine(w1.ToString("X4") + " " + w2.ToString("X4") + " "
                            + w3.ToString("X4") + " " + w4.ToString("X4"));
        }
    }

    public class SkipjackMode:IMode
    {
        public List<Skipjack> inputList;
        public SkipjackMode(String input, long[] key)
        {
            this.inputList = new List<Skipjack>();
            Padding(ref input);
            for(int i = 0; i < input.Length; i += 16)
            {
                inputList.Add(new Skipjack(Convert.ToInt64(input.Substring(i, 16), 16), key));
            }
        }

        public void ECB()
        {
            for(int i = 0; i < inputList.Count; i++)
            {
                inputList[i].Encryption(false);
            }
        }

        public void CBC()
        {
            throw new NotImplementedException();
        }

        public void CFB()
        {
            throw new NotImplementedException();
        }

        public void OFB()
        {
            throw new NotImplementedException();
        }

        public void printResult()
        {
            String result = null;
            for(int i = 0; i < inputList.Count; i++)
            {
                result += inputList[i].getResult().ToString("X8");
            }
            Console.WriteLine(result);
        }

        static void Padding(ref String input)
        {
            String[] pads =
            {
                "00",
                "01",
                "0202",
                "030303",
                "04040404",
                "0505050505",
                "060606060606",
                "07070707070707",
                "0808080808080808"
            };

            //Assuming length is even
            int bytes = input.Length / 2;

            if ((bytes % 8 > 8 || bytes % 8 < 8) && bytes % 8 != 0)
            {
                input += pads[8 - bytes % 8];
            }
            else if (bytes % 8 == 0)
            {
                input += pads[8];
            }
        }
    }
    


}