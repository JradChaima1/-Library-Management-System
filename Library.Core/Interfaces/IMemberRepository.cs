using Library.Core.Models;

namespace Library.Core.Interfaces {

public Interface IMemberRepository : IRepository <Member>{
Task<IEnumerable<Member>> GetActiveMembersAsync();
Task<IEnumerable<Member>> GetExpiredMembershipsAsync();
Task<IEnumerable<Member>> GetMembersByTypeAsync(int membershipTypeId);
Task<Member> GetMemberByNumberAsync(string membershipNumber);
Task<Member> GetMemberWithLoansAsync(int memberId);
Task<Member> GetMemberWithFinesAsync(int memberId);
Task<IEnumerable<Member>> SearchMembersAsync(string searchTerm);
Task<bool> HasOverdueBooksAsync(int memberId);
Task<bool> HasUnpaidFinesAsync(int memberId);
Task<int> GetActiveLoanCountAsync(int memberId);

}
}