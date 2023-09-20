using MangaSrbija.BLL.helpers;
using MangaSrbija.BLL.services.MangaServices;
using MangaSrbija.BLL.services.UserServices;
using MangaSrbija.DAL.Repositories;
using MangaSrbija.DAL.Repositories.category;
using MangaSrbija.DAL.Repositories.chapters;
using MangaSrbija.DAL.Repositories.mangaReadLater;
using MangaSrbija.DAL.Repositories.mangasCategories;
using MangaSrbija.DAL.Repositories.mangaUserFavorites;
using MangaSrbija.DAL.Repositories.users;
using MangaSrbija.Presentation.Attributes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Newtonsoft;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string key = builder.Configuration["AppSettings:EncryptionKey"];

builder.Services.AddAuthentication(builder => {

    builder.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    builder.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;


}).AddJwtBearer(builder => {

    builder.RequireHttpsMetadata = false;
    builder.SaveToken = true;
    builder.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


builder.Services.AddCors(options => options.AddPolicy("AllowAll", builder => {
    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
}));

//services
builder.Services.AddScoped<MangasCategoriesService>();
builder.Services.AddScoped<MangaService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ChapterService>();
builder.Services.AddScoped<ChapterPhotoService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<JwtAuthenticationManager>();


//Repositories
builder.Services.AddScoped<IMangaRepository, MangaRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IMangasCategoriesRepository, MangasCategoriesRepository>();
builder.Services.AddScoped<IChapterRepository, ChapterRepository>();
builder.Services.AddScoped<IChapterPhotoRepository, ChapterPhotoRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMangaUserFavorites, MangaUserFavorites>();
builder.Services.AddScoped<IMangaUserReadLater, MangaUserReadLater>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var redisConfiguration = builder.Configuration.GetSection("Redis").Get<RedisConfiguration>();
builder.Services.AddStackExchangeRedisExtensions<NewtonsoftSerializer>(redisConfiguration);
builder.Services.AddDistributedMemoryCache();


var app = builder.Build();

var environment = builder.Environment;



PathRegistry.GetInstance(environment.ContentRootPath, 
    Path.Combine(environment.ContentRootPath, "wwwroot", "Images"));


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseExceptionHandler("/error-dev");
}
else
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowAll");


app.MapControllers();

app.Run();
