using UnityEngine;  
using UnityEngine.UI;
using System.Collections;  
using System.Net;  
using System.Net.Sockets;  
using System.Text;  
using System.Threading;  



public class Server : MonoBehaviour
{
	[SerializeField]
    private Text connStatus = null;


    private byte connFlag = 0;

    byte sNumber, tmpData1, tmpData2, tmpData3, tmpData4, tmpData5, tmpData6; 

    //以下默認都是私有的成員 
    Socket serverSocket; //服務器端socket  
    Socket clientSocket; //客户端socket  
    IPEndPoint ipEnd; //監聽端口  
    string recvStr; //接收的字符串 
    string sendStr; //發送的字符串
    byte[] recvData=new byte[1024]; //發送的數據，必須為字節  
    byte[] sendData=new byte[1024]; //發送的數據，必須為字節  
    int recvLen; //接收的數據長度 
    Thread connectThread; //連接線程

    private CommandHandler commandHandler;

    //初始化  
    void InitSocket() {  
        Debug.Log ("InitSocket");
        //定義監聽端口，監聽任何IP，port9999可替換
        ipEnd=new IPEndPoint(IPAddress.Any,60123);
        //定義套貼字類型，在主線程中定義
        serverSocket=new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);  
        //連接
        serverSocket.Bind(ipEnd);  
        //開始監聽，最大10個連接
        serverSocket.Listen(10);  

        //開啟一個線程連接，必須的，否則主線程卡死 
        connectThread=new Thread(new ThreadStart(SocketReceive));
        connectThread.Start();
    }

    //連接
    void SocketConnet() {
        if (clientSocket != null) {
            clientSocket.Close ();
            connFlag = 2;
        }
        //控制台輸出監聽狀態
        Debug.Log("wait for a client");  
        //一旦接受連接，創建一個客戶端
        clientSocket=serverSocket.Accept();  
        //獲取客戶端的IP和端口
        IPEndPoint ipEndClient=(IPEndPoint)clientSocket.RemoteEndPoint;  
        //輸出客戶端的IP和端口
        print("Connect with "+ipEndClient.Address.ToString()+":"+ipEndClient.Port.ToString());  
        //連線成功則發送數據
        sendStr="Welcome to my server";
        connFlag = 1;
        Debug.Log("welcome");
        SocketSend(sendStr); 
    }

    void SocketSend(string sendStr) {  
        //清空發送緩存
        sendData=new byte[1024];  
        //數據類型轉換
        sendData=Encoding.ASCII.GetBytes(sendStr);  
        //發送 
        clientSocket.Send(sendData,sendData.Length,SocketFlags.None);  
    }  

    //服務器接收
    void SocketReceive() {  
        //連接
        SocketConnet();        
        //進入接收循環 
        while(true) {  
            //對data清零
            recvData=new byte[1024];  
            //獲取收到的數據的長度
            recvLen=clientSocket.Receive(recvData);  
            //如果收到的數據長度為0，則重連並進入下一個循環  
            if(recvLen==0) {
                SocketConnet();  
                continue;  
            }
 
            //做資料解析  
            /*
            if (recvData[0] == 0xCE) {
                if (recvData [8] == 0xED) {
                    sNumber = recvData [1];    //sensor number
                    tmpData1 = recvData [2];   
                    tmpData2 = recvData [3]; 
                    tmpData3 = recvData [4];
                    tmpData4 = recvData [5];
                    tmpData5 = recvData [6];
                    tmpData6 = recvData [7];
                    resolve ();
                }
            } 
            */

            //輸出接收到的數據
            recvStr=Encoding.ASCII.GetString(recvData,0,recvLen);  
            commandHandler.commands.Add(recvStr);
            print(recvStr);

            
            //將接收到的數據經過處理再發送出去
            //sendStr="From Server: "+recvStr;  
            //SocketSend(sendStr);  
        }
    }

    //連接關閉 
    void SocketQuit() {
        //先關閉客戶端
        if(clientSocket!=null)  
            clientSocket.Close();  
        //在關閉線程 
        if(connectThread!=null) {
            connectThread.Interrupt();  
            connectThread.Abort();  
        }
        //最後關閉服務器
        serverSocket.Close();
        Debug.Log("diconnect"); 
    }

    void resolve() {
        if (sNumber == 0x01) {
            Debug.Log( "Here is one" );
        }
        if (sNumber == 0x02) {
            Debug.Log( "Here is two" );
        }
    } 

    // Use this for initialization
    void Start() {  
        commandHandler = GameObject.FindObjectOfType<CommandHandler>();
        InitSocket(); //在這裡初始化server
    }

    void Update() {

    }
    
    void OnApplicationQuit() { 
        SocketQuit();  
    }
}