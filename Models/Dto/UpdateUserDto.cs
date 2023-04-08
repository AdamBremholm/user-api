namespace UserApi.Models.Dto;

public sealed record UpdateUserDto(int Id, string? FirstName, string? LastName, string Cdsid);
