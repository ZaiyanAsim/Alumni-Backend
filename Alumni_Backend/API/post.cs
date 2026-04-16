using Alumni_Portal.FileUploads;
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
        private FileService _fileService;


        public PostController(PostService handler, FileService fileService )
        {
            _handler = handler;
            _fileService = fileService;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult> GetPostById(int id)
        {
            var post = await _handler.GetPostById(id);
            if (post == null) return NotFound();
            return Ok(post);
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
                Message = "Post successfully created.",
                Post_ID = createdId,
                accessUrl = $"/api/Admin/posts/{createdId}"
            };
            return Ok(response);
        }



        [HttpGet("{postId}/media")]
        public async Task<IActionResult> GetPostMedia(int postId)
        {
            var media = await _handler.GetMediaByPostId(postId);
            var result = media.Select(m => new
            {
                m.Post_Media_Id,
                m.Media_Title,
                m.Media_File_Name,
                m.Media_File_Location,
                m.Media_Date
            });
            return Ok(result);
        }
        

        [HttpPost("media/{postId}")]
        public async Task<IActionResult> AddMediaToPost([FromForm] List<IFormFile> media, int postId)
        {
            var uploadResult = await _fileService.UploadMedia(media);

            if (!uploadResult.UploadedFiles.Any())
                return BadRequest(new { message = uploadResult.errorMessage, errors = uploadResult.errors });

            var postMediaList = uploadResult.UploadedFiles.Select(file => new Post_Media
            {
                Post_Id = postId,
                Media_Title = file.Media_Title,
                Media_Date = file.Media_Date,
                Media_File_Location = file.Media_File_Location,
                Media_File_Name = file.Media_File_Name,
                Progress_Value = "Uploaded",
                Status_Value = "Active",
                Progress_Id = 1,
                Status_Id = 1,
                Created_By_Id = 1,
                Created_By_Name = "Admin",
                Created_Date = DateTime.UtcNow,
            }).ToList();

            await _handler.AddMedia(postMediaList);

            return Ok(new { Status = "Success", message = "Media successfully associated with post." });
        }

        [HttpDelete("delete")]

        public async Task<IActionResult> DeletePosts([FromBody] List<int> postIds)
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
