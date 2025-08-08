using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Model;


using UserManagementAPI.Responses;
namespace UserManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private static readonly List<User> Users = new List<User>();

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            try
            {
                int page = 1, pageSize = 50;
                if (Request.Query.ContainsKey("page"))
                    int.TryParse(Request.Query["page"], out page);
                if (Request.Query.ContainsKey("pageSize"))
                    int.TryParse(Request.Query["pageSize"], out pageSize);
                var pagedUsers = Users.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                
                if(pagedUsers.Count == 0)
                    return NotFound(new ApiResponse {
                        Success = false,
                        Message = "No users found.",
                        Data = null
                    });

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Users retrieved successfully.",
                    Data = pagedUsers
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse {
                    Success = false,
                    Message = $"Internal server error: {ex.Message}",
                    Data = null
                });
            }
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetUser(int id)
        {
            try
            {
                var user = Users.FirstOrDefault(u => u.Id == id);
                if (user == null)
                    return NotFound();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse {
                    Success = false,
                    Message = $"Internal server error: {ex.Message}",
                    Data = null
                });
            }
        }

        [HttpPost]
        public ActionResult<User> CreateUser([FromBody] User user)
        {
            try
            {
                user.Id = 0;
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (Users.Any(u => u.Email == user.Email))
                    return Conflict("A user with this email already exists.");
                user.Id = Users.Count > 0 ? Users[^1].Id + 1 : 1;
                Users.Add(user);
                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse {
                    Success = false,
                    Message = $"Internal server error: {ex.Message}",
                    Data = null
                });
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, User updatedUser)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var user = Users.FirstOrDefault(u => u.Id == id);
                if (user == null)
                    return NotFound();

                if (Users.Any(u => u.Email == updatedUser.Email && u.Id != id))
                    return Conflict("A user with this email already exists.");
                user.Name = updatedUser.Name;
                user.Email = updatedUser.Email;
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse {
                    Success = false,
                    Message = $"Internal server error: {ex.Message}",
                    Data = null
                });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                var user = Users.FirstOrDefault(u => u.Id == id);
                if (user == null)
                    return NotFound();
                Users.Remove(user);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse {
                    Success = false,
                    Message = $"Internal server error: {ex.Message}",
                    Data = null
                });
            }
        }
    }
}
