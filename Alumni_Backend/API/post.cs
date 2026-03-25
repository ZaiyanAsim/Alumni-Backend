using Alumni_Portal.Infrastructure.Data_Models;
using Entity_Directories.Services;
using Entity_Directories.Services;
using Entity_Directories.Services.DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;





namespace Admin.Controllers

{


    [Route("api/Admin/posts")]
    [ApiController]

    public class PostController : Controller

    {

        private PostService _handler;

        public PostController(PostService handler)
        {
            _handler= handler;
        }

        [HttpGet]

        public async Task<ActionResult> GetPosts([FromQuery] PostFilters filters, int _page, int _size)
        {

            var response = await _handler.GetPostsPaginated(filters, _page, _size);
            return Ok(response);


        }

        [HttpGet("tagging-search")]
        public async Task<ActionResult> PostTaggingSearch([FromQuery] PostTaggingSearchFilters filters)


        {
            if (filters.Search_Term == null)
            {
                return BadRequest("Search Term is NULL");
            }
            var response = await _handler.PostMentions(filters.Search_Term, filters.Tag_Type);
            return Ok(response);

        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] PostCreationDTO postDetails)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            int createdId=await _handler.CreatePost(postDetails);

            var response = new
            {
                Status = "Success",
                Message = "Project successfully created.",
                Post_ID = createdId,
                accessUrl = $"/api/Admin/posts/{createdId}"
            };
            return Ok(response);
        }

        [HttpPost("media{postId}")]

        public async Task<IActionResult> AddMediaToPost([FromForm] List<IFormFile> media, int postId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _handler.AddMedia(media, postId);
            var response = new
            {
                Status = "Success",
                message = "Media successfully associated with post." 
            };

            return Ok(response);
        }

        [HttpDelete("delete")]

        public async Task<IActionResult> DeleteProjects([FromBody] List<int> postIds)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Console.WriteLine(postIds == null ? "IDS IS NULL" : $"IDS COUNT: {postIds.Count}");

            List<int> failedDeletes = await _handler.DeletePostsBulk(postIds);

            var response = new
            {

                failedDeletes = failedDeletes
            };


            return Ok(response);




        }

    }

}
