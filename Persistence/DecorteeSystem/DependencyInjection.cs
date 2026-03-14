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
using Microsoft.EntityFrameworkCore.Diagnostics;
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
                    Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
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
            {
                options.UseSqlServer(connectionString, sqlOptions =>
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null));

                options.ConfigureWarnings(warnings =>
                    warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
            });

            services.AddMapsterConfig();
            services.AddAuthConfig(configuration);

            services.AddScoped<TransactionMiddleware>();

            // Auth
            services.AddScoped<IAuthRepository, AuthRepositor>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEmailService, EmailService>();

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
            services.AddScoped<ISavedPostRepository, SavedPostRepository>();
            // Profile
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProfileService, ProfileService>();

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

            return services;
        }

        public static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IjwtProvider, JwtProvider>();

            services.AddOptions<JwtOptions>()
                .BindConfiguration(JwtOptions.SectionName);

            var jwtOptions = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();
            
            if (jwtOptions == null)
                throw new InvalidOperationException("JWT options are not configured properly.");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key))
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
