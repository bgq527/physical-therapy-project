//using UnityEngine;
//using System.Threading;
//using System.Collections;
//using System.Timers;
//using NetMQ; // for NetMQConfig
//using NetMQ.Sockets;

//public class PupilRemoteController : MonoBehaviour
//{

//    Thread client_thread_;
//    private Object thisLock_ = new Object();
//    bool stop_thread_ = false;

//    void Start()
//    {
//        Debug.Log("Start a request thread.");
//        client_thread_ = new Thread(NetMQClient);
//        client_thread_.Start();
//    }

//    // Client thread which does not block Update()
    
//    void NetMQClient()
//    {
        
//            AsyncIO.ForceDotNet.Force();
//#pragma warning disable CS0618 // Type or member is obsolete
//            NetMQConfig.ManualTerminationTakeOver();
//#pragma warning restore CS0618 // Type or member is obsolete
//#pragma warning disable CS0618 // Type or member is obsolete
//            NetMQConfig.ContextCreate(true);
//#pragma warning restore CS0618 // Type or member is obsolete

//            string msg;
//            var timeout = new System.TimeSpan(0, 0, 1); //1sec

//            Debug.Log("Connect to the server.");
//            var requestSocket = new RequestSocket(">tcp://127.0.0.1:50020");
//            requestSocket.SendFrame("SUB_PORT");
//            bool is_connected = requestSocket.TryReceiveFrameString(timeout, out msg);

//            if (is_connected)
//            {
//                requestSocket.SendFrame("R");
//            }


//            //while (is_connected && stop_thread_ == false)
//            //{
//            //    Debug.Log("Request a message.");
//            //    //requestSocket.SendFrame("R");
//            //    is_connected = requestSocket.TryReceiveFrameString(timeout, out msg);
//            //    Debug.Log("Sleep");
//            //    Thread.Sleep(1000);
//            //}

//            requestSocket.Close();
//            Debug.Log("ContextTerminate.");
//#pragma warning disable CS0618 // Type or member is obsolete
//            NetMQConfig.ContextTerminate();
//#pragma warning restore CS0618 // Type or member is obsolete
//        }
        
        
    


   

//    void OnApplicationQuit()
//    {
//        lock (thisLock_) stop_thread_ = true;
//        client_thread_.Join();
//        Debug.Log("Quit the thread.");
//    }

//}