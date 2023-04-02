namespace Test
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using RestEase;

    [GeneratedCode("test", "test")]
    public interface IPetsApiService
    {
        [Get("/pets")]
        Task<IReadOnlyList<Pet>> ListPets(int limit);
        [Post("/pets")]
        Task CreatePets();
        [Get("/pets/{petId}")]
        Task<Pet> ShowPetById([Required] string petId);
    }

    [GeneratedCode("test", "test")]
    public class Pet
    {
        [Required]
        public long Id { set; get; }

        [Required]
        public string Name { set; get; }

        [Obsolete]
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