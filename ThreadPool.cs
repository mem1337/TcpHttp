using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TcpHttp;

class TcpServer2
{
    public static IPAddress Address = IPAddress.Parse("127.0.0.1");
    public static Int32 Port = 8080;
    public static TcpListener Server = new TcpListener(Address,Port);
    public static TcpClient Client;

    public static void StartTcpServer()
    {
        Server.Start();
        Console.WriteLine("--- Waiting for the client ----");

        while(true)
        {
            Client = Server.AcceptTcpClient();
            ThreadPool.QueueUserWorkItem(childThread);
            Thread.Sleep(100);
            Console.WriteLine(ThreadPool.ThreadCount);
        }
    }
    static void childThread(Object stateInfo)
    {
        // No state object was passed to QueueUserWorkItem, so stateInfo is null.
        var strem = Client.GetStream();
        byte[] read = new byte[900];
        strem.Read(read, 0, read.Length);
        Console.WriteLine(Encoding.UTF8.GetString(read));
        strem.Write(sendResponseOk(),0,sendResponseOk().Length);
    }

    public static byte[] sendResponseOk()
    {
        // html
        string htmlResponse = "";

        using (StreamReader sr = new StreamReader("index.html"))
        {
            string line;
            // Read and display lines from the file until the end of
            // the file is reached.
            while ((line = sr.ReadLine()) != null)
            {
                htmlResponse+=line;
            }
        }

        // response header
        string response = "HTTP/1.1 200 OK\r\n";
        response += "Content-type: text/html; charset=utf-8\r\n";
        response += "Content-Length: " + htmlResponse.Length + "\r\n";
        response += "\r\n";
        response += htmlResponse;

        // send
        byte[] sendData = Encoding.UTF8.GetBytes(response);
        return sendData;
    }
}
