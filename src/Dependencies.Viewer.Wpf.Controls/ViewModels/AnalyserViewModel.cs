using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Dependencies.Exchange.Base;
using Dependencies.Viewer.Wpf.Controls.Base;
using Dependencies.Viewer.Wpf.Controls.Extensions;
using Dependencies.Viewer.Wpf.Controls.Models;
using Dependencies.Viewer.Wpf.Controls.Services;
using Dependencies.Viewer.Wpf.Controls.ViewModels.Settings;
using Dependencies.Viewer.Wpf.Controls.Views;
using Dragablz;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;

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
        private readonly IServiceFactory<AnalyseResultViewModel> analyserViewModelFactory;
        private readonly AppLoggerService<AnalyserViewModel> logger;
        private readonly IList<IImportAssembly> importServices;
        private readonly IList<IExportAssembly> exportServices;

        private AnalyseResultViewModel? selectedItem;
        private bool isSettingsOpen;

        public AnalyserViewModel(AnalyserProvider analyserProvider,
                                 IServiceFactory<AnalyseResultViewModel> analyserViewModelFactory,
                                 IInterTabClient interTabClient,
                                 SettingsViewModel settingsViewModel,
                                 IEnumerable<IImportAssembly> importServices,
                                 IEnumerable<IExportAssembly> exportServices,
                                 AppLoggerService<AnalyserViewModel> logger)
        {
            this.analyserProvider = analyserProvider;
            this.analyserViewModelFactory = analyserViewModelFactory;
            this.interTabClient = interTabClient;
            SettingsViewModel = settingsViewModel;
            this.logger = logger;
            this.exportServices = exportServices.ToList();
            this.importServices = importServices.ToList();

            SettingsCommand = new Command(() => IsSettingsOpen = true);
            CloseCommand = new Command(() => Application.Current.Shutdown());
            OpenFileCommand = new Command(async () => await BusyActionAsync(OpenFileAsync).ConfigureAwait(false), () => !IsBusy);
            OnDragOverCommand = new Command<DragEventArgs>(OnDragOver);
            OnDropCommand = new Command<DragEventArgs>(async (x) => await BusyActionAsync(async () => await OnDrop(x).ConfigureAwait(false)).ConfigureAwait(false), _ => !IsBusy);

            OnDragEnterCommand = new Command<DragEventArgs>((x) => IsDragFile = true, CanDrag);
            OnDragLeaveCommand = new Command<DragEventArgs>((x) => IsDragFile = false, CanDrag);

            ExportCommands = GenerateExportCommand();
            ImportCommands= GenerateImportCommand();

            CloseResultCommand = new Command<AnalyseResultViewModel>(CloseResult);

            GlobalCommand.OpenAssemblyAction = OpenSubAssembly;
            GlobalCommand.ViewParentReference = ViewParentReferenceAsync;
        }

        public ISnackbarMessageQueue MessageQueue => logger.MessageQueue;

        public ObservableCollection<AnalyseResultViewModel> AnalyseDetailsViewModels { get; } = new ObservableCollection<AnalyseResultViewModel>();

        public AnalyseResultViewModel? SelectedItem
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

        public IList<ExchangeCommand> ExportCommands { get; }
        public IList<ExchangeCommand> ImportCommands { get; }

        public Func<DragEventArgs, bool> CanDragFunc => (x) => CanDrag(x);

        public bool IsDragFile
        {
            get => isDragFile;
            set => Set(ref isDragFile, value);
        }

        private async Task BusyActionAsync(Func<Task> actionAsync)
        {
            IsBusy = true;

            try
            {
                await actionAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.LogError("", ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void BusyAction(Action action)
        {
            IsBusy = true;

            try
            {
                action();
            }
            catch (Exception ex)
            {
                logger.LogError("", ex);
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

        public async Task InitialiseAsync(string? filePath = null)
        {
            if (filePath == null)
                return;

            await BusyActionAsync(async () => await AnalyseAsync(filePath).ConfigureAwait(false)).ConfigureAwait(false);
        }

        private async Task AnalyseAsync(string filePath)
        {
            var analyser = analyserProvider.CurrentAnalyserFactory?.GetAnalyser();

            if (analyser is null) return;

            var (assembly, links) = await analyser.AnalyseAsync(filePath).ConfigureAwait(false);

            AddAssemblyResult(assembly.ToAssemblyModel(links.Values));
        }

        private void AddAssemblyResult(AssemblyModel assembly)
        {
            var newViewModel = analyserViewModelFactory.Create();
            newViewModel.AssemblyResult = assembly;

            new Action(() =>
            {
                AnalyseDetailsViewModels.Add(newViewModel);
                SelectedItem = newViewModel;
            }).InvokeUiThread();
        }

        private void OpenSubAssembly(AssemblyModel assembly)
        {
            Task.Run(() => BusyAction(() => AddAssemblyResult(assembly.IsolatedShadowClone())));
        }

        public async Task ViewParentReferenceAsync(AssemblyModel baseAssembly, ReferenceModel reference)
        {
            await BusyActionAsync(async () =>
            {
                var vm = new AssemblyParentsViewModel(reference.LoadedAssembly, baseAssembly);

                _ = await DialogHost.Show(vm).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        private IList<ExchangeCommand> GenerateExportCommand()
        {
            return exportServices.Select(x => new ExchangeCommand
            (
                x.Name,
                new Command(async () => await BusyActionAsync(async () => await ExportAsync(x).ConfigureAwait(false)).ConfigureAwait(false), () => x.IsReady && !IsBusy && SelectedItem?.AssemblyResult is not null)
            )).ToList();
        }

        private IList<ExchangeCommand> GenerateImportCommand()
        {
            return importServices.Select(x => new ExchangeCommand
            (
                x.Name,
                new Command(async () => await BusyActionAsync(async () => await ImportAsync(x).ConfigureAwait(false)).ConfigureAwait(false), () => x.IsReady && !IsBusy)
            )).ToList();
        }

        private async Task ExportAsync(IExportAssembly exportAssembly)
        {
            if (selectedItem?.AssemblyResult is null) return;

            var (assembly, dependencies) = selectedItem.AssemblyResult.ToExchangeModel();

            await exportAssembly.ExportAsync(assembly, dependencies, CreateExchangeView).ConfigureAwait(false);
        }

        private async Task ImportAsync(IImportAssembly importAssembly)
        {
            var result = await importAssembly.ImportAsync(CreateExchangeView).ConfigureAwait(false);

            if (result == default)
                return;

            AddAssemblyResult(result.Assembly.ToAssemblyModel(result.Dependencies));
        }

        private async Task<T> CreateExchangeView<T>(UserControl view, IExchangeViewModel<T> viewModel)
        {
            var exchangeView = new ExchangeView();
            var exchangeViewModel = new ExchangeViewModel<T>((x) => CloseExchangeDialog(x), viewModel, logger.Logger);

            exchangeView.DataContext = exchangeViewModel;
            exchangeView.Control.Content = view;
            view.DataContext = viewModel;

            var result = await DialogHost.Show(exchangeView).ConfigureAwait(false);

            if (result is null)
                return default(T)!;

            return (T)result;
        }

        private static void CloseExchangeDialog<T>(T x) => new Action(() => DialogHost.CloseDialogCommand.Execute(x, null)).InvokeUiThread();

        private void OnDragOver(DragEventArgs e)
        {
            e.Effects = CanDrag(e) ? DragDropEffects.Move : DragDropEffects.None;
            e.Handled = true;
        }

        private bool CanDrag(DragEventArgs? e) => e is not null && !IsBusy && e.Data.GetDataPresent(DataFormats.FileDrop);

        private void CloseResult(AnalyseResultViewModel x) => TabablzControl.CloseItem(x);
    }
}