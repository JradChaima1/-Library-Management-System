using Library.Core.Models;

namespace Library.Core.Interfaces;
{
 
public Interface IFineRepository: IRepository <Fine>{  

Task<IEnumerable<Fine>> GetUnpaidFinesAsync();
Task<IEnumerable<Fine>> GetFinesByMemberAsync(int memberId);
Task<IEnumerable<Fine>> GetFinesByStatusAsync(string status);
Task<decimal> GetTotalUnpaidFinesByMemberAsync(int memberId);
Task<IEnumerable<Fine>> GetFinesByDateRangeAsync(DateTime startDate, DateTime endDate);
Task<decimal> GetTotalCollectedFinesAsync(DateTime startDate, DateTime endDate);


}
}