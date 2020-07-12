using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.IO;
using System.Net.Sockets;
//using Newtonsoft.Json;
//using System.Threading;

public class AndroidNetwork : MonoBehaviour
{
    public String host;
    public Int32 port;

    internal Boolean socket_ready = false;
    internal String input_buffer = "";
    TcpClient tcp_socket;
    NetworkStream net_stream;

    StreamWriter socket_writer;
    StreamReader socket_reader;

    private void Start()
    {
        setupSocket();
    }


    void Update()
    {
        string received_data = readSocket();
        if (received_data != "")
        {
            Debug.Log(" received:" + received_data);
            //text = Encoding.UTF8.GetString(data);
        }
    }

    void OnApplicationQuit()
    {
        closeSocket();
    }

    // Helper methods for:
    //...setting up the communication
    public void setupSocket()
    {
        
        try
        {
            tcp_socket = new TcpClient();
            
            IPAddress ipAddress = Dns.GetHostEntry(host).AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port);
            tcp_socket.Connect(ipEndPoint);
            
            net_stream = tcp_socket.GetStream();
            socket_writer = new StreamWriter(net_stream);
            socket_reader = new StreamReader(net_stream);
            socket_ready = true;
        }
        catch (Exception e)
        {
            // Something went wrong
            Debug.Log("Socket error: " + e);
        }
    }

    //... writing to a socket...
    public void writeSocket(string line)
    {
        if (!socket_ready)
            return;

        line = line + "\r\n";
        socket_writer.Write(line);
        socket_writer.Flush();
    }

    //... reading from a socket...
    public String readSocket()
    {
        if (!socket_ready)
            return "";

        if (net_stream.DataAvailable)
            return socket_reader.ReadLine();

        return "";
    }

    //... closing a socket...
    public void closeSocket()
    {
        if (!socket_ready)
            return;

        socket_writer.Close();
        socket_reader.Close();
        tcp_socket.Close();
        socket_ready = false;
    }
}
