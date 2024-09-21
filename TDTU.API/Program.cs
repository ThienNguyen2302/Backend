using CloudinaryDotNet;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using TDTU.API.BackgroundServices;
using TDTU.API.Data.Seeding;
using TDTU.API.Implements;
using Udemy.Middlewares;

namespace TDTU.API
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var assembly = Assembly.GetExecutingAssembly();
			var builder = WebApplication.CreateBuilder(args);
			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services.AddAuthorization();
			builder.Services.AddAutoMapper(assembly);

			#region DataContext
			var connectionString = builder.Configuration.GetConnectionString("Database");

			AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
			builder.Services.AddDbContext<DataContext>((sp, options) =>
			{
				options.UseNpgsql(connectionString);
			});
			builder.Services.AddSingleton(TimeProvider.System);
			#endregion

			#region Swagger
			builder.Services.AddSwaggerGen(swa =>
			{
				swa.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "TDTU Web API",
					Version = "v1"
				});

				var securitySchema = new OpenApiSecurityScheme
				{
					Description =
						"JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
					Name = "Authorization",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.Http,
					Scheme = JwtBearerDefaults.AuthenticationScheme,
					Reference = new OpenApiReference
					{
						Type = ReferenceType.SecurityScheme,
						Id = JwtBearerDefaults.AuthenticationScheme
					}
				};
				swa.AddSecurityDefinition("Bearer", securitySchema);

				var securityRequirement = new OpenApiSecurityRequirement
				{
					{ securitySchema, new[] { "Bearer" } }
				};
				swa.AddSecurityRequirement(securityRequirement);
			});
			#endregion

			#region JWT
			builder.Services.AddAuthentication(opt =>
			{
				opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options =>
			{
				options.RequireHttpsMetadata = false;
				options.SaveToken = true;
				options.TokenValidationParameters = new TokenValidationParameters()
				{
					ValidateIssuer = true,
					ValidIssuer = JWTConstant.ValidIssuer,
					ValidateAudience = true,
					ValidAudience = JWTConstant.ValidAudience,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ClockSkew = System.TimeSpan.Zero,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTConstant.Secret))
				};
			});
			#endregion

			#region CORS
			builder.Services.AddCors(options =>
			{
				options.AddDefaultPolicy(
					builder =>
					{
						builder.AllowAnyOrigin()
								.AllowAnyMethod()
								.AllowAnyHeader()
								.WithExposedHeaders("Content-Disposition");
					});
			});
			#endregion

			#region Service
			builder.Services.AddScoped<IUserService, UserService>();
			builder.Services.AddScoped<ISettingService, SettingService>();
			builder.Services.AddScoped<ISkillService, SkillService>();
			builder.Services.AddScoped<ICompanyService, CompanyService>();
			builder.Services.AddScoped<IStorageService, StorageService>();
			builder.Services.AddScoped<IStudentService, StudentService>();
			builder.Services.AddScoped<IRegularJobService, RegularJobService>();
			builder.Services.AddScoped<IInternshipJobService, InternshipJobService>();
			builder.Services.AddScoped<IStudentProfileService, StudentProfileService>();
			builder.Services.AddScoped<IInternshipTermService, InternshipTermService>();
			builder.Services.AddScoped<IInternshipOrderService, InternshipOrderService>();
			builder.Services.AddScoped<IApplicationStatusService, ApplicationStatusService>();
			builder.Services.AddScoped<IRegistrationStatusService, RegistrationStatusService>();
			builder.Services.AddScoped<IRegularJobApplicationService, RegularJobApplicationService>();
			builder.Services.AddScoped<IInternshipRegistrationService, InternshipRegistrationService>();
			builder.Services.AddScoped<IInternshipJobApplicationService, InternshipJobApplicationService>();

			builder.Services.AddScoped<IDataContext>(provider => provider.GetRequiredService<DataContext>());
			builder.Services.AddTransient<IDataContextInitializer, DataContextInitializer>();
			#endregion

			#region Cloudinary
			var cloudinaryConfiguration = builder.Configuration.GetSection("CloudinarySettings");
			builder.Services.Configure<CloudinarySettings>(cloudinaryConfiguration);

			builder.Services.AddSingleton(provider =>
			{
				var config = provider.GetRequiredService<IOptions<CloudinarySettings>>().Value;
				return new Cloudinary(new Account(config.CloudName, config.ApiKey, config.ApiSecret));
			});
			#endregion

			builder.Services.AddControllers().AddJsonOptions(options =>
			{
				options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
			});

			builder.Services.AddHostedService<InternshipTermBackgroundService>();
			var app = builder.Build();

			app.UseSwagger();
			app.UseSwaggerUI();
			app.UseCors();
			app.UseHttpsRedirection();
			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();
			app.UseMiddleware<ExceptionMiddleware>();
			app.MapControllers();

			using (var scope = app.Services.CreateScope())
			{
				IServiceProvider services = scope.ServiceProvider;
				IDataContextInitializer initializer = services.GetRequiredService<IDataContextInitializer>();
				await initializer.SeedAsync();
				scope.Dispose();
			}

			app.Run();
		}
	}
}
