using back_dotnet.Exceptions;
using back_dotnet.Models.Domain;
using back_dotnet.Models.DTOs.Post;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace back_dotnet.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly HairunSiContext DbContext;
        public PostRepository(HairunSiContext dbContext)
        {
            this.DbContext = dbContext;
        }

        public async Task<Post> CreatePost(Post post)
        {
            var department = await DbContext.Departments.FirstOrDefaultAsync(d => d.Id == post.IdDepartment);
            if (department is null)
            {
                throw new HttpException(StatusCodes.Status404NotFound, "Le département n'existe pas");
            }

            try
            {
                var postCreated = new Post
                {
                    Name = post.Name,
                    IdDepartment = department.Id,
                    CreatedAt = new DateTime()
                };

                await DbContext.Posts.AddAsync(postCreated);
                await DbContext.SaveChangesAsync();

                return postCreated;
            }
            catch (DbUpdateException ex)
            {
                PostgresException? innerException = ex.InnerException as PostgresException;

                if (innerException is not null && innerException.SqlState == "23505")
                {
                    throw new HttpException(StatusCodes.Status409Conflict, "Le poste existe déja");
                }

                throw;
            }
        }

        public async Task<List<Post>> GetAllPost()
        {
            return await DbContext.Posts.Include(p => p.IdDepartmentNavigation).OrderByDescending(p => p.CreatedAt).ToListAsync();
        }

        public async Task<Post> UpdatePost(Guid id, Post post)
        {
            var department = await DbContext.Departments.FirstOrDefaultAsync(d => d.Id == post.IdDepartment);
            if (department is null)
            {
                throw new HttpException(StatusCodes.Status404NotFound, "Le département n'existe pas");
            }

            var postUpdated = await GetPostById(id);
            if (postUpdated is null)
            {
                throw new HttpException(StatusCodes.Status404NotFound, "Le poste n'existe pas");
            }

            try
            {
                postUpdated.Name = post.Name;
                postUpdated.IdDepartment = post.IdDepartment;
                await DbContext.SaveChangesAsync();

                return postUpdated;
            }
            catch (DbUpdateException ex)
            {
                PostgresException? innerException = ex.InnerException as PostgresException;

                if (innerException is not null && innerException.SqlState == "23505")
                {
                    throw new HttpException(StatusCodes.Status409Conflict, "Le poste existe déja");
                }

                throw;
            }
        }

        public async Task<Post?> GetPostById(Guid id)
        {
            return await DbContext.Posts.Include(p => p.IdDepartmentNavigation).FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Post?> DeletePost(Guid id)
        {
            var post = await GetPostById(id);

            if (post is null)
            {
                throw new HttpException(StatusCodes.Status404NotFound, "Le poste n'existe pas");
            }

            DbContext.Remove(post);
            await DbContext.SaveChangesAsync();

            return post;
        }

        public async Task<List<Post>> SearchPosts(string search)
        {
            search = search.ToLower();
            return await DbContext.Posts
            .Include(post => post.IdDepartmentNavigation)
                .ThenInclude( departement => departement.IdRoleNavigation)
            .AsSplitQuery()
            .Where(post => post.Name.ToLower().Contains(search)
                || post.IdDepartmentNavigation.Name.ToLower().Contains(search)
                //  A VOIR SI AJOUTER CE CRITERE ENCOMMENTAIRE
                /*|| post.IdDepartmentNavigation.IdRoleNavigation.Name.ToLower().Contains(search)*/)
            .ToListAsync();
        }
    }
}