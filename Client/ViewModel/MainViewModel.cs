using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using TcpCommunication;
using System;
using System.Collections.ObjectModel;
using Client;

namespace CodingDojo4Client.ViewModel {
	public class MainViewModel : ViewModelBase {

		private TcpCommunication.Client com;

		private string ip = "127.0.0.1";
		private int port = 10100;
		private string _newMessage;
		private bool _isConnected;

		public bool IsConnected {
			get => _isConnected; set {
				_isConnected = value;
				RaisePropertyChanged();
			}
		}
		public string NewMessage {
			get => _newMessage; set {
				_newMessage = value;
				RaisePropertyChanged();
			}
		}
		public string ChatName { get; set; }
		public ObservableCollection<string> MessagesList { get; set; }

		// Commands
		public RelayCommand ConnectCmd { get; set; }
		public RelayCommand SendCmd { get; set; }


		public MainViewModel() {
			MessagesList = new ObservableCollection<string>();

			ConnectCmd = new RelayCommand(
					() => {
						com = new TcpCommunication.Client(ip, port, this.UpdateGui, ChatName, () => {
							App.Current.Dispatcher.Invoke(() => IsConnected = false);
						});
						IsConnected = true;
					},
					() => ChatName != null && ChatName.Length >= 1 && !IsConnected
				);

			SendCmd = new RelayCommand(
					() => {
						com.SendMessage(NewMessage);
						NewMessage = "";
					},
					() => NewMessage != null && NewMessage.Length >= 1 && IsConnected
				);
		}

		private void UpdateGui(string message) {
			App.Current.Dispatcher.Invoke(() => {
				if (message.Equals("@quit")) {
					message = "You've been disconnected from the server";
				} else {
					string[] splitted = message.Split(':');
					message = splitted[0].Equals(ChatName) ? "YOU: " + splitted[1] : message;
				}
				MessagesList.Add(message);
			});
		}
	}
}