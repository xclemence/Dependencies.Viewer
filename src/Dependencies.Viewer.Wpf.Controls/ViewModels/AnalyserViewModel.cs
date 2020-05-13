using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Dependencies.Analyser.Base;
using Dependencies.Analyser.Base.Models;
using Dependencies.Exchange.Base;
using Dependencies.Viewer.Wpf.Controls.Extensions;
using Dependencies.Viewer.Wpf.Controls.Base;
using Dependencies.Viewer.Wpf.Controls.Views;
using Dragablz;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;

public class ExchangeCommand
{
    public string Title { get; set; }
    public ICommand Command { get; set; }
}

namespace Dependencies.Viewer.Wpf.Controls.ViewModels
{
    public class AnalyserViewModel : ObservableObject
    {
        private const string OpenFileFilter =
            "Software (*.exe)|*.exe|" +
            "Assembly (*.dll)|*.dll|" +
            "All Files|*.*";

        private bool isBusy;
        private bool isDragFile;
        private IInterTabClient interTabClient;
        private readonly AnalyserProvider analyserProvider;
        private readonly IAnalyserServiceFactory<AnalyseResultViewModel> analyserViewModelFactory;
        private readonly IList<IImportAssembly> importServices;
        private readonly IList<IExportAssembly> exportServices;

        private AnalyseResultViewModel selectedItem;
        private bool isSettingsOpen;

        public AnalyserViewModel(AnalyserProvider analyserProvider,
                                 IAnalyserServiceFactory<AnalyseResultViewModel> analyserViewModelFactory,
                                 IInterTabClient interTabClient,
                                 SettingsViewModel settingsViewModel,
                                 ISnackbarMessageQueue messageQueue,
                                 IEnumerable<IImportAssembly> importServices,
                                 IEnumerable<IExportAssembly> exportServices)
        {
            this.analyserProvider = analyserProvider;
            this.analyserViewModelFactory = analyserViewModelFactory;
            InterTabClient = interTabClient;
            SettingsViewModel = settingsViewModel;
            MessageQueue = messageQueue;
            this.exportServices = exportServices.ToList();
            this.importServices = importServices.ToList();

            SettingsCommand = new Command(() => IsSettingsOpen = true);
            CloseCommand = new Command(() => Application.Current.Shutdown());
            OpenFileCommand = new Command(async () => await BusyAction(OpenFileAsync).ConfigureAwait(false), () => !IsBusy);
            OnDragOverCommand = new Command<DragEventArgs>(OnDragOver);
            OnDropCommand = new Command<DragEventArgs>(async (x) => await BusyAction(async () => await OnDrop(x).ConfigureAwait(false)).ConfigureAwait(false), _ => !IsBusy);

            OnDragEnterCommand = new Command<DragEventArgs>((x) => IsDragFile = true, CanDrag);
            OnDragLeaveCommand = new Command<DragEventArgs>((x) => IsDragFile = false, CanDrag);

            BuildExportCommand();
            BuildImportCommand();

            CloseResultCommand = new Command<AnalyseResultViewModel>(CloseResult);

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

        public IList<ExchangeCommand> ExportCommands { get; private set; }
        public IList<ExchangeCommand> ImportCommands { get; private set; }

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
                await actionAsync().ConfigureAwait(false);
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

            await AnalyseAsync(openFileDialog.FileName).ConfigureAwait(false);
        }

        private async Task OnDrop(DragEventArgs e)
        {
            e.Handled = true;
            IsDragFile = false;
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

            var filenames = (string[])e.Data.GetData(DataFormats.FileDrop, true);

            if (filenames.Length == 0) return;

            await AnalyseAsync(filenames[0]).ConfigureAwait(false);
        }

        public async Task InitialiseAsync(string filePath = null)
        {
            if (filePath == null)
                return;

            await BusyAction(async () => await AnalyseAsync(filePath).ConfigureAwait(false)).ConfigureAwait(false);
        }

        private async Task AnalyseAsync(string filePath)
        {
            var analyser = analyserProvider.CurrentAnalyserFactory.GetAnalyser();
            AddAssemblyResult(await analyser.AnalyseAsync(filePath).ConfigureAwait(false));
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

        private void BuildExportCommand()
        {
            ExportCommands = exportServices.Select(x => new ExchangeCommand
            {
                Title = x.Name,
                Command = new Command(async () => await BusyAction(async () => await ExportAsync(x).ConfigureAwait(false)).ConfigureAwait(false), () => x.IsReady && !IsBusy && SelectedItem?.AssemblyResult != null)
            }).ToList(); ;
        }

        private void BuildImportCommand()
        {
            ImportCommands = importServices.Select(x => new ExchangeCommand
            {
                Title = x.Name,
                Command = new Command(async () => await BusyAction(async () => await ImportAsync(x).ConfigureAwait(false)).ConfigureAwait(false), () => x.IsReady && !IsBusy)
            }).ToList(); ;
        }

        private async Task ExportAsync(IExportAssembly exportAssembly)
        {
            var (assembly, dependencies) = selectedItem.AssemblyResult.ToExchangeModel();

            await exportAssembly.ExportAsync(assembly, dependencies, CreateExchangeView).ConfigureAwait(false);
        }

        private async Task ImportAsync(IImportAssembly importAssembly)
        {
            var result = await importAssembly.ImportAsync(CreateExchangeView).ConfigureAwait(false);

            if (result == default)
                return;

            AddAssemblyResult(result.Assembly.ToInformationModel(result.Dependencies));
        }

        private static async Task<T> CreateExchangeView<T>(UserControl view, IExchangeViewModel<T> viewModel)
        {
            var exchangeView = new ExchangeView();
            var exchanegViewModel = new ExchangeViewModel<T>((x) => CloseExchangeDialog(x), viewModel);

            exchangeView.DataContext = exchanegViewModel;
            exchangeView.Control.Content = view;
            view.DataContext = viewModel;

            var result = await DialogHost.Show(exchangeView).ConfigureAwait(false);

            return (T)result;
        }

        private static void CloseExchangeDialog<T>(T x) => new Action((() => DialogHost.CloseDialogCommand.Execute(x, null))).InvokeUiThread();

        private void OnDragOver(DragEventArgs e)
        {
            e.Effects = CanDrag(e) ? DragDropEffects.Move : DragDropEffects.None;
            e.Handled = true;
        }

        private bool CanDrag(DragEventArgs e) => !IsBusy && e.Data.GetDataPresent(DataFormats.FileDrop);

        private void CloseResult(AnalyseResultViewModel x) => TabablzControl.CloseItem(x);
    }
}