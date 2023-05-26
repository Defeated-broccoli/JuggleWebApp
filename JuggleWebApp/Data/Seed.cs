using JuggleWebApp.Data;
using JuggleWebApp.Models;
using Microsoft.AspNetCore.Identity;
using System.Net;


namespace JuggleWebApp.Data
{
    public class Seed
    {
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                context.Database.EnsureCreated();

                if (!context.Posts.Any())
                {
                    context.Posts.AddRange(new List<Post>()
                    {

                         new Post()
                                    {
                                        Title = "Post without Turnament 1",
                                        Description = "This is the post's description",
                                        Image = "https://images.unsplash.com/photo-1531564701487-f238224b7ce3?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxzZWFyY2h8Mnx8cG9zdCUyMG9mZmljZXxlbnwwfHwwfHw%3D&w=1000&q=80",
                                        Date = DateTime.Now,
                                        LikesNumber = 10,
                                        Points = 5,
                                    },
                          new Post()
                                    {
                                        Title = "Post without Turnament 2",
                                        Description = "This is the post's description",
                                        Image = "https://images.unsplash.com/photo-1531564701487-f238224b7ce3?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxzZWFyY2h8Mnx8cG9zdCUyMG9mZmljZXxlbnwwfHwwfHw%3D&w=1000&q=80",
                                        Date = DateTime.Now,
                                        LikesNumber = 10,
                                        Points = 5,
                                    },
                           new Post()
                                    {
                                        Title = "Post without Turnament 3",
                                        Description = "This is the post's description",
                                        Image = "https://images.unsplash.com/photo-1531564701487-f238224b7ce3?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxzZWFyY2h8Mnx8cG9zdCUyMG9mZmljZXxlbnwwfHwwfHw%3D&w=1000&q=80",
                                        Date = DateTime.Now,
                                        LikesNumber = 10,
                                        Points = 5,
                                    },

                    });
                }

                if (!context.Tournaments.Any())
                {
                    context.Tournaments.AddRange(new List<Tournament>()
                        {
                            new Tournament()
                            {
                                Title = "Ball Pit",
                                Description = "The most balls win! (Additional points for drawning in balls)",

                            },
                            new Tournament()
                            {
                                Title = "Stranger Things",
                                Description = "Wow. so many weird objects. Well done! Points!"
                            },
                            new Tournament()
                            {
                                Title = "Ballroom Blitz",
                                Description = "Take a deep breath, you won't have time for this when it starts! If you can juggle slow you can juggle fast. Heh."
                            },
                        });


                    context.SaveChanges();
                }


            }
        }

        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                string adminUserEmail = "";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new AppUser()
                    {
                        UserName = "admin",
                        Email = adminUserEmail,
                        EmailConfirmed = true,

                    };
                    await userManager.CreateAsync(newAdminUser, "");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }

                string appUserEmail = "";

                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser == null)
                {
                    var newAppUser = new AppUser()
                    {
                        UserName = "",
                        Email = appUserEmail,
                        EmailConfirmed = true,

                    };
                    await userManager.CreateAsync(newAppUser, "");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                }
            }
        }


    }
}

