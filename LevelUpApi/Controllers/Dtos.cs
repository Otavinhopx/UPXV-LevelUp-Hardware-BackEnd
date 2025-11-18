public record RegisterDto(string Name, string Email, string Password);

public record ReviewDto(int Stars, string Comment);
public record ProductDto(string? Title, string? Brand, string? Description, decimal? Price, string? AffiliateUrl, string? ImageUrl);
public record ArticleDto(string Title, string Content, string? Author);
