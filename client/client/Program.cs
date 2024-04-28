using System;
using System.Net.Sockets;
using System.Text;

class CurrencyClient
{
    static void Main(string[] args)
    {
        string serverIP = "127.0.0.1";
        int serverPort = 1234;
        TcpClient client = new TcpClient(serverIP, serverPort);
        NetworkStream stream = client.GetStream();
        Console.WriteLine("Введіть назву валюти для співвідношення через '_' (USD_EUR|EUR_USD):");
        string request = Console.ReadLine();
        byte[] buffer = Encoding.ASCII.GetBytes(request);
        stream.Write(buffer, 0, buffer.Length);
        byte[] responseBuffer = new byte[1024];
        int bytesRead = stream.Read(responseBuffer, 0, responseBuffer.Length);
        string response = Encoding.ASCII.GetString(responseBuffer, 0, bytesRead);
        Console.WriteLine("Курс валюти " + response);
        client.Close();
    }
}
