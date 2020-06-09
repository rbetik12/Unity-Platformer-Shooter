using System;
using System.Collections.Generic;
using System.Net;
using Net;
using UnityEngine;

namespace Managers {
    public class NetworkManager : MonoBehaviour {
        [SerializeField] private GameObject clientPrefab;

        private ClientHandler clientHandlerInstance;
        private ClientSend clientSendInstance;
        private GameManager gameManagerInstance;
        private Client clientInstance;

        private void Start() {
            clientInstance = Instantiate(clientPrefab, Vector3.zero, Quaternion.identity).GetComponent<Client>();
            gameManagerInstance = GameObject.Find("GameManager").GetComponent<GameManager>();
            clientHandlerInstance = new ClientHandler(gameManagerInstance, clientInstance, this);
            clientSendInstance = new ClientSend(this);
            clientInstance.SetPacketHandlers(
                new Dictionary<int, Client.PacketHandler>() {
                    { (int) ServerPackets.welcome, clientHandlerInstance.Welcome },
                    { (int) ServerPackets.spawnPlayer, clientHandlerInstance.SpawnPlayer },
                    { (int) ServerPackets.playerPosition, clientHandlerInstance.PlayerPosition },
                    { (int) ServerPackets.playerRotation, clientHandlerInstance.PlayerRotation },
                    { (int) ServerPackets.playerDisconnected, clientHandlerInstance.PlayerDisconnected },
                }
            );
        }

        public ClientHandler GetClientHandler() {
            return clientHandlerInstance;
        }

        public ClientSend GetClientSend() {
            return clientSendInstance;
        }

        public Client GetClient() {
            return clientInstance;
        }

        public class ClientHandler {
            private GameManager gameManagerInstance;
            private Client clientInstance;
            private NetworkManager instance;

            public ClientHandler(GameManager gameManager, Client client, NetworkManager instance) {
                gameManagerInstance = gameManager;
                clientInstance = client;
                this.instance = instance;
            }

            public void Welcome(Packet packet) {
                string msg = packet.ReadString();
                int id = packet.ReadInt();
                Debug.Log(msg);
                clientInstance.myId = id;
                instance.clientSendInstance.WelcomeReceived();
                clientInstance.GetUDP()
                    .Connect(((IPEndPoint) clientInstance.GetTCP().GetSocket().Client.LocalEndPoint).Port);
            }

            public void SpawnPlayer(Packet packet) {
                int id = packet.ReadInt();
                string username = packet.ReadString();
                Vector3 position = packet.ReadVector3();
                Quaternion rotation = packet.ReadQuaternion();

                gameManagerInstance.network.SpawnPlayer(id, username, position, rotation);
            }

            public void PlayerPosition(Packet packet) {
                int id = packet.ReadInt();
                Vector3 position = packet.ReadVector3();

                gameManagerInstance.network.GetPlayers()[id].transform.position = position;
            }

            public void PlayerRotation(Packet packet) {
                int id = packet.ReadInt();
                Quaternion rotation = packet.ReadQuaternion();

                gameManagerInstance.network.GetPlayers()[id].transform.rotation = rotation;
            }

            public void PlayerDisconnected(Packet packet) {
                int id = packet.ReadInt();


                Destroy(gameManagerInstance.network.GetPlayers()[id].gameObject);
                gameManagerInstance.network.GetPlayers().Remove(id);
            }
        }

        public class ClientSend {
            private NetworkManager instance;

            public ClientSend(NetworkManager instance) {
                this.instance = instance;
            }

            private void SendTCPData(Packet packet) {
                packet.WriteLength();
                instance.clientInstance.GetTCP().SendData(packet);
            }

            private void SendUDPData(Packet packet) {
                packet.WriteLength();
                instance.clientInstance.GetUDP().SendData(packet);
            }

            #region Packets

            public void WelcomeReceived() {
                using (Packet packet = new Packet((int) ClientPackets.welcomeReceived)) {
                    packet.Write(instance.clientInstance.myId);
                    packet.Write(instance.gameManagerInstance.ui.GetNetUIManager().GetUsernameField().text);

                    SendTCPData(packet);
                }
            }

            public void PlayerMovement(float[] input) {
                using (Packet packet = new Packet((int) ClientPackets.playerMovement)) {
                    packet.Write(input[0]);
                    packet.Write(input[1]);
                    packet.Write(input[2]);
                    packet.Write(instance.gameManagerInstance.network.GetPlayers()[instance.clientInstance.myId]
                        .transform.rotation);

                    SendUDPData(packet);
                }
            }

            #endregion
        }
    }
}