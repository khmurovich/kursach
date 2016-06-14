using System;
using System.Threading;
using System.Windows.Forms;
using System.Net.Sockets;

namespace Client
{
    public partial class Form1 : Form
    {
        int port=13000;
        string address;
        bool flag;
        string[] duplicateFiles;
        string path;
        public Form1()
        {
            InitializeComponent();

        }
        private void GetFiles(object state)
        {

            try
            {
                Client client = new Client(address, port);
                duplicateFiles= client.GetDuplicateFiles(path);
                checkedListBox1.Items.AddRange(duplicateFiles);
                
                flag = false;
            }
            catch(SocketException)
            {
                MessageBox.Show("Неверный IP адрес или имя сервера!");
                flag = false;
            }
            catch(DuplicateFilesNotFound)
            {
                MessageBox.Show("Дублирующиеся файлы не найдены!");
                flag = false;
            }
            catch (IncorrectPath)
            {
                MessageBox.Show("Некорректный путь!");
                flag = false;

            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == String.Empty || textBox3.Text == String.Empty)
            {
                MessageBox.Show("Поля не заполнены!");
                return;
            }
            address = textBox1.Text;
            path = textBox3.Text;
            if (flag)
            {
                MessageBox.Show("Поиск уже начался!");
                return;
            }
            checkedListBox1.Items.Clear();
            flag = true;
            ThreadPool.QueueUserWorkItem(GetFiles);
            // 

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string deletefiles = String.Empty;
            if(checkedListBox1.CheckedItems.Count==0)
            {
                MessageBox.Show("Не выбраны файлы для удаления!");
                return;
            }
            var check = checkedListBox1.CheckedItems;
            string[] checka = new string[checkedListBox1.CheckedItems.Count];
            int j = 0;
            foreach(var c in check)
            {
                checka[j] = c.ToString();
                j++;
            }
            for(int i=0;i<checka.Length;i++)
            {
                checkedListBox1.Items.Remove(checka[i]);
            }
            
            Client client = new Client(address, port);

            bool f= client.DeleteFiles(checka);
            if (f)
            {
                MessageBox.Show("Файлы успешно удалены!");
            }

        }
    }
}
