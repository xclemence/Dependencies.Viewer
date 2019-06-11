using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Dependencies.Analyser.Base;
using Dependencies.Analyser.Base.Models;
using Dependencies.Viewer.Wpf.Controls.Extensions;
using Dragablz;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;

namespace Dependencies.Viewer.Wpf.Controls.ViewModels
{
    public class AnalyserViewModel : ViewModelBase
    {
        private const string OpenFileFilter =
            "Software (*.exe)|*.exe|" +
            "Assembly (*.dll)|*.dll|" +
            "All Files|*.*";

        private const string ImportFileFilter =
            "Resutlts (*.xml)|*.xml|" +
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
                                 SettingsViewModel settingsViewModel,
                                 ISnackbarMessageQueue messageQueue)
        {
            this.analyserProvider = analyserProvider;
            this.analyserViewModelFactory = analyserViewModelFactory;
            InterTabClient = interTabClient;
            SettingsViewModel = settingsViewModel;
            MessageQueue = messageQueue;

            SettingsCommand = new RelayCommand(() => IsSettingsOpen = true);
            CloseCommand = new RelayCommand(() => Application.Current.Shutdown());
            OpenFileCommand = new RelayCommand(async () => await BusyAction(OpenFileAsync), () => !IsBusy);
            OnDragOverCommand = new RelayCommand<DragEventArgs>(OnDragOver);
            OnDropCommand = new RelayCommand<DragEventArgs>(async (x) => await BusyAction(async () => await OnDrop(x)), !IsBusy);

            OnDragEnterCommand = new RelayCommand<DragEventArgs>((x) => IsDragFile = true, CanDrag);
            OnDragLeaveCommand = new RelayCommand<DragEventArgs>((x) => IsDragFile = false, CanDrag);

            ImportAnalyseCommand = new RelayCommand(async () => await BusyAction(ImportResultsAsync), () => !IsBusy);
            ExportSelectedAnalyseCommand = new RelayCommand(async () => await BusyAction(ExportResultsAsync), () => !IsBusy && SelectedItem?.AssemblyResult != null);

            CloseResultCommand = new RelayCommand<AnalyseResultViewModel>(CloseResult);

            GlobalCommand.OpenAssemblyAction = AddAssemblyResult;
        }

        public ISnackbarMessageQueue MessageQueue { get; }

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
        public ICommand ExportSelectedAnalyseCommand { get; }
        public ICommand ImportAnalyseCommand { get; }

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
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                    ex = ex.InnerException;

                MessageQueue.Enqueue($"Error : {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OpenFileAsync()
        {
            var openFileDialog = new OpenFileDialog { Title = "Open File", Filter = OpenFileFilter, Multiselect = false };
            if (openFileDialog.ShowDialog() != true) return;

            await AnalyseAsync(openFileDialog.FileName);
        }



        private async Task ImportResultsAsync()
        {
            var openFileDialog = new OpenFileDialog { Title = "Import File", Filter = ImportFileFilter, Multiselect = false };
            if (openFileDialog.ShowDialog() != true) return;

            var file = new FileInfo(openFileDialog.FileName);

            AddAssemblyResult(await file.DeserializeObject<AssemblyInformation>());
        }

        private async Task ExportResultsAsync() 
        {
            if (SelectedItem == null) return;

            var saveFileDialog = new SaveFileDialog { Title = "Save Analyse", Filter = ImportFileFilter, FileName = SelectedItem.AssemblyResult.Name };
            if (saveFileDialog.ShowDialog() != true) return;

            await SelectedItem.AssemblyResult.SerializeObject(saveFileDialog.FileName);
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

        public bool FindInfinyLoop(AssemblyInformation info, IList<AssemblyInformation> path = null)
        {
            var currentPath = path?.ToList() ?? new List<AssemblyInformation>();

            if (currentPath.Contains(info))
            {
                return false;
            }

            currentPath.Add(info);

            info.Links = info.Links.Where(x => FindInfinyLoop(x.Assembly, currentPath)).ToList();

            return true;
        }

        private void AddAssemblyResult(AssemblyInformation info)
        {
            if (info == null)
                return;

            FindInfinyLoop(info);

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