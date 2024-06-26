﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

class CurrencyServer
{
    static Dictionary<string, double> exchangeRates = new Dictionary<string, double>()
    {
        { "USD_EUR", 0.85 },
        { "EUR_USD", 1.18 }
    };

    static void Main(string[] args)
    {
        IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        int port = 1234;
        TcpListener listener = new TcpListener(ipAddress, port);
        listener.Start();
        Console.WriteLine("Сервер запущено...");
        while (true)
        {
            TcpClient client = listener.AcceptTcpClient();
            Console.WriteLine($"Клієнт приєднався о {DateTime.Now}");
            HandleClient(client);
            Console.WriteLine($"Клієнт відключився о {DateTime.Now}");
        }
    }
    static void HandleClient(TcpClient client)
    {
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        string request = Encoding.ASCII.GetString(buffer, 0, bytesRead);
        Console.WriteLine("Отримано запит: " + request);

        string response = GetExchangeRate(request);
        byte[] responseBuffer = Encoding.ASCII.GetBytes(response);
        stream.Write(responseBuffer, 0, responseBuffer.Length);
        Console.WriteLine("Надіслано відповідь: " + response);
        client.Close();
    }
    static string GetExchangeRate(string request)
    {
        string[] currencies = request.Split('_');
        string key = currencies[0] + "_" + currencies[1];
        if (exchangeRates.ContainsKey(key))
        {
            double rate = exchangeRates[key];
            return $"{key}: {rate}";
        }
        else
        {
            return "Курс обміну не знайдено.";
        }
    }
}