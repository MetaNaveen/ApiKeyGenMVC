using ApiKeyGenMVC.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//var salt = builder.Configuration["AppSettings:SecretKey"] ?? "";

//builder.Services.AddScoped<ISharedDataService, SharedDataService> ();
//builder.Services.AddTransient<ISharedDataService, SharedDataService> ();
builder.Services.AddSingleton<ISharedDataService, SharedDataService> ();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
