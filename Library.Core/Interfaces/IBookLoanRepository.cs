using Libray.Core.Models;
namespace Libray.Core.Interfaces 
{
public interface IBookLoanRepository: IRepository<BookLoan> {
Task<IEnumerable<BookLoan>> GetActiveLoansByMemberAsync(int memberId);
Task<IEnumerable<BookLoan>> GetOverdueLoansAsync();
Task<IEnumerable<BookLoan>> GetLoansByStatusAsync(string status);
Task<IEnumerable<BookLoan>> GetLoansByDateRangeAsync(DateTime startDate, DateTime endDate);
Task<IEnumerable<BookLoan>> GetLoanHistoryByMemberAsync(int memberId);
Task<IEnumerable<BookLoan>> GetLoanHistoryByBookAsync(int bookId);
Task<BookLoan> GetActiveLoanByBookAndMemberAsync(int bookId, int memberId);
Task<int> GetOverdueDaysAsync(int loanId);
Task<IEnumerable<BookLoan>> GetDueSoonLoansAsync(int days);
}
}