using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using UlubioneMiejsca.DataModels;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Linq;
using Microsoft.Data.SqlClient;
using UlubioneMiejsca.Methods;
using UlubioneMiejsca.DataModels.DbModels;
using Microsoft.AspNetCore.Server.IIS.Core;
using UlubioneMiejsca.DataModels.Respones;

namespace UlubioneMiejsca.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UlubioneMiejscaController
    {
        [Tags("GetUserFavoritePlaces")]
        [HttpGet]
        public List<LocationResponse> GetUserFavoritePlaces(Guid userId)
        {
            using (var db = new MyDbContext())
            {
                var favoritePlaces = db.Locations
                    .Where(location => location.UserId == userId)
                    .Select(location => new LocationResponse
                    {
                        Address = location.Address,
                        Rating = location.Rating,
                        Opinion = location.Opinion,
                        TypeOfFood = location.TypeOfFood
                    })
                    .ToList();

                return favoritePlaces;
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
                    Password = new PasswordManager().HashPassword(userData.Password),
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
            location.UserId = userId;
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

        [Tags("UserLogin")]
        [HttpPost("userLoginData")]
        public async Task<Guid> UserLogin([FromBody] UserLoginData userLoginData)
        {
            string connectionString = "Server=.;Database=UlubioneMiejsce;Trusted_Connection=True;TrustServerCertificate=True"; // Replace with your actual connection string

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sqlQuery = "SELECT Password FROM [UlubioneMiejsce].[dbo].[Users] " +
                    "WHERE [Name] = '" + userLoginData.Name + "' " +
                    "AND [Email] = '" + userLoginData.Email + "' ";
                //"AND [Password] = HASHBYTES('SHA2_256','" + userLoginData.Password + "')";
                SqlCommand command = new SqlCommand(sqlQuery, connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        if (new PasswordManager().MatchPassword(userLoginData.Password, reader.GetString(reader.GetOrdinal("Password"))))
                        {
                            connection.Close();
                            sqlQuery = "SELECT Token,Id FROM [UlubioneMiejsce].[dbo].[Users] " +
                            "WHERE [Name] = '" + userLoginData.Name + "'" +
                            "AND [Email] = '" + userLoginData.Email + "' ";
                            command = new SqlCommand(sqlQuery, connection);
                            connection.Open();
                            using (SqlDataReader GuidReader = command.ExecuteReader())
                            {
                                GuidReader.Read();
                                await new TokenGenerator().CreateToken((Guid)GuidReader.GetGuid(GuidReader.GetOrdinal("Id")));
                            }
                            connection.Close();
                            connection.Open();
                            using (SqlDataReader GuidReader = command.ExecuteReader())
                            {
                                GuidReader.Read();
                               return (Guid)GuidReader.GetGuid(GuidReader.GetOrdinal("Token"));
                            }
                        }
                        else
                        {
                            throw new Exception("Invalid password");
                        }
                    }
                    else
                    {
                        throw new Exception("User not found");
                    }
                }
            }
        }
        [Tags("AddProfileToUser")]
        [HttpPost("userProfile")]
        public async Task<string> AddUserProfile([FromBody] UserProfile userProfile)
        {
            var validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(userProfile, new ValidationContext(userProfile), validationResults);

            if (!isValid)
            {
                // If the input data is not valid, return a bad request response with validation errors
                return string.Join("; ", validationResults.Select(r => r.ErrorMessage));
            }
            try
            {
                using (var context = new MyDbContext())
                {
                    context.UserProfiles.Add(userProfile);
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return "Profile has been updated";
        }
        [Tags("Friends")]
        [HttpGet("{UserId}")]
        public List<FriendsResponse> ShowFriends(Guid UserId)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    var Friends = context.Users
                        .Where(u => u.Id == UserId)
                        .SelectMany(u => u.Friends)
                        .Select(f => new FriendsResponse
                        {
                            Name = f.Name,
                            Surname = f.Surname,
                            Phone = f.Phone,
                            Locations = f.Locations.Select(l => new LocationResponse
                            {
                                Address = l.Address,
                                Rating = l.Rating,
                                Opinion = l.Opinion,
                                TypeOfFood = l.TypeOfFood
                            }).ToList()
                        })
                        .ToList();
                    return Friends;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Tags("Friends")]
        [HttpPost("{UserId}/FriendsId")]
        public async Task<IActionResult> UpdateFriendsList(Guid UserId, [FromBody] List<Guid> FriendsId)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    var user = await context.Users.FindAsync(UserId);

                    if (user != null)
                    {
                        // Modify the Friends property
                        user.Friends = await context.Users.Where(u => FriendsId.Contains(u.Id)).ToListAsync();

                        // Mark the user entity as modified
                        context.Update(user);

                        // Save the changes to the database
                        await context.SaveChangesAsync();

                        return new OkObjectResult("Friend(s) have been added");
                    }

                    return new NotFoundObjectResult("User not found");
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.ToString());
            }
        }


    }
}