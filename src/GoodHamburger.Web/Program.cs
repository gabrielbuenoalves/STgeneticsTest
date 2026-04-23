using GoodHamburger.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddHttpClient<IGoodHamburgerApiClient, GoodHamburgerApiClient>((sp, client) =>
{
	var configuration = sp.GetRequiredService<IConfiguration>();
	var apiBaseUrl = configuration["ApiBaseUrl"]
		?? throw new InvalidOperationException("ApiBaseUrl nao configurada.");
	client.BaseAddress = new Uri(apiBaseUrl);
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
