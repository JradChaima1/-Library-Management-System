namespace Library.Core.Interfaces
{
public interface IFineService
{
Task<Fine> GenerateFineAsync(int loanId);
Task<Fine> RecordPaymentAsync(int fineId, decimal amount, string paymentMethod, string transactionId);
Task<Fine> WaiveFineAsync(int fineId, int staffId, string reason);
Task<IEnumerable<Fine>> GetMemberFinesAsync(int memberId);
Task<IEnumerable<Fine>> GetUnpaidFinesAsync();
Task<decimal> CalculateFineAmountAsync(int loanId);
Task<decimal> GetTotalUnpaidFinesAsync(int memberId);
Task SendFineRemindersAsync();

}
}