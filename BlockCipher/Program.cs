using System;

namespace BlockCipher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //testSkipJack();
            testSkipJackECB();
        }

        static void testSkipJack()
        {
            String s = "33221100DDCCBBAA";
            String str = "00998877665544332211";
            long hex = Convert.ToInt64(s, 16);
            

            
            //Test Encryption
            Console.WriteLine("Encryption Process:");
            var encrypt = new Skipjack(hex, inputKey(str));
            encrypt.Encryption(true);
            Console.WriteLine("The resulting encrypted text is: " + encrypt.getResult().ToString("X16"));

            //Test Decryption
            Console.WriteLine("Decryption Process:");
            encrypt.Decryption(true);
            Console.WriteLine("The resulting decrypted text is: " + encrypt.getResult().ToString("X16"));
        }

        static void testSkipJackECB()
        {
            // Test Encryption in ECB
            String s = "33221100DDCCBBAA33221100DDCCBBAA";
            String str = "00998877665544332211";
            Console.WriteLine("Encryption Process:");
            var encrypt = new SkipjackMode(s, inputKey(str));

            encrypt.ECB();
            encrypt.printResult();
        }

        static long[] inputKey(String key)
        {
            long[] array = new long[10];
            int index = 0;

            for (int i = 0; i < key.Length; i += 2)
            {
                String hex = key.Substring(i, 2);
                array[index] = Convert.ToInt64(hex, 16);
                index++;
            }
            return array;
        }
    }

}
