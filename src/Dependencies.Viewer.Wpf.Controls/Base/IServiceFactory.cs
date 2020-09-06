namespace Dependencies.Viewer.Wpf.Controls.Base
{
    public interface IServiceFactory<out T>
    {
        T Create();
    }
}