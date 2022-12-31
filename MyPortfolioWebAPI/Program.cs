using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyPortfolioWebAPI.Data;
using Microsoft.AspNetCore.Cors;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<MyPortfolioContext>(options => options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]));



    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



//Enable cores

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigion", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
    
});


//Json Serializer
builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
    .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else if (app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(C =>
    {
        C.SwaggerEndpoint("/swagger/v1/swagger.json", "Portfolio API V1");
        C.RoutePrefix = String.Empty;
    });
}


app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(options=>options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
builder.Services.AddDirectoryBrowser();
app.UseStaticFiles();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath,"Images")),
    RequestPath = "/Images"
});


app.Run();
