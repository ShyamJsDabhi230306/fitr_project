using FITR_DC_FORM.Filters;
using FITR_DC_FORM.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// ✅ ADD SERVICES FIRST
builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.PropertyNamingPolicy =
            System.Text.Json.JsonNamingPolicy.CamelCase;
    });
//builder.Services.AddControllersWithViews();
builder.Services.AddScoped<PermissionFilter>();

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.AddService<PermissionFilter>();
});
// ? Call your DependencyInjection class
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddSession();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
