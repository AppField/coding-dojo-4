using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingDojo4Server.ViewModel {
	public class LogViewModel : ViewModelBase {

		public string SelectedLog { get; set; }

		public ObservableCollection<string> LogsList { get; set; }
		public ObservableCollection<string> LogsContent { get; set; }

		public RelayCommand ShowLogFileCmd { get; set; }
		public RelayCommand DropLogFile { get; set; }

		public LogViewModel() {
			LogsList = new ObservableCollection<string>() {"asdf", "asdfasdf" };
			LogsContent = new ObservableCollection<string>();

			ShowLogFileCmd = new RelayCommand(() => throw new NotImplementedException());

			DropLogFile = new RelayCommand(() => throw new NotImplementedException());
		}
	}
}
