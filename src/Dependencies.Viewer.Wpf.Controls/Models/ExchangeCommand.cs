using System.Windows.Input;

namespace Dependencies.Viewer.Wpf.Controls.Models
{
    public record ExchangeCommand(string Title, ICommand Command);
}
