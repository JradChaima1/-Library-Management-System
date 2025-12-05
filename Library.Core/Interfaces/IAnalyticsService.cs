using Library.Core.Models;

namespace Library.Core.Interfaces
{
public interface IAnalyticsService
{
Task<DashboardStats> GetDashboardStatsAsync(int? staffId = null);
Task<IEnumerable<BookStats>> GetPopularBooksAsync(int count);
Task<IEnumerable<MemberStats>> GetMostActiveMembersAsync(int count);
Task<IEnumerable<CategoryStats>> GetCategoryStatisticsAsync();
Task<LoanTrendStats> GetLoanTrendsAsync(DateTime startDate, DateTime endDate);
Task<RevenueStats> GetRevenueReportAsync(DateTime startDate, DateTime endDate);
Task<int> GetTotalBooksAsync();
Task<int> GetTotalMembersAsync();
Task<int> GetActiveLoansCountAsync();
Task<int> GetOverdueLoansCountAsync();
Task<decimal> GetTotalUnpaidFinesAsync();

}
}