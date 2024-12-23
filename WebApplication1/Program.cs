using WebApplication1;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapRazorPages();

app.MapPost("/generate-certificate", async (HttpContext context) =>
{
    var form = context.Request.Form;
    string firstName = form["FirstName"];
    string lastName = form["LastName"];
    string fullName = $"{firstName} {lastName}";

    // Generate the PDF using your PDF generator
    byte[] pdfBytes = PdfGenerator.GenerateCertificate(fullName);

    // Set response headers for file download
    context.Response.ContentType = "application/pdf";
    context.Response.Headers.Add("Content-Disposition", $"attachment; filename=Certificate_{firstName}_{lastName}.pdf");
    await context.Response.Body.WriteAsync(pdfBytes);
});


app.Run();


