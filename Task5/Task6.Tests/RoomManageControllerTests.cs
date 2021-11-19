using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task5.BLL.DTO;
using Task5.BLL.Interfaces;
using Task5.WebApi.Controllers;
using Task5.WebApi.ViewModels.Room;

namespace Task6.Tests
{
    [TestFixture]
    public class RoomManageControllerTests
    {
        private Mock<IRoomService> moqRoomService;
        private Mock<ICategoryService> moqCategoryService;
        private Mock<ICategoryDateService> moqCategoryDateService;

        private List<RoomDTO> roomList;
        private List<CategoryDTO> categoryList;
        private List<CategoryDateDTO> categoryDateList;
        [SetUp]
        public void SetUp()
        {
            moqRoomService = new Mock<IRoomService>();
            moqCategoryService = new Mock<ICategoryService>();
            moqCategoryDateService = new Mock<ICategoryDateService>();
            SetData();
        }

        private void SetData()
        {
            categoryList = new List<CategoryDTO>
            {
                new CategoryDTO { Id = Guid.NewGuid(), Name = "Base"},
                new CategoryDTO { Id = Guid.NewGuid(), Name = "Vip"},
            };
            roomList = new List<RoomDTO>
            {
                new RoomDTO { Id = Guid.NewGuid(), CategoryId = categoryList[0].Id, Number = 1},
                new RoomDTO { Id = Guid.NewGuid(), CategoryId = categoryList[1].Id, Number = 2},
                new RoomDTO { Id = Guid.NewGuid(), CategoryId = categoryList[0].Id, Number = 3},
                new RoomDTO { Id = Guid.NewGuid(), CategoryId = categoryList[0].Id, Number = 4},
                new RoomDTO { Id = Guid.NewGuid(), CategoryId = categoryList[0].Id, Number = 5}
            };
            categoryDateList = new List<CategoryDateDTO>
            {
                new CategoryDateDTO { Id = Guid.NewGuid(), CategoryId = categoryList[0].Id, StartDate = DateTime.Now.AddDays(-30), EndDate = DateTime.Now.AddDays(-18), Price = 4000 },
                new CategoryDateDTO { Id = Guid.NewGuid(), CategoryId = categoryList[1].Id, StartDate = DateTime.Now.AddDays(-30), EndDate = DateTime.Now.AddDays(-10), Price = 10000 },
                new CategoryDateDTO { Id = Guid.NewGuid(), CategoryId = categoryList[0].Id, StartDate = DateTime.Now.AddDays(-18), EndDate = DateTime.Now.AddDays(5), Price = 4500 },
                new CategoryDateDTO { Id = Guid.NewGuid(), CategoryId = categoryList[1].Id, StartDate = DateTime.Now.AddDays(-10), Price = 12000 },
                new CategoryDateDTO { Id = Guid.NewGuid(), CategoryId = categoryList[0].Id, StartDate = DateTime.Now.AddDays(5), Price = 5000 }
            };
        }

        [Test]
        public void RoomList()
        {
            moqRoomService.Setup(rooms => rooms.GetAll()).Returns(roomList);
            moqCategoryService.Setup(categories => categories.GetAll()).Returns(categoryList);
            moqCategoryDateService.Setup(categoryDates => categoryDates.GetAll()).Returns(categoryDateList);

            var controller = new RoomManageController(moqRoomService.Object, moqCategoryService.Object, moqCategoryDateService.Object);

            var result = controller.RoomList();

            Assert.AreEqual(roomList.Count, result.Count());
        }

        [Test]
        [TestCase(1, 4500, "Base")]
        [TestCase(2, 12000, "Vip")]
        [TestCase(3, 4500, "Base")]
        [TestCase(4, 4500, "Base")]
        [TestCase(5, 4500, "Base")]
        public void Get(int number, decimal expectedPrice, string expectedCategoryName)
        {
            var room = roomList.FirstOrDefault(x => x.Number == number);

            moqRoomService.Setup(rooms => rooms.GetAll()).Returns(roomList);
            moqCategoryService.Setup(categories => categories.Get(room.CategoryId)).Returns(categoryList.Find(x => x.Id == room.CategoryId));
            moqCategoryDateService.Setup(categoryDates => categoryDates.GetAll()).Returns(categoryDateList);
            var controller = new RoomManageController(moqRoomService.Object, moqCategoryService.Object, moqCategoryDateService.Object);

            var result = controller.Get(number);

            Assert.AreEqual(number, result.Value.Number);
            Assert.AreEqual(expectedPrice, result.Value.Price);
            Assert.AreEqual(expectedCategoryName, result.Value.Category);
        }

        [Test]
        [TestCase(6, "Vip", 12000)]
        [TestCase(7, "Base", 4500)]
        [TestCase(8, "Vip", 12000)]
        [TestCase(9, "Base", 4500)]
        [TestCase(10, "Base", 4500)]
        public async Task Add(int number, string category, decimal expectedPrice)
        {
            moqRoomService.Setup(rooms => rooms.GetAll()).Returns(roomList);
            moqCategoryService.Setup(categories => categories.GetAll()).Returns(categoryList);
            moqCategoryDateService.Setup(categoryDate => categoryDate.GetAll()).Returns(categoryDateList);
            var controller = new RoomManageController(moqRoomService.Object, moqCategoryService.Object, moqCategoryDateService.Object);

            var response = new RoomViewModel
            {
                Category = category,
                Number = number
            };

            var result = await controller.Add(response);

            Assert.AreEqual(number, result.Value.Number);
            Assert.AreEqual(category, result.Value.Category);
            Assert.AreEqual(expectedPrice, result.Value.Price);
        }

        [Test]
        [TestCase(2, 6, "Vip", 6, 12000)]
        [TestCase(4, 0, "Vip", 4, 12000)]
        public async Task Edit(int number, int newNumber, string category, int expectedNumber, decimal expectedPrice)
        {
            moqRoomService.Setup(rooms => rooms.GetAll()).Returns(roomList);
            moqCategoryService.Setup(categories => categories.GetAll()).Returns(categoryList);
            moqCategoryDateService.Setup(categoryDate => categoryDate.GetAll()).Returns(categoryDateList);
            var controller = new RoomManageController(moqRoomService.Object, moqCategoryService.Object, moqCategoryDateService.Object);

            var roomBase = roomList.FirstOrDefault(x => x.Number == number);

            var room = new RoomDTO
            {
                Id = roomBase.Id,
                CategoryId = categoryList.FirstOrDefault(x => x.Name == category).Id,
                Number = expectedNumber
            };

            var response = new ChangeRoomViewModel
            {
                Number = number,
                NewCategory = category,
                NewNumber = newNumber
            };

            var result = await controller.Edit(response);

            Assert.AreEqual(expectedNumber, result.Value.Number);
            Assert.AreEqual(category, result.Value.Category);
            Assert.AreEqual(expectedPrice, result.Value.Price);
            moqRoomService.Verify(rooms => rooms.UpdateAsync(room));
        }

        [Test]
        [TestCase(3)]
        [TestCase(5)]
        public async Task Delete(int number)
        {
            moqRoomService.Setup(rooms => rooms.GetAll()).Returns(roomList);

            var controller = new RoomManageController(moqRoomService.Object, moqCategoryService.Object, moqCategoryDateService.Object);

            var room = roomList.FirstOrDefault(x => x.Number == number);

            var result = await controller.Delete(number);

            moqRoomService.Verify(rooms => rooms.DeleteAsync(room.Id));
        }
    }
}