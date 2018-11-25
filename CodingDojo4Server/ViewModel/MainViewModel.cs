using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using TcpCommunication;
using System;
using System.Collections.ObjectModel;

namespace CodingDojo4Server.ViewModel {

	public class MainViewModel : ViewModelBase {

		private TcpServer server;
		private string ip = "127.0.0.1";
		private int port = 10100;

		bool isRunning = false;
		private int _messageNumbers;

		public int MessageNumbers {
			get => _messageNumbers; set {
				_messageNumbers = value;
				RaisePropertyChanged();
			}
		}
		public ObservableCollection<string> UserList { get; set; }
		public ObservableCollection<string> MessagesList { get; set; }
		public string SelectedUser { get; set; }

		// COMMANDS
		public RelayCommand StartServerCmd { get; set; }
		public RelayCommand StopServerCmd { get; set; }
		public RelayCommand DropUserCmd { get; set; }
		public RelayCommand SaveToLogCmd { get; set; }


		public MainViewModel() {
			UserList = new ObservableCollection<string>();
			MessagesList = new ObservableCollection<string>();
			MessageNumbers = 0;

			StartServerCmd = new RelayCommand(
				() => {
					isRunning = true;
					server = new TcpServer(ip, port, UpdateUserGui, UpdateMessageListGui);

				}, () => !isRunning);

			StopServerCmd = new RelayCommand(
				() => {
					isRunning = false;
					server.CloseConnection();
				}, () => isRunning);

			DropUserCmd = new RelayCommand(
				() => {
					server.DropUser(SelectedUser);
					SelectedUser = null;
				}, () => isRunning && SelectedUser != null);

			SaveToLogCmd = new RelayCommand(
				() => { }, () => isRunning);
		}

		private void UpdateUserGui(string user, bool isNew) {
			App.Current.Dispatcher.Invoke(() => {
				if (isNew) {
					UserList.Add(user);
				} else {
					UserList.Remove(user);
				}
			});
		}

		private void UpdateMessageListGui(string message) {
			App.Current.Dispatcher.Invoke(() => {
				MessageNumbers++;
				MessagesList.Add(message);
				server.SendData(message);
			});
		}
	}
}