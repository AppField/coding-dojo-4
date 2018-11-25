using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace TcpCommunication {
	public class TcpServer {

		Socket socket;
		private List<ClientHandler> connectedClients = new List<ClientHandler>();
		private Action<string, bool> updateUser;
		private Action<string> updateMessage;
		private Thread clientsAccepter;

		public TcpServer(string ip, int port, Action<string, bool> updateUser, Action<string> updateMessage) {
			this.updateUser = updateUser;
			this.updateMessage = updateMessage;

			socket = new Socket(
				AddressFamily.InterNetwork,
				SocketType.Stream,
				ProtocolType.Tcp
				);
			socket.Bind(new IPEndPoint(IPAddress.Parse(ip), port));
			socket.Listen(5);
			clientsAccepter = new Thread(AcceptClients);
			clientsAccepter.Start();
		}

		private void AcceptClients(object state) {
			while (true && clientsAccepter.IsAlive) {
				connectedClients.Add(new ClientHandler(socket.Accept(), updateUser, updateMessage, ClientDisconnected));
			}
		}

		public void SendData(string message) {
			foreach (var item in connectedClients) {
				item.Send(message);
			}
		}

		public void CloseConnection() {
			clientsAccepter.Abort();
			socket.Close();
			connectedClients.ForEach(client => client.InitDisconnect());
		}

		public void DropUser(string selectedUser) {
			foreach (var item in connectedClients) {
				if (item.chatName.Equals(selectedUser)) {
					item.InitDisconnect();
					break;
				}
			}
		}

		private void ClientDisconnected(ClientHandler client) {
			connectedClients.Remove(client);
			updateUser(client.chatName, false);
			InformClientsAboutLeavement(client.chatName);
		}

		private void InformClientsAboutLeavement(string userLeft) {
			string informMessage = String.Format("{0} has left the chat", userLeft);
			connectedClients.ForEach(client => client.Send(informMessage));
			updateMessage(informMessage);
		}
	}
}
