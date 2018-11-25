using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace ServerCommunication {
	public class TcpServer {

		Socket socket;
		private List<ClientHandler> connectedClients = new List<ClientHandler>();
		private Action<string> updateUser;
		private Action<string> updateMessage;

		public TcpServer(string ip, int port, Action<string> updateUser, Action<string> updateMessage) {
			this.updateUser = updateUser;
			this.updateMessage = updateMessage;

			socket = new Socket(
				AddressFamily.InterNetwork,
				SocketType.Stream,
				ProtocolType.Tcp
				);
			socket.Bind(new IPEndPoint(IPAddress.Parse(ip), port));
			socket.Listen(5);
			ThreadPool.QueueUserWorkItem(AcceptClients);
		}

		private void AcceptClients(object state) {
			while (true) {
				connectedClients.Add(new ClientHandler(socket.Accept(), updateUser, updateMessage));
			}
		}

		public void SendData(string message) {
			foreach (var item in connectedClients) {
				item.Send(message);
			}
		}

		public void CloseConnection() {
			socket.Close();
		}

		public void DropUser(string selectedUser) {
			foreach (var item in connectedClients) {
				if (item.chatName.Equals(selectedUser)) {
					item.Send("@quit");
					connectedClients.Remove(item);
					break;
				}
			}
		}
	}
}
