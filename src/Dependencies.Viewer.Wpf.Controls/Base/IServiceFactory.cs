namespace Dependencies.Viewer.Wpf.Controls.Base
{
    public interface IServiceFactory<T>
    {
        T Create();
    }
}