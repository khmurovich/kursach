using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    //dfsfsdfsdf
    




        //dsfsdfsdfsdf
    public class DuplicateFilesNotFound : Exception
    {
        public DuplicateFilesNotFound()
            : base("Дублирующиеся файлы не найдены!")
        {
        }
    }
    public class IncorrectPath : Exception
    {
        public IncorrectPath()
            : base("Некорректный путь!")
        {
        }
    }
    class Client:TCPClient
    {
        public Client(string server, int port)
            :base(server, port)
        {

        }
        public string[] GetDuplicateFiles(string path)
        {
            Send("<GETFILES> " + path + "<TheEnd>");
            string receive = Receive().Trim();
            if (receive == String.Empty)
            {
                throw new DuplicateFilesNotFound();

            }
            if (receive.IndexOf("<ERROR>") > -1)
            {
                throw new IncorrectPath();
            }
            receive = receive.TrimEnd('|');
            string[] arr = receive.Split('|');
            return arr;
        }



        public bool DeleteFiles(string[] files)
        {
            string deletefiles = String.Empty;
            foreach (var c in files)
            {
                deletefiles += c + "|";
            }
            Send("<DELETEFILES> " + deletefiles + "<TheEnd>");
            string receive=String.Empty;
            receive = Receive();
            if(receive.IndexOf("<true>")>-1)
            {
                return true;
            }
            return false;
        }
    }
}
