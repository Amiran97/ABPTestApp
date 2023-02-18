using ABPTestApp.Models.DTOs.Validators;
using ABPTestApp.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ExperimentRequestValidator>());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddScoped<IExperimentRepository, ExperimentRepository>();
builder.Services.AddRazorPages();
var contact = new OpenApiContact()
{
    Name = "Amiran Todua",
    Email = "todua.amiran97@gmail.com"
};
var license = new OpenApiLicense()
{
    Name = "Amiran License",
    Url = new Uri("https://github.com/Amiran97")
};
var info = new OpenApiInfo()
{
    Version = "v1",
    Title = "ABP test application",
    Contact = contact,
    License = license
};
builder.Services.AddSwaggerGen(o =>
{
    o.EnableAnnotations();
    o.SwaggerDoc("v1", info);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints => {
    endpoints.MapRazorPages();
    endpoints.MapControllers();
});

app.Run();