﻿using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
    public class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProduction)
        {
            using (var servicesScoped = app.ApplicationServices.CreateScope())
            {
                SeedData(servicesScoped.ServiceProvider.GetService<AppDbContext>(), isProduction);
            }
        }

        private static void SeedData(AppDbContext context, bool isProduction)
        {
            if(isProduction)
            {
                Console.WriteLine("--> Attempting to apply migrations...");
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not run migrations: {ex.Message}");
                }
                
            }
            if (!context.Platforms.Any())
            {
                Console.WriteLine("Seeding data...");

                context.Platforms.AddRange(
                    new Platform()
                    {
                        Name = "DotNet",
                        Publisher = "Microsoft",
                        Cost = "Free",
                    },
                     new Platform()
                     {
                         Name = "SQL Server Express",
                         Publisher = "Microsoft",
                         Cost = "Free",
                     },
                      new Platform()
                      {
                          Name = "Kubernetes",
                          Publisher = "Cloud Native Computing Foundation",
                          Cost = "Free",
                      }
                    );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("We already have data!");
            }
        }
    }
}
