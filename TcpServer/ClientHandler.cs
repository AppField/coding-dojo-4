using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ServerCommunication {
	internal class ClientHandler {

		private Socket client;
		public string chatName;
		private byte[] buffer = new byte[512];
		private Action<string> updateMessage;

		public ClientHandler(Socket socket, Action<string> updateUser, Action<string> updateMessage) {
			this.client = socket;
			ThreadPool.QueueUserWorkItem(ReceiveData);

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
				message = string.Format("{0}: {1}", chatName, message);
				updateMessage(message);
			}
		}

		public void Send(string message) {
			client.Send(Encoding.UTF8.GetBytes(message));
		}
	}
}