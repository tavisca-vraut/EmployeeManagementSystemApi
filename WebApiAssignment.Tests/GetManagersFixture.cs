using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using WebApiAssignment.Controllers;
using WebApiAssignment.Models;
using Xunit;
using System.Linq;
using System.Collections.Generic;

namespace WebApiAssignment.Tests
{
    public class GetManagersFixture
    {
        [Fact]
        public void GetEmployeesOnEmptyList_ShouldReturn_NoData()
        {
            EmployeesController employeesController = new EmployeesController();

            var employees = new Employee[] { };
            ActionResult<IEnumerable<Employee>> result = employees;

            result.Should()
                  .BeEquivalentTo(employeesController.Get());
        }

        [Fact]
        public void GetEmployeesOnListWithOneEmployee_ShouldReturn_ThatEmployee()
        {
            EmployeesController employeesController = new EmployeesController();

            var employees = new Employee[]
            {
                new Employee()
                {
                    Name = "Vighnesh",
                    Age = 21,
                    ManagerId = -1
                }
            };
            ActionResult<IEnumerable<Employee>> result = employees;

            result.Should()
                  .BeEquivalentTo(employeesController.Get());
        }
    }
}
