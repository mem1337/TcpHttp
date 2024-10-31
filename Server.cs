using System.Net;
using System.Net.Sockets;
namespace TcpHttp;

class Server
{
    public static void StartServer()
    {
        try
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");

            Console.WriteLine("Starting TCP listener...");

            TcpListener listener = new TcpListener(ipAddress, 8080);

            listener.Start();

            while (true)
            {
                Console.WriteLine("Server is listening on " + listener.LocalEndpoint);

                Console.WriteLine("Waiting for a connection...");

                while (true)
                {
                    Socket client = listener.AcceptSocket();
                    Console.WriteLine("Connection accepted.");

                    var childSocketThread = new Thread(() =>
                    {
                        byte[] data = new byte[100];
                        int size = client.Receive(data);
                        Console.WriteLine("Recieved data: ");

                        for (int i = 0; i < size; i++)
                        {
                            Console.Write($"{Convert.ToChar(data[i])}");
                        }
                        client.Close();
                    });

                    childSocketThread.Start();
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: " + e.StackTrace);
            Console.ReadLine();
        }
    }
}