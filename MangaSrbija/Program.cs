using MangaSrbija.BLL.helpers;
using MangaSrbija.BLL.services.MangaServices;
using MangaSrbija.DAL.Repositories;
using MangaSrbija.DAL.Repositories.category;
using MangaSrbija.DAL.Repositories.chapters;
using MangaSrbija.DAL.Repositories.mangasCategories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//services
builder.Services.AddScoped<MangasCategoriesService>();
builder.Services.AddScoped<MangaService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ChapterService>();
builder.Services.AddScoped<ChapterPhotoService>();


//Repositories
builder.Services.AddScoped<IMangaRepository, MangaRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IMangasCategoriesRepository, MangasCategoriesRepository>();
builder.Services.AddScoped<IChapterRepository, ChapterRepository>();
builder.Services.AddScoped<IChapterPhotoRepository, ChapterPhotoRepository>();


var app = builder.Build();

var environment = builder.Environment;

PathRegistry.GetInstance(environment.ContentRootPath, Path.Combine(environment.ContentRootPath, "wwwroot", "Images"));

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

app.UseAuthorization();

app.MapControllers();

app.Run();
