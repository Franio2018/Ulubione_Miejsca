using UlubioneMiejsca.DataModels;
using UlubioneMiejsca.DataModels.DbModels;

namespace UlubioneMiejsca.Methods
{
    public class TokenGenerator
    {
        public async Task<Guid?> CreateToken(Guid UserId)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    var user = context.Users.FirstOrDefault(u => u.Id == UserId);
                    if (user != null)
                    {
                        user.Token = Guid.NewGuid();
                        user.TokenTime = DateTime.UtcNow;
                        await context.SaveChangesAsync();
                        return user.Token;
                    }
                    else
                    {
                        throw new Exception("User not found");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CleanupExpiredTokens()
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    var currentTimeMinus15Minutes = DateTime.UtcNow.AddMinutes(-15);
                    var expiredTokens = context.Users.Where(u => u.TokenTime < currentTimeMinus15Minutes).ToList();

                    if (expiredTokens.Count > 0)
                    {
                        foreach (var token in expiredTokens)
                        {
                            token.Token = null;
                            token.TokenTime = null;
                        }

                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
