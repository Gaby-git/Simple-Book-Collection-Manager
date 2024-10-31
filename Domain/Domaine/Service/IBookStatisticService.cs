using Domaine.Model;

namespace Domaine.Service
{
    public interface IBookStatisticsService
    {
        int GetTotalBooks();

        int GetBooksReadCount();

        int GetBooksUnreadCount();

        string GetFavoriteGenre();

        BookStatistics GenerateStatisticsReport();
    }
}
