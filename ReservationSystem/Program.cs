using ReservationSystem.Data;
using Microsoft.EntityFrameworkCore;
using ReservationSystem.Business.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using ReservationSystem.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// Authentication & Authorization
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();

// EF Core, RazorPages, Identity Hasher, HttpClient, Services
builder.Services.AddRazorPages();
builder.Services.AddDbContext<ApplicationDbContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddHttpClient<IHolidayService, HolidayService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("Smtp"));
builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();
builder.Services.AddScoped<IFeedbackService,   FeedbackService>();


var app = builder.Build();

// --- SEED AŞAMASI ---
using (var scope = app.Services.CreateScope())
{
    var ctx    = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>();

    // 1) Admin kullanıcısı
    if (!ctx.Users.Any())
    {
        ctx.Users.Add(new User {
            Username     = "admin",
            Role         = "Admin",
            PasswordHash = hasher.HashPassword(null, "admin"),
            CreatedAt    = DateTime.UtcNow,
            IsActive     = true
        });
    }

    // 2) Term kodları
    var termCodes = new[] {
        "ACS","ADA","AİT","ALM","APS","ARCH","BAF","BAI","BİL","BIO",
        "CAA","CE","CEC","CENG","CHEM","CHIN","CMPE","COGS","COMM","COMP",
        "COMS","CPR","CRP","CVLE","DSGN","EBB","ECE","ECM","ECON","EE",
        "EEM","ELCS","ELEC","ELL","ELTC","EM","ENG","ENTR","ESR","FEL",
        "FINA","FLF","FLR","FRAN","FTP","HIR","HIST","HMR","HUK","TAGR",
        "IE","INAR","INF","ING","INTR","INTT","IS","ISG","IT","ITAL",
        "ITIM","IVA","JOR","JPS","LAW","MAN","MARK","MAT","MATH","MCS",
        "ME","MECE","MIS","MSE","MSI","MUS","OBEC","OTT","PCS","PES",
        "PHIL","PHYS","PLS","PMY","PSI","PSY","RME","RPW","RTW","RUS",
        "SENG","SOC","SOS","SPAN","STAT","THEA","THM","TINS","TURK","UTL",
        "YBD"
    };

    // Bugünün tarihi baz alınıyor, istersen sabit bir döneme çekersin
    var termStart = DateTime.Today;
    var termEnd   = termStart.AddMonths(6);

    foreach (var code in termCodes)
    {
        if (!ctx.Terms.Any(t => t.Name == code))
        {
            ctx.Terms.Add(new Term {
                Name      = code,
                StartDate = termStart,
                EndDate   = termEnd
            });
        }
    }

    ctx.SaveChanges();
}
// --- SEED SONU ---

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();

app.Run();
