using AutoMapper;
using back_dotnet.Exceptions;
using back_dotnet.Models.DTOs.Post;
using back_dotnet.Services;
using back_dotnet.Utils;
using Microsoft.AspNetCore.Mvc;

namespace back_dotnet.Controllers
{
    [ApiController]
    [Route("post")]
    public class PostController : ControllerBase
    {
        private readonly IPostService PostService;
        public IMapper Mapper { get; }
        public PostController(IPostService postService, IMapper mapper)
        {
            this.Mapper = mapper;
            this.PostService = postService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPost()
        {
            try
            {
                var posts = await PostService.GetAllPost();

                return Ok(posts);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDto createPostDto)
        {
            if (!TryValidateModel(createPostDto))
            {
                var ErrorValidationResponse = ValidationResponse.GetResponseValidation(ModelState);

                return StatusCode(StatusCodes.Status422UnprocessableEntity, new { status = StatusCodes.Status422UnprocessableEntity, error = ErrorValidationResponse });
            }

            try
            {
                var post = await PostService.CreatePost(createPostDto);

                return CreatedAtAction(nameof(GetPostById), new { id = post.Id }, post);
            }
            catch (Exception ex)
            {
                var innerException = ex as HttpException;
                if (innerException is not null)
                {
                    return StatusCode(innerException.Status, new { status = innerException.Status, error = innerException.Message });
                }
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdatePostAsync([FromRoute] Guid id, [FromBody] UpdatePostDto updatePostDto)
        {

            if (!TryValidateModel(updatePostDto))
            {
                var ErrorValidationResponse = ValidationResponse.GetResponseValidation(ModelState);

                return StatusCode(StatusCodes.Status422UnprocessableEntity, new { status = StatusCodes.Status422UnprocessableEntity, error = ErrorValidationResponse });
            }

            try
            {
                var post = await PostService.UpdatePost(id, updatePostDto);

                return Ok(post);
            }
            catch (Exception ex)
            {
                var innerException = ex as HttpException;
                if (innerException is not null)
                {
                    return StatusCode(innerException.Status, new { status = innerException.Status, error = innerException.Message });
                }
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetPostById([FromRoute] Guid id)
        {
            try
            {
                var post = await PostService.GetPostById(id);

                return Ok(post);
            }
            catch (Exception ex)
            {
                var innerException = ex as HttpException;
                if (innerException is not null)
                {
                    return StatusCode(innerException.Status, new { status = innerException.Status, error = innerException.Message });
                }
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeletePost([FromRoute] Guid id)
        {
            try
            {
                var post = await PostService.DeletePost(id);

                return StatusCode(StatusCodes.Status200OK, new { message = "Poste supprimé" });
            }
            catch (Exception ex)
            {
                var innerException = ex as HttpException;
                if (innerException is not null)
                {
                    return StatusCode(innerException.Status, new { status = innerException.Status, error = innerException.Message });
                }
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult> SearchPosts([FromQuery] string search)
        {
            try
            {
                return Ok( await PostService.SearchPosts( search ) );
            }
            catch(Exception unknowknException)
            {
                HttpException? knowknException = unknowknException as HttpException;
                if( knowknException == null )   //ERREUR INATTENDU
                    return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, error = "Une erreur interne s'est produite, plus d'information dans le journal de log" } );
                return StatusCode(knowknException.Status, new { status = knowknException.Status, error = knowknException.Message } );
            }
        }
    }
}