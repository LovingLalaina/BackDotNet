using AutoMapper;
using back_dotnet.Exceptions;
using back_dotnet.Models.Domain;
using back_dotnet.Models.DTOs.Post;
using back_dotnet.Repositories;

namespace back_dotnet.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository PostRepository;
        private readonly IMapper Mapper;
        private readonly ILogger<PostService> _logger;
        public PostService(IPostRepository postRepository, IMapper mapper,  ILogger<PostService> logger)
        {
            Mapper = mapper;
            PostRepository = postRepository;
            _logger = logger;
        }

        public async Task<PostDto> CreatePost(CreatePostDto createPostDto)
        {
            var postCreated = new Post
            {
                Name = createPostDto.Name,
                IdDepartment = Guid.Parse(createPostDto.Department)
            };
            var post = await PostRepository.CreatePost(postCreated);

            return Mapper.Map<PostDto>(post);
        }

        public async Task<PostDto?> DeletePost(Guid id)
        {
            var post = await PostRepository.DeletePost(id);

            return Mapper.Map<PostDto>(post);
        }

        public async Task<List<PostDto>> GetAllPost()
        {
            var posts = await PostRepository.GetAllPost();

            return Mapper.Map<List<PostDto>>(posts);
        }

        public async Task<PostDto?> GetPostById(Guid id)
        {
            var post = await PostRepository.GetPostById(id);
            if (post is null)
            {
                throw new HttpException(StatusCodes.Status404NotFound, "Aucun poste existant avec cet identifiant");
            }

            return Mapper.Map<PostDto>(post);
        }

        public async Task<PostDto> UpdatePost(Guid id, UpdatePostDto updatePostDto)
        {
            var updatedPost = new Post
            {
                Name = updatePostDto.Name,
                IdDepartment = Guid.Parse(updatePostDto.Department)
            };
            var post = await PostRepository.UpdatePost(id, updatedPost);

            return Mapper.Map<PostDto>(post);
        }

        public async Task<List<SearchPostDto>> SearchPosts(string search)
        {
            try
            {
                return Mapper.Map<List<SearchPostDto>>( await PostRepository.SearchPosts( search ) );
            }
            catch(Exception unknowknError)
            {
                if (unknowknError is not HttpException knownError)
                {
                    _logger.LogError(unknowknError, "Une erreur s'est produite lors de la recherche de poste");
                    throw;
                }
                throw knownError;
            }
        }
    }
}