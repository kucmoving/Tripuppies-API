using API.Data;
using API.Models;
using API.Repository;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace API.Tests.Repository
{
    public class UserRepositoryTests
    {
        private async Task<DataContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var databaseContext = new DataContext(options);
            databaseContext.Database.EnsureCreated();
            if (await databaseContext.Users.CountAsync() < 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    databaseContext.Users.Add(
                      new AppUser()
                      {
                          UserName = "TestUserName",
                          NickName = "TestNickName",
                          Story = "My life is testing",
                          Role = "Traveller",
                          PreferStyle = "Leisure",
                          Experience = 8,
                          Gender = "Male",
                          Region = "England",
                          Photos = new List<Photo>()
                          {
                              new Photo { Url = "1afaefeafafaf", IsMain = true, PublicId = "1rewrwerwerwerwe" },
                              new Photo { Url = "2afaefeafafaf", IsMain = false, PublicId = "2rewrwerwerwerwe" },
                            }
                      });
                    await databaseContext.SaveChangesAsync();
                }
            }
            return databaseContext;
        }
        [Fact]
        public async void UserRepository_GetPuppyAsync_ReturnPuppy()
        {
            var name = "TestUserName";
            var dbContext = await GetDatabaseContext();
            var userRepository = new UserRepository(dbContext);

            var result = userRepository.GetPuppyAsync(name);
            result.Should().NotBeNull();
            result.Should().BeOfType<AppUser>();
        }
    }
}




