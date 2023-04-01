namespace Test
{
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    [GeneratedCode("test", "test")]
    public interface IPetsApiService
    {
        string ListPets(int limit);
        string CreatePets();
        string ShowPetById([Required] string petId);
    }

    [GeneratedCode("test", "test")]
    public class Pet
    {
        [Required]
        public long Id { set; get; }

        [Required]
        public string Name { set; get; }

        public string Tag { set; get; }
    }

    [GeneratedCode("test", "test")]
    public class MyPets
    {
        public IReadOnlyList<Pet> Pets { set; get; }
    }

    [GeneratedCode("test", "test")]
    public class Error
    {
        [Required]
        public int Code { set; get; }

        [Required]
        public string Message { set; get; }
    }
}