using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogAPI.Migrations
{
    /// <inheritdoc />
    public partial class Seed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"

INSERT INTO BlogPosts(Slug, Title, Description,Body)
VALUES('how-to-make-image-not-snap-to-cursor', 'How to make image not snap to cursor?', 'Desc','Trying to make an image follow the mouse. It works, but the problem Im having is that upon refresh/first movement, the image will appear at the top left and snap to the mouse position. How do I get rid of that process and just have the image appear from under the mouse?'),
('awesome-coding','Awesome coding','Aweomse coding examples', 'Coding is a uniquer skill, it requires a lot of dedication.')

INSERT INTO Tags(Name)
VALUES('Code'),
('Snap'),
('Image'),
('Skill')

INSERT INTO PostTags(BlogPostId, TagId)
VALUES((SELECT TOP 1 Id FROM BlogPosts ),(SELECT TOP 1 Id FROM Tags WHERE Name ='Snap')),
((SELECT TOP 1 Id FROM BlogPosts ),(SELECT TOP 1 Id FROM Tags WHERE Name ='Image')),
((SELECT TOP 1 Id FROM BlogPosts ORDER By Id DESC),(SELECT TOP 1 Id FROM Tags WHERE Name ='Code')),
((SELECT TOP 1 Id FROM BlogPosts ORDER By Id DESC),(SELECT TOP 1 Id FROM Tags WHERE Name ='Skill'))

INSERT INTO Comments(BlogPostId, Body)
VALUES((SELECT TOP 1 Id FROM BlogPosts ), 'I want to learn more about this.'),
((SELECT TOP 1 Id FROM BlogPosts ), 'Please share more'),
((SELECT TOP 1 Id FROM BlogPosts ORDER By Id DESC), 'This is awesome'),
((SELECT TOP 1 Id FROM BlogPosts ORDER By Id DESC), 'This is not working for me')
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
