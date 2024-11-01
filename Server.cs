using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TcpHttp;

class TcpServer
{
    public static void StartTcpServer()
    {
        IPAddress address = IPAddress.Parse("127.0.0.1");
        Int32 port = 8080;
        TcpListener server = new TcpListener(address, port);
        server.Start();

        Console.WriteLine("--- Waiting for the client ----");
        while(true)
        {
            TcpClient client = server.AcceptTcpClient();
            var childThread = new Thread(() =>
            {
                var strem = client.GetStream();
                byte[] read = new byte[900];
                strem.Read(read, 0, read.Length);
                Console.WriteLine(Encoding.UTF8.GetString(read));
                strem.Write(sendResponseOk(),0,sendResponseOk().Length);
            });
            childThread.Start();
        }
    }
    public static byte[] sendResponseOk()
    {
        // html
        string htmlResponse = "<html>";

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
