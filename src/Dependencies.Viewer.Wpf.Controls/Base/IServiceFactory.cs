namespace Dependencies.Viewer.Wpf.Controls.Base;

public interface IServiceFactory
{
    T Create<T>() where T : class;
}
