using API.DTOs;
using API.Entities;
using API.Helper;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<IEnumerable<AppUser>> GetUsersAsync();  // return empty array if no result
        Task<AppUser?> GetUserByIdAsync(int id); // return null if no result
        Task<AppUser?> GetUserByUserNameAsync(string username);
        Task<PageList<MemberDto>> GetMembersAsync(UserParams userParams);  // return empty array if no result
        Task<MemberDto?> GetMemberAsync(string username);
    }
}
