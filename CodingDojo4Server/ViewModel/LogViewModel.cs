using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using LogFileCommunication;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingDojo4Server.ViewModel {
	public class LogViewModel : ViewModelBase {

		public string SelectedLog {
			get => _selectedLog; set {
				_selectedLog = value;
				RaisePropertyChanged();
			}
		}

		public ObservableCollection<string> LogsList {
			get => _logsList; set {
				_logsList = value;
				RaisePropertyChanged();
			}
		}

		public ObservableCollection<string> LogsContent {
			get => _logsContent; set {
				_logsContent = value;
				RaisePropertyChanged();
			}
		}

		public RelayCommand ShowLogFileCmd { get; set; }
		public RelayCommand DropLogFileCmd { get; set; }

		private LogHandler logHandler;
		private ObservableCollection<string> _logsContent;
		private string _selectedLog;
		private ObservableCollection<string> _logsList;

		public LogViewModel() {
			logHandler = new LogHandler();
			GetLogFiles();

			ShowLogFileCmd = new RelayCommand(() => {
				LogsContent = new ObservableCollection<string>(logHandler.ReadLogFile(SelectedLog));
			}, () => SelectedLog != null);

			DropLogFileCmd = new RelayCommand(() => {
				bool isLogDeleted = logHandler.DeleteLogFile(SelectedLog);
				if (isLogDeleted) {
					SelectedLog = null;
					LogsList = new ObservableCollection<string>(logHandler.GetLogFiles());
					LogsContent.Clear();
				}
			}, () => SelectedLog != null);
		}

		public void GetLogFiles() {
			LogsList = new ObservableCollection<string>(logHandler.GetLogFiles());
		}
	}
}
