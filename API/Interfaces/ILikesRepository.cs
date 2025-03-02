using API.DTOs;
using API.Entities;
using API.Helper;

namespace API.Interfaces
{
    public interface ILikesRepository
    {
        Task<UserLike?> GetUserLike(int sourceUserId, int targetUserId);
        Task<PageList<MemberDto>> GetUserLikes(LikesParams likesParams);
        Task<IEnumerable<int>> GetCurrentUserLikeIds(int currentUserId);

        void DeleteLike(UserLike like);
        void AddLike(UserLike like);

    }
}
