using back_dotnet.Models.Domain;
using back_dotnet.Models.DTOs.Post;

namespace back_dotnet.Repositories
{
    public interface IPostRepository
    {
        Task<List<Post>> GetAllPost();
        Task<Post> CreatePost(Post post);
        Task<Post> UpdatePost(Guid id, Post post);
        Task<Post?> GetPostById(Guid id);
        Task<Post?> DeletePost(Guid id);
        Task<List<Post>> SearchPosts(string search);
    }
}