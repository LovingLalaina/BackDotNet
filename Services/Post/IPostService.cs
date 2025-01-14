using back_dotnet.Models.DTOs;
using back_dotnet.Models.DTOs.Post;

namespace back_dotnet.Services
{
    public interface IPostService
    {
        Task<List<PostDto>> GetAllPost();
        Task<PostDto> CreatePost(CreatePostDto createPostDto);
        Task<PostDto> UpdatePost(Guid id, UpdatePostDto createPostDto);
        Task<PostDto?> GetPostById(Guid id);
        Task<PostDto?> DeletePost(Guid id);
        Task<List<SearchPostDto>> SearchPosts(string search);
    }
}