using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Net.NetworkInformation;

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
    private ExperimentManager experiment;

    private void Start()
    {
        manager = GameObject.FindObjectOfType<ApplicationManager>();
        experiment = GameObject.FindObjectOfType<ExperimentManager>();
        SetupSocket();
    }


    void Update()
    {

    }

    public bool GetConnected()
    {
        if (tcpClient != null)
        {
            return tcpClient.Connected;
        }

        return false;
    }

    void OnApplicationQuit()
    {
        closeSocket();
    }

    // Helper methods for:
    //...setting up the communication
    public void SetupSocket()
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
                if (manager)
                {
                    manager.Push(recv);
                }
                if (experiment)
                {
                    if (experiment.isStartExperiment)
                        experiment.Push(recv);
                }
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
