using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace md5
{
    static class md5hasher
    {
        static public string Calculate(string fileName)
        {
            BinaryReader sIn = new BinaryReader(File.Open(fileName,FileMode.Open,FileAccess.Read));

            int length=(int)sIn.BaseStream.Position; //получаем длину входного сообщения.

            if (length > 0)
            {
                int rests = length % 64; //остаток от деления на 64байта.
                int size = 0; //тут будет храниться размер сообщения после первых 2ух шагов.

                //Шаг 1.
                if (rests < 56) //если остаток от деления на 64 меньше 56
                    size = length - rests + 56 + 8; //подгоняем размер так, что бы он был кратен 64(+8 байт для 2ого шага).
                else //иначе (если остаток больше 56)
                    size = length + 64 - rests + 56 + 8; //подгоняем размер так, что бы он был кратен 64(+8 байт для 2ого шага).

                char[] cIn = new char[size]; //создаем динамический массив для хранения сообщения, которое далее будет кодироваться

                for (int i = 0; i < length; i++) //первые length элементов сIn
                    cIn[i] = sIn.ReadChar(); //заполняем символами входного сообщения



                for (int i = length + 1; i < size; i++) //а все остальное
                    cIn[i] = '0'; //заполняем нулями

                //Шаг 2.
                Int64 bitLength = (uint)(length) * 8; //длина сообщения в битах.

                for (int i = 0; i < 8; i++) //последние 8 байт
                    cIn[size - 8 + i] = (char)(bitLength >> i * 8); //заполняем 64-битным представлением длины данных до выравнивания

                //Шаг 3.
                uint A = 0x67452301, //Инициализируем начальные значения регистров.
                        B = 0xefcdab89,
                        C = 0x98badcfe,
                        D = 0x10325476;

                uint[] T = new uint[64]; //64-элементная таблица данных (констатнт).

                for (int i = 0; i < 63; i++) //всю таблицу констант
                    T[i] = Convert.ToUInt32(Math.Pow(2, 32) * Math.Abs(Math.Sin(i))); //заполняем в соответствии с алгоритмом.

                uint[] X = new uint[size / 4]; //создаем массив Х, в котором будет 32-разрядное представление сообщения.

                //загоняем в массив Х сообщение cIn(в данном случае оно само разбиваеться на 32-разрядные слова).


                for (int i = 0; i < size / 4; i++)
                    X[i] = (uint)(cIn[i * 4 + 3] << 24) + (uint)(cIn[i * 4 + 2] << 16) + (uint)(cIn[i * 4 + 1] << 8) + (uint)(cIn[i * 4 + 0]);

                //Шаг 4.
                uint AA, BB, CC, DD;

                for (int i = 0; i < size / 4; i += 16)
                {
                    AA = A; BB = B; CC = C; DD = D;

                    //раунд 1
                    A = B + RotateLeft((A + F(B, C, D) + X[i + 0] + T[1]), 7);
                    D = A + RotateLeft((D + F(A, B, C) + X[i + 1] + T[2]), 12);
                    C = D + RotateLeft((C + F(D, A, B) + X[i + 2] + T[3]), 17);
                    B = C + RotateLeft((B + F(C, D, A) + X[i + 3] + T[4]), 22);

                    A = B + RotateLeft((A + F(B, C, D) + X[i + 4] + T[5]), 7);
                    D = A + RotateLeft((D + F(A, B, C) + X[i + 5] + T[6]), 12);
                    C = D + RotateLeft((C + F(D, A, B) + X[i + 6] + T[7]), 17);
                    B = C + RotateLeft((B + F(C, D, A) + X[i + 7] + T[8]), 22);

                    A = B + RotateLeft((A + F(B, C, D) + X[i + 8] + T[9]), 7);
                    D = A + RotateLeft((D + F(A, B, C) + X[i + 9] + T[10]), 12);
                    C = D + RotateLeft((C + F(D, A, B) + X[i + 10] + T[11]), 17);
                    B = C + RotateLeft((B + F(C, D, A) + X[i + 11] + T[12]), 22);

                    A = B + RotateLeft((A + F(B, C, D) + X[i + 12] + T[13]), 7);
                    D = A + RotateLeft((D + F(A, B, C) + X[i + 13] + T[14]), 12);
                    C = D + RotateLeft((C + F(D, A, B) + X[i + 14] + T[15]), 17);
                    B = C + RotateLeft((B + F(C, D, A) + X[i + 15] + T[16]), 22);

                    //раунд 2
                    A = B + RotateLeft((A + G(B, C, D) + X[i + 1] + T[17]), 5);
                    D = A + RotateLeft((D + G(A, B, C) + X[i + 6] + T[18]), 9);
                    C = D + RotateLeft((C + G(D, A, B) + X[i + 11] + T[19]), 14);
                    B = C + RotateLeft((B + G(C, D, A) + X[i + 0] + T[20]), 20);

                    A = B + RotateLeft((A + G(B, C, D) + X[i + 5] + T[21]), 5);
                    D = A + RotateLeft((D + G(A, B, C) + X[i + 10] + T[22]), 9);
                    C = D + RotateLeft((C + G(D, A, B) + X[i + 15] + T[23]), 14);
                    B = C + RotateLeft((B + G(C, D, A) + X[i + 4] + T[24]), 20);

                    A = B + RotateLeft((A + G(B, C, D) + X[i + 9] + T[25]), 5);
                    D = A + RotateLeft((D + G(A, B, C) + X[i + 14] + T[26]), 9);
                    C = D + RotateLeft((C + G(D, A, B) + X[i + 3] + T[27]), 14);
                    B = C + RotateLeft((B + G(C, D, A) + X[i + 8] + T[28]), 20);

                    A = B + RotateLeft((A + G(B, C, D) + X[i + 13] + T[29]), 5);
                    D = A + RotateLeft((D + G(A, B, C) + X[i + 2] + T[30]), 9);
                    C = D + RotateLeft((C + G(D, A, B) + X[i + 7] + T[31]), 14);
                    B = C + RotateLeft((B + G(C, D, A) + X[i + 12] + T[32]), 20);

                    //раунд 3
                    A = B + RotateLeft((A + H(B, C, D) + X[i + 5] + T[33]), 4);
                    D = A + RotateLeft((D + H(A, B, C) + X[i + 8] + T[34]), 11);
                    C = D + RotateLeft((C + H(D, A, B) + X[i + 11] + T[35]), 16);
                    B = C + RotateLeft((B + H(C, D, A) + X[i + 14] + T[36]), 23);

                    A = B + RotateLeft((A + H(B, C, D) + X[i + 1] + T[37]), 4);
                    D = A + RotateLeft((D + H(A, B, C) + X[i + 4] + T[38]), 11);
                    C = D + RotateLeft((C + H(D, A, B) + X[i + 7] + T[39]), 16);
                    B = C + RotateLeft((B + H(C, D, A) + X[i + 10] + T[40]), 23);

                    A = B + RotateLeft((A + H(B, C, D) + X[i + 13] + T[41]), 4);
                    D = A + RotateLeft((D + H(A, B, C) + X[i + 0] + T[42]), 11);
                    C = D + RotateLeft((C + H(D, A, B) + X[i + 3] + T[43]), 16);
                    B = C + RotateLeft((B + H(C, D, A) + X[i + 6] + T[44]), 23);

                    A = B + RotateLeft((A + H(B, C, D) + X[i + 9] + T[45]), 4);
                    D = A + RotateLeft((D + H(A, B, C) + X[i + 12] + T[46]), 11);
                    C = D + RotateLeft((C + H(D, A, B) + X[i + 15] + T[47]), 16);
                    B = C + RotateLeft((B + H(C, D, A) + X[i + 2] + T[48]), 23);

                    //раунд 4
                    A = B + RotateLeft((A + I(B, C, D) + X[i + 0] + T[49]), 6);
                    D = A + RotateLeft((D + I(A, B, C) + X[i + 7] + T[50]), 10);
                    C = D + RotateLeft((C + I(D, A, B) + X[i + 14] + T[51]), 15);
                    B = C + RotateLeft((B + I(C, D, A) + X[i + 5] + T[52]), 21);

                    A = B + RotateLeft((A + I(B, C, D) + X[i + 12] + T[53]), 6);
                    D = A + RotateLeft((D + I(A, B, C) + X[i + 3] + T[54]), 10);
                    C = D + RotateLeft((C + I(D, A, B) + X[i + 10] + T[55]), 15);
                    B = C + RotateLeft((B + I(C, D, A) + X[i + 1] + T[56]), 21);

                    A = B + RotateLeft((A + I(B, C, D) + X[i + 8] + T[57]), 6);
                    D = A + RotateLeft((D + I(A, B, C) + X[i + 15] + T[58]), 10);
                    C = D + RotateLeft((C + I(D, A, B) + X[i + 6] + T[59]), 15);
                    B = C + RotateLeft((B + I(C, D, A) + X[i + 13] + T[60]), 21);

                    A = B + RotateLeft((A + I(B, C, D) + X[i + 4] + T[61]), 6);
                    D = A + RotateLeft((D + I(A, B, C) + X[i + 11] + T[62]), 10);
                    C = D + RotateLeft((C + I(D, A, B) + X[i + 2] + T[63]), 15);
                    B = C + RotateLeft((B + I(C, D, A) + X[i + 9] + T[64]), 21);

                    A = AA + A;
                    B = BB + B;
                    C = CC + C;
                    D = DD + D;
                }

                //Шаг 5.
                String oust = ToHex(A) + ToHex(B) + ToHex(C) + ToHex(D); //заполняем выходную строку hex-//представлением, полученных в шаге 4, регистров.
                sIn.Close();
                return oust;
            }
            else
            {
                sIn.Close();
                return "-1";
            }

        }

        static uint F(uint X, uint Y, uint Z)
        {
            return (X & Y) | ((~X) & Z);
        }
        static uint G(uint X, uint Y, uint Z)
        {
            return (X & Z) | (Y & (~Z));
        }
        static uint H(uint X, uint Y, uint Z)
        {
            return X ^ Y ^ Z;
        }
        static uint I(uint X, uint Y, uint Z)
        {
            return Y ^ (X | (~Z));
        }
        static uint RotateLeft(uint value, int shift)
        {
            return value << shift | value >> (32 - shift);
        }
        static String ToHex(uint value)
        {
            String ouat = ""; //строка в которой будет храниться результат.
            //в переменной inHex будут храниться каждые 2 символа(в hex) входного значения.
            char inHex;
 
            while(value >= 0) //пока входное значение не станет равным нулю...
             {
             inHex=Convert.ToChar(value%256); //...получаем dec-представление 2ух символов(поэтому *256, а не на 16).
             ouat+=Convert.ToChar(inHex); //прибавляем к результирующей строке hex-представление
                                                        // значения, полученного в строке выше.                     
             value=value/256; //переходим к следующим 2ум символам(в dec-представлении).
             }
 
            return ouat; //возвращаем результирующую строку.
        }

    }
}
