using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace md5
{
   public delegate void MyDelegate(string s,string s1, string s3); // Делегат
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        
        static public event MyDelegate MyDelegateEvent; // Ваш евент

        static public void ErrorMessage(string fileName, string s)
        {
            try
            {
                StringBuilder sBuilder_md5 = new StringBuilder();
                StringBuilder sBuilder_sha1 = new StringBuilder();
                StringBuilder sBuilder_ripemd = new StringBuilder();

                if (s == "-1")
                {
                    #region MD5

                    MD5 md5Hasher = MD5.Create();
                    Stream stream = File.Open(fileName, FileMode.Open, FileAccess.Read);
                    byte[] data_md5 = md5Hasher.ComputeHash(stream);

                    
                    // Преобразуем каждый байт хэша в шестнадцатеричную строку
                    for (int i = 0; i < data_md5.Length; i++)
                    {
                        //указывает, что нужно преобразовать элемент в шестнадцатиричную строку длиной в два символа
                        sBuilder_md5.Append(data_md5[i].ToString("X2"));
                    }
                    stream.Close();
                    MyDelegateEvent(sBuilder_md5.ToString(), "", "");
                    #endregion
                


                    #region SHA1
                    SHA1 sha1Hasher = SHA1.Create();
                    Stream stream1 = File.Open(fileName, FileMode.Open, FileAccess.Read);
                    byte[] data_sha1 = sha1Hasher.ComputeHash(stream1);

                    
                    // Преобразуем каждый байт хэша в шестнадцатеричную строку
                    for (int i = 0; i < data_sha1.Length; i++)
                    {
                        //указывает, что нужно преобразовать элемент в шестнадцатиричную строку длиной в два символа
                        sBuilder_sha1.Append(data_sha1[i].ToString("X2"));
                    }
                    stream1.Close();
                    MyDelegateEvent(sBuilder_md5.ToString(), sBuilder_sha1.ToString(), "");
                    #endregion

                    #region ripemd
                    RIPEMD160 ripemdHasher = RIPEMD160.Create();
                    Stream stream3 = File.Open(fileName, FileMode.Open, FileAccess.Read);
                    byte[] data_ripemd = ripemdHasher.ComputeHash(stream3);

                    
                    // Преобразуем каждый байт хэша в шестнадцатеричную строку
                    for (int i = 0; i < data_ripemd.Length; i++)
                    {
                        //указывает, что нужно преобразовать элемент в шестнадцатиричную строку длиной в два символа
                        sBuilder_ripemd.Append(data_ripemd[i].ToString("X2"));
                    }
                    stream3.Close();
                    MyDelegateEvent(sBuilder_md5.ToString(), sBuilder_sha1.ToString(), sBuilder_ripemd.ToString());
                    #endregion
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(String.Format("Невозможно открыть файл: {0}\n{1}", fileName, e.Message));
            }
        }
    }
}
