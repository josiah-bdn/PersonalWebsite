using API.Logic;
using API.Logic.Services.BlogServiceLogic;
using AutoMapper;
using Data.DTO;
using Data.Enum;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Test.UnitTests {
    public class BlogUnitTests {
        private readonly DataContext _db;
        private readonly IMapper _mapper;
        private readonly BlogService _blogService;
        private static readonly string s_validBody = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.";

        public BlogUnitTests() {
            var options = new DbContextOptionsBuilder<DataContext>()
                   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                   .Options;

            _db = new DataContext(options);

            var mapperConfig = new MapperConfiguration(cfg => {
                cfg.AddProfile(new MappingProfile());
            });
            _mapper = mapperConfig.CreateMapper();

            _blogService = new BlogService(_db, _mapper);

            _blogService = new BlogService(_db, _mapper);

            }

        [Fact]
        public async Task Create_Then_Get_Blog() {

            var userId = GenerateGuid();
            var blog = GenerateBlogDto();

            var newBlog = await _blogService.CreateBlogAsync(userId, blog);
            Assert.NotNull(newBlog);

            var retrievedBlog = await _blogService.GetBlogAsync(newBlog.BlogId);

            Assert.Equal(newBlog.AppUserId, retrievedBlog.AppUserId);
            Assert.Equal(blog.Title, retrievedBlog.Title);
            Assert.Equal(blog.ImageUrl, retrievedBlog.ImageUrl);

            }

        [Fact]
        public async Task Update_Blog() {
            var userId = GenerateGuid();
            var blog = GenerateBlogDto();

            var newBlog = await _blogService.CreateBlogAsync(userId, blog);
            Assert.NotNull(newBlog);

            var updateBlogObj = new UpdateBlogDto {
                Title = "This is an updated title",
                Body = "This is an updated body",
                };

            var updatedBlog = await _blogService.UpdateBlogAsync(userId, newBlog.BlogId, updateBlogObj);
            Assert.NotNull(updatedBlog);

            Assert.Equal(updatedBlog.Title, updateBlogObj.Title);
            Assert.Equal(updatedBlog.Body, updateBlogObj.Body);

            }

        [Fact]
        public async Task Delete_Blog() {

            DeleteBlogFlow();

            var deletedBlog = await _db.Blog.FirstOrDefaultAsync();

            Assert.NotNull(deletedBlog);

            Assert.True(deletedBlog.IsDeleted);
            }

        [Fact]
        public async Task Cannot_Retrieve_Deleted_Blog() {
            DeleteBlogFlow();

            var deletedBlogId = await _db.Blog.Select(b => b.BlogId).FirstOrDefaultAsync();

            try {
                await _blogService.GetBlogAsync(deletedBlogId);
                }
            catch (Exception ex) {
                Assert.NotNull(ex.Message);
                Assert.Equal("Blog is not found", ex.Message);
                }

            }

        [Fact]
        public async Task Unauthorized_User_Attempts_To_Edit_Blog() {
            var userId = GenerateGuid();
            var otherUserId = GenerateGuid();
            var blog = GenerateBlogDto();

            var newBlog = await _blogService.CreateBlogAsync(userId, blog);
            Assert.NotNull(newBlog);

            var updateBlogObj = new UpdateBlogDto {
                Title = "This is an updated title",
                Body = "This is an updated body",
                };

            try {
                await _blogService.UpdateBlogAsync(otherUserId, newBlog.BlogId, updateBlogObj);

                }
            catch (Exception ex) {
                Assert.Equal("User is not authorized to modify blog", ex.Message);
                }
            }

        [Fact]
        public async Task User_Attempts_To_Retrieve_Non_Existent_Blog() {
            var blogId = GenerateGuid();

            try {
                await _blogService.GetBlogAsync(blogId);
                }
            catch (Exception ex) {
                Assert.Equal("Blog is not found", ex.Message);
                }
            }

        private CreateBlogDto GenerateBlogDto() {
            return new CreateBlogDto {
                Category = Data.Enum.BlogCategories.Bitcoin,
                Title = "The White Paper",
                Description = "A P2P cash payment system",
                Body = "Lorem Ipsum salt and pepper",
                ImageDescription = "A photo",
                ImageUrl = $"https://i.pravatar.cc/300/{Guid.NewGuid()}"
                };
            }

        private Guid GenerateGuid() {
            return Guid.NewGuid();
            }

        private async void DeleteBlogFlow() {
            var userId = GenerateGuid();
            var blog = GenerateBlogDto();

            var newBlog = await _blogService.CreateBlogAsync(userId, blog);
            Assert.NotNull(newBlog);

            await _blogService.DeleteBlogAsync(userId, newBlog.BlogId);

            }

        public static IEnumerable<object[]> GetTestData() {
            return new List<object[]> {
        new object[] { "Title must be between 5 and 100 characters", BlogCategories.Bitcoin, "Hi", s_validBody, "My image", "https://i.pravatar.cc/300/433245" },
        new object[] { "Descritpion must be less than 50 characters", BlogCategories.Bitcoin, "the best", s_validBody, s_validBody, "My image", "https://i.pravatar.cc/300/433245" }
            };
            }
        }
    }