using System.Net;
using System.Net.Sockets;
using System.Text;
namespace TcpHttp;

class TcpServer
{
    public static void StartTcpServer()
    {
        IPAddress Address = IPAddress.Parse("127.0.0.1");
        Int32 Port = 8080;
        TcpListener Server = new TcpListener(Address, Port);
        Server.Start();

        Console.WriteLine("--- Waiting for the client ----");
        while(true)
        {
            TcpClient client = Server.AcceptTcpClient();
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
        htmlResponse += "<head><title>FAHRI</title></head>";
        htmlResponse += "<body><h1>FAHRI</h1></body>";
        htmlResponse += "</html>";

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
