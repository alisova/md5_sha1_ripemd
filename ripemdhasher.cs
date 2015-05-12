using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace md5
{
    static class ripemdhasher
    {
        static public string Calculate(string fileName)
        {
            string res = "";
            BinaryReader sIn = new BinaryReader(File.Open(fileName, FileMode.Open, FileAccess.Read));
            const int SIZE_OF_BLOCK = 64;


            int file_size = (int)sIn.BaseStream.Position;
            if (file_size > 0)
            {
                uint[] R1 = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10,11, 12, 13, 14, 15,
                7, 4, 13, 1, 10, 6, 15, 3, 12, 0, 9, 5, 2, 14, 11, 8,
                3, 10, 14, 4, 9, 15, 8, 1, 2, 7, 0, 6, 13, 11, 5, 12,
                1, 9, 11, 10, 0, 8, 12, 4, 13, 3, 7, 15, 14, 5, 6, 2,
                4, 0, 5, 9, 7, 12, 2, 10, 14, 1, 3, 8, 11, 6, 15, 13};
                uint[] R2 = {5,14, 7, 0, 9, 2, 11, 4, 13, 6, 15, 8, 1, 10, 3, 12,
                        6, 11, 3, 7, 0, 13, 5, 10, 14, 15, 8, 12, 4, 9, 1, 2,
                        15, 5, 1, 3, 7, 14, 6, 9, 11, 8, 12, 2, 10, 0, 4, 13,
                        8, 6, 4, 1, 3, 11, 15, 0, 5, 12, 2, 13, 9, 7, 10, 14,
                        12, 15, 10, 4, 1, 5, 8, 7, 6, 2, 13, 14, 0, 3, 9, 11};

                uint[] S1 = {11, 14, 15, 12, 5, 8, 7,9, 11, 13, 14, 15, 6, 7, 9, 8,
                        7, 6, 8, 13, 11, 9, 7, 15, 7, 12, 15, 9, 11, 7, 13, 12,
                        11, 13, 6, 7, 14, 9, 13, 15, 14, 8, 13, 6, 5, 12, 7, 5,
                        11, 12, 14, 15, 14, 15, 9, 8, 9, 14, 5, 6, 8, 6, 5, 12,
                        9, 15, 5, 11, 6, 8, 13, 12, 5, 12, 13, 14, 11, 8, 5, 6};
                uint[] S2 = {8, 9, 9, 11, 13, 15, 15, 5,7, 7, 8, 11, 14, 14, 12, 6,
                        9, 13, 15, 7, 12, 8, 9, 11, 7, 7, 12, 7, 6, 15, 13, 11,
                        9, 7, 15, 11, 8, 6, 6, 14, 12, 13, 5, 14, 13, 13, 7, 5,
                        15, 5, 8, 11, 14, 14, 6, 14, 6, 9, 12, 9, 12, 5, 15, 8,
                        8, 5, 12, 9, 12, 5, 14, 6, 8, 13, 6, 5, 15, 13, 11, 11};

                uint h0 = 0x67452301, h1 = 0xEFCDAB89, h2 = 0x98BADCFE, h3 = 0x10325476, h4 = 0xC3D2E1F0;
                uint[,] x = new uint[4, 4];
                return res;

            }
            else
            {
                sIn.Close();
                return "-2";
            }
        }



        static uint F(uint j, uint x, uint y, uint z)
        {
            if (j <= 15)
            {
                return x ^ y ^ z;
            }
            else if (j <= 31)
            {
                return (x & y) | (~x & z);
            }
            else if (j <= 47)
            {
                return (x | ~y) ^ z;
            }
            else if (j <= 63)
            {
                return (x & z) | (y & ~z);
            }
            else if (j <= 79)
            {
                return x ^ (y | ~z);
            }
            else
                return 0;
        }

        static uint K1(uint j)
        {
            if (j <= 15)
            {
                return 0x00000000;
            }
            else if (j <= 31)
            {
                return 0x5A827999;
            }
            else if (j <= 47)
            {
                return 0x6ED9EBA1;
            }
            else if (j <= 63)
            {
                return 0x8F1BBCDC;
            }
            else if (j <= 79)
            {
                return 0xA953FD4E;
            }
            else
                return 0;
        }
        static uint K2(uint j)
        {
            if (j <= 15)
            {
                return 0x50A28BE6;
            }
            else if (j <= 31)
            {
                return 0x5C4DD124;
            }
            else if (j <= 47)
            {
                return 0x6D703EF3;
            }
            else if (j <= 63)
            {
                return 0x7A6D76E9;
            }
            else if (j <= 79)
            {
                return 0x00000000;
            }
            else
                return 0;
        }

        static uint fourBytesToUint(string array)
        {
            uint res = 0;
            res |= ((uint)array[3] << 24) & 0xFF000000;
            res |= ((uint)array[2] << 16) & 0xFF0000;
            res |= ((uint)array[1] << 8) & 0xFF00;
            res |= ((uint)array[0] << 0) & 0xFF;
            return res;
        }
        static uint messageExtension(string theMessage)
        {
            uint bitSizeOriginMessage = (uint)theMessage.Length << 3;

            uint blocks = (uint)(theMessage.Length / 64) + 1;

            uint[] X = new uint[blocks];
            for (int i = 0; i < blocks; i++)
            {
                for (int j = 0; j < (i == blocks - 1 ? 14 : 16); j++)
                    X[j] = fourBytesToUint(theMessage);

                if (i == blocks - 1)
                {
                    X[14] = bitSizeOriginMessage & 0xFFFFFFFF;
                    X[15] = bitSizeOriginMessage >> 32 & 0xFFFFFFFF;
                }
            }

            return blocks;
        }

        static uint inv(uint a)
        {
            uint b = 0;
            b |= ((a >> 0) & 0xFF) << 24;
            b |= ((a >> 8) & 0xFF) << 16;
            b |= ((a >> 16) & 0xFF) << 8;
            b |= ((a >> 24) & 0xFF) << 0;
            return b;
        }


    }
}
