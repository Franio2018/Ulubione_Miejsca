using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using UlubioneMiejsca.DataModels;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace UlubioneMiejsca.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UlubioneMiejscaController
    {
        [Tags("GetUserFavoritePlaces")]
        [HttpGet]
        public List<Location> GetUserFavoritePlaces(Guid userId)
        {
            using (var db = new MyDbContext())
            {
                var user = db.Users.Include(u => u.Locations).FirstOrDefault(u => u.Id == userId);
                if (user == null)
                {
                    throw new Exception($"User with ID {userId} not found");
                }

                return user.Locations;
            }
        }
        [Tags("AddUser")]
        [HttpPost]
        public async Task<string> AddUser([FromBody] User userData)
        {
            var validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(userData, new ValidationContext(userData), validationResults);

            if (!isValid)
            {
                // If the input data is not valid, return a bad request response with validation errors
                return string.Join("; ", validationResults.Select(r => r.ErrorMessage));
            }

            using (var _context = new MyDbContext())
            {
                var user = new User
                {
                    Name = userData.Name,
                    Surname = userData.Surname,
                    Email = userData.Email ?? "Unknown",
                    Password = userData.Password,
                    Phone = userData.Phone ?? "Unknown"
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            return "User Has been added";
        }
        [Tags("AddLocationToUser")]
        [HttpPost("{userId}/locations")]
        public async Task<IActionResult> AddLocationToUser(Guid userId, [FromBody] Location location)
        {
            using (var _context = new MyDbContext())
            {
                var user = await _context.Users.Include(u => u.Locations).SingleOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    return new NotFoundObjectResult("Not found user with this id");
                }

                if (location == null || string.IsNullOrEmpty(location.Address))
                {
                    return new BadRequestObjectResult("Location address is required");
                }

                if (user.Locations.Any(l => l.Address == location.Address))
                {
                    return new BadRequestObjectResult("A location with the same address already exists for this user");
                }

                //user.Locations.Add(location);
                _context.Locations.Add(location);
                await _context.SaveChangesAsync();


                return new OkObjectResult("Location has been added to the user");
            }
        }


    }
}