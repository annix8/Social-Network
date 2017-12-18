namespace SocialNetwork.Tests.Utils
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using SocialNetwork.DataModel;
    using SocialNetwork.DataModel.Models;
    using System;

    public class MockManager
    {
        public static SocialNetworkDbContext GetMockDatabase()
        {
            var dbOptions = new DbContextOptionsBuilder<SocialNetworkDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new SocialNetworkDbContext(dbOptions);
        }

        public static Mock<UserManager<User>> GetMockUserManger()
        {
            return new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
        }
    }
}
