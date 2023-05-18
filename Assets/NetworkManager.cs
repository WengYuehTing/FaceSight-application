using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Net.NetworkInformation;

/*
public static class TcpStateExtensions
{
    /// <summary>
    /// Shuffles the element order of the specified list.
    /// </summary>
    public static TcpState GetState(this TcpClient tcpClient)
    {
        var foo = IPGlobalProperties.GetIPGlobalProperties()
          .GetActiveTcpConnections()
          .FirstOrDefault(x => x.LocalEndPoint.Equals(tcpClient.Client.LocalEndPoint));
        return foo != null ? foo.State : TcpState.Unknown;
    }
}
*/

public class NetworkManager : MonoBehaviour
{
    public AndroidClient clientPrefab;
    public AndroidClient currentClient;

    public float timeRemaining = 10.0f;
    public float reconnectedTimeout = 10.0f;
    public bool isStartReconnected = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!currentClient)
        {
            currentClient = GameObject.Instantiate(clientPrefab) as AndroidClient;
            currentClient.gameObject.SetActive(true);
            currentClient.SetupSocket();
        }
        else
        {
            bool isConnected = currentClient.GetConnected();
            if (!isConnected)
            {
                if (!isStartReconnected)
                {
                    isStartReconnected = true;
                    timeRemaining = reconnectedTimeout;
                }
            }
            
        }

        if (isStartReconnected)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining < 0.0f)
            {
                currentClient = null;
                isStartReconnected = false;
            }
        }
    }

    public AndroidClient GetCurrentClient()
    {
        return currentClient;
    }
}
