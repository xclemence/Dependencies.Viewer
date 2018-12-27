using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using CommonServiceLocator;
using Dependencies.Analyser.Base;
using Dependencies.Analyser.Base.Models;
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
        private IAssemblyAnalyser analyser;

        public AnalyserViewModel(IAssemblyAnalyser analyser, IInterTabClient interTabClient)
        {
            this.analyser = analyser;
            CloseCommand = new RelayCommand(() => Application.Current.Shutdown());
            OpenFileCommand = new RelayCommand(async () => await BusyAction(OpenFileAsync), () => !IsBusy);
            OnDragOverCommand = new RelayCommand<DragEventArgs>(OnDragOver);
            OnDropCommand = new RelayCommand<DragEventArgs>(async (x) => await BusyAction(async () => await OnDrop(x)), !IsBusy);

            OnDragEnterCommand = new RelayCommand<DragEventArgs>((x) => IsDragFile = true, CanDrag);
            OnDragLeaveCommand = new RelayCommand<DragEventArgs>((x) => IsDragFile = false, CanDrag);

            CloseResultCommand = new RelayCommand<AssemblyReferencesViewModel>(CloseResult);
            
            InterTabClient = interTabClient;
        }
      
        public ObservableCollection<AssemblyReferencesViewModel> AnalyseDetailsViewModels { get; } = new ObservableCollection<AssemblyReferencesViewModel>();

        private AssemblyReferencesViewModel selectedItem;
        public AssemblyReferencesViewModel SelectedItem
        {
            get => selectedItem;
            set => Set(ref selectedItem, value);
        }


        public IInterTabClient InterTabClient
        {
            get => interTabClient;
            private set => Set(ref interTabClient, value);
        }

        public bool IsBusy
        {
            get => isBusy;
            private set => Set(ref isBusy, value);
        }

        public ICommand OpenFileCommand { get; }
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

        private async Task AnalyseAsync(string filePath) =>
            AddAssemmblyResult(await analyser.AnalyseAsync(filePath));

        private void AddAssemmblyResult(AssemblyInformation info)
        {
            var newViewModel = ServiceLocator.Current.GetInstance<AssemblyReferencesViewModel>();
            newViewModel.AssemblyResult = info;
            newViewModel.AnalyseResultViewModel.OpenSubResult = AddAssemmblyResult;

            InvokeUiThread(() =>
            {
                AnalyseDetailsViewModels.Add(newViewModel);
                SelectedItem = newViewModel;
            });
        }

        public static void InvokeUiThread(Action action)
        {
            if (Dispatcher.CurrentDispatcher != Application.Current.Dispatcher)
                Application.Current.Dispatcher.Invoke(action, DispatcherPriority.Normal);
            else
                action();
        }

        private void OnDragOver(DragEventArgs e)
        {
            e.Effects = CanDrag(e) ? DragDropEffects.Move : DragDropEffects.None;
            e.Handled = true;
        }

        private bool CanDrag(DragEventArgs e) =>
            !IsBusy && e.Data.GetDataPresent(DataFormats.FileDrop);

        private void CloseResult(AssemblyReferencesViewModel x) =>
            TabablzControl.CloseItem(x);

    }
}