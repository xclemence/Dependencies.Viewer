using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Dependencies.Analyser.Base;
using Dependencies.Analyser.Base.Models;
using Dependencies.Viewer.Wpf.Controls.Extensions;
using Dragablz;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;

namespace Dependencies.Viewer.Wpf.Controls.ViewModels
{
    public class AnalyserViewModel : ViewModelBase
    {
        internal const string FileFilter =
           "Software (*.exe)|*.exe|" +
           "Assembly (*.dll)|*.dll|" +
            "All Files|*.*";

        private bool isBusy;
        private bool isDragFile;
        private IInterTabClient interTabClient;
        private readonly AnalyserProvider analyserProvider;
        private readonly IServiceFactory<AnalyseResultViewModel> analyserViewModelFactory;
        private AnalyseResultViewModel selectedItem;
        private bool isSettingsOpen;

        public AnalyserViewModel(AnalyserProvider analyserProvider,
                                 IServiceFactory<AnalyseResultViewModel> analyserViewModelFactory,
                                 IInterTabClient interTabClient,
                                 SettingsViewModel settingsViewModel)
        {
            this.analyserProvider = analyserProvider;
            this.analyserViewModelFactory = analyserViewModelFactory;
            InterTabClient = interTabClient;
            SettingsViewModel = settingsViewModel;
            SettingsCommand = new RelayCommand(() => IsSettingsOpen = true);
            CloseCommand = new RelayCommand(() => Application.Current.Shutdown());
            OpenFileCommand = new RelayCommand(async () => await BusyAction(OpenFileAsync), () => !IsBusy);
            OnDragOverCommand = new RelayCommand<DragEventArgs>(OnDragOver);
            OnDropCommand = new RelayCommand<DragEventArgs>(async (x) => await BusyAction(async () => await OnDrop(x)), !IsBusy);

            OnDragEnterCommand = new RelayCommand<DragEventArgs>((x) => IsDragFile = true, CanDrag);
            OnDragLeaveCommand = new RelayCommand<DragEventArgs>((x) => IsDragFile = false, CanDrag);

            CloseResultCommand = new RelayCommand<AnalyseResultViewModel>(CloseResult);

            GlobalCommand.OpenAssemblyAction = AddAssemblyResult;
        }
      
        public ObservableCollection<AnalyseResultViewModel> AnalyseDetailsViewModels { get; } = new ObservableCollection<AnalyseResultViewModel>();

        public AnalyseResultViewModel SelectedItem
        {
            get => selectedItem;
            set => Set(ref selectedItem, value);
        }

        public bool IsSettingsOpen
        {
            get => isSettingsOpen;
            set => Set(ref isSettingsOpen, value);
        }

        public IInterTabClient InterTabClient
        {
            get => interTabClient;
            private set => Set(ref interTabClient, value);
        }
        public SettingsViewModel SettingsViewModel { get; }

        public bool IsBusy
        {
            get => isBusy;
            private set => Set(ref isBusy, value);
        }

        public ICommand OpenFileCommand { get; }
        public ICommand SettingsCommand { get; }
        
        public ICommand CloseCommand { get; }
        public ICommand OnDragOverCommand { get; }
        public ICommand OnDropCommand { get; }
        public ICommand OnDragEnterCommand { get; }
        public ICommand OnDragLeaveCommand { get; }
        public ICommand CloseResultCommand { get; }
        
        public Func<DragEventArgs, bool> CanDragFunc => (x) => CanDrag(x);

        public bool IsDragFile
        {
            get => isDragFile;
            set => Set(ref isDragFile, value);
        }

        private async Task BusyAction(Func<Task> actionAsync)
        {
            IsBusy = true;

            try
            {
                await actionAsync();
            }
            catch(Exception ex)
            {
                while (ex.InnerException != null)
                    ex = ex.InnerException;

                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OpenFileAsync()
        {
            var openFileDialog = new OpenFileDialog { Title = "Open File", Filter = FileFilter, Multiselect = false };
            if (openFileDialog.ShowDialog() != true) return;

            await AnalyseAsync(openFileDialog.FileName);
        }

        private async Task OnDrop(DragEventArgs e)
        {
            e.Handled = true;
            IsDragFile = false;
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

            var filenames = (string[])e.Data.GetData(DataFormats.FileDrop, true);

            if (filenames.Length == 0) return;

            await AnalyseAsync(filenames[0]);
        }

        public async Task InitialiseAsync(string filePath = null)
        {
            if (filePath == null)
                return;
            
            await BusyAction(async () => await AnalyseAsync(filePath));
        }

        private async Task AnalyseAsync(string filePath)
        {
            var analyser = analyserProvider.CurrentAnalyserFactory.GetAnalyser();
            AddAssemblyResult(await analyser.AnalyseAsync(filePath));
        }

        private void AddAssemblyResult(AssemblyInformation info)
        {
            if (info == null)
                return;

            var newViewModel = analyserViewModelFactory.Create();
            newViewModel.AssemblyResult = info;

            new Action(() =>
            {
                AnalyseDetailsViewModels.Add(newViewModel);
                SelectedItem = newViewModel;
            }).InvokeUiThread();
        }


        private void OnDragOver(DragEventArgs e)
        {
            e.Effects = CanDrag(e) ? DragDropEffects.Move : DragDropEffects.None;
            e.Handled = true;
        }

        private bool CanDrag(DragEventArgs e) =>
            !IsBusy && e.Data.GetDataPresent(DataFormats.FileDrop);

        private void CloseResult(AnalyseResultViewModel x) =>
            TabablzControl.CloseItem(x);

    }
}