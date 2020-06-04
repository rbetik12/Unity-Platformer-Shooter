using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace Net {
    public class Client : MonoBehaviour {
        public int myId;
        public delegate void PacketHandler(Packet packet);
        
        private int dataBufferSize = 4096;
        private string ip = "192.168.1.105";
        private Int32 port = 26950;
        private TCP tcp;
        private UDP udp;
        private bool isConnected = false;
        private Dictionary<int, PacketHandler> packetHandlers;

        private void Start() {
            tcp = new TCP(this);
            udp = new UDP(this);
        }

        public void SetPacketHandlers(Dictionary<int, PacketHandler> packetHandlers) {
            this.packetHandlers = packetHandlers;
        }

        public TCP GetTCP() {
            return tcp;
        }

        public UDP GetUDP() {
            return udp;
        }

        private void OnApplicationQuit() {
            Disconnect();
        }

        public void ConnectToServer() {
            InitClientData();
            isConnected = true;
            tcp.Connect();
        }

        public class TCP {
            private TcpClient socket;
            private NetworkStream stream;
            private byte[] receiveBuffer;
            private Packet receivedData;
            private Client instance;

            public TCP(Client instance) {
                this.instance = instance;
            }

            public TcpClient GetSocket() {
                return socket;
            }

            public void Connect() {
                socket = new TcpClient {
                    ReceiveBufferSize = instance.dataBufferSize,
                    SendBufferSize = instance.dataBufferSize
                };
                receiveBuffer = new byte[instance.dataBufferSize];
                socket.BeginConnect(instance.ip, instance.port, ConnectCallback, socket);
            }

            private void ConnectCallback(IAsyncResult result) {
                socket.EndConnect(result);

                if (!socket.Connected) {
                    return;
                }

                stream = socket.GetStream();
                receivedData = new Packet();
                stream.BeginRead(receiveBuffer, 0, instance.dataBufferSize, ReceiveCallback, null);
                Debug.Log("Connected via TCP");
            }

            public void SendData(Packet packet) {
                try {
                    if (socket != null) {
                        stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
                    }
                }
                catch (Exception ex) {
                    Debug.Log($"Error sending data to server via TCP: {ex}");
                }
            }

            private void ReceiveCallback(IAsyncResult result) {
                try {
                    int byteLength = stream.EndRead(result);
                    if (byteLength <= 0) {
                        instance.Disconnect();
                        return;
                    }

                    byte[] data = new byte[byteLength];
                    Array.Copy(receiveBuffer, data, byteLength);
                    receivedData.Reset(HandleData(data));
                    stream.BeginRead(receiveBuffer, 0, instance.dataBufferSize, ReceiveCallback, null);
                }
                catch (Exception ex) {
                    Console.WriteLine($"Error receiving TCP data: {ex}");
                    Disconnect();
                }
            }

            private bool HandleData(byte[] data) {
                int packetLength = 0;

                receivedData.SetBytes(data);

                if (receivedData.UnreadLength() >= 4) {
                    packetLength = receivedData.ReadInt();
                    if (packetLength <= 0) {
                        return true;
                    }
                }

                while (packetLength > 0 && packetLength <= receivedData.UnreadLength()) {
                    byte[] packetBytes = receivedData.ReadBytes(packetLength);
                    ThreadManager.ExecuteOnMainThread(() => {
                        using (Packet packet = new Packet(packetBytes)) {
                            int packetId = packet.ReadInt();
                            instance.packetHandlers[packetId](packet);
                        }
                    });

                    packetLength = 0;
                    if (receivedData.UnreadLength() >= 4) {
                        packetLength = receivedData.ReadInt();
                        if (packetLength <= 0) {
                            return true;
                        }
                    }
                }

                if (packetLength <= 1) {
                    return true;
                }

                return false;
            }

            private void Disconnect() {
                instance.Disconnect();

                stream = null;
                receiveBuffer = null;
                receivedData = null;
                socket = null;
            }
        }

        public class UDP {
            private UdpClient socket;
            private IPEndPoint endPoint;
            private Client instance;
            
            public UDP(Client instance) {
                endPoint = new IPEndPoint(IPAddress.Parse(instance.ip), instance.port);
                this.instance = instance;
            }

            public UdpClient GetSocket() {
                return socket;
            }

            public void Connect(int localPort) {
                socket = new UdpClient(localPort);

                socket.Connect(endPoint);
                socket.BeginReceive(ReceiveCallback, null);
                using (Packet packet = new Packet()) {
                    SendData(packet);
                }

                Debug.Log("Connected via UDP");
            }

            public void SendData(Packet packet) {
                try {
                    packet.InsertInt(instance.myId);
                    if (socket != null) {
                        socket.BeginSend(packet.ToArray(), packet.Length(), null, null);
                    }
                }
                catch (Exception exception) {
                    Debug.Log($"Exception occured during sending data via UDP: {exception}");
                }
            }

            private void ReceiveCallback(IAsyncResult result) {
                try {
                    byte[] data = socket.EndReceive(result, ref endPoint);
                    socket.BeginReceive(ReceiveCallback, null);

                    if (data.Length < 4) {
                        instance.Disconnect();
                        return;
                    }

                    HandleData(data);
                    Debug.Log("Received data via UDP");
                }
                catch (Exception ex) {
                    Disconnect();
                }
            }

            private void HandleData(byte[] data) {
                using (Packet packet = new Packet(data)) {
                    int packetLength = packet.ReadInt();
                    data = packet.ReadBytes(packetLength);
                }

                ThreadManager.ExecuteOnMainThread(() => {
                    using (Packet packet = new Packet(data)) {
                        int packetId = packet.ReadInt();
                        instance.packetHandlers[packetId](packet);
                    }
                });
            }

            private void Disconnect() {
                instance.Disconnect();

                endPoint = null;
                socket = null;
            }
        }

        private void InitClientData() {
            Debug.Log("Initialized packets");
        }

        private void Disconnect() {
            if (isConnected) {
                isConnected = false;
                tcp.GetSocket().Close();
                udp.GetSocket().Close();

                Debug.Log("Disconnected from server");
            }
        }
    }
}