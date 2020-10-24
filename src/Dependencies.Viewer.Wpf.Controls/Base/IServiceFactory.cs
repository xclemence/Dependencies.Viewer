namespace Dependencies.Viewer.Wpf.Controls.Base
{
    public interface IServiceFactory<out T>
        where T : class
    {
        T Create();
    }
}