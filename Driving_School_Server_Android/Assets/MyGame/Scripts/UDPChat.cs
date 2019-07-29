using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace HD
{

    [System.Serializable]
    public class UDPChat
	{

		private bool isServer;
        public string targetAddress;
        public int port;
        UdpClient connection2;
        private int currentPhase= 0;
        MyMessage lastMessage;

        /// <summary>
        /// IP for clients to connect to. Null if you are the server.
        /// </summary>

        public IPAddress serverIp;

		/// <summary>
		/// For Clients, there is only one and it's the connection to the server.
		/// For Servers, there are many - one per connected client.
		/// </summary>
		List<IPEndPoint> clientList = new List<IPEndPoint>();

		/// <summary>
		/// The string to render in Unity.
		/// </summary>
		public string messageToDisplay;
		public string text;

		public UDPChat(bool isServer) {
			this.isServer = isServer;
			clientList = new List<IPEndPoint>();           
		}
        public bool IsServer()
        {
            return isServer;
        }

        public MyMessage GetLastMessage()
        {
            return lastMessage;
        }

        public void DeleteLastMessage()
        {
             lastMessage=null;
        }

        public int GetCurrentPhase()
        {
            return currentPhase;
        }

        public void Init()
		{
            Debug.Log ("Init()");

			try{
                if (isServer)
                {
                    connection2 = new UdpClient(port);
                    Debug.Log("Created new UDPChat-Server on port: " + port);
                }
                else
                {
                    targetAddress = Regex.Replace(targetAddress, "[^0-9.]||[^.]", "");
                    Debug.Log("Target address : '" + targetAddress + "'");
                    System.Net.IPAddress ipAddress = System.Net.IPAddress.Parse(targetAddress);
                    IPEndPoint ipLocalEndPoint = new IPEndPoint(ipAddress, port);
                    AddClient(ipLocalEndPoint); //the serverIP
                    connection2 = new UdpClient();

                    Debug.Log("Created new UDPChat-Client: " + ipLocalEndPoint.Address.ToString() + ", port: " + ipLocalEndPoint.Port);
                }
               
            }
			catch (Exception e ) {
				Console.WriteLine(e.ToString());
			}




        }


        public void StartListening() //calls listener once ... or else it locks itself somehow ...
        {
            Debug.Log("UDPChat: Starting listening ...");
            connection2.BeginReceive(new AsyncCallback(ReceiveAsync), null);
        }


        internal void AddClient(IPEndPoint ipEndpoint)
		{
			if(clientList.Contains(ipEndpoint) == false)
			{ // If it's a new client, add to the client list
				Debug.Log("Adding endpoint to list: " + ipEndpoint);
				clientList.Add(ipEndpoint);
				//normally there should be a timeout with a ping, to check if client is online - if ping fails -> delete from list
			}
		}

        public int countConnections()
        {
            return clientList.Count;
        }

		/// <summary>
		/// TODO: We need to add timestamps to timeout and remove clients from the list.
		/// </summary>
		internal void RemoveClient(IPEndPoint ipEndpoint)
		{ 
			clientList.Remove(ipEndpoint);
		}

		public void OnApplicationQuit()
        {
            Debug.Log("OnApplicationQuit()");
		
            if(null != connection2)
                connection2.Close();
		}

	


     

        void ReceiveAsync(IAsyncResult ar)
		{
			Debug.Log ("Received Something ...");


            try
            {
                IPEndPoint ipEndpoint = null;// = new IPEndPoint(IPAddress.Any, 0); //sender ip
                byte[] data = connection2.EndReceive(ar, ref ipEndpoint);
                // string datastr = System.Text.Encoding.UTF8.GetString(data);
                // Debug.Log("Received message: " + datastr);


                lastMessage = MyMessageSerializer.DeserializeFromBytes<MyMessage>(data);

                currentPhase = lastMessage.Phase;
                //Debug.Log("lastMessage phase: " + lastMessage.Phase);
                Debug.Log("current phase: " + currentPhase.ToString()); ////https://www.dotnetperls.com/enum-parse
                if (isServer) AddClient(ipEndpoint);//do not remove this, or else the server cannot write back!!
                
                
                //  Debug.Log("\nlastMessage SteeringInput: " + lastMessage.SteeringInput);

                //Application.Quit();
            }
			catch(SocketException e)
			{
                // This happens when a client disconnects, as we fail to send to that port.
                Console.WriteLine(e.ToString());

            }
            connection2.BeginReceive(new AsyncCallback(ReceiveAsync), null);
        }


        /*
		public void Send(string message)
		{
			//Debug.Log ("Send()");
            //Debug.Log("Targets to send to: " + countConnections());
            byte[] data = System.Text.Encoding.UTF8.GetBytes(message);
            BroadcastChatMessage(data);
		}
        */

        public void Send(MyMessage mymessage)
        {
           // Debug.Log("Send()");
            byte[] data = MyMessageSerializer.SerializeToBytes<MyMessage>(mymessage);
            BroadcastChatMessage(data);
        }

        internal void BroadcastChatMessage(byte[] data)
		{
            foreach (var ip in clientList)
            {
                try
                {
                   Debug.Log("Sending to Target: " + ip);
                   connection2.Send(data, data.Length, ip);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
		}
	}
}