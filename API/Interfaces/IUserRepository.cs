using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetUsersAsync();  // return empty array if no result
        Task<AppUser?> GetUserByIdAsync(int id); // return null if no result
        Task<AppUser?> GetUserByUserNameAsync(string username);
        Task<IEnumerable<MemberDto>> GetMembersAsync();  // return empty array if no result
        Task<MemberDto?> GetMemberAsync(string username);
    }
}
