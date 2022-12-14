using System;
namespace be_abisoft.Models
{
    public class RequestUser_CU
    {
        public RequestUser_CU(int id, string name, int age, DateTime birthdate, DateTime inscriptionDate, decimal price)
        {
            Id = id;
            Name = name;
            Age = age;
            Birthdate = birthdate;
            InscriptionDate = inscriptionDate;
            Price = price;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime Birthdate { get; set; }
        public DateTime InscriptionDate { get; set; }
        public decimal Price { get; set; }

    }

    public class RequestUser
    {
        public RequestUser(int id)
        {
            Id = id;
        }

        public int Id { get; set; }

    }

}

