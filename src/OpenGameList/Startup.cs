﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using OpenGameList.Data;

using Nelibur.ObjectMapper;
using OpenGameList.Data.Items;
using OpenGameListWebApp.ViewModels;

using OpenGameList.Data.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using OpenGameListWebApp.Classes;
using Microsoft.IdentityModel.Tokens;

namespace OpenGameList
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add a reference to the Configuration object for DI    
            services.AddSingleton<IConfiguration>(
                c => { return Configuration; }
            );

            // Add framework services.
            services.AddMvc();

            //Add EntityFramework's Identity support.
            services.AddEntityFramework();

            //Add Identity Services & Stores
            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
             {
                 config.User.RequireUniqueEmail = true;
                 config.Password.RequireNonAlphanumeric = false;
                 config.Cookies.ApplicationCookie.AutomaticChallenge = false;
             }).AddEntityFrameworkStores<ApplicationDbContext>()
             .AddDefaultTokenProviders();

            //Add ApplicationDbContext
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));

            //Register the OpenIddict services, including the default Entity Framework stores.
            services.AddOpenIddict<ApplicationUser, ApplicationDbContext>()
                //Integrate with EFCore
                .AddEntityFramework<ApplicationDbContext>()
                //Use Json Web Tokens(JWT)
                .UseJsonWebTokens()
                //Set a custom token endpoint(default is /connect/token)
                .EnableTokenEndpoint(Configuration["Authentication:OpenIddict:TokenEndPoint"])
                //Set a custom auth endpoint (defualt is /connect/authorize
                .EnableAuthorizationEndpoint("/api/connect/authorize")
                //Allow client application to use the grant_type=password flow.
                .AllowPasswordFlow()
                //Enable support for both authorization & implicit flows
                .AllowAuthorizationCodeFlow()
                .AllowImplicitFlow()
                //Allow the client to refresh tokens.
                .AllowRefreshTokenFlow()
                //Disable the HTTPS requirement (not recommended in production)
                .DisableHttpsRequirement()
                //Register a new ephemeral key for development
                //We will register a Z.509 certificate in production.
                .AddEphemeralSigningKey()
                .EnableAuthorizationEndpoint(Configuration["Authentication:OpenIddict:AuthorizationEndPoint"]);


            //Add ApplicationDbContext's DbSeeder
            services.AddSingleton<DbSeeder>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            DbSeeder dbSeeder)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            // Configure a rewrite rule to auto-lookup for standard default files such as index.html. 
            app.UseDefaultFiles();

            // Serve static files (html, css, js, images & more). See also the following URL: 
            // https://docs.asp.net/en/latest/fundamentals/static-files.html for further reference. 
            app.UseStaticFiles(new StaticFileOptions()
            {

                OnPrepareResponse = (context) =>

                {

                    // Disable caching for all static files.

                    context.Context.Response.Headers["Cache-Control"] =
                    Configuration["StaticFiles:Headers:Cache-Control"];

                    context.Context.Response.Headers["Pragma"] =
                    Configuration["StaticFiles:Headers:Pragma"];

                    context.Context.Response.Headers["Expires"] =
                    Configuration["StaticFiles:Headers:Expires"];

                }
            });

            app.UseIdentity();
            
            app.UseGoogleAuthentication(new GoogleOptions()
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                ClientId = Configuration["Authentication:Google:ClientId"],
                ClientSecret = Configuration["Authentication:Google:ClientSecret"],
                CallbackPath = "/signin-google",
                Scope = { "email" }
            });
            

            // Add a custom Jwt Provider to generate Tokens
            //app.UseJwtProvider();

            //Add OpenIddict middleware
            //Note: UseOpenIddict() must be registered after app.useIdentity() and the external social providers.
            app.UseOpenIddict();

            // Add the Jwt Bearer Header Authentication to validate Tokens
            app.UseJwtBearerAuthentication(new JwtBearerOptions()
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                RequireHttpsMetadata = false,
                Authority = Configuration["Authentication:OpenIddict:Authority"],
                TokenValidationParameters = new TokenValidationParameters()
                {
                    //IssuerSigningKey = JwtProvider.SecurityKey,
                    //ValidateIssuerSigningKey = true,
                    //ValidIssuer = JwtProvider.Issuer,
                    ValidateIssuer = false,
                    ValidateAudience = false
                }
            });


            app.UseMvc();

            // TinyMapper binding configuration
            TinyMapper.Bind<Item, ItemViewModel>();

            // Seed the Database (if needed)
            try
            {
                dbSeeder.SeedAsync().Wait();
            }
            catch (AggregateException e)
            {
                throw new Exception(e.ToString());
            }
        }
    }
}
