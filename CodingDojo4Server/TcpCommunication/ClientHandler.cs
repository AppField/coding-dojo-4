using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TcpCommunication {
	internal class ClientHandler {

		private Socket client;
		public string chatName;
		private byte[] buffer = new byte[512];
		private Action<string> updateUser;
		private Action<string> updateMessage;
		private Action<ClientHandler> clientDisconnected;
		private string endCommand = "@quit";
		Thread receiver;

		public ClientHandler(
				Socket socket,
				Action<string> updateUser,
				Action<string> updateMessage,
				Action<ClientHandler> clientDisconnected
			) {
			client = socket;
			this.updateUser = updateUser;
			this.clientDisconnected = clientDisconnected;
			receiver = new Thread(ReceiveData);
			receiver.Start();

			this.updateMessage = updateMessage;

			// get Chat Name
			int length = client.Receive(buffer);
			chatName = Encoding.UTF8.GetString(buffer, 0, length);
			updateUser(chatName);
		}

		private void ReceiveData(object state) {
			while (true) {
				int length = client.Receive(buffer);
				string message = Encoding.UTF8.GetString(buffer, 0, length);
				if (!message.Equals("")) {
					message = string.Format("{0}: {1}", chatName, message);
					if (message.Contains(endCommand)) {
						InitDisconnect();
					} else updateMessage(message);
				}
			}
		}

		public void InitDisconnect() {
			Send(endCommand);
			clientDisconnected(this);
			receiver.Abort();
		}

		public void Send(string message) {
			client.Send(Encoding.UTF8.GetBytes(message));
		}
	}
}