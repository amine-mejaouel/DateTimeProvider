// ReSharper disable once CheckNamespace
namespace DateTimeProviders
{
    public interface IDateTimeProvider
    {
        System.DateTimeOffset Now { get; }
    }
}