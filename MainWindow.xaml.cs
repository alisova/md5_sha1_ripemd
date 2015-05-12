using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Windows;

namespace md5
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string fileName;

        public MainWindow()
        {
            InitializeComponent();
        }

        
        private void TextBox_filename_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            App.MyDelegateEvent += new MyDelegate(my_MyDelegateEvent);

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            if (dlg.ShowDialog() == true)
            {
                fileName = dlg.FileName;
                TextBox_filename.Text = fileName;

                string s = md5hasher.Calculate(fileName);
                if (s == "-1")
                    App.ErrorMessage(fileName,s);
                else
                    TextBox_md5.Text = s;

                s = sha1hasher.sha1(fileName);
                if (s == "-2")
                    App.ErrorMessage(fileName, s);
                else
                    TextBox_sha1.Text = s;


                //TextBox_sha1.Text = sha1hasher.Calculate(fileName);
                //TextBox_ripemd.Text = ripemdhasher.Calculate(fileName);

                
                

            }
        }

        void my_MyDelegateEvent(string s,string s1, string s2)
        {
            TextBox_md5.Text = s;
            TextBox_sha1.Text = s1;
            TextBox_ripemd.Text = s2;
        }
    }
}
