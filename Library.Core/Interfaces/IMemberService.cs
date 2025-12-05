using Library.Core.Models;

namespace Library.Core.Interfaces
{
public interface IMemberService
{
Task<IEnumerable<Member>> GetAllMembersAsync();
Task<Member> GetMemberByIdAsync(int id);
Task<Member> RegisterMemberAsync(Member member, string username, string password);
Task UpdateMemberAsync(Member member);
Task DeactivateMemberAsync(int id);
Task<Member> GetMemberDetailsAsync(int id);
Task<bool> ValidateMemberEligibilityAsync(int memberId);
Task<bool> CanBorrowBookAsync(int memberId);
Task RenewMembershipAsync(int memberId, int years);
Task<IEnumerable<Member>> SearchMembersAsync(string searchTerm);
Task<IEnumerable<Member>> GetExpiredMembershipsAsync();


}

}