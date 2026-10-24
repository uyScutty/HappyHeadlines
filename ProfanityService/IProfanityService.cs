public interface IProfanityService
{
   

    Task<bool> ContainsProfanityAsync(string text);
}