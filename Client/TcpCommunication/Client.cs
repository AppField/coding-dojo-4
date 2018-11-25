using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TcpCommunication {
	public class Client {

		private Socket client;
		private byte[] buffer = new byte[512];
		private Action<string> update;
		private Action disconnect;


		private bool isConnected;
		Thread receiver;

		public Client(string ip, int port, Action<string> update, string chatName, Action disconnect) {
			this.update = update;
			this.disconnect = disconnect;

			TcpClient tcpClient = new TcpClient();
			tcpClient.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
			client = tcpClient.Client;
			isConnected = true;

			receiver = new Thread(ReceiveData);
			receiver.Start();
			SendMessage(chatName);
		}

		private void ReceiveData() {
			string message = "";
			while (!message.Contains("@quit")) {
				int length = client.Receive(buffer);
				message = Encoding.UTF8.GetString(buffer, 0, length);
				update(message);
			}
			isConnected = false;
			Close();
		}

		public void SendMessage(string message) {
			if (client != null) {
				client.Send(Encoding.UTF8.GetBytes(message));
			}
		}

		private void Close() {
			client.Close();
			isConnected = false;
			disconnect();
			receiver.Abort();
		}
	}


}
