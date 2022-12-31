using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Threading;  

public class AndroidClient : MonoBehaviour
{
    public String host;
    public Int32 port;
    [ReadOnly] public Boolean isConnected = false;

    private TcpClient tcpClient;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;
    private Thread recvThread; 

    private ApplicationManager manager;

    private void Start()
    {
        manager = GameObject.FindObjectOfType<ApplicationManager>();
        setupSocket();
    }


    void Update()
    {
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
            tcpClient = new TcpClient();
            
            IPAddress ipAddress = IPAddress.Parse(host);
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port);
            tcpClient.Connect(ipEndPoint);
            
            stream = tcpClient.GetStream();
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);
            isConnected = true;
            
            recvThread = new Thread(new ThreadStart(Read));
            recvThread.Start();
        }
        catch (Exception e)
        {
            // Something went wrong
            Debug.Log("Socket error: " + e);
        }
    }

    //... writing to a socket...
    public void Write(string line)
    {
        if (!isConnected)
            return;

        line = line + "\n";
        writer.Write(line);
        writer.Flush();
    }

    //... reading from a socket...
    public void Read()
    {
        while(isConnected) {
            string recv = "";
            try {
                recv = reader.ReadLine();
                print(recv);
                manager.Push(recv);
            } catch(Exception e) {
                print(e.Message);
            }
            
        }
    }

    //... closing a socket...
    public void closeSocket()
    {
        if (!isConnected)
            return;

        
        writer.Close();
        reader.Close();
        tcpClient.Close();
        isConnected = false;

        if(recvThread!=null) {
            recvThread.Interrupt();  
            recvThread.Abort();  
        }
    }
}
