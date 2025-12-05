namespace Library.Core.Interfaces
{
public interface ILoanService
{

Task<BookLoan> IssueBookAsync(int memberId, int bookId, int staffId);
Task<BookLoan> ReturnBookAsync(int loanId, int staffId);
Task<BookLoan> RenewLoanAsync(int loanId);
Task<IEnumerable<BookLoan>> GetActiveLoansByMemberAsync(int memberId);
Task<IEnumerable<BookLoan>> GetOverdueLoansAsync();
Task<IEnumerable<BookLoan>> GetLoanHistoryAsync(int memberId);
Task<bool> ValidateLoanEligibilityAsync(int memberId, int bookId);
Task<int> CalculateOverdueDaysAsync(int loanId);
Task<IEnumerable<BookLoan>> GetDueSoonLoansAsync(int days);
Task CancelLoanAsync(int loanId, string reason);

}

}