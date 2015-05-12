using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace md5
{
    static class sha1hasher
    {
        static int shl(int a, int s)
        {
            return ((a<<s)|(a>>(32-s)));
        }

       static void hash_one(int[] W, int H0, int H1,
                        int H2, int H3, int H4)
        {
            
            int a, b, c, d, e, f = 0, k = 0, temp;

            
            for(int i = 16; i < 80; i++) {
                W[i] = shl((W[i - 3] ^ W[i - 8] ^ W[i - 14] ^ W[i - 16]), 1);
            }

            a = H0;
            b = H1;
            c = H2;
            d = H3;
            e = H4;

            for(int i = 0; i < 80; i++) {
                if (i >= 0 && i < 20) {
                    f = (b & c) | ((~ b) & d);
                    k = 0x5A827999;
                }
                else if (i >= 20 && i < 40) {
                    f = b ^ c ^ d;
                    k = 0x6ED9EBA1;
                }
                else if (i >= 40 && i < 60) {
                    f = (b & c) | (b & d) | (c & d);
                    k = Convert.ToInt32(0x8F1BBCDC);
                }
                else if (i >= 60 && i < 80) {
                    f = b ^ c ^ d;
                    k = Convert.ToInt32(0xCA62C1D6);
                }

                temp = shl(a, 5) + f + e + k + W[i];
                e = d;
                d = c;
                c = shl(b, 30);
                b = a;
                a = temp;
            }
            H0 += a;
            H1 += b;
            H2 += c;
            H3 += d;
            H4 += e;
        }

        static public String sha1(string fileName)
        {
            
            BinaryReader sIn = new BinaryReader(File.Open(fileName,FileMode.Open,FileAccess.Read));
            const int SIZE_OF_BLOCK = 64;

            
            int file_size = (int)sIn.BaseStream.Position;
            if (file_size > 0)
            {

                int last_size = file_size % SIZE_OF_BLOCK;

                int zero_size;
                if (last_size < 56) zero_size = 56 - last_size;
                else zero_size = SIZE_OF_BLOCK - last_size + 56;


                int num_of_blocks = (file_size + zero_size + 8) / SIZE_OF_BLOCK;


                int blocks_mod = 1;
                if (last_size >= 56)
                    blocks_mod++;


                int H0 = 0x67452301;
                int H1 = Convert.ToInt32(0xefcdab89);
                int H2 = Convert.ToInt32(0x98badcfe);
                int H3 = 0x10325476;
                int H4 = Convert.ToInt32(0xc3d2e1f0);


                int[] R = new int[80];
                char[] b = new char[64];
                for (int i = 0; i < num_of_blocks - blocks_mod; i++)
                {

                    b[i] = sIn.ReadChar();
                    for (int j = 0; j < 16; j++)
                    {
                        R[j] = 256 * 256 * 256 * b[4 * j] + 256 * 256 * b[4 * j + 1] + 256 * b[4 * j + 2] + b[4 * j + 3];
                    }
                    hash_one(R, H0, H1, H2, H3, H4);
                }

                if (last_size < 56)
                {

                    char[] buf = new char[64];



                    int it = last_size;
                    buf[it++] = (char)0x80;
                    for (int i = 0; i < zero_size - 1; buf[it++] = '0', i++) ;


                    int bit_file_size = (file_size * 8);
                    for (int i = 0; i < 8; i++)
                    {
                        buf[it++] = Convert.ToChar(bit_file_size & 0xFF);
                        bit_file_size /= 256;
                    }


                    for (int i = 0; i < 16; i++)
                    {
                        R[i] = 256 * 256 * 256 * buf[4 * i] + 256 * 256 * buf[4 * i + 1] + 256 * buf[4 * i + 2] + buf[4 * i + 3];
                    }
                    hash_one(R, H0, H1, H2, H3, H4);
                }
                else
                {

                    char[] buf = new char[128];




                    int it = last_size;
                    buf[it++] = (char)0x80;
                    for (int i = 0; i < zero_size - 1; buf[it++] = '0', i++) ;


                    int bit_file_size = Convert.ToChar(file_size * 8);
                    for (int i = 0; i < 8; i++)
                    {
                        buf[it++] = Convert.ToChar(bit_file_size & 0xFF);
                        bit_file_size /= 256;
                    }


                    for (int i = 0; i < 16; i++)
                    {
                        R[i] = 256 * 256 * 256 * buf[4 * i] + 256 * 256 * buf[4 * i + 1] + 256 * buf[4 * i + 2] + buf[4 * i + 3];
                    }
                    hash_one(R, H0, H1, H2, H3, H4);


                    for (int i = 16; i < 32; i++)
                    {
                        R[i - 16] = 256 * 256 * 256 * buf[4 * i] + 256 * 256 * buf[4 * i + 1] + 256 * buf[4 * i + 2] + buf[4 * i + 3];
                    }
                    hash_one(R, H0, H1, H2, H3, H4);
                }

                String str = Convert.ToString(H0) + Convert.ToString(H1) + Convert.ToString(H2) + Convert.ToString(H3) + Convert.ToString(H4);
                sIn.Close();
                return str;

            }
            else
            {
                sIn.Close();
                return "-3";
            }
        }
    }
}
