using Application.Authntication;
using Application.Services.Implementation;
using Application.Services.Interfaces;
using DecorteeSystem.MiddleWare;
using Domain.IRepositories;
using Infrastructure;
using Infrastructure.Repositories;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using FluentValidation;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using Microsoft.OpenApi.Models;

namespace DecorteeSystem
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                    options.JsonSerializerOptions.DefaultIgnoreCondition = 
                        System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
                    options.JsonSerializerOptions.WriteIndented = true;
                });
            
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "Decortee API", 
                    Version = "v1",
                    Description = "Interior Design API"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT token as: Bearer {your token}"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
                
                c.SupportNonNullableReferenceTypes();
            });

            services.AddFluentValidationConfig();
            
            var connectionString = configuration.GetConnectionString("DefaultConnection") ??
                throw new InvalidOperationException("Connection string 'DefaultConnection' is not found!");

            services.AddDbContext<DecorteeDbContext>(options => 
                options.UseSqlServer(connectionString));

            services.AddMapsterConfig();
            services.AddAuthConfig(configuration);

            services.AddScoped<TransactionMiddleware>();

            // Auth
            services.AddScoped<IAuthRepository, AuthRepositor>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProfileService, ProfileService>();

            // Generic Repository
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Repositories
            services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();
            services.AddScoped<IDesignStyleRepository, DesignStyleRepository>();
            services.AddScoped<IShowcaseDesignRepository, ShowcaseDesignRepository>();
            services.AddScoped<IAIDesignRepository, AIDesignRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IVoteRepository, VoteRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IRatingRepository, RatingRepository>();
            services.AddScoped<IChatMessageRepository, ChatMessageRepository>();
            services.AddScoped<IAuthRepository, AuthRepositor>();

            // Services
            services.AddScoped<IRoomTypeService, RoomTypeService>();
            services.AddScoped<IDesignStyleService, DesignStyleService>();
            services.AddScoped<IShowcaseDesignService, ShowcaseDesignService>();
            services.AddScoped<IAIDesignService, AIDesignService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IVoteService, VoteService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IRatingService, RatingService>();
            services.AddScoped<IChatMessageService, ChatMessageService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEmailService, EmailService>();

            return services;
        }

        public static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IjwtProvider, JwtProvider>();
            services.AddOptions<JwtOptions>().BindConfiguration(JwtOptions.SectionName);

            var jwtOptions = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()
                ?? throw new InvalidOperationException("JWT configuration is missing");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
                        ValidateIssuer = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtOptions.Audience,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            return services;
        }

        public static IServiceCollection AddMapsterConfig(this IServiceCollection services)
        {
            var mappingConfig = TypeAdapterConfig.GlobalSettings;
            mappingConfig.Scan(Assembly.GetExecutingAssembly());
            
            services.AddSingleton<IMapper>(new Mapper(mappingConfig));
            return services;
        }

        public static IServiceCollection AddFluentValidationConfig(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
                .AddFluentValidationAutoValidation();
            return services;
        }
    }
}
