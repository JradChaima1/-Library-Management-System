using Library.Core.Models;

namespace Library.Core.Interfaces;
{
 
public Interface IReservationRepository: IRepository <Staff>{  
Task<IEnumerable<Staff>> GetActiveStaffAsync();
Task<IEnumerable<Staff>> GetStaffByPositionAsync(string position);
Task<Staff> GetStaffWithDetailsAsync(int staffId);

}
}