using System.Collections.Generic;
using System.Linq;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Queries;
using PaymentContext.Domain.ValueObjects;
using Xunit;

namespace PaymentContext.Tests.Handlers
{
    public class StudentQueriesTests
    {
        private readonly IList<Student> _students;
        public StudentQueriesTests()
        {
            _students = new List<Student>();
            for (var i = 0; i <= 5; i++){
                _students.Add(new Student(
                    new Name("Aluno", i.ToString()),
                    new Document("1111111111" + i.ToString(), EDocumentType.CPF),
                    new Email(i.ToString() + "@balta.io")
                ));
            }
        }

        [Fact]
        public void Students_ShouldReturnNullWhenDocumentDoesNotExists()
        {
            var exp = StudentQueries.GetStudent("12345678910");
            var student = _students.AsQueryable().FirstOrDefault(exp);

            Assert.Null(student);
        }

        [Fact]
        public void Students_ShouldReturnStudentWhenDocumentExists()
        {
            var exp = StudentQueries.GetStudent("11111111111");
            var student = _students.AsQueryable().FirstOrDefault(exp);

            Assert.NotNull(student);
        }
    }
}